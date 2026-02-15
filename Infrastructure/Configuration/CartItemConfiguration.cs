using Domain.Entities.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public partial class ProductConfiguration
    {
        public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
        {
            public void Configure(EntityTypeBuilder<CartItem> builder)
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Id).ValueGeneratedOnAdd();
                builder.Property(c => c.UnitPrice).HasColumnType("decimal(18,2)");

            }
        }
    }
}
