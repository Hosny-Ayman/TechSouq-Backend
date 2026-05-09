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
    internal class UserConfiguration:IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName).HasColumnType("NVARCHAR(50)").IsRequired();
            builder.Property(x => x.SecondName).HasColumnType("NVARCHAR(50)").IsRequired();

            builder.Property(x => x.Email).HasColumnType("NVARCHAR(255)").IsRequired();
            builder.HasIndex(x=>x.Email).IsUnique();

            builder.Property(x => x.Password).HasColumnType("NVARCHAR(255)").IsRequired();

            builder.HasOne(x=>x.Role).WithMany(x=>x.Users).HasForeignKey(x=>x.RoleId).OnDelete(DeleteBehavior.Restrict);

        }

    }
}
