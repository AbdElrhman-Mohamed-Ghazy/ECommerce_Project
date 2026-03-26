using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Entities.Product.Queries.GetAllProductsList
{
    public sealed class GetAllProductsListQueryHandler(IProductRepository repository,IMapper mapper) : IRequestHandler<GetAllProductsListQuery, List<ProductDto>>
    {
        public async Task<List<ProductDto>> Handle(GetAllProductsListQuery request, CancellationToken cancellationToken)
        {
            var products = await repository.GetAllAsync();
            if (products == null || !products.Any())
            {
                throw new NotFoundException(nameof(Product));
            }
            return mapper.Map<List<ProductDto>>(products);
            //return products.Select(p => new ProductDto
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Description = p.Description,
            //    Price = p.Price,
            //    StockQuantity = p.StockQuantity,
            //    CategoryId = p.CategoryId
            //}).ToList();
        }


    }
}