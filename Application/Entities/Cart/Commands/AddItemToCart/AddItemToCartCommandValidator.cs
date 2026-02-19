using Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Cart.Commands.AddItemToCart
{
    public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
    {
        private readonly IProductRepository _productRepository;

        public AddItemToCartCommandValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .MustAsync(async (id, ct) =>
                    await _productRepository.IsExistAsync(id, ct))
                .WithMessage("Product does not exist");

            RuleFor(x => x.Quantity)
                .GreaterThan(0);

            RuleFor(x => x)
                .MustAsync(async (command, ct) =>
                {
                    var availableQuantity =
                        await _productRepository.GetProductQuantityAsync(command.ProductId, ct);

                    return command.Quantity <= availableQuantity;
                })
                .WhenAsync(async (command, ct) =>
                    await _productRepository.IsExistAsync(command.ProductId, ct))
                .WithMessage("Insufficient product quantity");
        }
    }
}
