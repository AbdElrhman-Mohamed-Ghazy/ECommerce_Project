using Application.Interfaces;
using Domain.Entities.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<(bool Succeeded, IReadOnlyCollection<string> Errors)> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public Task<ApplicationUser?> FindByEmailAsync(string email) => _userManager.FindByEmailAsync(email);

        public Task<ApplicationUser?> FindByIdAsync(string userId) => _userManager.FindByIdAsync(userId);

        public async Task<IReadOnlyList<ApplicationUser>> GetAllUsersAsync() => await _userManager.Users.ToListAsync();

        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password) => _userManager.CheckPasswordAsync(user, password);

        public async Task<IReadOnlyList<string>> GetRolesAsync(ApplicationUser user)
            => (await _userManager.GetRolesAsync(user)).ToList();

        public Task<bool> RoleExistsAsync(string role) => _roleManager.RoleExistsAsync(role);

        public async Task<bool> CreateRoleAsync(string role)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            return result.Succeeded;
        }

        public async Task<bool> AddToRoleAsync(ApplicationUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
            => _userManager.GenerateEmailConfirmationTokenAsync(user);

        public async Task<bool> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
            => _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<(bool Succeeded, IReadOnlyCollection<string> Errors)> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }
    }
}
