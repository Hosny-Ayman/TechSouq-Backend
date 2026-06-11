using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using Stripe;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using TechSouq.API;
using TechSouq.API.Extensions;
using TechSouq.API.Policies;
using TechSouq.Application;
using TechSouq.Application.Extensions;
using TechSouq.Application.Queries;
using TechSouq.Infrastructure.Data;
using TechSouq.Infrastructure.Extensions;

namespace TechSouq_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Serilog.Debugging.SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithProperty("Application", "TechSouq_API")
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File(
                @"D:\Programming 2026\TechSouq Project\TechSouqLogs\log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                buffered: false)
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            Log.Information("Program Work Good");



            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();

                var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");



                builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));

                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["accessToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
          };
        });

                builder.Services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(ConnectionString)); 

                builder.Services.AddHangfireServer();

                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy("OwnerOnly", policy =>
                        policy.Requirements.Add(new ResourceOwnerRequirement()));
                });

                builder.Services.AddSingleton<IAuthorizationHandler, ResourceOwnerHandler>();

                builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var errors = actionContext.ModelState
                            .Where(e => e.Value.Errors.Count > 0)
                            .SelectMany(x => x.Value.Errors)
                            .Select(x => x.ErrorMessage)
                            .ToList();

                        var logger = actionContext.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

                        var path = actionContext.HttpContext.Request.Path;
                        logger.LogWarning("Automatic Validation Failed at Endpoint: {Path}. Errors: {@Errors}", path, errors);

                        var operationResult = OperationResult<object>.BadRequest("Validation Error", errors);

                        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(operationResult);
                    };
                });

                builder.Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = builder.Configuration.GetConnectionString("Redis");
                    options.InstanceName = "TechSouq_"; 
                });

                builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));

                StripeConfiguration.ApiKey = builder.Configuration["StripeSettings:SecretKey"];



                builder.Services.AddInfrastructureServices();

                builder.Services.AddApplicationServices();

                // Add services to the container.

                builder.Services.AddFluentValidationAutoValidation();
                //builder.Services.AddFluentValidationClientsideAdapters();

                builder.Services.AddCustomRateLimiting();

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "دخل التوكن بتاعك هنا على طول من غير كلمة Bearer"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
                });

                //builder.Services.AddAutoMapper(typeof(TechSouq.Application.Mappings.MappingProfiles));

                builder.Services.AddCorsPolicy(builder.Configuration);

                var app = builder.Build();

                app.UseMiddleware<ExceptionHandlingMiddleware>();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseCors("TechSouqCorsPolicy");


                app.UseStaticFiles();


                app.UseHttpsRedirection();


                app.UseAuthentication();

                app.UseAuthorization();

                app.UseRateLimiter();

                app.MapControllers();


                app.UseHangfireDashboard("/hangfire");
                RecurringJob.AddOrUpdate<TechSouq.Application.Services.ProductService>(
                    "Remove-Expired-Discounts-Job",
                    service => service.RunDailyDiscountCleanupJob(),
                    Cron.Daily);

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start correctly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}