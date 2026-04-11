using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using TechSouq.Domian.Interfaces; 

namespace TechSouq.Application.Services
{
    public class BrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BrandService> _logger;

        public BrandService(IBrandRepository brandRepository, IMapper mapper, ILogger<BrandService> logger)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> AddBrand(BrandDto brandDto)
        {
            var brand = _mapper.Map<Brand>(brandDto);
            var newId = await _brandRepository.AddBrand(brand);

            if (newId == 0)
            {
                _logger.LogError("Failed to add brand to the database");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Brand Created With Id: {Id} Successfully", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<BrandDto>> GetBrand(int brandId)
        {
            if (brandId <= 0)
            {
                _logger.LogWarning("Read Brand With Id: {BrandId} Invalid", brandId);
                return OperationResult<BrandDto>.BadRequest("Invalid Data", new List<string> { $"Invalid brandId {brandId}" });
            }

            var result = await _brandRepository.GetBrand(brandId);

            if (result == null)
            {
                _logger.LogWarning("Read Brand With Id: {BrandId} NotFound", brandId);
                return OperationResult<BrandDto>.NotFound($"Brand With Id NotFound {brandId}");
            }

            var brandDto = _mapper.Map<BrandDto>(result);

            _logger.LogInformation("Read Brand With Id: {BrandId} Successfully", brandId);
            return OperationResult<BrandDto>.Success(brandDto);
        }

        public async Task<OperationResult<bool>> UpdateBrand(BrandDto brandDto)
        {
            if (brandDto.Id <= 0)
            {
                _logger.LogWarning("Update Brand Failed - User Enter Id Under 1 ({BrandId})", brandDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id", new List<string> { $"Invalid id {brandDto.Id}" });
            }

            var brand = await _brandRepository.GetBrand(brandDto.Id);

            if (brand == null)
            {
                _logger.LogWarning("Brand id: {Id} Not Found", brandDto.Id);
                return OperationResult<bool>.NotFound($"Brand id: {brandDto.Id} not found"); // ضفنا return هنا
            }

            _mapper.Map(brandDto, brand);

            var result = await _brandRepository.UpdateBrand(brand);

            if (!result)
            {
                _logger.LogError("Update brand With id: {Id} Failed", brandDto.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Update brand With Id {Id} Successfully", brand.Id);
            return OperationResult<bool>.Success(result);
        }

        public async Task<OperationResult<bool>> DeleteBrand(int brandId)
        {
            if (brandId <= 0)
            {
                _logger.LogWarning("Delete Brand With Id: {BrandId} Invalid", brandId);
                return OperationResult<bool>.BadRequest("Invalid Data", new List<string> { $"Invalid brandId {brandId}" });
            }

            var isExists = await _brandRepository.IsBrandExists(brandId);

            if (!isExists)
            {
                _logger.LogWarning("Brand With Id: {BrandId} Not Found. Delete Failed", brandId);
                return OperationResult<bool>.NotFound($"Brand With Id: {brandId} Not Found");
            }

            var result = await _brandRepository.DeleteBrand(brandId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting brand with Id {BrandId}.", brandId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete Brand With Id: {BrandId} Successfully", brandId);
            return OperationResult<bool>.Success(result);
        }
    }
}