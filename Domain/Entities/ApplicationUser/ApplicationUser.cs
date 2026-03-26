using Application.Entities.Token;
using Domain.Entities.Orders;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ApplicationUser
{

    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public Cart Cart { get; set; } = null!;

    }
}
