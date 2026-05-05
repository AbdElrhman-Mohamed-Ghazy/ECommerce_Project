using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        Task StoreRefreshTokenAsync(string userId, string refreshToken, DateTime expiresAt);
        Task<bool> RotateRefreshTokenAsync(string userId, string refreshToken, string newRefreshToken, DateTime expiresAt);
        Task RevokeUserRefreshTokensAsync(string userId);
    }
}
