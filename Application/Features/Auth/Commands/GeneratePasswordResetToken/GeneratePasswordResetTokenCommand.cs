using Application.Common.Results;
using MediatR;

namespace Application.Features.Auth.Commands.GeneratePasswordResetToken
{
    public class GeneratePasswordResetTokenCommand : IRequest<Result>
    {
        public string Email { get; init; } = string.Empty;
    }
}
