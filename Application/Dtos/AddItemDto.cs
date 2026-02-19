namespace ECommerceAPI.Controllers
{
    public class AddItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}