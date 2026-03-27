using Application.Dtos;
using Application.Entities.ProductImage.Commands.AddProductImage;
using Application.Entities.ProductImage.Commands.DeleteProductImage;
using Application.Entities.ProductImage.Queries.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/products/images")]
    [Authorize(Roles = "Admin")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImages(Guid productId,
            [FromForm] List<IFormFile> images)
        {
            var dtos = new List<ImageFileDto>();

            foreach (var img in images)
            {
                using var ms = new MemoryStream();
                await img.CopyToAsync(ms);

                dtos.Add(new ImageFileDto(
                    ms.ToArray(),
                    img.FileName,
                    img.ContentType));
            }

            await _mediator.Send(
                new AppProductImageCommand(productId, dtos));

            return Ok("Images uploaded");
        }


        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetImages(Guid productId)
        {
            return Ok(await _mediator.Send(
                new GetProductImagesQuery(productId)));
        }

        [HttpDelete("{imageId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteImage(Guid imageId)
        {
            await _mediator.Send(
                new DeleteProductImageCommand(imageId));

            return Ok("Image deleted");
        }
    }



}
