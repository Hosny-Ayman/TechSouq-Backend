using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.Queries;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechSouq.Application.Services
{
    public class DeliveryZoneService
    {

        private readonly IDeliveryZone _DeliveryZone;
        private readonly IDeliveryZonesQuery _DeliveryZonesQuery;
        private readonly IMapper _mapper;
        private readonly ILogger<DeliveryMethodDto> _logger;

        public DeliveryZoneService(IDeliveryZone deliveryZone, IMapper mapper, ILogger<DeliveryMethodDto> logger, IDeliveryZonesQuery deliveryZonesQuery)
        {
            _DeliveryZone = deliveryZone;
            _mapper = mapper;
            _logger = logger;
            _DeliveryZonesQuery = deliveryZonesQuery;
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

        public async Task<OperationResult<PagedResponse<DeliveryZoneDto>>> GetAllDeliveryZonesPaged(int pageNumber, int pageSize, string? NameSearch)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                _logger.LogWarning("Invalid data Pagenumber:{pageNumber} or pageSize:{pageSize}", pageNumber, pageSize);
                return OperationResult<PagedResponse<DeliveryZoneDto>>.BadRequest("Invalid data");
            }

            var data = await _DeliveryZonesQuery.GetAllDeliveryZonesPaged(pageNumber, pageSize, NameSearch);

            _logger.LogInformation("Get All Delivery Zones Paged Successfully");
            return OperationResult<PagedResponse<DeliveryZoneDto>>.Success(data);

        }

        public async Task<OperationResult<bool>> UpdateDeliveryZone(DeliveryZone DeliveryZone)
        {
            if (DeliveryZone.Id < 1 )
            {
                _logger.LogWarning("Invalid Id", DeliveryZone.Id);
                return OperationResult<bool>.BadRequest("Invalid data");
            }

            var IUpdate = await _DeliveryZone.UpdateDeliveryZone(DeliveryZone);

            if(IUpdate==false)
            {
                _logger.LogError("Failed to add Update Delivery Zone to the database.");
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("UpdateDeliveryZone Successfully");
            return OperationResult<bool>.Success(IUpdate);
        }

        public async Task<OperationResult<bool>> DeleteDeliveryZone(int id)
        {
            var coupon = await _DeliveryZone.GetById(id);

            if (coupon is null)
            {
                _logger.LogWarning("DeliveryZone Not Found for id:{id} ", id);
                return OperationResult<bool>.NotFound("DeliveryZone Not Found");
            }

            var result = await _DeliveryZone.DeleteDeliveryZone(id);

            if (!result)
            {
                _logger.LogError("DeliveryZone Delete for id:{id} Failed", id);
                return OperationResult<bool>.Failure("DeliveryZone Failed Try Again Later");
            }

            _logger.LogInformation("DeliveryZone Delete for id:{id} Successfully", id);
            return OperationResult<bool>.Success(result);
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
