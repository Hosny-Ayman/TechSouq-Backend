using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Queries;
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
        private readonly ICartItemsQueryService _cartItemsQueryService;

        public CartItemService(ICartItemRepository cartItemRepository, IMapper mapper, ILogger<CartItemService> logger, ICartRepository cartRepository, ICartItemsQueryService cartItemsQueryService)
        {
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
            _logger = logger;
            _cartRepository = cartRepository;
            _cartItemsQueryService = cartItemsQueryService;
        }

        public async Task<OperationResult<int>> AddCartItem(int userId,CartItemDto cartItemDto)
        {
            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            bool IsSaved = false;

            var userCart = await _cartRepository.GetCartIdbyUserId(userId);

            if(userCart != null)
            {
                cartItem.CartId = userCart.Id;

                IsSaved = await _cartItemRepository.AddOrUpdateCartItemAsync(cartItem.CartId, cartItem.ProductId);

                if (!IsSaved)
                {
                    _logger.LogError("Failed to add cartItems to the database");
                    return OperationResult<int>.Failure();
                }
            }
            else
            {
                userCart = new Cart { UserId = userId };

                userCart.Id = await _cartItemRepository.AddCartAndCartItems(cartItem, userCart);

                if (userCart.Id == 0)
                {
                    _logger.LogError("Failed to add cartItems to the database");
                    return OperationResult<int>.Failure();
                }

            }


            var cartItemsLength = await _cartItemRepository.GetCartItemsLengthAsync(userCart.Id);
            

            _logger.LogInformation("Item added Successfully. ProductId: {ProductId}", cartItemDto.ProductId);
            return OperationResult<int>.Success(cartItemsLength);
        }

        public async Task<OperationResult<int>> AddCartItems(int userId, List<CartItemDto> cartItemsDto)
        {
            var cartItems = _mapper.Map<List<CartItem>>(cartItemsDto);
            int IsSavedUpdateOrAddCartItem = 0;
            int newcartId = 0;

            var userCart = await _cartRepository.GetCartIdbyUserId(userId);

            if (userCart != null)
            {
                //cartItems[0].CartId = userCart;

                IsSavedUpdateOrAddCartItem = await _cartItemRepository.AddCartItems(cartItems, userCart);

                if (IsSavedUpdateOrAddCartItem==0)
                {
                    _logger.LogError("Failed to add cartItems to the database");
                    return OperationResult<int>.Failure();
                }
            }
            else
            {
                var Cart = new Cart();
                Cart.UserId = userId;
                Cart.Id = 0;

                newcartId = await _cartItemRepository.AddCartItems(cartItems, Cart);

                if (newcartId == 0)
                {
                    _logger.LogError("Failed to add cartItems to the database");
                    return OperationResult<int>.Failure();
                }

            }


            var cartItemsLength = await _cartItemRepository.GetCartItemsLengthAsync(newcartId);


            _logger.LogInformation("Items added Successfully. userId: {userId}", userId);
            return OperationResult<int>.Success(cartItemsLength);
        }

        public async Task<OperationResult<List<CartItemDto>>> GetCartItems(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("User Try To Get CartItems userId: {userId} (Invalid)", userId);
                return OperationResult<List<CartItemDto>>.BadRequest("Invalid Data");
            }

            var userCart = await _cartRepository.GetCartIdbyUserId(userId);

            if (userCart == null)
            {
                _logger.LogWarning("No Items Found With userId: {userId}", userId);
                return OperationResult<List<CartItemDto>>.NotFound("Cart not found or empty.");
            }

            var result = await _cartItemRepository.GetCartItems(userCart.Id);

            if (result == null || !result.Any())
            {
                _logger.LogWarning("No Items Found With userCart: {userCart}", userCart);
                return OperationResult<List<CartItemDto>>.NotFound("Cart not found or empty.");
            }

            var mapped = _mapper.Map<List<CartItemDto>>(result);
            return OperationResult<List<CartItemDto>>.Success(mapped);
        }

        public async Task<OperationResult<bool>> UpdateCartItem(int userId,List<CartItemDto> cartItemsDto)
        {

            var CartItems = _mapper.Map<List<CartItem>>(cartItemsDto);
            var result = await _cartItemRepository.UpdateCartItems(userId, CartItems);

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

            var usercart = await _cartRepository.GetCartIdbyUserId(userId);

            var result = await _cartItemRepository.RemoveCartItem(usercart.Id, productId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting CartItem with userId: {userId} and productId:{productId}", userId, productId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete CartItem With Id: {CartItemId} Successfully", usercart);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<CartItemDto>> GetCartItemAsync(int userId, int ProductId)
        {
            if (userId <= 0 || ProductId <= 0)
            {
                _logger.LogWarning("Read Cart Invalid Data userId: {userId} or ProductId: {ProductId}", userId, ProductId);
                return OperationResult<CartItemDto>.BadRequest("Invalid Data");
            }

            var userCart = await _cartRepository.GetCartIdbyUserId(userId);

            if (userCart == null)
            {
                _logger.LogWarning("No Items Found With userId: {userId}", userId);
                return OperationResult<CartItemDto>.NotFound("Cart not found or empty.");
            }

            var result = await _cartItemRepository.GetCartItemAsync(userCart.Id, ProductId);

            if (result == null)
            {
                _logger.LogWarning("Read CartId: {CartId} or ProductId: {ProductId} Failed. Not Found.", userCart, ProductId);
                return OperationResult<CartItemDto>.NotFound("Cart Not Found");
            }

            var cartItemDto = _mapper.Map<CartItemDto>(result);

            _logger.LogInformation("Read Cart Successfully CartId: {CartId} and ProductId: {ProductId}", userCart, ProductId);
            return OperationResult<CartItemDto>.Success(cartItemDto);
        }

        public async Task<OperationResult<List<CartItemsWithProductDetailsDto>>> GetAllCartItemsWithProductDetailsAsync(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Read Cart Invalid Data userId: {userId} ", userId);
                return OperationResult<List<CartItemsWithProductDetailsDto>>.BadRequest("Invalid Data");
            }

            var CartItemsWithProductData = await _cartItemsQueryService.GetAllCartItemsWithProductDetailsAsync(userId);

            if (CartItemsWithProductData == null ||!CartItemsWithProductData.Any())
            {
                _logger.LogWarning("No CartItems Found With userId: {userId}", userId);
                return OperationResult<List<CartItemsWithProductDetailsDto>>.NotFound("CartItems not found or empty.");
            }

            _logger.LogInformation("Read CartItems Successfully userId: {userId}", userId);
            return OperationResult<List<CartItemsWithProductDetailsDto>>.Success(CartItemsWithProductData);

        }
    }
}