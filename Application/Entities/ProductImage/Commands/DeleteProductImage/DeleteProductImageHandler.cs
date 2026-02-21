using Application.Common.Interfaces;
using MediatR;

namespace Application.Entities.ProductImage.Commands.DeleteProductImage
{
    public class DeleteProductImageHandler(
        IProductImageRepository imageRepo,
        IUnitOfWorkRepository unitOfWork) : IRequestHandler<DeleteProductImageCommand>
    {
        public async Task Handle(DeleteProductImageCommand request, CancellationToken ct)
        {
            var image = await imageRepo.GetByIdAsync(request.ImageId, ct);
            if (image == null)
                throw new KeyNotFoundException("Image");


            if (File.Exists(image.FilePath))
                File.Delete(image.FilePath);

  
            imageRepo.Delete(image);
            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}