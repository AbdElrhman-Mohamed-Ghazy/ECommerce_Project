using Application.Common.Results;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.AssignUserRole
{
    public class AssignUserRoleCommandHandler : IRequestHandler<AssignUserRoleCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public AssignUserRoleCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure("User not found.");
            }

       
            if (!await _identityService.RoleExistsAsync(request.Role))
            {
                var created = await _identityService.CreateRoleAsync(request.Role);
                if (!created)
                {
                    return Result.Failure("Failed to create role.");
                }
            }

            var added = await _identityService.AddToRoleAsync(user, request.Role);
            return added ? Result.Success() : Result.Failure("Failed to assign role.");
        }
    }
}
