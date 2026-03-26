using Application.Common.Interfaces;
using Application.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Entities.Order.Queries.GetAllOrders
{
    public sealed class GetAllOrdersHandler(IOrderRepository orderRepository,IMapper mapper) : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<OrderDto>>(orders);
            //return orders.Select(o => new OrderDto
            //{
            //    UserId = o.UserId,
            //    Id = o.Id,
            //    CreatedAt = o.CreatedAt,
            //    ShippingAddress = o.ShippingAddress,
            //    Status = o.Status,
            //    TotalPrice = o.TotalPrice

            //}
            //).ToList();
            }
          }
           
        }
