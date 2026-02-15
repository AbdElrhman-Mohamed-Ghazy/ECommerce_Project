using Domain.Entities.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public partial class ProductConfiguration
    {
        public class CartConfiguration : IEntityTypeConfiguration<Cart>
        {
            public void Configure(EntityTypeBuilder<Cart> builder)
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Id).ValueGeneratedOnAdd();
                builder.HasMany(c=>c.Items).WithOne(c=>c.Cart).HasForeignKey(c=>c.CartId);

            }
        }
    }
}
