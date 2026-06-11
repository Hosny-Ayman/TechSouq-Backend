using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Address> Addresses { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Categorie> Categories { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ProductReview> ProductReview { get; set; }

        public DbSet<Coupon> Coupons { get; set; }

        public DbSet<SystemSettings> SystemSettings { get; set; }

        public DbSet<DeliveryZone> DeliveryZones { get; set; }

        public DbSet<PaymentWay> PaymentWays { get; set; }


        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelbuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);

        }
    }



   
}
