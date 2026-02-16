using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Entities.Categories;
using MediatR;

namespace Application.Entities.Category.Queries.GetAllCategories
{
    public sealed class GetAllCategoriesQueryHandler(ICategoryRepository repository) : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
    {
        public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await repository.GetAllAsync(cancellationToken);
            if (categories == null || !categories.Any())
            {
                throw new NotFoundException(nameof(Category));
            }

            return categories.Select(c => new CategoryDto
            {
                categoryId = c.Id,
                Name = c.Name
            }).ToList();
        }
    }
}