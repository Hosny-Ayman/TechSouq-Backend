using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class CartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CartItemService> _logger;

        public CartItemService(ICartItemRepository cartItemRepository, IMapper mapper, ILogger<CartItemService> logger)
        {
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> AddCartItem(CartItemDto cartItemDto)
        {
            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            var newId = await _cartItemRepository.AddCartItem(cartItem);

            if (newId == 0)
            {
                _logger.LogError("Failed to add cartItem to the database");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Item added Successfully. ProductId: {ProductId}", cartItemDto.ProductId);
            return OperationResult<int>.Success(newId, "Cart Items Added Successfully");
        }

        public async Task<OperationResult<List<CartItemDto>>> GetCartItems(int cartId)
        {
            if (cartId <= 0)
            {
                _logger.LogWarning("User Try To Get CartId: {CartId} (Invalid)", cartId);
                return OperationResult<List<CartItemDto>>.BadRequest("Invalid Data", new List<string> { $"Invalid Cart ID: {cartId}" });
            }

            var result = await _cartItemRepository.GetCartItems(cartId);

            if (result == null || !result.Any())
            {
                _logger.LogWarning("No Items Found With CartId: {CartId}", cartId);
                return OperationResult<List<CartItemDto>>.NotFound("Cart not found or empty.");
            }

            var mapped = _mapper.Map<List<CartItemDto>>(result);
            return OperationResult<List<CartItemDto>>.Success(mapped);
        }

        public async Task<OperationResult<bool>> UpdateCartItems(List<CartItemDto> cartItemsDtos)
        {
            if (cartItemsDtos == null || !cartItemsDtos.Any())
            {
                _logger.LogWarning("Update attempt with empty or null cart items list.");
                return OperationResult<bool>.BadRequest("Update Failed empty or null cart items list");
            }

            var ctList = _mapper.Map<List<CartItem>>(cartItemsDtos);
            var result = await _cartItemRepository.UpdateCartItems(ctList);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while updating CartItems.");
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Successfully updated {Count} cart items.", ctList.Count);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> DeleteCartItem(int cartItemId) 
        {
            if (cartItemId <= 0)
            {
                _logger.LogWarning("Delete CartItem With Id: {CartItemId} Invalid", cartItemId);
                return OperationResult<bool>.BadRequest("Invalid Data", new List<string> { $"Invalid CartItemId {cartItemId}" });
            }

            var isExists = await _cartItemRepository.IsCartItemExists(cartItemId);

            if (!isExists)
            {
                _logger.LogWarning("CartItem With Id: {CartItemId} Not Found. Deleted Failed", cartItemId);
                return OperationResult<bool>.NotFound($"CartItem With Id: {cartItemId} Not Found");
            }

            var result = await _cartItemRepository.DeleteCartItem(cartItemId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting CartItem with Id {CartItemId}.", cartItemId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete CartItem With Id: {CartItemId} Successfully", cartItemId);
            return OperationResult<bool>.Success(result);
        }
    }
}