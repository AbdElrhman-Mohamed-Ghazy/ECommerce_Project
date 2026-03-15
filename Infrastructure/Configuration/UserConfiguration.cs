using Domain.Entities;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public partial class ProductConfiguration
    {
        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Id).ValueGeneratedOnAdd();
                builder.HasMany( u=> u.Orders).WithOne(o => o.user).HasForeignKey(u => u.UserId);
                builder.HasOne(u => u.Cart).WithOne(c => c.user).HasForeignKey<Cart>(c => c.UserId);
                builder.Property(u => u.UserName).HasMaxLength(50);
                builder.Property(o => o.Role).HasMaxLength(150);
                builder.Property(o => o.Email).HasMaxLength(150);

            }
        }
    }
}
