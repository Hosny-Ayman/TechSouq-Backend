using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.Tasks;
using TechSouq.API.Extensions;
using TechSouq.Application;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Expires = DateTime.UtcNow.AddDays(7), 
                Secure = true,
                SameSite = SameSiteMode.None 
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        private void SetAccessTokenInCookie(string accessToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("accessToken", accessToken, cookieOptions);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.Register(dto);
            return this.ToHttpResponse(result);
        }

        [EnableRateLimiting("StrictAuth")]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.Login(dto);

            if (result.IsSuccess)
            {
                SetRefreshTokenInCookie(result.Data.RefreshToken);
                SetAccessTokenInCookie(result.Data.AccessToken);

                return Ok(OperationResult<object>.Success(dto.Email));
            }

            return this.ToHttpResponse(result);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken() 
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var accessToken = Request.Cookies["accessToken"]; 

            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized(new { message = "Tokens are missing." });
            }

            var tokenDto = new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };
            var result = await _authService.RefreshToken(tokenDto);

            if (result.IsSuccess)
            {
                SetRefreshTokenInCookie(result.Data.RefreshToken);
                SetAccessTokenInCookie(result.Data.AccessToken);

                return Ok(OperationResult<object>.Success("Token refreshed successfully."));
            }

            return this.ToHttpResponse(result);
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                    ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);
            var result = await _authService.Logout(userId);

            Response.Cookies.Delete("refreshToken");
            Response.Cookies.Delete("accessToken");

            return this.ToHttpResponse(result);
        }


    }
}