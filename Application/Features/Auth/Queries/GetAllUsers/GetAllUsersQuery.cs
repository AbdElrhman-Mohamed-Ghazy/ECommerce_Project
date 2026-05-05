using Application.Common.Results;
using Application.Features.Auth.Models;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Auth.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<Result<IReadOnlyList<UserDto>>>
    {
    }
}
