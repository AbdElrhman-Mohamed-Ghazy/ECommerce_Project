using Application.Common.Interfaces;
using Application.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ECommerceAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _userService.RegisterAsync(request);
            if (!result.IsSuccess) return BadRequest(result.Message);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        [EnableRateLimiting("AuthLimiter")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _userService.LoginAsync(request);

            if (!result.IsSuccess) return Unauthorized(result.Message);
            return Ok(new { AccessToken = result.AccessToken , RefreshToken=result.RefreshToken});
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody]RefreshTokenRequestDto refreshToken)
        {
            var token = await _userService.RefreshTokenAsync(refreshToken);
            return Ok(token);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            var result = await _userService.LogoutAsync(request.Email);

            return Ok(result);
        }
    }
}
