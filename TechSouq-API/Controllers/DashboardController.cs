using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application.interfaces;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _DashboardService;
        public DashboardController(DashboardService DashboardService)
        {
            _DashboardService = DashboardService;

        }

        [HttpGet("RecentSales")]
        public async Task<IActionResult> RecentSales([FromQuery] RecentSaleceQueryParams queryParams)
        {
            var result = await _DashboardService.RecentSales(queryParams);

            return this.ToHttpResponse(result);
        }

        [HttpGet("ShowDashboardInfo")]
        public async Task<IActionResult> ShowDashboardInfo()
        {
            var result = await _DashboardService.ShowDashboardInfo();

            return this.ToHttpResponse(result);
        }

        [HttpGet("SalesLast7Days")]
        public async Task<IActionResult> SalesLast7Days()
        {
            var result = await _DashboardService.SalesLast7Days();

            return this.ToHttpResponse(result);
        }

        [HttpGet("BestSellingProducts")]
        public async Task<IActionResult> BestSellingProducts()
        {
            var result = await _DashboardService.BestSellingProducts();

            return this.ToHttpResponse(result);
        }



    }
}
