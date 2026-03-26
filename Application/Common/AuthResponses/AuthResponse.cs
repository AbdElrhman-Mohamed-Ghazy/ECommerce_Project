using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.AuthResponses
{
    public class AuthResponse
    {
        public bool IsSuccess { get; set; }
        public string AccessToken { get; set; }=string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Message { get; set; }=string.Empty;
    }
}
