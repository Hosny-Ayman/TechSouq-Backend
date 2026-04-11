using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;

namespace TechSouq.Application.Services
{
    public class ProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductImageService> _logger;

        public ProductImageService(IProductImageRepository productImageRepository, IMapper mapper, ILogger<ProductImageService> logger)
        {
            _productImageRepository = productImageRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> AddProductImage(ProductImageDto productImageDto)
        {
            var productImage = _mapper.Map<ProductImage>(productImageDto);
            var newId = await _productImageRepository.AddProductImage(productImage);

            if (newId == 0)
            {
                _logger.LogError("Failed to add ProductImage to the database");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Create ProductImage: {Id} Successfully", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<ProductImageDto>> GetProductImageById(int productImageId)
        {
            if (productImageId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", productImageId);
                return OperationResult<ProductImageDto>.BadRequest("Invalid data", new List<string> { $"Invalid Id: {productImageId}" });
            }

            var productImage = await _productImageRepository.GetProductImage(productImageId);

            if (productImage == null)
            {
                _logger.LogWarning("ProductImage With id: {Id} Not Found Or Deleted", productImageId);
                return OperationResult<ProductImageDto>.NotFound("ProductImage not found.");
            }

            _logger.LogInformation("Result Id: {Id} Get Successfully", productImageId);
            return OperationResult<ProductImageDto>.Success(_mapper.Map<ProductImageDto>(productImage));
        }

        public async Task<OperationResult<bool>> UpdateProductImage(ProductImageDto productImageDto)
        {
            if (productImageDto.Id <= 0)
            {
                _logger.LogWarning("Update ProductImage {Id} Failed - User Enter Id Under 1", productImageDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id", new List<string> { $"Invalid id {productImageDto.Id}" });
            }

            var existingProductImage = await _productImageRepository.GetProductImage(productImageDto.Id);

            if (existingProductImage == null)
            {
                _logger.LogWarning("Update ProductImage {Id} Failed - Record not found", productImageDto.Id);
                return OperationResult<bool>.NotFound($"Id: {productImageDto.Id} not found");
            }

            _mapper.Map(productImageDto, existingProductImage);
            var result = await _productImageRepository.UpdateProductImage(existingProductImage);

            if (!result)
            {
                _logger.LogError("Update ProductImage With id: {Id} Failed", productImageDto.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Update ProductImage {Id} Successfully", productImageDto.Id);
            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> DeleteProductImage(int productImageId)
        {
            if (productImageId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", productImageId);
                return OperationResult<bool>.BadRequest("Invalid data");
            }

            var isExists = await _productImageRepository.IsProductImageExists(productImageId);

            if (!isExists)
            {
                _logger.LogWarning("ProductImage With Id: {Id} Not Found. Deleted Failed", productImageId);
                return OperationResult<bool>.NotFound("Not Found");
            }

            var result = await _productImageRepository.DeleteProductImage(productImageId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting ProductImage with Id {Id}.", productImageId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete ProductImageId: {Id} Successfully", productImageId);
            return OperationResult<bool>.Success(result);
        }
    }
}