using Application.Dtos;
using Domain.Entities.Products;
using MediatR;
using System.Collections.Generic;

namespace Application.Entities.ProductImage.Queries.GetProductImages
{
    public sealed record GetProductImagesQuery(Guid ProductId) : IRequest<List<ProductImageDto>>;
}