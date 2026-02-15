using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Queries.GetProductById
{
    public sealed record  GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>
    {
    }
}
