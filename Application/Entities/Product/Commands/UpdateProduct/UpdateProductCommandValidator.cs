using Application.Common.Interfaces;
using Application.Entities.Product.Commands.CreateProduct;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {

        private readonly ICategoryRepository _categoryRepository;
        public UpdateProductCommandValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            RuleFor(x => x.ProductDto.Name)
          .NotEmpty().WithMessage("Product name is required");

            RuleFor(x => x.ProductDto.Price)
                .GreaterThan(0);

            RuleFor(x => x.ProductDto.StockQuantity)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ProductDto.CategoryId).NotEmpty().WithMessage("Category is required").MustAsync(async (id, ct) =>
           await _categoryRepository.IsExistAsync(id, ct)).WithMessage("Category does not exist");

        }
    }
 }

