using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Entities.Cart.Commands.RemoveItemFromCart
{
    public sealed class RemoveItemFromCartCommandValidator : AbstractValidator<RemoveItemFromCartCommand>
    {
        public RemoveItemFromCartCommandValidator(IProductRepository productRepository)
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product is required").MustAsync(async (id, ct) =>
         await productRepository.IsExistAsync(id, ct)).WithMessage("Product does not exist");
        }
    }
}