using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechSouq.Application;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;
using TechSouq.API.Extensions;

namespace TechSouq.API.Controllers
{
    [Authorize(Roles = "Customer,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IAuthorizationService _authService;
        public UsersController(UserService userService, IAuthorizationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetUser(int id)
        {
            var authResult = await _authService.AuthorizeAsync(User, id, "OwnerOnly");


            if (!authResult.Succeeded)
            {
                return StatusCode(403, new { message = "You are not allowed to modify this account." });
            }



            var result = await _userService.GetUserById(id);
            return this.ToHttpResponse(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUser(UserDto dto)
        {
            var authResult = await _authService.AuthorizeAsync(User, dto.Id, "OwnerOnly");


            if (!authResult.Succeeded)
            {
                return StatusCode(403, new { message = "You are not allowed to modify this account." });
            }


            var result = await _userService.UpdateUser(dto);
            return this.ToHttpResponse(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var authResult = await _authService.AuthorizeAsync(User, id, "OwnerOnly");


            if (!authResult.Succeeded)
            {
                return StatusCode(403, new { message = "You are not allowed to modify this account." });
            }

            var result = await _userService.DeleteUser(id);
            return this.ToHttpResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllCustomersPaged")]
        public async Task<IActionResult> GetAllCustomersPaged(int pageNumber, int pageSize, string? EmailSearch)
        {
            var result = await _userService.GetAllCustomersPaged(pageNumber, pageSize, EmailSearch);

            return this.ToHttpResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetCustomerDetails")]
        public async Task<IActionResult> GetCustomerDetails(int customerId)
        {
            var result = await _userService.GetCustomerDetails(customerId);

            return this.ToHttpResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("IsActive")]
        public async Task<IActionResult> IsActive(int customerId)
        {
            var result = await _userService.IsActive(customerId);

            return this.ToHttpResponse(result);
        }
    }
}