using Application.Dtos;
using Application.Entities.Cart.Commands.AddItemToCart;
using Application.Entities.Cart.Commands.ClearCart;
using Application.Entities.Cart.Commands.CreateCart;
using Application.Entities.Cart.Commands.RemoveItemFromCart;
using Application.Entities.Cart.Commands.UpdateCartItemQuantity;
using Application.Entities.Cart.Queries.GetCartByUserId;
using Domain.Entities.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cart(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("CreateCart/{userId:guid}", Name = "CreateCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCart(Guid userId)
        {
             await _mediator.Send(new CreateCartCommand(userId));
            return CreatedAtRoute("GetCartByUserId", new { userId = userId }, null);
        }

        [HttpGet("GetCartByUserId/{userId:guid}", Name = "GetCartByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCartByUserId(Guid userId)
        {
            var result = await _mediator.Send(new GetCartInfoByUserIdCommand(userId));
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost("AddItemToCart/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AddItemToCart( Guid userId, [FromBody] AddItemDto dto)
        {
            await _mediator.Send(new AddItemToCartCommand(userId, dto.ProductId, dto.Quantity));
            return NoContent();
        }

        [HttpPut("UpdateItemQuantity/{userId:guid}/{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItemQuantity([FromRoute] Guid userId , [FromRoute] Guid productId, [FromBody] int Quantity)
        {
            var updateCommand = new UpdateCartItemQuantityCommand(userId, productId, Quantity);
            await _mediator.Send(updateCommand);
            return NoContent();
        }

        [HttpDelete("RemoveItemFromCart/{userId:guid}/{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveItemFromCart(Guid userId, Guid productId)
        {
            var command = new RemoveItemFromCartCommand(userId, productId);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("ClearCart/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ClearCart(Guid userId)
        {
            var command = new ClearCartCommand(userId);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}