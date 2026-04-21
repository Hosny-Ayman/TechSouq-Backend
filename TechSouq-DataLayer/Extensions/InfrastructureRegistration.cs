using Microsoft.Extensions.DependencyInjection;
using TechSouq.DataLayer.Repositories;
using TechSouq.Domain.Interfaces;
using TechSouq.Domian.Interfaces;
using TechSouq.Infrastructure.Repositories;
using TechSouq.Application.Queries;
using TechSouq.Infrastructure.Queries;


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

            return services;
        }
    }
}