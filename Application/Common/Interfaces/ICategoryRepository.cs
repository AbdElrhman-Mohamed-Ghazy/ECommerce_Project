using Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public  Task<bool> IsExistAsync(Guid categoryId, CancellationToken cancellationToken = default);
    }
}
