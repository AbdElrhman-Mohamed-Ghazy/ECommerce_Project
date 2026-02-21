using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Entities.ProductImage.Commands.AddProductImage
{
    public class AddProductImageHandler(IProductRepository productRepo,IProductImageRepository imageRepo,
            IUnitOfWorkRepository unitOfWork) : IRequestHandler<AppProductImageCommand>
    {
     

        public async Task Handle( AppProductImageCommand request,CancellationToken ct)
        {
            var product = await productRepo.GetByIdAsync(request.ProductId, ct);
            if (product == null)
                throw new Exception("Product");

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (var img in request.Images)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                await File.WriteAllBytesAsync(filePath, img.Content, ct);

                var image = new Domain.Entities.Products.ProductImage
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    FileName = fileName,
                    FilePath = filePath,
                    FileExtension = Path.GetExtension(img.FileName),
                    ContentType = img.ContentType,
                    ImageUrl = $"/images/{fileName}"
                    
                };

                await imageRepo.AddAsync(image, ct);
            }

            await unitOfWork.SaveChangesAsync(ct);
         
        }
    }
}
