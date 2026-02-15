using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Queries.GetProductsByName
{
    public sealed record  GetProductsByNameQuery(string Name) : IRequest<List<ProductDto>>
    {
    }
}
