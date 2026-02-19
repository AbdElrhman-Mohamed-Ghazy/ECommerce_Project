using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Product.Commands.DeleteProduct
{
    public sealed class DeleteProductCommandHandler(IProductRepository repository, IUnitOfWorkRepository unitOfWorkRepository) : IRequestHandler<DeleteProductCommand>
    {
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        { 
            var product = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
            { 
                throw new NotFoundException(nameof(product)); 
            }

            await repository.DeleteAsync(product, cancellationToken);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
