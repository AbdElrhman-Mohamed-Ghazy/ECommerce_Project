using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Entities.Cart.Commands.ClearCart
{
    public class ClearCartCommandHandler(ICartRepository cartRepository) : IRequestHandler<ClearCartCommand>
    {
        public async Task Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (cart is null)
            {
                throw new NotFoundException(nameof(cart));
            }

            cart.Clear();
            await cartRepository.UpdateAsync(cart, cancellationToken);
            await cartRepository.SaveChangesAsync(cancellationToken);
        }
    }
}