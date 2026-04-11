using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechSouq.API.Extensions;
using TechSouq.Application;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly ProductImageService _productImageService;

        public ProductImagesController(ProductImageService productImageService)
        {
            _productImageService = productImageService;
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> CreateProductImage(ProductImageDto dto)
        {
            var result = await _productImageService.AddProductImage(dto);
            return this.ToHttpResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("Get")]
        public async Task<IActionResult> GetProductImage(int id)
        {
            var result = await _productImageService.GetProductImageById(id);
            return this.ToHttpResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductImage(ProductImageDto dto)
        {
            var result = await _productImageService.UpdateProductImage(dto);
            return this.ToHttpResponse(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            var result = await _productImageService.DeleteProductImage(id);
            return this.ToHttpResponse(result);
        }
    }
}