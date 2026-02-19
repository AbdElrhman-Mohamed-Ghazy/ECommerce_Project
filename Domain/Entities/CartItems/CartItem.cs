using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CartItems
{
    public class CartItem
    {

        public Guid Id { get; set; }
        public Guid CartId {  get; set; }

        public Cart Cart { get; set; } = null!;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public CartItem(Guid productId, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public void Increase(int quantity)
        {
            Quantity += quantity;
        }
        public void SetQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            Quantity = quantity;
        }

    }
}
