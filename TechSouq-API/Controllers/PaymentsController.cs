using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        
        [HttpPost("CreateIntent")]
        public async Task<IActionResult> CreatePaymentIntent(ConfirmOrderDto ShippingCost)
        {

            var userId = User.GetUserId();
            

            var result = await _paymentService.CreatePaymentIntent(ShippingCost, userId);

            return this.ToHttpResponse(result);
        }

        [HttpPost("CreateCash")]
        public async Task<IActionResult> CreatePaymentForCash(ConfirmOrderDto ShippingCost)
        {

            var userId = User.GetUserId();


            var result = await _paymentService.CreatePaymentForCash(ShippingCost, userId);

            return this.ToHttpResponse(result);
        }





    }
}
