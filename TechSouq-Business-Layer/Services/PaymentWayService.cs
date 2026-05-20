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
    public class PaymentWayService
    {

        private readonly IPaymentWayRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentWayService> _logger;

        public PaymentWayService(
            IPaymentWayRepository repository,
            IMapper mapper,
            ILogger<PaymentWayService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> Add(PaymentWayDto dto)
        {
            var isExists = await _repository.IsNameExists(dto.Name);

            if (isExists)
            {
                _logger.LogWarning("PaymentWay Name Already Exists for Name:{Name} ", dto.Name);
                return OperationResult<int>.BadRequest("PaymentWay Name Already Exists");
            }

            var paymentWay = _mapper.Map<PaymentWay>(dto);

            var id = await _repository.Add(paymentWay);

            _logger.LogInformation("PaymentWay Add Successfully");
            return OperationResult<int>.Success(id);
        }

        public async Task<OperationResult<bool>> Update(PaymentWayDto dto)
        {
            if (dto.Id <= 0)
            {
                _logger.LogWarning("Failed to update PaymentWay. Reason: Invalid Id ({PaymentWayId}).", dto.Id);
                return OperationResult<bool>.BadRequest("Invalid id");
            }

            var paymentWay = await _repository.GetById(dto.Id);

            if (paymentWay is null)
            {
                _logger.LogWarning("PaymentWay Not Found for id:{id} ", dto.Id);
                return OperationResult<bool>.NotFound("PaymentWay Not Found");
            }

            paymentWay.Name = dto.Name;

            var result = await _repository.Update(paymentWay);

            if (!result)
            {
                _logger.LogWarning("PaymentWay Updating Failed for id:{id} ", dto.Id);
                return OperationResult<bool>.BadRequest("Updating Failed");
            }

            _logger.LogInformation("PaymentWay Updating Successfully for id:{id} ", dto.Id);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> Delete(int id)
        {
            var paymentWay = await _repository.GetById(id);

            if (paymentWay is null)
            {
                _logger.LogWarning("PaymentWay Not Found for id:{id} ", id);
                return OperationResult<bool>.NotFound("PaymentWay Not Found");
            }

            var result = await _repository.Delete(id);

            if (!result)
            {
                _logger.LogError("PaymentWay Delete for id:{id} Failed", id);
                return OperationResult<bool>.Failure("Delete Failed Try Again Later");
            }

            _logger.LogInformation("PaymentWay Delete for id:{id} Successfully", id);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<PaymentWayDto>> GetPaymentWayByName(string Name)
        {
            var paymentWay = await _repository.GetByName(Name);

            if (paymentWay is null)
            {
                _logger.LogWarning("PaymentWay Not Found for Name:{Name} ", Name);
                return OperationResult<PaymentWayDto>.NotFound("PaymentWay Not Found");
            }

            var paymentWayDto = _mapper.Map<PaymentWayDto>(paymentWay);

            _logger.LogInformation("PaymentWay Get by Name:{Name} Successfully", Name);
            return OperationResult<PaymentWayDto>.Success(paymentWayDto);
        }


    }
}
