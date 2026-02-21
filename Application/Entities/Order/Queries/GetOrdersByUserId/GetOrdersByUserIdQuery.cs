using Application.Dtos;
using MediatR;

namespace Application.Entities.Order.Queries.GetOrdersByUserId
{
    public sealed record GetOrdersByUserIdQuery(Guid UserId) : IRequest<List<OrderDto>>
    {
    }
}