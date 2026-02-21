using Application.Common.Interfaces;
using Application.Dtos;
using MediatR;

namespace Application.Entities.Order.Queries.GetAllOrders
{
    public sealed class GetAllOrdersHandler(IOrderRepository orderRepository) : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetAllAsync(cancellationToken);
            return orders.Select(o => new OrderDto
            {
                UserId = o.UserId,
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                ShippingAddress = o.ShippingAddress,
                Status = o.Status,
                TotalPrice = o.TotalPrice

            }
            ).ToList(); }
          }
           
        }
