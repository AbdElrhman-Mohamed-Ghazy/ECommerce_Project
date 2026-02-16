using Application.Dtos;
using Application.Entities.Product.Commands.CreateProduct;
using Application.Entities.Product.Commands.DeleteProduct;
using Application.Entities.Product.Commands.UpdateProduct;
using Application.Entities.Product.Queries.GetAllProductsList;
using Application.Entities.Product.Queries.GetProductById;
using Application.Entities.Product.Queries.GetProductsByCategory;
using Application.Entities.Product.Queries.GetProductsByName;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _mediator.Send(new GetAllProductsListQuery());
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("GetProductById/{productId:guid}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(productId));
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("GetProductByName/", Name = "GetProductByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductByName(string productName)
        {
            var result = await _mediator.Send(new GetProductsByNameQuery(productName));
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("by-GetProductByCategoryName/", Name = "GetProductByCategoryName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductByCategory(string categoryName)
        {
            var result = await _mediator.Send(new GetProductsByCategoryQuery(categoryName));
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            var command = new CreateProductCommand(productDto);
            var productId = await _mediator.Send(command);
            return CreatedAtRoute("GetProductById", new { productId }, null);
        }

        [HttpPut("UpdateProductbyId/{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] ProductDto productDto)
        {
            var command = new UpdateProductCommand(productId, productDto);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("DeleteProductbyId/{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid productId)
        {
            var command = new DeleteProductCommand(productId);
            await _mediator.Send(command);
            return NoContent();
        }
    }

}

