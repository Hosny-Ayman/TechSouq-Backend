using Microsoft.Extensions.DependencyInjection;
using TechSouq.Application.Mappings;
using TechSouq.Application.Services;
using TechSouq.Application.Validators;
using FluentValidation; 

namespace TechSouq.Application.Extensions
{
    public static class ApplicationRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<AddressService>();
            services.AddScoped<BrandService>();
            services.AddScoped<CartItemService>();
            services.AddScoped<CartService>();
            services.AddScoped<CategorieService>();
            services.AddScoped<DeliveryMethodService>();
            services.AddScoped<OrderService>();
            services.AddScoped<OrderItemService>();
            services.AddScoped<ProductService>();
            services.AddScoped<ProductImageService>();
            services.AddScoped<RoleService>();
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<TokenService>();
            services.AddScoped<ProductReviewService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<DeliveryZoneService>();
            services.AddScoped<SystemSettingsService>();
            services.AddScoped<CouponService>();
            services.AddScoped<PaymentWayService>();
            services.AddScoped<DashboardService>();

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            services.AddValidatorsFromAssemblyContaining<AddressValidator>();

            return services;
        }
    }
}