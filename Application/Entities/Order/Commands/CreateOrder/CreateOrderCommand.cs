using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Order.Commands.CreateOrder
{
    public sealed record  CreateOrderCommand(Guid UserId,string Address):IRequest<Guid>
    {
    }
}
