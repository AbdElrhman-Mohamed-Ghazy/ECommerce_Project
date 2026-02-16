using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Category.Commands.UpdateCategory
{
    public sealed record class UpdateCategoryCommand(Guid Id, CategoryDto CategoryDto):IRequest
    {
    }
}
