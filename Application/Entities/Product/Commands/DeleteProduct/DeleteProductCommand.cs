using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Commands.DeleteProduct
{
    public sealed record DeleteProductCommand(Guid Id) : IRequest
    {
    }
}
