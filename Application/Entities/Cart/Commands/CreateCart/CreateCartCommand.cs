using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Cart.Commands.CreateCart
{
    public sealed record CreateCartCommand(Guid UserId) : IRequest<Guid>
    { }


}
