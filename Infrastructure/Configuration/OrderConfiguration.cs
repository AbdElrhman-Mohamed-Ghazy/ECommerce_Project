using Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public partial class ProductConfiguration
    {
        public class OrderConfiguration : IEntityTypeConfiguration<Order>
        {
            public void Configure(EntityTypeBuilder<Order> builder)
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Id).ValueGeneratedOnAdd();
                builder.HasMany(c => c.Items).WithOne(c => c.Order).HasForeignKey(c => c.OrderId);
                builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(50);
                builder.Property(o=>o.ShippingAddress).HasMaxLength(150);
                builder.Property(c => c.TotalPrice).HasColumnType("decimal(18,2)");
            }
        }
    }
}
