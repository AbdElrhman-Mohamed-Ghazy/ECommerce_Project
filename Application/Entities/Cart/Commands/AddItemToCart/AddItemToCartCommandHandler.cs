using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Cart.Commands.AddItemToCart
{
    public class AddItemToCartCommandHandler(ICartRepository cartrepository, IProductRepository productRepository) : IRequestHandler<AddItemToCartCommand>
    {
       
        public async Task Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await cartrepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (cart is null)
            {
                throw new NotFoundException(nameof (cart));
            }
            var availableQuantity =  await productRepository.GetProductQuantityAsync(request.ProductId, cancellationToken);

            if (request.Quantity > availableQuantity)
                throw new NotFoundException(nameof(availableQuantity));

            var productPrice = await productRepository.GetProductPriceAsync(request.ProductId, cancellationToken);
             cart.AddItem(request.ProductId, productPrice, request.Quantity);
            await cartrepository.UpdateAsync(cart, cancellationToken);
            await cartrepository.SaveChangesAsync(cancellationToken);

        }
    }
}
