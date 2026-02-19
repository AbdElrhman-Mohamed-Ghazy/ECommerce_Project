using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Entities.Categories;
using MediatR;
using System;

using System.Threading.Tasks;
namespace Application.Entities.Category.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWorkRepository unitOfWorkRepository) : IRequestHandler<CreateCategoryCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {

            var category = new  Domain.Entities.Categories.Category
            {
                Id = Guid.NewGuid(),
                Name=  request.CategoryDto.Name,
            };
           await categoryRepository.AddAsync(category, cancellationToken);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
    }
}
