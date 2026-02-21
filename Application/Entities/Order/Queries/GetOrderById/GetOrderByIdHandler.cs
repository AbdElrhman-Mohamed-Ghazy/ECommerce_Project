using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using MediatR;

namespace Application.Entities.Order.Queries.GetOrderById
{
    public sealed class GetOrderByIdHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                throw new NotFoundException(nameof(order));
            }

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                ShippingAddress = order.ShippingAddress,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                CreatedAt = order.CreatedAt
            };
        }
    }
}