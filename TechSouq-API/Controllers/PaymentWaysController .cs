using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    [ApiController]
    public class PaymentWaysController : ControllerBase
    {
        private readonly PaymentWayService _paymentWayService;

        public PaymentWaysController(PaymentWayService paymentWayService)
        {
            _paymentWayService = paymentWayService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(PaymentWayDto dto)
        {
            var result = await _paymentWayService.Add(dto);

            return this.ToHttpResponse(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(PaymentWayDto dto)
        {
            var result = await _paymentWayService.Update(dto);

            return this.ToHttpResponse(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _paymentWayService.Delete(id);

            return this.ToHttpResponse(result);
        }

        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _paymentWayService.GetPaymentWayByName(name);

            return this.ToHttpResponse(result);
        }

    }
}
