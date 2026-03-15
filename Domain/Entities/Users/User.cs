using Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Domain.Entities.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }=string.Empty;
        public string Email { get; set; }=string.Empty;
        public string PasswordHash { get; set; }=string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; }=string.Empty;

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public Cart Cart { get; set; } = null!;
    }
}
