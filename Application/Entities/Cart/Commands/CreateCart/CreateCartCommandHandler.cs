using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Cart.Commands.CreateCart
{
    public sealed class CreateCartCommandHandler(ICartRepository repository, IUnitOfWorkRepository unitOfWorkRepository) : IRequestHandler<CreateCartCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
          var cart=new Domain.Entities.Cart(request.UserId);
           
           await repository.AddAsync(cart, cancellationToken);
            await unitOfWorkRepository.SaveChangesAsync(cancellationToken);
            return cart.Id;
        }
    }
}
