using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Order.Queries.GetFullOrderInfoById
{
    public sealed class GetFullOrderInfoByIdHandler(IOrderRepository orderRepository) : IRequestHandler<GetFullOrderInfoByIdCommand, FullOrderInfoDto>
    {
        public async Task<FullOrderInfoDto> Handle(GetFullOrderInfoByIdCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetFullOrderInfoByOrderIdAsync(request.OrderId, cancellationToken);
            if (order == null )
            {
                throw new NotFoundException(nameof(order));
            }
            return order.Items.Select(item => new FullOrderInfoDto
            {
                Id = order.Id,
                UserId = order.UserId,
                ShippingAddress = order.ShippingAddress,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                Name = item.Product.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).FirstOrDefault()!;
        }
    }
}
