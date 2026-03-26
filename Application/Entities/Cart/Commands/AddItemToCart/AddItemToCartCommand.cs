using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Cart.Commands.AddItemToCart
{
    public sealed record AddItemToCartCommand(string UserId,Guid ProductId,int Quantity) : IRequest;

}
