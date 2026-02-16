using Application.Common.Interfaces;
using Application.Entities.Category.Commands.CreateCategory;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Category.Commands.CreateProduct
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {

        public CreateCategoryCommandValidator( )
        {
     
            RuleFor(x=>x.CategoryDto.Name).NotEmpty().WithMessage("Name is required");
        }
    }
}
