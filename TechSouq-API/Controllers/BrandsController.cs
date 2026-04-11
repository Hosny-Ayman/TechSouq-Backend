using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using TechSouq.API.Extensions;
using TechSouq.Application;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ApiController]
    public class BrandsController : ControllerBase
    {

        private readonly BrandService _brandService;

        public BrandsController(BrandService brandService)
        {
            _brandService = brandService;
        }

      
        [HttpPost("Create")]
        public async Task <IActionResult> CreateBrand(BrandDto brand)
        {
            var result = await _brandService.AddBrand(brand);

            return this.ToHttpResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("Get")]
        public async Task<IActionResult> GetBrand(int BrandId)
        {
            var result = await _brandService.GetBrand(BrandId);

            return this.ToHttpResponse(result);
        }
     
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateBrand(BrandDto Brand)
        {
            var result = await _brandService.UpdateBrand(Brand);

            return this.ToHttpResponse(result);
        }

       
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteBrand(int BrandId)
        {
            var result = await _brandService.DeleteBrand(BrandId);

            return this.ToHttpResponse(result);
        }

    }
}
