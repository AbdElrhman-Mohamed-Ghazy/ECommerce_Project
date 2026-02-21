using Application.Dtos;
using MediatR;

namespace Application.Entities.Order.Queries.GetAllOrders
{
    public sealed record GetAllOrdersQuery : IRequest<List<OrderDto>>
    {
    }
}