using MediatR;

namespace Application.Entities.Cart.Commands.RemoveItemFromCart
{
    public sealed record RemoveItemFromCartCommand(Guid UserId, Guid ProductId) : IRequest;
}