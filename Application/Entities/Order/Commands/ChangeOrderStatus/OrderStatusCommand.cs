using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Order.Commands.ChangeOrderStatus
{
    public sealed record OrderStatusCommand(Guid OrderId, UpdateOrderStatusDto OrderStatus) : IRequest
    {
    }
}
