using Application.Entities.Token;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _context;

        public TokenService(AppDbContext context)
        {
            _context = context;
        }

        public async Task StoreRefreshTokenAsync(string userId, string refreshToken, DateTime expiresAt)
        {
            var entity = new RefreshToken
            {
                RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                Expires = expiresAt,
                UserId = userId,
                IsRevoked = false
            };

            _context.RefreshTokens.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RotateRefreshTokenAsync(string userId, string refreshToken, string newRefreshToken, DateTime expiresAt)
        {
            var existingTokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked && t.Expires > DateTime.UtcNow)
                .ToListAsync();

            var matchingToken = existingTokens
                .FirstOrDefault(t => BCrypt.Net.BCrypt.Verify(refreshToken, t.RefreshTokenHash));

            if (matchingToken == null)
            {
                return false;
            }

            matchingToken.IsRevoked = true;
            matchingToken.Expires = DateTime.UtcNow;

            _context.RefreshTokens.Add(new RefreshToken
            {
                RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken),
                Expires = expiresAt,
                UserId = userId,
                IsRevoked = false
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RevokeUserRefreshTokensAsync(string userId)
        {
            var activeTokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked && t.Expires > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.Expires = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
