using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.Queries;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using static System.Net.Mime.MediaTypeNames;


namespace TechSouq.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly IProductQuery _productQueryService;
        private readonly IDistributedCache _cash;
        private readonly IConnectionMultiplexer _redis;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(
              IProductRepository productRepository,
              IMapper mapper,
              ILogger<ProductService> logger,
              IProductQuery productQueryService,
              IDistributedCache distributedCache,
              IConnectionMultiplexer redis
            , IWebHostEnvironment webHostEnvironment
              ) 
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
            _productQueryService = productQueryService;
            _cash = distributedCache;
            _redis = redis;
            _webHostEnvironment = webHostEnvironment;

        }

        private List<string> uploadedFilesPaths = new List<string>();

        private string GetUploadsFolder()
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImages");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
            return uploadsFolder;
        }


        private async Task<string> AddSingleImageToFileAsync(IFormFile? mainImage, List<string> rollbackPaths)
        {
            if (mainImage == null || mainImage.Length == 0) return null;

            string uploadsFolder = GetUploadsFolder();
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + mainImage.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await mainImage.CopyToAsync(fileStream);
            }

            rollbackPaths.Add(filePath);
            return uniqueFileName;
        }

       
        private async Task<List<string>> AddImagesToFileAsync(List<IFormFile>? images, List<string> rollbackPaths)
        {
            var uniqueFilesName = new List<string>();

            if (images == null || !images.Any())
                return uniqueFilesName;

            string uploadsFolder = GetUploadsFolder();
            var uploadTasks = new List<Task>();

            foreach (var image in images)
            {
                if (image.Length == 0) continue;

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                uniqueFilesName.Add(uniqueFileName);
                rollbackPaths.Add(filePath); 

                uploadTasks.Add(SaveFileAsync(image, filePath));
            }

            await Task.WhenAll(uploadTasks);
            return uniqueFilesName;
        }

        private async Task SaveFileAsync(IFormFile image, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
        }

        private void RollbackUploadedFiles(List<string> filePaths)
        {
            foreach (var path in filePaths)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete file for rollback: {Path}", path);
                    }
                }
            }
        }


        public async Task<OperationResult<int>> AddProduct(CreateUpdateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.ProductImages = new List<ProductImage>();

            var rollbackPaths = new List<string>();

            var uniqueFileName = await AddSingleImageToFileAsync(productDto.MainImage, rollbackPaths);

            if (uniqueFileName == null)
            {
                _logger.LogError("Main Image Save Failed");
                return OperationResult<int>.Failure();
            }

            product.ProductImages.Add(new ProductImage { ImageUrl = uniqueFileName });

            var additionalImagesNames = await AddImagesToFileAsync(productDto.AdditionalImages, rollbackPaths);

            foreach (var img in additionalImagesNames)
            {
                product.ProductImages.Add(new ProductImage { ImageUrl = img });
            }

            var newId = await _productRepository.AddProduct(product);

            if (newId == 0)
            {
                RollbackUploadedFiles(rollbackPaths);
                _logger.LogError("Failed to add Product to the database");
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Create Product: {Id} Successfully", newId);
            return OperationResult<int>.Success(newId);
        }

        public async Task<OperationResult<ProductDto>> GetProductById(int productId, bool? deatils = false, bool? NoTraking = true)
        {
            if (productId <= 0)
            {
                _logger.LogWarning("Invalid data result Id: {Id}", productId);
                return OperationResult<ProductDto>.BadRequest("Invalid data", new List<string> { $"Invalid data result Id: {productId}" });
            }

            var product = await _productQueryService.GetProductById(productId, deatils, NoTraking);

            if (product == null)
            {
                _logger.LogWarning("Product With id: {ProductId} Not Found Or Deleted", productId);
                return OperationResult<ProductDto>.NotFound("Product not found or already deleted.");
            }

            var productDto = _mapper.Map<ProductDto>(product);



            _logger.LogInformation("Result Id: {Id} Get Successfully", productId);
            return OperationResult<ProductDto>.Success(productDto);
        }

        public async Task<OperationResult<PagedResponse<ProductDto>>> GetProductsPaged(int PageNumber, int PageSize,
            string? searchTerm = null, string? Catogrie = null, bool? bypassCache = false, bool? deatails = false)
        {
            if (PageNumber <= 0 && PageSize <= 0)
            {
                _logger.LogWarning("Invalid data result PageNumber or PageSize under 0 or 0");
                return OperationResult<PagedResponse<ProductDto>>.BadRequest("Invalid data", new List<string> { "Invalid data result PageNumber or PageSize under 0 or 0" });
            }

            string CashKey="";

            if (bypassCache==false)
            {
                CashKey = $"Product_Page_{PageNumber}_size_{PageSize}_search_{searchTerm}_cat_{Catogrie}";

                var cachedData = await _cash.GetStringAsync(CashKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    var cachedProducts = JsonSerializer.Deserialize<PagedResponse<ProductDto>>(cachedData);
                    _logger.LogInformation("Get categories from Cache Successfully");
                    return OperationResult<PagedResponse<ProductDto>>.Success(cachedProducts);
                }
            }
 

            var products = await _productQueryService.GetProductsPaged(PageNumber, PageSize, searchTerm, Catogrie, deatails);

            if (products.Data.Count() == 0)
            {
                _logger.LogWarning("Products Not Found PageNumber: {PageNumber} , PageSize: {PageSize} ", PageNumber, PageSize);
                return OperationResult<PagedResponse<ProductDto>>.NotFound("Product not found or already deleted.");
            }

            if(bypassCache == false)
            {
                var cacheOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
                var jsonData = JsonSerializer.Serialize(products);
                await _cash.SetStringAsync(CashKey, jsonData, cacheOptions);
            }


            _logger.LogInformation("Result products Get Successfully");
            return OperationResult<PagedResponse<ProductDto>>.Success(products);
        }

        public async Task RunDailyDiscountCleanupJob()
        {
            int updatedRows = await _productRepository.RemoveAllExpiredDiscountsAsync();

            if (updatedRows > 0)
            {
                var keys = await ClearProductPagesCache.ClearProductPagesCacheAsync(_redis);

                _logger.LogInformation($"Hangfire Job Executed: Removed discounts for {updatedRows} products and cleared {keys} cache keys.");
            }
        }

        
        private void RemoveImagesFromDisk(List<string> fileNames)
        {
            if (fileNames == null || !fileNames.Any()) return;

            string uploadsFolder = GetUploadsFolder();

            foreach (var fileName in fileNames)
            {
                var filePath = Path.Combine(uploadsFolder, fileName);
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Image Delete failed path:{FilePath}", filePath);
                    }
                }
            }
        }

        public void DeleteProductImages(List<ProductImage> imagesToDelete)
        {
            _productRepository.RemvoeProductImages(imagesToDelete);
        }

        public async Task<OperationResult<bool>> UpdateProduct(CreateUpdateProductDto productDto)
        {
            if (!productDto.Id.HasValue || productDto.Id.Value <= 0)
            {
                _logger.LogWarning("Update Product Failed - Invalid Id: {Id}", productDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id", new List<string> { $"Invalid id {productDto.Id}" });
            }

            var existingProduct = await _productRepository.GetProduct(productDto.Id.Value);

            if (existingProduct == null)
            {
                return OperationResult<bool>.NotFound($"Id: {productDto.Id} not found");
            }

            existingProduct.ProductImages ??= new List<ProductImage>();

            var imagesToDeleteFromDb = new List<ProductImage>();
            var oldImagesToDeleteFromDisk = new List<string>();
            var rollbackPathsForNewImages = new List<string>();

            if (productDto.MainImage != null && productDto.MainImage.Length > 0)
            {
                var oldMainImage = existingProduct.ProductImages.FirstOrDefault();
                if (oldMainImage != null)
                {

                    oldImagesToDeleteFromDisk.Add(oldMainImage.ImageUrl);
                    imagesToDeleteFromDb.Add(oldMainImage);
                    existingProduct.ProductImages.Remove(oldMainImage);
                }

                var newMainImageName = await AddSingleImageToFileAsync(productDto.MainImage, rollbackPathsForNewImages);
                existingProduct.ProductImages.Insert(0, new ProductImage { ImageUrl = newMainImageName });
            }

            if (productDto.AdditionalImages != null && productDto.AdditionalImages.Count > 0)
            {
                var oldAdditionalImages = existingProduct.ProductImages.Skip(1).ToList();

                foreach (var oldImg in oldAdditionalImages)
                {
                    if(imagesToDeleteFromDb.Contains(oldImg))
                    {
                         oldImagesToDeleteFromDisk.Add(oldImg.ImageUrl);
                         imagesToDeleteFromDb.Add(oldImg);
                         existingProduct.ProductImages.Remove(oldImg);
                    }
                   
                }

                var newAdditionalImages = await AddImagesToFileAsync(productDto.AdditionalImages, rollbackPathsForNewImages);
                foreach (var newImg in newAdditionalImages)
                {
                    existingProduct.ProductImages.Add(new ProductImage { ImageUrl = newImg });
                }
            }

            if(productDto.RemovedImagesUrls !=null&& productDto.RemovedImagesUrls.Count>0)
            {
                foreach(var img in productDto.RemovedImagesUrls)
                {
                    var imageToRemove = existingProduct.ProductImages.FirstOrDefault(x => x.ImageUrl == img);

                    if(imageToRemove != null)
                    {
                        imagesToDeleteFromDb.Add(imageToRemove);
                        existingProduct.ProductImages.Remove(imageToRemove);

                        oldImagesToDeleteFromDisk.Add(imageToRemove.ImageUrl);
                    }
                }

               
            }

           
            _mapper.Map(productDto, existingProduct);

            if (imagesToDeleteFromDb.Any())
            {
                DeleteProductImages(imagesToDeleteFromDb);
            }

            var result = await _productRepository.UpdateProduct(existingProduct, true); 

            if (!result)
            {
                RollbackUploadedFiles(rollbackPathsForNewImages);
                _logger.LogError("Update Product With id: {Id} Failed", productDto.Id);
                return OperationResult<bool>.Failure();
            }

            if (oldImagesToDeleteFromDisk.Any())
            {
                RemoveImagesFromDisk(oldImagesToDeleteFromDisk);
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

            var product = await _productRepository.GetProduct(productId);

           

            if (product==null)
            {
                _logger.LogWarning("Product With Id: {ProductId} Not Found. Deleted Failed", productId);
                return OperationResult<bool>.NotFound($"Product With Id: {productId} Not Found");
            }

            List<string> additionalImagesUrls = product.ProductImages.OrderBy(x => x.Id).Skip(1).Select(x => x.ImageUrl).ToList();



            var result = await _productRepository.DeleteProduct(productId, additionalImagesUrls);

            if (!result)
            {
                _logger.LogError("An unexpected error occurred while deleting Product with Id {ProductId}.", productId);
                return OperationResult<bool>.Failure();
            }

            if(additionalImagesUrls.Count > 0)
            {
                RemoveImagesFromDisk(additionalImagesUrls);
            }
             

            await ClearProductPagesCache.ClearProductPagesCacheAsync(_redis);

            _logger.LogInformation("Delete ProductId: {ProductId} Successfully", productId);
            return OperationResult<bool>.Success(result);
        }
    }
}