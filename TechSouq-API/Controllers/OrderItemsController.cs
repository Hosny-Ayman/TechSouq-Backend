using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {

        private readonly OrderItemService _orderItemService;

        public OrderItemsController(OrderItemService OrderItemService)
        {
            _orderItemService = OrderItemService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrderItem(OrderItemDto orderItemDto)
        {
            var result = await _orderItemService.AddOrderItem(orderItemDto);

            return this.ToHttpResponse(result);

        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetOrderItem(int orderItemId)
        {
            var result = await _orderItemService.GetOrderItem(orderItemId);

            return this.ToHttpResponse(result);

        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateOrderItem(OrderItemDto orderItemDto)
        {
            var result = await _orderItemService.UpdateOrderItem(orderItemDto);

            return this.ToHttpResponse(result);

        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteOrderItem(int OrderItemId)
        {
            var result = await _orderItemService.DeleteOrderItem(OrderItemId);

            return this.ToHttpResponse(result);

        }
    }
}
