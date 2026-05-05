using Application.Common.Results;
using Application.Features.Auth.Models;
using MediatR;

namespace Application.Features.Auth.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Result<RegisterUserResponse>>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string Role { get; init; } = "User";
        public string FullName { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
    }
}
