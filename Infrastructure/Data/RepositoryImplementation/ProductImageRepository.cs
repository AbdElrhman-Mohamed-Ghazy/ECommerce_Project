using Application.Common.Interfaces;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.RepositoryImplementation
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext _context;

        public ProductImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProductImage image, CancellationToken ct)
        {
            await _context.ProductImages.AddAsync(image, ct);
        }

        public async Task<ProductImage?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<List<ProductImage>> GetByProductIdAsync(Guid productId, CancellationToken ct)
        {
            return await _context.ProductImages
                .Where(x => x.ProductId == productId)
                .ToListAsync(ct);
        }

        public void Delete(ProductImage image)
        {
            _context.ProductImages.Remove(image);
        }
    }
}
