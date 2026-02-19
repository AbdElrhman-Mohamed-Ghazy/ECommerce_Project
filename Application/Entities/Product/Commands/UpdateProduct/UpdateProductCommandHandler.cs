using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Entities.Product.Commands.DeleteProduct;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler(IProductRepository repository, IUnitOfWorkRepository unitOfWorkRepository) : IRequestHandler<UpdateProductCommand>
    {
        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
            {
                throw new NotFoundException(nameof(product));
            }
            product.Description =request.ProductDto.Description;
            product.Price = request.ProductDto.Price;
            product.Name = request.ProductDto.Name;
            product.StockQuantity = request.ProductDto.StockQuantity;
            product.CategoryId = request.ProductDto.CategoryId;

            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
