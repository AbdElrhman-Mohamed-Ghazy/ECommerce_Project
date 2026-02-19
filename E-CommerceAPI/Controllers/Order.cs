using Application.Dtos;
using Application.Entities.Order.Commands.CreateOrder;
using Application.Entities.Product.Commands.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Order(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPost("AddOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddOrder(Guid userId,  string Address)
        {
            var command = new CreateOrderCommand(userId, Address);
            var order = await _mediator.Send(command);
            return Ok(order);
           // return CreatedAtRoute("GetProductById", new { productId }, null);
        }
    }
}
