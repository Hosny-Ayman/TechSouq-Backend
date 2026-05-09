using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [Authorize]
        [HttpPost("CreateIntent")]
        public async Task<IActionResult> CreatePaymentIntent()
        {

            var userId = User.GetUserId();
            

            var result = await _paymentService.CreatePaymentIntent(userId);

            return this.ToHttpResponse(result);
        }

    }
}
