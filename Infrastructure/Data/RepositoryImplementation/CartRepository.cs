using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.RepositoryImplementation
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Cart> _dbSet;
        public CartRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Cart>();
        }

        public Task AddAsync(Cart cart, CancellationToken ct)
        {
            _dbSet.Add(cart);
            return Task.CompletedTask;
        }

        public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return await _dbSet.Include(c => c.Items) .FirstOrDefaultAsync(c => c.UserId == userId, ct);
        }

        public Task UpdateAsync(Cart cart, CancellationToken ct)
        {
            _dbSet.Update(cart);
            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync(CancellationToken ct)
        {
           await   _context.SaveChangesAsync(ct);
        }
    }
}
