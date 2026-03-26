using MediatR;

namespace Application.Entities.Cart.Commands.RemoveItemFromCart
{
    public sealed record RemoveItemFromCartCommand(string UserId, Guid ProductId) : IRequest;
}