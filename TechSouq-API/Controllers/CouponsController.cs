using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{

    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    [ApiController]
    public class CouponsController : ControllerBase
    {

        private readonly CouponService _service;

        public CouponsController(CouponService service)
        {
            _service = service;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(CouponDto dto)
        {
            var result = await _service.Add(dto);

            return this.ToHttpResponse(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(CouponDto dto)
        {
            var result = await _service.Update(dto);

            return this.ToHttpResponse(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);

            return this.ToHttpResponse(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCouponByCode(string Code)
        {
            var result = await _service.GetCouponByCode(Code);

            return this.ToHttpResponse(result);
        }

        [HttpGet("GetCouponById")]
        public async Task<IActionResult> GetCouponById(int id)
        {
            var result = await _service.GetCouponById(id);

            return this.ToHttpResponse(result);
        }

        [HttpGet("GetAllCouponsPaged")]
        public async Task<IActionResult> GetAllCouponsPaged(int pageNumber, int pageSize, string? CodeSearch)
        {
            var result = await _service.GetAllCouponsPaged(pageNumber,  pageSize, CodeSearch);

            return this.ToHttpResponse(result);
        }


    }
}
