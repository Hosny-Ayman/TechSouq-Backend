using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechSouq.API.Extensions;
using TechSouq.Application;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;
        public RolesController(RoleService roleService) => _roleService = roleService;

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRole(RoleDto dto)
        {
            var result = await _roleService.AddRole(dto);
            return this.ToHttpResponse(result);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetRole(int id)
        {
            var result = await _roleService.GetRoleById(id);
            return this.ToHttpResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole(RoleDto dto)
        {
            var result = await _roleService.UpdateRole(dto);
            return this.ToHttpResponse(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRole(id);
            return this.ToHttpResponse(result);
        }
    }
}