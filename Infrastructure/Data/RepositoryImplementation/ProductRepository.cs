using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.RepositoryImplementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _Context;
        private readonly DbSet<Product> _dbSet;
        public ProductRepository(AppDbContext context)
        {
            _Context = context;
            _dbSet = context.Set<Product>();
        }

        public async Task AddAsync(Product entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(Product entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id, cancellationToken);

        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _Context.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(Product entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryName(string categoryName, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Include(x => x.Category).Where(x => x.Category.Name.ToLower() == categoryName.ToLower()).AsNoTracking().ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<Product>> GetProductsByName(string name, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(x => x.Name.ToLower() == name.ToLower()).ToListAsync(cancellationToken);
        }

        public async Task<bool> IsExistAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().AnyAsync(g => g.Id == productId, cancellationToken);
        }
        public async Task<decimal> GetProductPriceAsync(Guid productId, CancellationToken ct)
        {
            var product = await _dbSet.AsNoTracking().Select(x => new { x.Id, x.Price }).FirstAsync(x => x.Id == productId, ct);
            return product.Price;
        }

        public async Task<int> GetProductQuantityAsync(Guid productId, CancellationToken ct)
        {
            var product = await _dbSet.AsNoTracking().Select(x => new { x.Id, x.StockQuantity }).FirstAsync(x => x.Id == productId, ct);
            return product.StockQuantity;
        }

        public async Task SetProductQuantityAsync(Guid productId, int quantity, CancellationToken ct)
        {
            var product = await _dbSet.FirstAsync(x => x.Id == productId, ct);
            product.StockQuantity += quantity;
            _dbSet.Update(product);
        }
        public async Task<List<Product>> GetAllProductsByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _Context.Products.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        }
    }
}


