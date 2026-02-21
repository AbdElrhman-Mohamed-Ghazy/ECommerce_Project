using Application.Dtos;
using Domain.Entities.Order;
using MediatR;

namespace Application.Entities.Order.Queries.GetOrderById
{
    public sealed record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto>
    {
    }
}