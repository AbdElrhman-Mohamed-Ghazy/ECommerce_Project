using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Category.Commands.CreateCategory
{
    public sealed record  CreateCategoryCommand(CategoryDto CategoryDto) :IRequest<Guid>
    {
    }
}
