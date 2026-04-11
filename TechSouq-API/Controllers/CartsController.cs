using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using TechSouq.API.Extensions;
using TechSouq.Application;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ApiController]
    public class CartsController : ControllerBase
    {

        private readonly CartService _CartServices;

        public CartsController(CartService cartServices)
        {
            _CartServices = cartServices;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> GetMyCart()
        {
            var userId = User.GetUserId();
            var result = await _CartServices.GetCart(userId);
            return this.ToHttpResponse(result);
        }

        //[HttpDelete("ClearMyCart")]
        //public async Task<IActionResult> ClearCart()
        //{
        //    var userId = User.GetUserId();
        //    var result = await _CartServices.ClearCartByUserId(userId);
        //    return this.ToHttpResponse(result);
        //}

        [HttpGet("Get")]
        public async Task<IActionResult> GetCart(int CartId)
        {
            var result = await _CartServices.GetCart(CartId);

            return this.ToHttpResponse(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCart(CartDto Cart)
        {
            var result = await _CartServices.UpdateCart(Cart);

            return this.ToHttpResponse(result);
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCart(int CartId)
        {
            var result = await _CartServices.DeleteCart(CartId);

            return this.ToHttpResponse(result);
        }

    }
}
