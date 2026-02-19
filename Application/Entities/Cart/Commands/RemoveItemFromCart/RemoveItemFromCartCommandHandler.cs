using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Entities.Cart.Commands.RemoveItemFromCart
{
    public class RemoveItemFromCartCommandHandler(ICartRepository cartRepository, IUnitOfWorkRepository unitOfWorkRepository) : IRequestHandler<RemoveItemFromCartCommand>
    {
        public async Task Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (cart is null)
            {
                throw new NotFoundException(nameof(cart));
            }

            cart.RemoveItem(request.ProductId);
            await cartRepository.UpdateAsync(cart, cancellationToken);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);
        }
    }
}