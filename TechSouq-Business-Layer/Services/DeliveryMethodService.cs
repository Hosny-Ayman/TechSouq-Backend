using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class DeliveryMethodService
    {
        private readonly IDeliveryMethodRepository _deliveryMethodRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeliveryMethodService> _logger;

        public DeliveryMethodService(IDeliveryMethodRepository deliveryMethodRepository, IMapper mapper, ILogger<DeliveryMethodService> logger)
        {
            _deliveryMethodRepository = deliveryMethodRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> AddDeliveryMethod(DeliveryMethodDto deliveryMethodDto)
        {
            var deliveryMethod = _mapper.Map<DeliveryMethod>(deliveryMethodDto);
            var newId = await _deliveryMethodRepository.AddDeliveryMethod(deliveryMethod);

            if (newId == 0)
            {
                _logger.LogError("Failed to add DeliveryMethod to the database");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Create DeliveryMethod: {Id} Successfully", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<DeliveryMethodDto>> GetDeliveryMethod(int deliveryMethodId)
        {
            if (deliveryMethodId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", deliveryMethodId);
                return OperationResult<DeliveryMethodDto>.BadRequest("Invalid data", new List<string> { $"Invalid data result Id: {deliveryMethodId}" });
            }

            var deliveryMethod = await _deliveryMethodRepository.GetDeliveryMethod(deliveryMethodId);

            if (deliveryMethod == null)
            {
                _logger.LogWarning("GetDeliveryMethod With id: {DeliveryMethodId} Not Found Or Deleted", deliveryMethodId);
                return OperationResult<DeliveryMethodDto>.NotFound("Delivery Method not found or already deleted.");
            }

            var deliveryMethodDto = _mapper.Map<DeliveryMethodDto>(deliveryMethod);

            _logger.LogInformation("Result Id: {Id} Get Successfully", deliveryMethodId);
            return OperationResult<DeliveryMethodDto>.Success(deliveryMethodDto);
        }

        public async Task<OperationResult<bool>> UpdateDeliveryMethod(DeliveryMethodDto deliveryMethodDto)
        {
            if (deliveryMethodDto.Id <= 0)
            {
                _logger.LogWarning("Update DeliveryMethod {DeliveryMethodId} Failed - User Enter Id Under 1", deliveryMethodDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id", new List<string> { $"Invalid id {deliveryMethodDto.Id}" });
            }

            var existingDeliveryMethod = await _deliveryMethodRepository.GetDeliveryMethod(deliveryMethodDto.Id);

            if (existingDeliveryMethod == null)
            {
                _logger.LogWarning("Update DeliveryMethod {Id} Failed - Record not found", deliveryMethodDto.Id);
                return OperationResult<bool>.NotFound($"Id: {deliveryMethodDto.Id} not found");
            }

            _mapper.Map(deliveryMethodDto, existingDeliveryMethod);

            var result = await _deliveryMethodRepository.UpdateDeliveryMethod(existingDeliveryMethod);

            if (!result)
            {
                _logger.LogError("Update DeliveryMethod {Id} Failed", deliveryMethodDto.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Update DeliveryMethod {Id} Successfully", deliveryMethodDto.Id);
            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> DeleteDeliveryMethod(int deliveryMethodId) 
        {
            if (deliveryMethodId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", deliveryMethodId);
                return OperationResult<bool>.BadRequest("Invalid data", new List<string> { $"Invalid data result Id: {deliveryMethodId}" });
            }

            var isExists = await _deliveryMethodRepository.IsDeliveryMethodExists(deliveryMethodId);

            if (!isExists)
            {
                _logger.LogWarning("DeliveryMethod With Id: {DeliveryMethodId} Not Found. Deleted Failed", deliveryMethodId);
                return OperationResult<bool>.NotFound($"DeliveryMethod With Id: {deliveryMethodId} Not Found");
            }

            var result = await _deliveryMethodRepository.DeleteDeliveryMethod(deliveryMethodId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting deliveryMethod with Id {DeliveryMethodId}.", deliveryMethodId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete deliveryMethodId: {DeliveryMethodId} Successfully", deliveryMethodId);
            return OperationResult<bool>.Success(result);
        }
    }
}