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
using TechSouq.Domian.Interfaces;

namespace TechSouq.Application.Services
{
    public class ProductReviewService
    {

        private readonly IProductReview _ProductReview;
        private readonly ILogger<ProductReviewDto> _logger;
        private readonly IMapper _mapper;
        private readonly IProductReviewQueryService _productReviewQueryService;

        public ProductReviewService(IProductReview productReview , ILogger<ProductReviewDto> logger, IMapper mapper, IProductReviewQueryService productReviewQueryService)
        {
            _ProductReview = productReview;
            _logger = logger;
            _mapper = mapper;
            _productReviewQueryService = productReviewQueryService;
        }

        public async Task<OperationResult<int>> AddReviewAsync(ProductReviewDto productReviewDto)
        {
            var productReview = _mapper.Map<ProductReview>(productReviewDto);

            var productReviewId = await _ProductReview.AddReviewAsync(productReview);

            if(productReviewId==0)
            {
                _logger.LogError("add Review Failed With UserId: {UserId}", productReview.UserId);
                return OperationResult<int>.Failure();
            }

            _logger.LogInformation("Added Review With Id {Id} Successfully", productReviewId);
            return OperationResult<int>.Success(productReviewId);
        }

        public async Task<OperationResult<bool>> UpdateReviewAsync(ProductReviewDto productReviewDto)
        {
            if (productReviewDto.Id <= 0)
            {
                _logger.LogWarning("Update Review Failed - User Enter Id Under 1 ({ReviewId})", productReviewDto.Id);
                return OperationResult<bool>.BadRequest("Invalid id", new List<string> { $"Invalid id {productReviewDto.Id}" });
            }

            var productReview = _mapper.Map<ProductReview>(productReviewDto);

            var IsScuess = await _ProductReview.UpdateReviewAsync(productReview);

            if (!IsScuess)
            {
                _logger.LogError("Update Review Failed With UserId: {UserId}", productReview.UserId);
                return OperationResult<bool>.Failure();
            }

            _logger.LogInformation("Update Review With  UserId: {UserId}", productReview.UserId);
            return OperationResult<bool>.Success(IsScuess);
        }

        public async Task<OperationResult<PagedResponse<ProductReviewDto>>> GetAllReviewsPagedAsync(int pageNumber, int pageSize, int productId)
        {
            if(pageNumber <= 0 || pageSize <= 0|| productId <= 0)
            {
                _logger.LogWarning("Invalid Data");
                return OperationResult<PagedResponse<ProductReviewDto>>.BadRequest("Invalid id");
            }

            var review = await _productReviewQueryService.GetAllReviewsPagedAsync(pageNumber, pageSize, productId);

            if (review.Data.Count() == 0)
            {
                _logger.LogWarning("review Not Found PageNumber: {PageNumber} , PageSize: {PageSize} ", pageNumber, pageSize);
                return OperationResult<PagedResponse<ProductReviewDto>>.NotFound("Product not found or already deleted.");
            }

            _logger.LogInformation("review for productId : {productId} Get Successfully", productId);
            return OperationResult<PagedResponse<ProductReviewDto>>.Success(review);


        }


    }
}
