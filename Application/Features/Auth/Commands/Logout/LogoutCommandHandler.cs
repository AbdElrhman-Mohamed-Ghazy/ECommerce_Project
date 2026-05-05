using Application.Common.Results;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public LogoutCommandHandler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Success();
            }

            await _tokenService.RevokeUserRefreshTokensAsync(user.Id);
            return Result.Success();
        }
    }
}
