using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.interfaces;
using TechSouq.Application.Queries;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Enums;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderQuery _orderQueryService;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, ILogger<OrderService> logger,
            ICartRepository cartRepository, IOrderQuery orderQueryService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
            _cartRepository = cartRepository;
            _orderQueryService = orderQueryService;
        }

        public async Task<OperationResult<int>> AddOrder(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            var newId = await _orderRepository.AddOrder(order);

            if (newId == 0)
            {
                _logger.LogError("Failed to add Order to the database");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Create Order: {Id} Successfully", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<OrderDto>> GetOrderById(int orderId,int userId,bool isAdmin)
        {
            if (orderId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", orderId);
                return OperationResult<OrderDto>.BadRequest("Invalid data", new List<string> { $"Invalid data result Id: {orderId}" });
            }

            if(!isAdmin)
            {
                var IsExists = await _orderRepository.IsOrderExists(orderId, userId);

                if (!IsExists)
                {

                    _logger.LogWarning("Order With id: {OrderId} Not Found Or Deleted", orderId);
                    return OperationResult<OrderDto>.NotFound("Order not found or already deleted.");
                }
            }

            

            var order = await _orderRepository.GetOrder(orderId);

            if (order == null)
            {
                _logger.LogWarning("Order With id: {OrderId} Not Found Or Deleted", orderId);
                return OperationResult<OrderDto>.NotFound("Order not found or already deleted.");
            }

            var orderDto = _mapper.Map<OrderDto>(order);

            _logger.LogInformation("Result Id: {Id} Get Successfully", orderId);
            return OperationResult<OrderDto>.Success(orderDto);
        }

        public async Task<OperationResult<List<OrderSummarieDto>>> GetOrderSummaryAsync(int userId)
        {

            var userCart = await _cartRepository.GetCartIdbyUserIdAnyStatus(userId);

            if (userCart == null)
            {
                _logger.LogWarning("No Items Found With userId: {userId}", userId);
                return OperationResult<List<OrderSummarieDto>>.NotFound("Cart not found or empty.");
            }


            var orders = await _orderQueryService.GetOrderSummaryAsync(userCart.Id, userId);

            //if (isUpdate == null)
            //{
            //    _logger.LogWarning("Order With id: {OrderId} Not Found Or Deleted", orderId);
            //    return OperationResult<OrderDto>.NotFound("Order not found or already deleted.");
            //}

           

            _logger.LogInformation("Orders Get Successfully For userId:{userId}", userId);
            return OperationResult<List<OrderSummarieDto>>.Success(orders);
        }

        public async Task<OperationResult<bool>> UpdateOrder(OrderDto orderDto)
        {
            if (orderDto.Id <= 0)
            {
                _logger.LogWarning("Update Order {OrderId} Failed - User Enter Id Under 1", orderDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id", new List<string> { $"Invalid id {orderDto.Id}" });
            }

            var existingOrder = await _orderRepository.GetOrder(orderDto.Id);

            if (existingOrder == null)
            {
                _logger.LogWarning("Update Order {Id} Failed - Record not found", orderDto.Id);
                return OperationResult<bool>.NotFound($"Id: {orderDto.Id} not found");
            }

            _mapper.Map(orderDto, existingOrder);

            var result = await _orderRepository.UpdateOrder(existingOrder);

            if (!result)
            {
                _logger.LogError("Update Order With id: {Id} Failed", orderDto.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Update Order {Id} Successfully", orderDto.Id);
            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> DeleteOrder(int orderId) 
        {
            if (orderId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", orderId);
                return OperationResult<bool>.BadRequest("Invalid data", new List<string> { $"Invalid data result Id: {orderId}" });
            }

            var isExists = await _orderRepository.IsOrderExists(orderId);

            if (!isExists)
            {
                _logger.LogWarning("Order With Id: {OrderId} Not Found. Deleted Failed", orderId);
                return OperationResult<bool>.NotFound($"Order With Id: {orderId} Not Found");
            }

            var result = await _orderRepository.DeleteOrder(orderId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting Order with Id {OrderId}.", orderId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete OrderId: {OrderId} Successfully", orderId);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<PagedResponse<AdminOrderListDto>>> GetAllOrdersPaged(int PageNumber, int PageSize, OrderStatus? status, string? search)
        {
            if (PageNumber <= 0 || PageSize <= 0)
            {
                _logger.LogWarning("Invalid data result PageNumber or PageSize under 0 or 0");
                return OperationResult<PagedResponse<AdminOrderListDto>>.BadRequest("Invalid data", new List<string> { "Invalid data result PageNumber or PageSize under 0 or 0" });
            }

            var orders = await _orderQueryService.GetAllOrdersPaged(PageNumber, PageSize, status, search);


            _logger.LogInformation("Result Get Orders Successfully");
            return OperationResult<PagedResponse<AdminOrderListDto>>.Success(orders);
        }

        public async Task<OperationResult<AdminOrderDetailsDto>> GetOrderDtailsAdmin(int OrderId)
        {
            if (OrderId <= 0)
            {
                _logger.LogWarning("Invalid data");
                return OperationResult<AdminOrderDetailsDto>.BadRequest("Invalid data", new List<string> { "Invalid data result PageNumber or PageSize under 0 or 0" });
            }

            var order = await _orderQueryService.GetOrderDtailsAdmin(OrderId);

            if(order == null)
            {
                _logger.LogWarning("Order with Id: {Id} not found or Deleted", OrderId);
                return OperationResult<AdminOrderDetailsDto>.NotFound($"Id: {OrderId} not found");
               
            }
            _logger.LogInformation("Result Get Order Successfully");
            return OperationResult<AdminOrderDetailsDto>.Success(order);

        }

        public async Task<OperationResult<bool>> UpdateStatus(int OrderId, OrderStatus Status)
        {
            if (OrderId <= 0 || Status == null)
            {
                _logger.LogWarning("Invalid data");
                return OperationResult<bool>.BadRequest("Invalid data", new List<string> { "Invalid data OrderId under 0 or 0 or Status null" });
            }

            var isUpdate = await _orderRepository.UpdateStatus(OrderId, Status);

            if (isUpdate == false)
            {
                _logger.LogWarning("Update isUpdate Status with Id: {Id} failed or not found", OrderId);
                return OperationResult<bool>.NotFound($"Id: {OrderId} failed or not found");

            }
            _logger.LogInformation("Result Get Order Successfully");
            return OperationResult<bool>.Success(isUpdate);

        }

        
    }
}