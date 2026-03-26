using Domain.Entities.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Token
{
    public class RefreshToken
    {
        public Guid refreshTokenId { get; set; }

        public string RefreshTokenHash { get; set; } = string.Empty;

        public DateTime Expires { get; set; }

        public bool IsRevoked { get; set; }

        public ApplicationUser User { get; set; }= null!;

        public string UserId { get; set; } = string.Empty;
    }
}
