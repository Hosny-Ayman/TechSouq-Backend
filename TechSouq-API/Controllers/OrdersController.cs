using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;
using TechSouq.Domain.Enums;

namespace TechSouq.API.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _OrderService;

        public OrdersController(OrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [Authorize(Roles = "Customer")] 
        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder(OrderDto OrderDto)
        {
            OrderDto.UserId = User.GetUserId();
            var result = await _OrderService.AddOrder(OrderDto);
            return this.ToHttpResponse(result);
        }

        //[HttpGet("MyOrders")]
        //public async Task<IActionResult> GetMyOrders()
        //{
        //    var userId = User.GetUserId();
        //    var result = await _OrderService.GetOrderById(userId);
        //    return this.ToHttpResponse(result);
        //}

        [HttpGet("Get/{OrderId}")]
        public async Task<IActionResult> GetOrderDetails(int OrderId)
        {
            var userId = User.GetUserId();
            var isAdmin = User.IsInRole("Admin");

            var result = await _OrderService.GetOrderById(OrderId, userId, isAdmin);
            return this.ToHttpResponse(result);
        }

        [HttpGet("GetOrderSummary")]
        public async Task<IActionResult> GetOrderSummary()
        {
            var userId = User.GetUserId();

            var result = await _OrderService.GetOrderSummaryAsync(userId);
            return this.ToHttpResponse(result);
        }

        //[Authorize(Roles = "Admin")] 
        //[HttpPut("UpdateStatus")]
        //public async Task<IActionResult> UpdateOrderStatus(OrderDto OrderDto)
        //{
        //    var result = await _OrderService.UpdateOrder(OrderDto);
        //    return this.ToHttpResponse(result);
        //}

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllOrdersPaged")]
        public async Task<IActionResult> GetAllOrdersPaged(int PageNumber, int PageSize, OrderStatus? status, string? search)
        {
            var result = await _OrderService.GetAllOrdersPaged(PageNumber, PageSize, status, search);
            return this.ToHttpResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrderDtailsAdmin")]
        public async Task<IActionResult> GetOrderDtailsAdmin(int OrderId)
        {
            var result = await _OrderService.GetOrderDtailsAdmin(OrderId);
            return this.ToHttpResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(int OrderId, OrderStatus Status)
        {
            var result = await _OrderService.UpdateStatus(OrderId, Status);
            return this.ToHttpResponse(result);
        }
    }
}