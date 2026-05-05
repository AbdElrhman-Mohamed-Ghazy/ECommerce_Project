using Application.Common.Results;
using MediatR;

namespace Application.Features.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest<Result>
    {
        public string Email { get; init; } = string.Empty;
    }
}
