using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos;
using Domain.Entities.CartItems;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Dtos.CartDto;

namespace Application.Entities.Cart.Queries.GetCartByUserId
{
    public sealed class GetCartInfoByUserIdHandler(ICartRepository cartRepository) : IRequestHandler<GetCartInfoByUserIdCommand, CartDto>
    {
        public async Task<CartDto> Handle(GetCartInfoByUserIdCommand request, CancellationToken cancellationToken)
        {
            var cart = await cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (cart is null)
                throw new NotFoundException(nameof(Cart));
            if (cart.Items.Count == 0)
                throw new NotFoundException(nameof(cart.Items));
            var cartItems = cart.Items.Select(i => new CartItemDto
            {
                ProductId = i.ProductId,
                UnitPrice = i.UnitPrice,
               Quantity = i.Quantity
            }).ToList();

            return new CartDto
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(i => i.UnitPrice * i.Quantity),
                TotalQuantity = cartItems.Sum(i => i.Quantity)
            };
        }

    }
}
