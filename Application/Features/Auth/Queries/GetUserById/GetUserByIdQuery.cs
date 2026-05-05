using Application.Common.Results;
using Application.Features.Auth.Models;
using MediatR;

namespace Application.Features.Auth.Queries.GetUserById
{
    public   sealed record GetUserByIdQuery(string UserId) : IRequest<Result<UserDto>>
    { }
}
