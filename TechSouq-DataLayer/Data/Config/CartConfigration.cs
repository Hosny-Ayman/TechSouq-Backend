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
    public class CartConfigration:IEntityTypeConfiguration<Cart>
    {

        public void Configure(EntityTypeBuilder<Cart> builder)
        {

            builder.ToTable("Carts");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User).WithOne(x => x.cart).HasForeignKey<Cart>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.UserId).IsUnique();

            builder.Property(x => x.Status).IsRequired();

          

        }

    }
}
