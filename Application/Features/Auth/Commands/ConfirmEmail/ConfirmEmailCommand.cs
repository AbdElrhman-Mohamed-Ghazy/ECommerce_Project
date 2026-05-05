using Application.Common.Results;
using MediatR;

namespace Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<Result>
    {
        public string UserId { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
    }
}
