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

namespace TechSouq.Application.Services
{
    public class DeliveryZoneService
    {

        private readonly IDeliveryZone _DeliveryZone;
        private readonly IMapper _mapper;
        private readonly ILogger<DeliveryMethodDto> _logger;

        public DeliveryZoneService(IDeliveryZone deliveryZone, IMapper mapper, ILogger<DeliveryMethodDto> logger)
        {
            _DeliveryZone = deliveryZone;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<OperationResult<int>> AddDeliveryZone(DeliveryZoneDto DeliveryZoneDto)
        {
          
            var DeliveryZone = _mapper.Map<DeliveryZone>(DeliveryZoneDto);
            var newId = await _DeliveryZone.AddDeliveryZone(DeliveryZone);

            if (newId == 0)
            {
                _logger.LogError("Failed to add AddDeliveryZone to the database.");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Create DeliveryZone with Id: {Id} Successfully", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<List<DeliveryZoneDto>>> GetAllDeliveryZones()
        {

            var DeliveryZones = await _DeliveryZone.GetAllDeliveryZones();

            if (DeliveryZones == null || !DeliveryZones.Any())
            {
                _logger.LogWarning("DeliveryZones Not Found ");
                return OperationResult<List<DeliveryZoneDto>>.NotFound("DeliveryZone Not Found");
            }

            var deliveryZones = _mapper.Map<List<DeliveryZoneDto>>(DeliveryZones);

            _logger.LogInformation("retrieved All DeliveryZones successfully.");
            return OperationResult<List<DeliveryZoneDto>>.Success(deliveryZones);
        }


        public async Task<OperationResult<decimal>> GetOnlyShippingCost(string ShippingCity)
        {

            if (ShippingCity == null)
            {
                _logger.LogWarning("ShippingCity Not Found ");
                return OperationResult<decimal>.NotFound("ShippingCity Not Found");
            }

            var shippingCost = await _DeliveryZone.GetOnlyShippingCost(ShippingCity);

            _logger.LogInformation("retrieved ShippingCity successfully.");
            return OperationResult<decimal>.Success(shippingCost);
        }
    }
}
