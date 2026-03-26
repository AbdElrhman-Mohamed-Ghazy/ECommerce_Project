using Application.Dtos;
using Domain.Entities.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Application.Common.AuthResponses;
namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponse> LoginAsync(LoginRequestDto request);
        Task<bool> AddUserToRoleAsync(string userId, string role);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequestDto RefreshToken);
          Task<AuthResponse> LogoutAsync(string email);

    }
}
