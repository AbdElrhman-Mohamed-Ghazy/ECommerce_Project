using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Category.Commands.DeleteCategory
{
    public sealed record  DeleteCategoryCommand(Guid Id) : IRequest
    {
    }
}
