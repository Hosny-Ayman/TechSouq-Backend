using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewsController : ControllerBase
    {

        private readonly ProductReviewService _productReviewService;

        public ProductReviewsController(ProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductReview(ProductReviewDto ReviewDto)
        {
            ReviewDto.UserId = User.GetUserId();
            var result = await _productReviewService.AddReviewAsync(ReviewDto);
            return this.ToHttpResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductReview(ProductReviewDto ReviewDto)
        {
            ReviewDto.UserId = User.GetUserId();
            var result = await _productReviewService.UpdateReviewAsync(ReviewDto);
            return this.ToHttpResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllReviewsPaged")]
        public async Task<IActionResult> GetAllReviewsPaged(int pageNumber, int pageSize, int productId)
        {
            var result = await _productReviewService.GetAllReviewsPagedAsync(pageNumber, pageSize, productId);
            return this.ToHttpResponse(result);
        }

        
        [HttpGet("{productId}")]
        public async Task<IActionResult> CanUserReviewProduct(int productId)
        {
            var UserId = User.GetUserId();

            var result = await _productReviewService.CanUserReviewProductAsync(UserId, productId);
            return this.ToHttpResponse(result);
        }

        [HttpGet("CanUserEditHisReview")]
        public async Task<IActionResult> CanUserEditHisReview(int productId)
        {
            var UserId = User.GetUserId();

            var result = await _productReviewService.CanUserEditHisReview(productId, UserId);
            return this.ToHttpResponse(result);
        }


    }
}
