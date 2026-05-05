using Application.Common.Results;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public ConfirmEmailCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure("User not found.");
            }

            var confirmed = await _identityService.ConfirmEmailAsync(user, request.Token);
            return confirmed ? Result.Success() : Result.Failure("Invalid email confirmation token.");
        }
    }
}
