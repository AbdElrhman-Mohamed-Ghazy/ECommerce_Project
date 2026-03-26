using MediatR;

namespace Application.Entities.Cart.Commands.ClearCart
{
    public sealed record ClearCartCommand(string UserId) : IRequest;
}