using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using System.Security.Claims;

namespace TechSouq.API.Extensions
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("GeneralRule", httpContext =>
                {
                    var partitionKey = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                       ?? httpContext.Connection.RemoteIpAddress?.ToString()
                                       ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(partitionKey, partition => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100, 
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
                });

                options.AddPolicy("StrictAuth", httpContext =>
                {
                    var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(clientIp, partition => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3, 
                        Window = TimeSpan.FromMinutes(1)
                    });
                });
            });

            return services;
        }
    }
}