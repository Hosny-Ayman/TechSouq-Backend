using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class CartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(ICartRepository cartRepository, IMapper mapper, ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> AddCart(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);
            var newId = await _cartRepository.AddCart(cart);

            if (newId == 0)
            {
                _logger.LogError("Failed to add cart to the database");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Created Cart Successfully With Id: {Id}", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<CartDto>> GetCart(int cartId)
        {
            if (cartId <= 0)
            {
                _logger.LogWarning("Read Cart Invalid Data CartId: {CartId}", cartId);
                return OperationResult<CartDto>.BadRequest("Invalid Data", new List<string> { $"Invalid CartId: {cartId}" });
            }

            var result = await _cartRepository.GetCart(cartId);

            if (result == null)
            {
                _logger.LogWarning("Read {CartId} Failed. Not Found.", cartId);
                return OperationResult<CartDto>.NotFound("Cart Not Found");
            }

            var cartDto = _mapper.Map<CartDto>(result);

            _logger.LogInformation("Read Cart Successfully Id: {CartId}", cartId);
            return OperationResult<CartDto>.Success(cartDto);
        }

        public async Task<OperationResult<bool>> UpdateCart(CartDto cartDto)
        {
            if (cartDto.Id <= 0)
            {
                _logger.LogWarning("Update Cart {CartId} Failed - Invalid Id", cartDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id", new List<string> { $"Invalid id {cartDto.Id}" });
            }

            var cart = await _cartRepository.GetCart(cartDto.Id);

            if (cart == null)
            {
                _logger.LogWarning("Cart id: {Id} Not Found", cartDto.Id);
                return OperationResult<bool>.NotFound($"Cart id: {cartDto.Id} Not Found"); 
            }

            _mapper.Map(cartDto, cart);

            var result = await _cartRepository.UpdateCart(cart);

            if (!result)
            {
                _logger.LogError("Update cart With id: {Id} Failed", cartDto.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Update cart With Id {Id} Successfully", cart.Id);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> DeleteCart(int cartId)
        {
            if (cartId <= 0)
            {
                _logger.LogWarning("Delete Cart Invalid Data CartId: {CartId}", cartId);
                return OperationResult<bool>.BadRequest("Invalid Data", new List<string> { $"Invalid CartId: {cartId}" });
            }

            var isExists = await _cartRepository.IsCartExists(cartId);

            if (!isExists)
            {
                _logger.LogWarning("Cart With Id: {CartId} Not Found. Deleted Failed", cartId);
                return OperationResult<bool>.NotFound($"Cart With Id: {cartId} Not Found");
            }

            var result = await _cartRepository.DeleteCart(cartId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting Cart with Id {CartId}.", cartId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete Cart Successfully Id: {Id}", cartId);
            return OperationResult<bool>.Success(result);
        }
    }
}