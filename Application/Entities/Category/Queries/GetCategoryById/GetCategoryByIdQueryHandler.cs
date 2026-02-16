using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Entities.Categories;
using MediatR;

namespace Application.Entities.Category.Queries.GetCategoryById
{
    public sealed class GetCategoryByIdQueryHandler(ICategoryRepository repository) : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
            {
                throw new NotFoundException(nameof(Category));
            }

            return new CategoryDto
            {
                categoryId = category.Id,
                Name = category.Name
            };
        }
    }
}