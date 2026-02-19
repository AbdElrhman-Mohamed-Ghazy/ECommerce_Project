using Domain.Entities.CartItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }

        private readonly List<CartItem> _items = new();
        public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

        private Cart() { }

        public Cart(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
        }

        
        public void AddItem(Guid productId, decimal price, int quantity)
        {
            var item = _items.FirstOrDefault(x => x.ProductId == productId);

            if (item is null)
                _items.Add(new CartItem(productId, price, quantity));
            else
                item.Increase(quantity);
        }

        public void UpdateQuantity(Guid productId, int quantity)
        {
            var item = GetItem(productId);
            item.SetQuantity(quantity);
        }

        public void RemoveItem(Guid productId)
        {
            var item = GetItem(productId);
            _items.Remove(item);
        }

        public void Clear() => _items.Clear();

        private CartItem GetItem(Guid productId)
            => _items.First(x => x.ProductId == productId);
    }

}
