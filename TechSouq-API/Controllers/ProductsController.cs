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
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            var result = await _productService.AddProduct(productDto);

            return this.ToHttpResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("Get")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var result = await _productService.GetProductById(productId);

            return this.ToHttpResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("GetProductsPaged")]
        public async Task<IActionResult> GetProductsPaged(int PageNumber, int PageSize, string? searchTerm = null)
        {
            var result = await _productService.GetProductsPaged(PageNumber, PageSize, searchTerm);

            return this.ToHttpResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            var result = await _productService.UpdateProduct(productDto);

            return this.ToHttpResponse(result);
        }
        
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _productService.DeleteProduct(productId);

            return this.ToHttpResponse(result);
        }
    }
}