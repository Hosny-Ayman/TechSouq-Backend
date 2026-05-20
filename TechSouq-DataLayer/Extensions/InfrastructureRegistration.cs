using Microsoft.Extensions.DependencyInjection;
using TechSouq.Application.Queries;
using TechSouq.Application.Services;
using TechSouq.DataLayer.Repositories;
using TechSouq.Domain.Interfaces;
using TechSouq.Domian.Interfaces;
using TechSouq.Infrastructure.Queries;
using TechSouq.Infrastructure.Repositories;
using TechSouq.Infrastructure.Services;


namespace TechSouq.Infrastructure.Extensions
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICategorieRepository, CategorieRepository>();
            services.AddScoped<IDeliveryMethodRepository, DeliveryMethodRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>(); 
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductQueryService, ProductQueryService>();
            services.AddScoped<ICategorieQueryService, CategorieQueryService>();
            services.AddScoped<ICartItemsQueryService, CartItemsQueryService>();
            services.AddScoped<IProductReview, ProductReviewRepository>();
            services.AddScoped<IProductReviewQueryService, ProductReviewQueryService>();
            services.AddScoped<IDeliveryZone, DeliveryZoneRepository>();
            services.AddScoped<ISystemSettingsRepository, SystemSettingsRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IPaymentWayRepository, PaymentWayRepository>();
            services.AddScoped<IOrderQueryService, OrderQueryService>();
            services.AddScoped<IEmailService, EmailService>();


            return services;
        }
    }
}