using Application.Dtos;
using Application.Entities.Category.Commands.CreateCategory;
using Application.Entities.Category.Commands.DeleteCategory;
using Application.Entities.Category.Commands.UpdateCategory;
using Application.Entities.Category.Queries.GetAllCategories;
using Application.Entities.Category.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Category(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("GetCategoryById/{categoryId:guid}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(Guid categoryId)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(categoryId));
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost("AddCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
        {
            var command = new CreateCategoryCommand(categoryDto);
            var categoryId = await _mediator.Send(command);
            return CreatedAtRoute("GetCategoryById", new { categoryId }, null);
        }

        [HttpPut("UpdateCategorybyId/{categoryId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromBody] CategoryDto categoryDto)
        {
            var command = new UpdateCategoryCommand(categoryId, categoryDto);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("DeleteCategorybyId/{categoryId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var command = new DeleteCategoryCommand(categoryId);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}