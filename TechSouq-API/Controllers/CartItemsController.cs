using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {

        private readonly CartItemService _CartItemService;

        public CartItemsController(CartItemService cartItemService)
        {
            _CartItemService = cartItemService;
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItemToCart(CartItemDto CartItem)
        {
            var userId = User.GetUserId();
            var result = await _CartItemService.AddCartItem(userId, CartItem);
            return this.ToHttpResponse(result);
        }

        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateCartItem(CartItemDto cartItem)
        {
            var userId = User.GetUserId();
            var result = await _CartItemService.UpdateCartItem(userId, cartItem);
            return this.ToHttpResponse(result);
        }

        [HttpDelete("RemoveItem")]
        public async Task<IActionResult> RemoveCartItem(int productId)
        {
            var userId = User.GetUserId();
            var result = await _CartItemService.RemoveCartItem(userId, productId);
            return this.ToHttpResponse(result);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = User.GetUserId();
            var result = await _CartItemService.GetCartItems(userId);

            return this.ToHttpResponse(result);
        }

        //[HttpPost("Creat")]
        //public async Task<IActionResult> CreateItemCart(CartItemDto CartItem)
        //{
        //    var result = await _CartItemService.AddCartItem(CartItem);

        //    return this.ToHttpResponse(result);
        //}



        //[HttpPut("Update")]
        //public async Task<IActionResult> UpdateCartItems(List<CartItemDto> cartItems)
        //{
        //    var result = await _CartItemService.UpdateCartItems(cartItems);

        //    return this.ToHttpResponse(result);
        //}

        //[HttpDelete("Delete")]
        //public async Task<IActionResult> DeleteCartItems(int CartId)
        //{
        //    var result = await _CartItemService.DeleteCartItem(CartId);

        //    return this.ToHttpResponse(result);
        //}
    }
}
