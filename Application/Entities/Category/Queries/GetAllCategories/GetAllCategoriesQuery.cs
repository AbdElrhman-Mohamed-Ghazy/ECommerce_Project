using Application.Dtos;
using MediatR;

namespace Application.Entities.Category.Queries.GetAllCategories
{
    public sealed record GetAllCategoriesQuery : IRequest<List<CategoryDto>>
    {
    }
}