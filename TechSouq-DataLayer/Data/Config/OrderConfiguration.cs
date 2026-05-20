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
    public class OrderConfiguration:IEntityTypeConfiguration<Order>
    {

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);

            //builder.Property(x => x.OrderDate).HasColumnType("DateTime").IsRequired();

            //builder.Property(x => x.Status).HasColumnType("Nvarchar(20)").IsRequired();

            //builder.Property(x => x.TotalAmount).HasColumnType("Decimal(10,2)").IsRequired();

            //builder.Property(x => x.ShippingStreet).HasColumnType("Nvarchar(250)").IsRequired();

            //builder.Property(x => x.ShippingCity).HasColumnType("Nvarchar(100)").IsRequired();

            builder.HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict).OnDelete(DeleteBehavior.Restrict);





        }

    }
}
