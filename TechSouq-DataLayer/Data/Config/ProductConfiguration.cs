using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Infrastructure.Data.Config
{
    public class ProductConfiguration:IEntityTypeConfiguration<Product>
    {

        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.ToTable("Products");

            builder.HasKey(x => x.Id);


            builder.Property(x => x.Name).HasColumnType("NVARCHAR(MAX)").IsRequired();

            builder.Property(x => x.Description).HasColumnType("NVARCHAR(MAX)").IsRequired();

            builder.Property(x => x.Stock).HasColumnType("INTEGER").IsRequired();

            builder.Property(x => x.Price).HasColumnType("DECIMAL(10,2)").IsRequired();


            builder.HasOne(X => X.Categorie).WithMany(X => X.Products).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(X => X.Brand).WithMany(X => X.Products).HasForeignKey(x => x.BrandId).OnDelete(DeleteBehavior.Restrict);


        }

    }
}
