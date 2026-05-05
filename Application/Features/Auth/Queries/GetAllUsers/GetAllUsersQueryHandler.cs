using Application.Common.Results;
using Application.Features.Auth.Models;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IReadOnlyList<UserDto>>>
    {
        private readonly IIdentityService _identityService;

        public GetAllUsersQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<IReadOnlyList<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _identityService.GetAllUsersAsync();
            var result = users
                .Select(u => new UserDto(u.Id, u.Email ?? string.Empty, u.FullName))
                .ToList();

            return Result<IReadOnlyList<UserDto>>.Success(result);
        }
    }
}
