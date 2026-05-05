using Application.Features.Auth.Commands.AssignUserRole;
using Application.Features.Auth.Commands.ConfirmEmail;
using Application.Features.Auth.Commands.GeneratePasswordResetToken;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Logout;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.RegisterUser;
using Application.Features.Auth.Commands.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace ECommerceAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return Ok(result.Value);
        }

        [HttpPost("login")]
        [EnableRateLimiting("AuthLimiter")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess) return Unauthorized(result.Errors);
            return Ok(result.Value);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand refreshToken)
        {
            var result = await _mediator.Send(refreshToken);
            if (!result.IsSuccess) return Unauthorized(result.Errors);
            return Ok(result.Value);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return Ok();
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var decodedToken = DecodeToken(token);
            var result = await _mediator.Send(new ConfirmEmailCommand { UserId = userId, Token = decodedToken });

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok("Email confirmed successfully");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] GeneratePasswordResetTokenCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok("Check your email for reset link");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request)
        {
            var normalized = new ResetPasswordCommand
            {
                Email = DecodeToken(request.Email),
                Token = DecodeToken(request.Token),
                NewPassword = request.NewPassword
            };

            var result = await _mediator.Send(normalized);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok("Password changed successfully");
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignUserRoleCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return Ok();
        }

        private static string DecodeToken(string token)
        {
            try
            {
                var decoded = WebEncoders.Base64UrlDecode(token);
                return Encoding.UTF8.GetString(decoded);
            }
            catch
            {
                return token;
            }
        }
    }
}
