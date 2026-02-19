using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IProductRepository  : IGenericRepository<Product> 
    {
        Task<IEnumerable<Product>> GetProductsByCategoryName(string categoryName, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetProductsByName(string name, CancellationToken cancellationToken = default);
         Task<bool> IsExistAsync(Guid Id, CancellationToken cancellationToken = default);
        Task<decimal> GetProductPriceAsync(Guid productId, CancellationToken ct);
        Task<int> GetProductQuantityAsync(Guid productId, CancellationToken ct);
        Task SetProductQuantityAsync(Guid productId, int quantity, CancellationToken ct);
        Task<List<Product>> GetAllProductsByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    }
}
