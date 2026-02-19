using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Order.Commands.CreateOrder
{
    public sealed class CreateOrderCommandValidator :AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
                RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
        }
    }
}
