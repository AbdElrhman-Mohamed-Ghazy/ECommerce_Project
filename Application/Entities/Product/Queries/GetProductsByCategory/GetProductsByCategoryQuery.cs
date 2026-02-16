using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Queries.GetProductsByCategory
{
    public sealed record  GetProductsByCategoryQuery(string CategoryName) : IRequest<List<ProductDto>>
    {
    }
}
