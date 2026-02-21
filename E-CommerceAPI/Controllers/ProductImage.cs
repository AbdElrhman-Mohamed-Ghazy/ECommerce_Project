using Application.Dtos;
using Application.Entities.ProductImage.Commands.AddProductImage;
using Application.Entities.ProductImage.Commands.DeleteProductImage;
using Application.Entities.ProductImage.Queries.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/products/images")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{productId}")]
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
        public async Task<IActionResult> GetImages(Guid productId)
        {
            return Ok(await _mediator.Send(
                new GetProductImagesQuery(productId)));
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(Guid imageId)
        {
            await _mediator.Send(
                new DeleteProductImageCommand(imageId));

            return Ok("Image deleted");
        }
    }



}
