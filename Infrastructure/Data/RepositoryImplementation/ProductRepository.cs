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

        public async Task DeleteAsync(Product entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
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

        public async Task UpdateAsync(Product entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
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

    }
}


