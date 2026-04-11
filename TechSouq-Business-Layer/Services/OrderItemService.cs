using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechSouq.Application.Services
{
    public class OrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderItemService> _logger;      
        public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper, ILogger<OrderItemService> logger)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task <OperationResult<int>> AddOrderItem(OrderItemDto orderItemDto)
        {
            var order = _mapper.Map<OrderItem>(orderItemDto);

            int newId = await _orderItemRepository.AddOrderItem(order);

            if (newId == 0)
            {
                _logger.LogError("Failed to add orderIte to the database");
                return OperationResult<int>.Failure("Failed to add record.");
            }

            _logger.LogInformation("Create orderIte : {Id} Successfully", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<OrderItemDto>> GetOrderItem(int OrderItemId)
        {
            if (OrderItemId <= 0)
            {
                _logger.LogWarning("Failed to retrieve OrderItem. Reason: Invalid OrderItemId provided ({OrderItemId}).", OrderItemId);
                return OperationResult<OrderItemDto>.BadRequest($"Invalid data", new List<string> { $"Invalid OrderItemId: {OrderItemId}" });
            }

           

            var OrderItem = await _orderItemRepository.GetOrderItem(OrderItemId);

            if (OrderItem == null)
            {
                _logger.LogWarning("Failed to retrieve OrderItem. Reason: OrderItem with Id {OrderItemId} was not found.", OrderItemId);
                return OperationResult<OrderItemDto>.NotFound($"OrderItem with Id {OrderItemId} not found.");
            }

            var orderItemDto = _mapper.Map<OrderItemDto>(OrderItem);


            _logger.LogInformation("OrderItem with Id: {OrderItemId} retrieved successfully", OrderItemId);
            return OperationResult<OrderItemDto>.Success(orderItemDto);

        }

        public async Task<OperationResult<bool>> UpdateOrderItem(OrderItemDto OrderItemDto)
        {
            if (OrderItemDto.Id <= 0)
            {
                _logger.LogWarning("Failed to update OrderItem. Reason: Invalid OrderItemId provided ({OrderItemId}).", OrderItemDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id", new List<string> { $"Invalid id {OrderItemDto.Id}" });
            }

            var OrderItem = await _orderItemRepository.GetOrderItem(OrderItemDto.Id);

            if (OrderItem == null)
            {
                _logger.LogWarning("Failed to update OrderItem. Reason: OrderItem with Id {OrderItemId} was not found.", OrderItemDto.Id);
                return OperationResult<bool>.NotFound($"OrderItem with Id {OrderItemDto.Id} not found.");
            }

            _mapper.Map(OrderItemDto, OrderItem);



            var result = await _orderItemRepository.UpdateOrderItem(OrderItem);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while updating OrderItem with Id { OrderItemId}.", OrderItemDto.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("OrderItem with Id: {OrderItemId} updated successfully", OrderItemDto.Id);
            return OperationResult<bool>.Success(true);
        }


        public async Task<OperationResult<bool>> DeleteOrderItem(int orderItemId)
        {
            if (orderItemId <= 0)
            {
                _logger.LogWarning("Failed to delete OrderItem. Reason: Invalid OrderItemId provided ({OrderItemId}).", orderItemId);
                return OperationResult<bool>.BadRequest("Invalid data", new List<string> { $"Invalid data result Id: {orderItemId}" });
            }

            var isExists = await _orderItemRepository.IsOrderItemExists(orderItemId);

            if (!isExists)
            {
                _logger.LogWarning("Failed to delete OrderItem. Reason: OrderItem with Id {OrderItemId} was not found.", orderItemId);
                return OperationResult<bool>.NotFound($"OrderItem with Id {orderItemId} not found.");
            }

            var result = await _orderItemRepository.DeleteOrderItem(orderItemId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting OrderItem with Id {OrderItemId}.", orderItemId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("OrderItem with Id: {OrderItemId} deleted successfully", orderItemId);
            return OperationResult<bool>.Success(result);
        }
    }
}
