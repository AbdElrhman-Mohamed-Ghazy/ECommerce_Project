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
            return _dbSet.FirstOrDefaultAsync(o => o.UserId == userId, ct);
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

        public async Task<Order?> GetByOrderIdAsync(Guid orderId, CancellationToken ct)
        {
            return await _dbSet.FirstOrDefaultAsync(o => o.Id == orderId, ct);
        }

        public async Task<List<Order>> GetAllAsync(CancellationToken ct)
        {
            return await _dbSet.ToListAsync(ct);
        }
        public async Task<Order?>GetFullOrderInfoByOrderIdAsync(Guid orderId, CancellationToken ct)
        {
            return await _dbSet.Include(o=>o.Items).ThenInclude(i=>i.Product).FirstOrDefaultAsync(o => o.Id == orderId,ct);
        }
    }
}
