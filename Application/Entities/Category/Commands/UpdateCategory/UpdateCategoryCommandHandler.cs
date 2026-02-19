using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Category.Commands.UpdateCategory
{
    public sealed class UpdateCategoryCommandHandler(ICategoryRepository repository, IUnitOfWorkRepository unitOfWorkRepository) : IRequestHandler<UpdateCategoryCommand>
    {
        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
            {
                throw new NotFoundException(nameof(category));
            }
            category.Name = request.CategoryDto.Name;

            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
