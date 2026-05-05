using Application.Interfaces;
using Domain.Entities.ApplicationUser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Infrastructure.Services.Email
{
    public class FakeEmailService : IEmailService
    {
        private readonly ILogger<FakeEmailService> _logger;
        private readonly IConfiguration _configuration;

        public FakeEmailService(ILogger<FakeEmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Task SendEmailConfirmationAsync(ApplicationUser user, string token)
        {
            var url = BuildLink("/api/account/confirm-email", user.Id, token);
            _logger.LogInformation("Email confirmation link for {Email}: {Url}", user.Email, url);
            return Task.CompletedTask;
        }

        public Task SendPasswordResetAsync(ApplicationUser user, string token)
        {
            var url = BuildResetLink("/api/account/reset-password", user.Email ?? string.Empty, token);
            _logger.LogInformation("Password reset link for {Email}: {Url}", user.Email, url);
            return Task.CompletedTask;
        }

        private string BuildLink(string path, string userId, string token)
        {
            var baseUrl = _configuration["App:BaseUrl"] ?? "https://localhost:5001";
            var encodedToken = Base64UrlEncode(token);
            return $"{baseUrl}{path}?userId={userId}&token={encodedToken}";
        }

        private string BuildResetLink(string path, string email, string token)
        {
            var baseUrl = _configuration["App:BaseUrl"] ?? "https://localhost:5001";
            var encodedToken = Base64UrlEncode(token);
            var encodedEmail = Base64UrlEncode(email);
            return $"{baseUrl}{path}?email={encodedEmail}&token={encodedToken}";
        }

        private static string Base64UrlEncode(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}
