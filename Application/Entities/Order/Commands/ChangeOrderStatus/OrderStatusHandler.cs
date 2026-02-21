using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Order.Commands.ChangeOrderStatus
{
    public sealed class OrderStatusHandler(IUnitOfWorkRepository unitOfWorkRepository,IOrderRepository orderRepository) : IRequestHandler<OrderStatusCommand>
    {
        public async Task Handle(OrderStatusCommand request, CancellationToken cancellationToken)
        {
           var order = await orderRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                throw new NotFoundException(nameof(order));
            }

           order.ChangeStatus(request.OrderStatus.Status);

            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);
        
        }
    }
}
