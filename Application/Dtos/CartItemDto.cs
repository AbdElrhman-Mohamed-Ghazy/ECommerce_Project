namespace Application.Dtos
{
    public partial class CartDto
    {
        public class CartItemDto
        {
            public Guid ProductId { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
        }

    }
}
