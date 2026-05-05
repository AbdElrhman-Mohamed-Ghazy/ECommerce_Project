using Application.Common.Results;
using Application.Features.Auth.Models;
using Application.Interfaces;
using Domain.Entities.ApplicationUser;
using MediatR;

namespace Application.Features.Auth.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;

        public RegisterUserCommandHandler(IIdentityService identityService, IEmailService emailService)
        {
            _identityService = identityService;
            _emailService = emailService;
        }

        public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber
            };

            var createResult = await _identityService.CreateUserAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                return Result<RegisterUserResponse>.Failure(createResult.Errors.ToArray());
            }

            var role = request.Role.Equals("admin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User";
            if (!await _identityService.RoleExistsAsync(role))
            {
                var roleCreated = await _identityService.CreateRoleAsync(role);
                if (!roleCreated)
                {
                    return Result<RegisterUserResponse>.Failure("Failed to create role.");
                }
            }

            var addRoleResult = await _identityService.AddToRoleAsync(user, role);
            if (!addRoleResult)
            {
                return Result<RegisterUserResponse>.Failure("Failed to assign role.");
            }

            var token = await _identityService.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendEmailConfirmationAsync(user, token);

            return Result<RegisterUserResponse>.Success(new RegisterUserResponse("User registered. Check your email for confirmation."));
        }
    }
}
