using Application.Common.Results;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.GeneratePasswordResetToken
{
    public class GeneratePasswordResetTokenCommandHandler : IRequestHandler<GeneratePasswordResetTokenCommand, Result>
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;

        public GeneratePasswordResetTokenCommandHandler(IIdentityService identityService, IEmailService emailService)
        {
            _identityService = identityService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(GeneratePasswordResetTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure("Invalid email.");
            }

            var token = await _identityService.GeneratePasswordResetTokenAsync(user);
            await _emailService.SendPasswordResetAsync(user, token);

            return Result.Success();
        }
    }
}
