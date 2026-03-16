using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Queries.GetProductById
{
    public sealed class GetProductByIdQueryHandler(IProductRepository repository, IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null )
            {
                throw new NotFoundException(nameof(Product));
            }
            return mapper.Map<ProductDto>(product);
      
        }
    }
}