using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Cart.Queries.GetCartByUserId
{
    public sealed record  GetCartInfoByUserIdCommand(Guid UserId) : IRequest<CartDto>
    {
    }
}
