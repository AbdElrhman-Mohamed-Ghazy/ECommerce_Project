using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public partial class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200).HasColumnType("VARCHAR(200)");
            builder.Property(p => p.Description).HasMaxLength(255);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.HasMany(o => o.OrderItems).WithOne(o => o.Product).HasForeignKey(o => o.ProductId);
            builder.HasMany(c => c.CartItems).WithOne(c => c.Product).HasForeignKey(c => c.ProductId);
            builder.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);

        }
    }
}
