using Domain.Entities.ApplicationUser;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailConfirmationAsync(ApplicationUser user, string token);
        Task SendPasswordResetAsync(ApplicationUser user, string token);
    }
}
