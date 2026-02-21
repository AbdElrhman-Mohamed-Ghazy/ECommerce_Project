using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IProductImageRepository
    {
        Task AddAsync(ProductImage image, CancellationToken ct);
        Task<ProductImage?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<List<ProductImage>> GetByProductIdAsync(Guid productId, CancellationToken ct);
        void Delete(ProductImage image);
    }
}
