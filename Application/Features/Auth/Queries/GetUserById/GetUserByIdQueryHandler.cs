using Application.Common.Results;
using Application.Features.Auth.Models;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IIdentityService _identityService;

        public GetUserByIdQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Result<UserDto>.Failure("User not found.");
            }

            return Result<UserDto>.Success(new UserDto(user.Id, user.Email ?? string.Empty, user.FullName));
        }
    }
}
