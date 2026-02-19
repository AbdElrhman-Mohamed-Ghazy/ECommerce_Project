using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.Order;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Application.Entities.Order.Commands.CreateOrder
{
    public sealed class CreateOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository, ICartRepository cartRepository, IUnitOfWorkRepository unitOfWorkRepository) : IRequestHandler<CreateOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (cart == null || !cart.Items.Any())
            {
                throw new NotFoundException("Cart Items");
            }
            
            var productIds=cart.Items.Select(i => i.ProductId).ToList();
            var products = await productRepository.GetAllProductsByIds(productIds, cancellationToken);

            var order = new Domain.Entities.Order.Order(request.UserId, request.Address);

            using (var trasaction = await unitOfWorkRepository.BeginTransactionAsync(cancellationToken))
            {
                try
                {

                    foreach (var item in cart.Items)
                    {
                        var product = products.FirstOrDefault(p => p.Id == item.ProductId);

                        if (product == null)
                        {
                            throw new NotFoundException($"Product With id {item.ProductId} ");
                        }

                        if (product.StockQuantity < item.Quantity)
                        {
                            throw new ValidationFaliedException($"Insufficient stock for product: {product.Name}");
                        }

                        await productRepository.SetProductQuantityAsync(item.ProductId, item.Quantity *-1, cancellationToken);
                        order.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
                    }



                    await orderRepository.AddAsync(order, cancellationToken);
                    cart.Clear();
                    await cartRepository.UpdateAsync(cart, cancellationToken);
                    await unitOfWorkRepository.SaveChangesAsync(cancellationToken);
                    await trasaction.CommitAsync(cancellationToken);
                    return order.Id;
                }
                catch (Exception)
                {
                    await trasaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}
