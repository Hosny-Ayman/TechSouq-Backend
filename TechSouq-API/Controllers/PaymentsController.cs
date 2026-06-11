using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.V2.Core;
using System.IO;
using System.Threading.Tasks;
using TechSouq.API.Extensions;
using TechSouq.Application.Dtos;
using TechSouq.Application.Queries;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly IConfiguration _configuration;
        private readonly IOrderQuery _orderConfirmService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            PaymentService paymentService,
            IConfiguration configuration,
            IOrderQuery orderConfirmService,
            ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _configuration = configuration;
            _orderConfirmService = orderConfirmService;
            _logger = logger;
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

        [HttpPost("webhook")]
        [AllowAnonymous] 
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _configuration["StripeSettings:WebhookSecret"] 
                );

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    var userId = int.Parse(paymentIntent.Metadata["UserId"]);
                    var cartId = int.Parse(paymentIntent.Metadata["CartId"]);

                    var confirmOrderDto = new ConfirmOrderDto
                    {
                        ShippingStreet = paymentIntent.Metadata["ShippingStreet"],
                        ShippingCity = paymentIntent.Metadata["ShippingCity"],
                        ShippingFullName = paymentIntent.Metadata["ShippingFullName"],
                        Code = paymentIntent.Metadata["Code"],
                        PaymentWayId =int.Parse( paymentIntent.Metadata["PaymentWayId"]),
                        Phone = paymentIntent.Metadata["Phone"],
                        Email = paymentIntent.Metadata["Email"],
                        Building = paymentIntent.Metadata["Building"],
                        Country = paymentIntent.Metadata["Country"],
                        PaymentIntentId  = paymentIntent.PaymentMethodId,

                    };

                    await _orderConfirmService.ConfirmOrderAsync(confirmOrderDto, cartId, userId);

                    _logger.LogInformation("Webhook: Order Placed Successfully for User: {userId}", userId);
                }

                return Ok(); 
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Webhook error occurred");
                return BadRequest();
            }
        }
    }
}