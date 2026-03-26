using Application.Dtos;
using MediatR;

namespace Application.Entities.Order.Queries.GetOrdersByUserId
{
    public sealed record GetOrdersByUserIdQuery(string UserId) : IRequest<List<OrderDto>>
    {
    }
}