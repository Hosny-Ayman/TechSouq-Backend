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
        public async Task<IActionResult> AddItemToCartItem(CartItemDto CartItem)
        {
            var userId = User.GetUserId();
            var result = await _CartItemService.AddCartItem(userId, CartItem);
            return this.ToHttpResponse(result);
        }

        [HttpPost("AddItems")]
        public async Task<IActionResult> AddItemsToCartItem(List<CartItemDto> CartItems)
        {
            var userId = User.GetUserId();
            var result = await _CartItemService.AddCartItems(userId, CartItems);
            return this.ToHttpResponse(result);
        }

        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateCartItem(List<CartItemDto> cartItems)
        {
            var userId = User.GetUserId();
            var result = await _CartItemService.UpdateCartItem(userId, cartItems);
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

        [HttpGet("GetCartItem")]
        public async Task<IActionResult> GetCartItem(int productId)
        {
            var userId = User.GetUserId();
            var result = await _CartItemService.GetCartItemAsync(userId, productId);

            return this.ToHttpResponse(result);
        }


        [HttpGet("GetCartItemesWithProducts")]
        public async Task<IActionResult> GetCartItemesWithProducts()
        {
            var userId = User.GetUserId();
            var result = await _CartItemService.GetAllCartItemsWithProductDetailsAsync(userId);

            return this.ToHttpResponse(result);
        }

        //[HttpPost("Creat")]
        //public async Task<IActionResult> CreateItemCart(CartItemDto CartItems)
        //{
        //    var result = await _CartItemService.AddCartItem(CartItems);

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
