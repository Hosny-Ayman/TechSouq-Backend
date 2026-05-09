using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class PaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public PaymentService(ILogger<PaymentService> logger, ICartRepository cartRepository, ICartItemRepository cartItemRepository) 
        {
            _logger = logger;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;

        }

        public async Task<OperationResult<string>> CreatePaymentIntent(int userId)
        {

            var cart = await _cartRepository.GetCartIdbyUserId(userId);

            if(cart==null)
            {
                _logger.LogWarning("User Is not Have any card userid:{userId}", userId);
                return OperationResult<string>.NotFound("User Is not Have any card");
            }

            var totalAmount = await _cartItemRepository.GetCartItemsTotalAmounts(cart.Id);

            if (totalAmount <= 0)
            {
                _logger.LogWarning("Cart total amount is zero for userId: {userId}", userId);
                return OperationResult<string>.BadRequest("Cary is Empty");
            }

            var amountInCents = (long)(totalAmount * 100);

            var options = new PaymentIntentCreateOptions
            {
                Amount = amountInCents,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };

            var service = new PaymentIntentService();
            PaymentIntent intent = await service.CreateAsync(options);

            _logger.LogInformation("Create Payment With Amount:{totalAmount}", totalAmount);
            return OperationResult<string>.Success(intent.ClientSecret);

        }

    }
}
