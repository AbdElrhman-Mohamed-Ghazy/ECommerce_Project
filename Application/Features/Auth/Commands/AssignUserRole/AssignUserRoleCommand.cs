using Application.Common.Results;
using MediatR;

namespace Application.Features.Auth.Commands.AssignUserRole
{
    public class AssignUserRoleCommand : IRequest<Result>
    {
        public string UserId { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
    }
}
