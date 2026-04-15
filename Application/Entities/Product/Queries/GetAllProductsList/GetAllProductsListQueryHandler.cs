using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using AutoMapper;
using Domain.Entities.Products;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Application.Entities.Product.Queries.GetAllProductsList
{
    public sealed class GetAllProductsListQueryHandler(IProductRepository repository,IMapper mapper, ICacheService cache,
                                                      ILogger<GetAllProductsListQueryHandler> _logger) : IRequestHandler<GetAllProductsListQuery, List<ProductDto>>
    {
        public async Task<List<ProductDto>> Handle(GetAllProductsListQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = "products_all";
            // 1. Try Cache
            var cachedProducts = await cache.GetAsync<List<ProductDto>>(cacheKey);

            if (cachedProducts != null)
            {
                _logger.LogInformation("جاب الكاش");
                return cachedProducts;
            }

            var products = await repository.GetAllAsync();
            if (products == null || !products.Any())
            {
                throw new NotFoundException(nameof(Product));
            }

            var mappedProducts = mapper.Map<List<ProductDto>>(products);

            // 2. Set Cache
            try
            {
                await cache.SetAsync(cacheKey, mappedProducts, TimeSpan.FromMinutes(10));
                _logger.LogInformation("خزن الكاش");
            }
            catch (Exception ex)
            {
                // ليه؟ لأن الكاش Optional
                // لو فشل → النظام يكمل عادي
                _logger.LogError(ex, "Cache failed");
            }

            return mappedProducts;
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