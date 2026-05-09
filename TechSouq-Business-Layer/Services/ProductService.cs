using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.Queries;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;


namespace TechSouq.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly IProductQueryService _productQueryService;
        private readonly IDistributedCache _cash;

        public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger, IProductQueryService productQueryService, IDistributedCache distributedCache)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
            _productQueryService = productQueryService;
            _cash = distributedCache;
           
        }

        public async Task<OperationResult<int>> AddProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            var newId = await _productRepository.AddProduct(product);

            if (newId == 0)
            {
                _logger.LogError("Failed to add Product to the database");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Create Product: {Id} Successfully", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<ProductDto>> GetProductById(int productId)
        {
            if (productId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", productId);
                return OperationResult<ProductDto>.BadRequest("Invalid data", new List<string> { $"Invalid data result Id: {productId}" });
            }

            var product = await _productQueryService.GetProductById(productId);

            if (product == null)
            {
                _logger.LogWarning("Product With id: {ProductId} Not Found Or Deleted", productId);
                return OperationResult<ProductDto>.NotFound("Product not found or already deleted.");
            }

            var productDto = _mapper.Map<ProductDto>(product);



            _logger.LogInformation("Result Id: {Id} Get Successfully", productId);
            return OperationResult<ProductDto>.Success(productDto);
        }

        public async Task<OperationResult<PagedResponse<ProductDto>>> GetProductsPaged(int PageNumber,int PageSize, string? searchTerm = null, string? Catogrie = null)
        {
            if (PageNumber <= 0 && PageSize <= 0)
            {
                _logger.LogWarning("Invalid data result PageNumber or PageSize under 0 or 0");
                return OperationResult<PagedResponse<ProductDto>>.BadRequest("Invalid data", new List<string> { "Invalid data result PageNumber or PageSize under 0 or 0" });
            }

            var CashKey = $"Product_Page_{PageNumber}_size_{PageSize}_search_{searchTerm}_cat_{Catogrie}";

            var cachedData = _cash.GetString(CashKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                var cachedProducts = JsonSerializer.Deserialize<PagedResponse<ProductDto>>(cachedData);
                _logger.LogInformation("Get categories from Cache Successfully");
                return OperationResult<PagedResponse<ProductDto>>.Success(cachedProducts);
            }



            var products = await _productQueryService.GetProductsPaged(PageNumber, PageSize, searchTerm, Catogrie);

            if (products.Data.Count() == 0 )
            {
                _logger.LogWarning("Products Not Found PageNumber: {PageNumber} , PageSize: {PageSize} ", PageNumber, PageSize);
                return OperationResult<PagedResponse<ProductDto>>.NotFound("Product not found or already deleted.");
            }


            var cacheOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
            var jsonData = JsonSerializer.Serialize(products);
            await _cash.SetStringAsync(CashKey, jsonData, cacheOptions);


            _logger.LogInformation("Result products Get Successfully");
            return OperationResult<PagedResponse<ProductDto>>.Success(products);
        }

        public async Task<OperationResult<bool>> UpdateProduct(ProductDto productDto)
        {
            if (productDto.Id <= 0)
            {
                _logger.LogWarning("Update Product {ProductId} Failed - User Enter Id Under 1", productDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id", new List<string> { $"Invalid id {productDto.Id}" });
            }

            var existingProduct = await _productRepository.GetProduct(productDto.Id);

            if (existingProduct == null)
            {
                _logger.LogWarning("Update Product {Id} Failed - Record not found", productDto.Id);
                return OperationResult<bool>.NotFound($"Id: {productDto.Id} not found");
            }

            _mapper.Map(productDto, existingProduct);

            var result = await _productRepository.UpdateProduct(existingProduct);

            if (!result)
            {
                _logger.LogError("Update Product With id: {Id} Failed", productDto.Id);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Update Product {Id} Successfully", productDto.Id);
            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<bool>> DeleteProduct(int productId)
        {
            if (productId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", productId);
                return OperationResult<bool>.BadRequest("Invalid data", new List<string> { $"Invalid data result Id: {productId}" });
            }

            var isExists = await _productRepository.IsProductExists(productId);

            if (!isExists)
            {
                _logger.LogWarning("Product With Id: {ProductId} Not Found. Deleted Failed", productId);
                return OperationResult<bool>.NotFound($"Product With Id: {productId} Not Found");
            }

            var result = await _productRepository.DeleteProduct(productId);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting Product with Id {ProductId}.", productId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Delete ProductId: {ProductId} Successfully", productId);
            return OperationResult<bool>.Success(result);
        }
    }
}