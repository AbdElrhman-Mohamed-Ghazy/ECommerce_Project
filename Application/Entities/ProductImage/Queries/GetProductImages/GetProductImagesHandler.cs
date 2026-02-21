using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Entities.Products;
using MediatR;

namespace Application.Entities.ProductImage.Queries.GetProductImages
{

    public class GetProductImagesHandler  : IRequestHandler<GetProductImagesQuery, List<ProductImageDto>>
    {
        private readonly IProductImageRepository _imageRepo;

        public GetProductImagesHandler(IProductImageRepository imageRepo)
        {
            _imageRepo = imageRepo;
        }

        public async Task<List<ProductImageDto>> Handle(GetProductImagesQuery request, CancellationToken ct)
        {
            var images = await _imageRepo.GetByProductIdAsync(request.ProductId, ct);
            if (images == null)
            {
                throw new NotFoundException("Image");
            }
            return images .Select(x => new ProductImageDto(x.Id, x.ImageUrl))
                .ToList();
        }
    }
}