using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using TechSouq.API.Extensions;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class SystemSettingsController : ControllerBase
    {
        private readonly SystemSettingsService _service;

        public SystemSettingsController(SystemSettingsService service)
        {
            _service = service;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(SystemSettingDto dto)
        {
            var result = await _service.Add(dto);

            return this.ToHttpResponse(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(SystemSettingDto dto)
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
        [HttpGet("{key}")]
        public async Task<IActionResult> GetSettingByKey(string key)
        {
            var result = await _service.GetSystemSettingsByKey(key);

            return this.ToHttpResponse(result);
        }


    }
}
