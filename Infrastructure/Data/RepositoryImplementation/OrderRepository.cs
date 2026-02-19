using Application.Common.Interfaces;
using Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.RepositoryImplementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Order> _dbSet;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Order>();
        }

        public Task AddAsync(Order order, CancellationToken ct)
        {
           _dbSet.AddAsync(order, ct);
            return Task.CompletedTask;
        }

        public Task<Order?> GetByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return _dbSet.Include(o => o.Items)
                         .ThenInclude(oi => oi.Product)
                         .FirstOrDefaultAsync(o => o.UserId == userId, ct);
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _context.SaveChangesAsync(ct);
        }

        public Task UpdateAsync(Order order, CancellationToken ct)
        {
            _dbSet.Update(order);
            return Task.CompletedTask;
        }
    }
}
