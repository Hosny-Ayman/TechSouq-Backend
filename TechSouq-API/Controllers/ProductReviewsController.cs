using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
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
            var result = await _productReviewService.AddReviewAsync(ReviewDto);
            return this.ToHttpResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviewsPaged(int pageNumber, int pageSize, int productId)
        {
            var result = await _productReviewService.GetAllReviewsPagedAsync(pageNumber, pageSize, productId);
            return this.ToHttpResponse(result);
        }


    }
}
