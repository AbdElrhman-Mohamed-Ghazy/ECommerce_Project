using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Order
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Order(Guid userId,  string shippingAddress)
        {
            UserId = userId;
            Id  = Guid.NewGuid();
            Status = OrderStatus.Pending;
            ShippingAddress = shippingAddress;
        }
        public OrderStatus Status { get; set; }

        public decimal TotalPrice { get; set; }
        public void AddItem(Guid productId, int quantity, decimal unitPrice)
        {
            var items = new OrderItem(productId, quantity, unitPrice);
            items.Order = this;
            _items.Add(items);
            TotalPrice += items.Quantity * items.UnitPrice;


        }
        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items;
    }
}
