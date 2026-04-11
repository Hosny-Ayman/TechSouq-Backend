namespace TechSouq.API.Extensions
{
    public static class CorsRegistration
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services,IConfiguration configuration)
        {

            var allowedOrigins = configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();
            services.AddCors(options =>
            {
                options.AddPolicy("TechSouqCorsPolicy", builder =>
                {
                    if (allowedOrigins != null && allowedOrigins.Length > 0)
                    {
                        builder.WithOrigins(allowedOrigins)
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    }
                    //else
                    //{
                   
                    //    builder.AllowAnyOrigin()
                    //           .AllowAnyMethod()
                    //           .AllowAnyHeader();
                    //}
                });
            });

            return services;
        }
    }

}

