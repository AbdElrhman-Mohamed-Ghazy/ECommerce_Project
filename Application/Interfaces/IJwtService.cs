using Domain.Entities.ApplicationUser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user, IReadOnlyCollection<string> roles);
        string GenerateRefreshToken();
    }
}
