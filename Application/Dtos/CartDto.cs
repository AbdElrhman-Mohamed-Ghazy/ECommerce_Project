using Domain.Entities;
using Domain.Entities.CartItems;
using Domain.Entities.Products;

namespace Application.Dtos
{
    public partial class CartDto
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public IReadOnlyCollection<CartItemDto> CartItems { get; set; }= new List<CartItemDto>();
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }

    }
}
