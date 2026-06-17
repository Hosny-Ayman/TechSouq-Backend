using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Stripe;
using TechSouq.API.Extensions;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;
using TechSouq.Domain.Entities;

namespace TechSouq.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryZonesController : ControllerBase
    {
        private readonly DeliveryZoneService _deliveryZoneService;
        public DeliveryZonesController(DeliveryZoneService deliveryZoneService)
        { 
            _deliveryZoneService = deliveryZoneService;
        
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> AddDeliveryZone(DeliveryZoneDto DeliveryZoneDto)
        { 
            var result = await _deliveryZoneService.AddDeliveryZone(DeliveryZoneDto);
            return this.ToHttpResponse(result);
        }

        [EnableRateLimiting("GeneralRule")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllDeliveryZones()
        {          
            var result = await _deliveryZoneService.GetAllDeliveryZones();
            return this.ToHttpResponse(result);
        }

        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("GetOnlyShippingCost")]
        public async Task<IActionResult> GetOnlyShippingCost(string ShippingCity)
        {
            var result = await _deliveryZoneService.GetOnlyShippingCost(ShippingCity);
            return this.ToHttpResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllDeliveryZonesPaged")]
        public async Task<IActionResult> GetAllDeliveryZonesPaged(int pageNumber, int pageSize, string? NameSearch)
        {
            var result = await _deliveryZoneService.GetAllDeliveryZonesPaged(pageNumber, pageSize, NameSearch);

            return this.ToHttpResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateDeliveryZone")]
        public async Task<IActionResult> UpdateDeliveryZone(DeliveryZone DeliveryZone)
        {
            var result = await _deliveryZoneService.UpdateDeliveryZone(DeliveryZone);

            return this.ToHttpResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteDeliveryZone")]
        public async Task<IActionResult> DeleteDeliveryZone(int DeliveryZoneId)
        {
            var result = await _deliveryZoneService.DeleteDeliveryZone(DeliveryZoneId);

            return this.ToHttpResponse(result);
        }

    }
}
