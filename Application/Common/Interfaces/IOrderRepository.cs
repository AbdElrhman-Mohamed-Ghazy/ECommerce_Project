using Domain.Entities;
using Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByUserIdAsync(Guid userId, CancellationToken ct);
        Task AddAsync(Order order, CancellationToken ct);
        Task UpdateAsync(Order order, CancellationToken ct);
        Task<Order?> GetByOrderIdAsync(Guid orderId, CancellationToken ct);
        Task<List<Order>> GetAllAsync(CancellationToken ct);
        Task<Order?>GetFullOrderInfoByOrderIdAsync(Guid orderId, CancellationToken ct);
    }
}
