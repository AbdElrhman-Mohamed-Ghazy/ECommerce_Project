using Domain.Entities.ApplicationUser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, IReadOnlyCollection<string> Errors)> CreateUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<ApplicationUser?> FindByIdAsync(string userId);
        Task<IReadOnlyList<ApplicationUser>> GetAllUsersAsync();
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IReadOnlyList<string>> GetRolesAsync(ApplicationUser user);
        Task<bool> RoleExistsAsync(string role);
        Task<bool> CreateRoleAsync(string role);
        Task<bool> AddToRoleAsync(ApplicationUser user, string role);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<bool> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<(bool Succeeded, IReadOnlyCollection<string> Errors)> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
    }
}
