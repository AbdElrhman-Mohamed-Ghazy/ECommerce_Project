using MediatR;

namespace Application.Entities.Cart.Commands.ClearCart
{
    public sealed record ClearCartCommand(Guid UserId) : IRequest;
}