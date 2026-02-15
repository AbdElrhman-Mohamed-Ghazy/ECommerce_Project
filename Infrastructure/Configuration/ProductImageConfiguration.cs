using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public partial class ProductConfiguration
    {
        public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
        {
            public void Configure(EntityTypeBuilder<ProductImage> builder)
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.ImageUrl)
                       .IsRequired().HasMaxLength(300);

                builder.Property(x => x.FileName)
                       .HasMaxLength(255)
                       .IsRequired();

                builder.Property(x => x.FilePath)
                       .IsRequired();

                builder.Property(x => x.FileExtension)
                       .HasMaxLength(10)
                       .IsRequired();

                builder.Property(x => x.ContentType)
                       .HasMaxLength(100)
                       .IsRequired();

                builder.HasOne(x => x.Product)
                       .WithMany(p => p.ProductImages)
                       .HasForeignKey(x => x.ProductId)
                       .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
}
