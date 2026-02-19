using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken ct);
        Task AddAsync(Cart cart, CancellationToken ct);
        Task UpdateAsync(Cart cart, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);

    }
}
