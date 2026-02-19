using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Cart.Commands.UpdateCartItemQuantity
{
    public sealed class UpdateCartItemQuantityCommandHandler(ICartRepository cartRepository) : IRequestHandler<UpdateCartItemQuantityCommand>
    {
        public async Task Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (cart is null)
            {
                throw new NotFoundException(nameof(cart));
            }
          
            cart.UpdateQuantity(request.ProductId, request.Quantity);
            await cartRepository.UpdateAsync(cart, cancellationToken);
            await cartRepository.SaveChangesAsync(cancellationToken);
        }
    }
}