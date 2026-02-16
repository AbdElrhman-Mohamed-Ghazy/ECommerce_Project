using Application.Common.Interfaces;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.RepositoryImplementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Category> _dbSet;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Category>();
        }
        public async Task AddAsync(Category entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public  Task DeleteAsync(Category entity, CancellationToken cancellationToken = default)
        {
             _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Category>> FindAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public async Task<bool> IsExistAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
           return await _dbSet.AsNoTracking().AnyAsync(g=>g.Id == categoryId, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
           await  _context.SaveChangesAsync(cancellationToken);
        }

        public  Task UpdateAsync(Category entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
    }
}
