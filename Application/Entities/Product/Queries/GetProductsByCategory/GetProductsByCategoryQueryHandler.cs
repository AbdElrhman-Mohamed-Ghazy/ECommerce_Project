using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Queries.GetProductsByCategory
{
    public sealed class GetProductsByCategoryQueryHandler(IProductRepository repository) : IRequestHandler<GetProductsByCategoryQuery, List<ProductDto>>
    {
        public async Task<List<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var product = await repository.GetProductsByCategoryName(request.Category, cancellationToken);
            if (product == null || !product.Any())
            {
                throw new NotFoundException(nameof(Product));
            }
            return product.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p    .Name,
                Description = p.Description,
                Price = p.Price,

            }).ToList();
        }
    }
}
