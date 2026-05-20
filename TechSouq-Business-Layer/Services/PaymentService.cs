using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Queries;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class PaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderQueryService _OrderConfirmQueryService;

        public PaymentService(ILogger<PaymentService> logger, ICartRepository cartRepository, IOrderQueryService OrderConfirmQueryService) 
        {
            _logger = logger;
            _cartRepository = cartRepository;
            _OrderConfirmQueryService = OrderConfirmQueryService;

        }

        public async Task<OperationResult<string>> CreatePaymentIntent(ConfirmOrderDto confirmOrderDto, int userId)
        {

            var cart = await _cartRepository.GetCartIdbyUserId(userId);

            if(cart==null)
            {
                _logger.LogWarning("User Is not Have any card userid:{userId}", userId);
                return OperationResult<string>.NotFound("User Is not Have any card");
            }

            var totalAmount = await _OrderConfirmQueryService.ConfirmOrderAsync(confirmOrderDto, cart.Id, userId);
           
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

        public async Task<OperationResult<string>> CreatePaymentForCash(ConfirmOrderDto confirmOrderDto, int userId)
        {

            var cart = await _cartRepository.GetCartIdbyUserId(userId);

            if (cart == null)
            {
                _logger.LogWarning("User Is not Have any card userid:{userId}", userId);
                return OperationResult<string>.NotFound("User Is not Have any card");
            }

            var totalAmount = await _OrderConfirmQueryService.ConfirmOrderAsync(confirmOrderDto, cart.Id, userId);

            if (totalAmount <= 0)
            {
                _logger.LogWarning("Cart total amount is zero for userId: {userId}", userId);
                return OperationResult<string>.BadRequest("Cary is Empty");
            }

  
            _logger.LogInformation("Create Payment With Amount:{totalAmount} Cash", totalAmount);
            return OperationResult<string>.Success("Confirm Order Sucessfully");

        }



    }
}
