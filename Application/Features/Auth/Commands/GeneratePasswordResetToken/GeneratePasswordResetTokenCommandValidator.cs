using FluentValidation;

namespace Application.Features.Auth.Commands.GeneratePasswordResetToken
{
    public class GeneratePasswordResetTokenCommandValidator : AbstractValidator<GeneratePasswordResetTokenCommand>
    {
        public GeneratePasswordResetTokenCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
