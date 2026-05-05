using Application.Common.Results;
using Application.Features.Auth.Models;
using MediatR;

namespace Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<Result<AuthTokensDto>>
    {
        public string Email { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
    }
}
