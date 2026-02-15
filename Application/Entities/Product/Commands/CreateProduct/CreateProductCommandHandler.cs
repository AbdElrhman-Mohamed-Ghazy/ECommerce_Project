using Application.Common.Interfaces;
using Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Commands.CreateProduct
{
    public class CreateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<CreateProductCommand, Guid>
    {
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
          
            var product = new Domain.Entities.Products.Product
            {
                Id = Guid.NewGuid(),
               Description=  request.ProductDto.Description,
                Name=      request.ProductDto.Name,
                Price = request.ProductDto.Price,
                StockQuantity=  request.ProductDto.StockQuantity,
                CategoryId =request.ProductDto.CategoryId

            };
            await productRepository.AddAsync(product, cancellationToken);
            await productRepository.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
    }
}
