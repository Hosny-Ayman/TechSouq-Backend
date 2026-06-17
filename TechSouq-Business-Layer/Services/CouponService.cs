using AutoMapper;
using Microsoft.Extensions.Logging;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.Queries;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class CouponService
    {

        private readonly ICouponRepository _repository;
        private readonly ICouponsQuery _CouponsQuery;
        private readonly IMapper _mapper;
        private readonly ILogger<CouponService> _logger;

        public CouponService(
            ICouponRepository repository,
            IMapper mapper,
            ILogger<CouponService> logger,
            ICouponsQuery couponsQuery)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _CouponsQuery = couponsQuery;
        }

        public async Task<OperationResult<int>> Add(CouponDto dto)
        {
            var isExists = await _repository.IsCodeExists(dto.Code);

            if (isExists)
            {
                _logger.LogWarning("Coupon Code Already Exists for Code:{Code} ", dto.Code);
                return OperationResult<int>.BadRequest("Coupon Code Already Exists");
            }

            var coupon = _mapper.Map<Coupon>(dto);

            var id = await _repository.Add(coupon);

            if(id == 0)
            {
                _logger.LogError("Coupon Code Added Failed for Code:{Code} ", dto.Code);
                return OperationResult<int>.Failure("Coupon Code Added Failed");
            }

            _logger.LogInformation("Coupon Add Successfully");
            return OperationResult<int>.Success(id);
        }

        public async Task<OperationResult<bool>> Update(CouponDto dto)
        {
            if (dto.Id <= 0)
            {
                _logger.LogWarning("Failed to update Coupon. Reason: Invalid Id ({CouponId}).", dto.Id);
                return OperationResult<bool>.BadRequest("Invalid id");
            }

            var coupon = await _repository.GetById(dto.Id);

            if (coupon is null)
            {
                _logger.LogWarning("Coupon Not Found for id:{id} ", dto.Id);
                return OperationResult<bool>.NotFound("Coupon Not Found");
            }

            _mapper.Map(dto, coupon);

           

            var result = await _repository.Update(coupon);

            if (!result)
            {
                _logger.LogWarning("Coupon Updating Failed for id:{id} ", dto.Id);
                return OperationResult<bool>.BadRequest("Updating Failed");
            }

            _logger.LogInformation("Coupon Updating Successfully for id:{id} ", dto.Id);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> Delete(int id)
        {
            var coupon = await _repository.GetById(id);

            if (coupon is null)
            {
                _logger.LogWarning("Coupon Not Found for id:{id} ", id);
                return OperationResult<bool>.NotFound("Coupon Not Found");
            }

            var result = await _repository.Delete(id);

            if (!result)
            {
                _logger.LogError("Coupon Delete for id:{id} Failed", id);
                return OperationResult<bool>.Failure("Delete Failed Try Again Later");
            }

            _logger.LogInformation("Coupon Delete for id:{id} Successfully", id);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<GetCouponDto>> GetCouponByCode(string Code)
        {

            var isExists = await _repository.IsCodeExists(Code);

            if (!isExists)
            {
                _logger.LogWarning("Coupon Code Is Not Exists for Code:{Code} ", Code);
                return OperationResult<GetCouponDto>.BadRequest("Coupon Code Is Not Exists");
            }

            var coupon = await _repository.GetOnlyActiveCouponByCode(Code);

            if (coupon is null)
            {
                _logger.LogWarning("Coupon Is Expired for Code:{Code} ", Code);
                return OperationResult<GetCouponDto>.NotFound("Coupon Is Expired");
            }

            var couponDto = _mapper.Map<GetCouponDto>(coupon);

            _logger.LogInformation("Coupon Get by Code:{Code} Successfully", Code);
            return OperationResult<GetCouponDto>.Success(couponDto);
        }

        public async Task<OperationResult<CouponDto>> GetCouponById(int id)
        {

            if (id <= 0)
            {
                _logger.LogWarning("Invalid Id: ({CouponId}).", id);
                return OperationResult<CouponDto>.BadRequest("Invalid id");
            }

            var Coupon = await _CouponsQuery.GetCouponById(id);

            if (Coupon==null)
            {
                _logger.LogWarning("Coupon id Is Not Exists for id:{id} ", id);
                return OperationResult<CouponDto>.BadRequest("Coupon Code Is Not Exists");
            }

            _logger.LogInformation("Coupon Get by id:{id} Successfully", id);
            return OperationResult<CouponDto>.Success(Coupon);
        }

        public async Task RunDailyCouponsCleanupJob()
        {
             await _repository.RemoveAllExpiredCouponsAsync();

            _logger.LogInformation($"Hangfire Job Executed: Removed expiration  Coupons");

        }

        public async Task<OperationResult<PagedResponse<CouponSummaryDto>>> GetAllCouponsPaged(int pageNumber, int pageSize, string? CodeSearch)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                _logger.LogWarning("Invalid data Pagenumber:{pageNumber} or pageSize:{pageSize}", pageNumber, pageSize);
                return OperationResult<PagedResponse<CouponSummaryDto>>.BadRequest("Invalid data");
            }

            var data = await _CouponsQuery.GetAllCouponsPaged(pageNumber, pageSize, CodeSearch);

            _logger.LogInformation("Get All Coupons Paged Successfully");
            return OperationResult<PagedResponse<CouponSummaryDto>>.Success(data);

        }
    }

}