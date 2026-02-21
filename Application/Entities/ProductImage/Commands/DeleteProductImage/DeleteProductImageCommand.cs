using MediatR;

namespace Application.Entities.ProductImage.Commands.DeleteProductImage
{
    public sealed record DeleteProductImageCommand(Guid ImageId) : IRequest;
}