using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Enums;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class CartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CartItemService> _logger;
        private readonly ICartRepository _cartRepository;

        public CartItemService(ICartItemRepository cartItemRepository, IMapper mapper, ILogger<CartItemService> logger, ICartRepository cartRepository)
        {
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
            _logger = logger;
            _cartRepository = cartRepository;
        }

        public async Task<OperationResult<string>> AddCartItem(int userId,CartItemDto cartItemDto)
        {
            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            bool IsSavedUpdateOrAddCartItem = false;
            bool IsSavedAddCart = false;

            var userCartId = await _cartRepository.GetCartIdbyUserId(userId);

            if(userCartId != 0)
            {
                cartItem.CartId = userCartId;

                IsSavedUpdateOrAddCartItem = await _cartItemRepository.AddOrUpdateCartItemAsync(cartItem.CartId, cartItem.ProductId);
            }
            else
            {
                var Cart = new Cart();
                Cart.UserId = userId;
                cartItem.CartId = userCartId;

                IsSavedAddCart = await _cartItemRepository.AddCartAndCartItems(cartItem, Cart);

            }

            if (!IsSavedUpdateOrAddCartItem  || !IsSavedAddCart)
            {
                _logger.LogError("Failed to add cartItem to the database");
                return OperationResult<string>.Failure();
            }

            _logger.LogInformation("Item added Successfully. ProductId: {ProductId}", cartItemDto.ProductId);
            return OperationResult<string>.Success("Item added Successfully");
        }

        public async Task<OperationResult<List<CartItemDto>>> GetCartItems(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("User Try To Get CartItems userId: {userId} (Invalid)", userId);
                return OperationResult<List<CartItemDto>>.BadRequest("Invalid Data");
            }

            var userCartId = await _cartRepository.GetCartIdbyUserId(userId);

            if (userCartId == 0)
            {
                _logger.LogWarning("No Items Found With userId: {CartId}", userCartId);
                return OperationResult<List<CartItemDto>>.NotFound("Cart not found or empty.");
            }

            var result = await _cartItemRepository.GetCartItems(userCartId);

            if (result == null || !result.Any())
            {
                _logger.LogWarning("No Items Found With userCartId: {userCartId}", userCartId);
                return OperationResult<List<CartItemDto>>.NotFound("Cart not found or empty.");
            }

            var mapped = _mapper.Map<List<CartItemDto>>(result);
            return OperationResult<List<CartItemDto>>.Success(mapped);
        }

        public async Task<OperationResult<bool>> UpdateCartItem(int userId,CartItemDto cartItemDto)
        {
            //if (cartItemsDtos == null || !cartItemsDtos.Any())
            //{
            //    _logger.LogWarning("Update attempt with empty or null cart items list.");
            //    return OperationResult<bool>.BadRequest("Update Failed empty or null cart items list");
            //}

            var CartItem = _mapper.Map<CartItem>(cartItemDto);
            var result = await _cartItemRepository.UpdateCartItems(userId, CartItem);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while updating CartItems.");
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Successfully updated CartItem for UserId: {UserId}", userId);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> RemoveCartItem(int userId,int productId)
        {
            if (productId <= 0 || userId <= 0)
            {
                _logger.LogWarning("Delete CartItem With productId: {productId} or userId:{userId} Invalid",productId, userId);
                return OperationResult<bool>.BadRequest("Invalid Data");
            }

            //var isExists = await _cartItemRepository.IsCartItemExists(cartItemId);

            //if (!isExists)
            //{
            //    _logger.LogWarning("CartItem With Id: {CartItemId} Not Found. Deleted Failed", cartItemId);
            //    return OperationResult<bool>.NotFound($"CartItem With Id: {cartItemId} Not Found");
            //}

            var cartId = await _cartRepository.GetCartIdbyUserId(userId);

            var result = await _cartItemRepository.RemoveCartItem(cartId, productId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting CartItem with userId: {userId} and productId:{productId}", userId, productId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete CartItem With Id: {CartItemId} Successfully", cartId);
            return OperationResult<bool>.Success(result);
        }
    }
}