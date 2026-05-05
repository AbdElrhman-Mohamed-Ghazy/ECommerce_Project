using Application.Common.Results;
using Application.Features.Auth.Models;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthTokensDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtService _jwtService;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(IIdentityService identityService, IJwtService jwtService, ITokenService tokenService)
        {
            _identityService = identityService;
            _jwtService = jwtService;
            _tokenService = tokenService;
        }

        public async Task<Result<AuthTokensDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result<AuthTokensDto>.Failure("Invalid email or password.");
            }

            var roles = await _identityService.GetRolesAsync(user);
            var accessToken = await _jwtService.GenerateAccessTokenAsync(user, roles);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            var rotated = await _tokenService.RotateRefreshTokenAsync(user.Id, request.RefreshToken, newRefreshToken, DateTime.UtcNow.AddDays(7));
            if (!rotated)
            {
                return Result<AuthTokensDto>.Failure("Invalid refresh token.");
            }

            return Result<AuthTokensDto>.Success(new AuthTokensDto(accessToken, newRefreshToken));
        }
    }
}
