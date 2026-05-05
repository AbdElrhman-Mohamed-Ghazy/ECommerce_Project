using Application.Common.Results;
using Application.Features.Auth.Models;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthTokensDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtService _jwtService;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IIdentityService identityService, IJwtService jwtService, ITokenService tokenService)
        {
            _identityService = identityService;
            _jwtService = jwtService;
            _tokenService = tokenService;
        }

        public async Task<Result<AuthTokensDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result<AuthTokensDto>.Failure("Invalid email or password.");
            }

            var isPasswordValid = await _identityService.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Result<AuthTokensDto>.Failure("Invalid email or password.");
            }

            var roles = await _identityService.GetRolesAsync(user);
            var accessToken = await _jwtService.GenerateAccessTokenAsync(user, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            await _tokenService.StoreRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            return Result<AuthTokensDto>.Success(new AuthTokensDto(accessToken, refreshToken));
        }
    }
}
