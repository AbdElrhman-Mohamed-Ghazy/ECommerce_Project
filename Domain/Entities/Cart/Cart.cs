using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Cart
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal Total => CalculateTotal();

        private decimal CalculateTotal()
        { 
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.Quantity * item.UnitPrice;
            }
            return total;
        }
        }
}
