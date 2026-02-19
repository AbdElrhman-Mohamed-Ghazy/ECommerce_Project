using Application.Common.Interfaces;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Cart.Commands.UpdateCartItemQuantity
{
    public sealed class UpdateCartItemQuantityCommandValidator :AbstractValidator<UpdateCartItemQuantityCommand>
    {
        private readonly IProductRepository _productRepository;

        public UpdateCartItemQuantityCommandValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product is required").MustAsync(async (id, ct) =>
           await _productRepository.IsExistAsync(id, ct)).WithMessage("Product does not exist");

            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
