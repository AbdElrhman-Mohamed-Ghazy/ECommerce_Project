using Application.Dtos;
using Application.Entities.Order.Commands.ChangeOrderStatus;
using Application.Entities.Order.Commands.CreateOrder;
using Application.Entities.Order.Queries.GetAllOrders;
using Application.Entities.Order.Queries.GetFullOrderInfoById;
using Application.Entities.Order.Queries.GetOrderById;
using Application.Entities.Order.Queries.GetOrdersByUserId;
using Application.Entities.Product.Commands.CreateProduct;
using Application.Entities.Product.Commands.UpdateProduct;
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
        public async Task<IActionResult> AddOrder(Guid userId, string Address)
        {
            var command = new CreateOrderCommand(userId, Address);
            var order = await _mediator.Send(command);
           
             return CreatedAtRoute("GetOrdersByUserId", new { userId = userId }, null);
        }

        [HttpGet("GetOrderById/{orderId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
        {
            var query = new GetOrderByIdQuery(orderId);
            var order = await _mediator.Send(query);
            return Ok(order);
        }

        [HttpGet("GetOrdersByUserId/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrdersByUserId([FromRoute] Guid userId)
        {
            var query = new GetOrdersByUserIdQuery(userId);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        [HttpGet("GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllOrders()
        {
            var query = new GetAllOrdersQuery();
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        [HttpGet("GetFullOrderInfoById/{orderId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFullOrderInfoById([FromRoute] Guid orderId)
        {
            var query = new GetFullOrderInfoByIdCommand(orderId);
            var order = await _mediator.Send(query);
            return Ok(order);
        }


        [HttpPut("UpdateOrderStatus/{orderId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid orderId, [FromBody] UpdateOrderStatusDto orderStatusDto)
        {
            var command = new OrderStatusCommand(orderId, orderStatusDto);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
