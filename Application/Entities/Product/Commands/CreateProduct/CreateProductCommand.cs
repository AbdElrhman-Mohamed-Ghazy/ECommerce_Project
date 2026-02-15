using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Commands.CreateProduct
{
    public sealed record  CreateProductCommand(ProductDto ProductDto) :IRequest<Guid>
    {
    }
}
