using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryMethodsController : ControllerBase
    {
        private readonly DeliveryMethodService _deliveryMethodService;

        public DeliveryMethodsController(DeliveryMethodService deliveryMethodService)
        {
            _deliveryMethodService = deliveryMethodService;
        }

        [HttpPost("Create")]

        public async Task<IActionResult> CreateDeliveryMethod(DeliveryMethodDto deliveryMethodDto)
        {
            var result = await _deliveryMethodService.AddDeliveryMethod(deliveryMethodDto);

            return this.ToHttpResponse(result);

        }
        [Authorize(Roles = "Customer")]
        [HttpGet("Get")]
        public async Task<IActionResult> GetDeliveryMethod(int DeliveryMethodId)
        {
            var result = await _deliveryMethodService.GetDeliveryMethod(DeliveryMethodId);

            return this.ToHttpResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDeliveryMethod(DeliveryMethodDto deliveryMethodDto)
        {
            var result = await _deliveryMethodService.UpdateDeliveryMethod(deliveryMethodDto);

            return this.ToHttpResponse(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteDeliveryMethod(int deliveryMethodId)
        {
            var result = await _deliveryMethodService.DeleteDeliveryMethod(deliveryMethodId);

            return this.ToHttpResponse(result);
        }


    }
}
