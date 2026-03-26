using Domain.Entities;
using Domain.Entities.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{

    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
        {
            public void Configure(EntityTypeBuilder<ApplicationUser> builder)
            {
                builder.HasMany(u => u.Orders).WithOne(o => o.user).HasForeignKey(u => u.UserId);
                builder.HasOne(u => u.Cart).WithOne(c => c.user).HasForeignKey<Cart>(c => c.UserId);
        }
               }
     
    }

