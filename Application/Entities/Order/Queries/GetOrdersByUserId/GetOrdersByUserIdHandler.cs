using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using MediatR;

namespace Application.Entities.Order.Queries.GetOrdersByUserId
{
    public sealed class GetOrdersByUserIdHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrdersByUserIdQuery, List<OrderDto>>
    {
        public async Task<List<OrderDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (order == null)
            {
                throw new NotFoundException(nameof(order));
            }

            return new List<OrderDto>
            {
                new OrderDto
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    ShippingAddress = order.ShippingAddress,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice,
                    CreatedAt = order.CreatedAt
                }
            };
        }
    }
}