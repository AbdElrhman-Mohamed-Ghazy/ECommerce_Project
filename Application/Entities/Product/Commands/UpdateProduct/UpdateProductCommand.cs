using Application.Dtos;
using Application.Entities.Product.Commands.CreateProduct;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Commands.UpdateProduct
{
    public sealed record  UpdateProductCommand(Guid ProductId, ProductDto ProductDto) : IRequest
    {
    }
}
