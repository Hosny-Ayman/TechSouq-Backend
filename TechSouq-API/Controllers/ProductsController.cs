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
        public async Task<IActionResult> CreateProduct([FromForm] CreateUpdateProductDto dto)
        {
            var result = await _productService.AddProduct(dto);
            return this.ToHttpResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("Get")]
        public async Task<IActionResult> GetProduct(int productId,bool? deatils =false)
        {
            var result = await _productService.GetProductById(productId, deatils);

            return this.ToHttpResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("GetProductsPaged")]
        public async Task<IActionResult> GetProductsPaged(int PageNumber, int PageSize, string? searchTerm = null,
            string? Catogrie = null,bool? bypassCache=false, bool? deatails = false)
        {
            var result = await _productService.GetProductsPaged(PageNumber, PageSize, searchTerm, Catogrie, bypassCache, deatails);

            return this.ToHttpResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromForm] CreateUpdateProductDto productDto)
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