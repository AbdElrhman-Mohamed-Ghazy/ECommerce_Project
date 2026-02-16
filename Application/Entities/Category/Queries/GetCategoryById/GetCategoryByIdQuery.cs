using Application.Dtos;
using MediatR;

namespace Application.Entities.Category.Queries.GetCategoryById
{
    public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto?>
    {
    }
}