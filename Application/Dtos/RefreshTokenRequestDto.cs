using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class RefreshTokenRequestDto
    {
            public string RefreshToken { get; set; }=string.Empty;  
            public string Email { get; set; }= string.Empty;
     }
}
