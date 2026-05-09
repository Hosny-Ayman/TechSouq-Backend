using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSouq.API.Extensions;
using TechSouq.Application;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;

namespace TechSouq.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly CategorieService _categorieService;

        public CategoriesController(CategorieService categorieService)
        {
            _categorieService = categorieService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategorie(CategorieDto Categorie)
        {
            var result = await _categorieService.CreateCategorie(Categorie);

            return this.ToHttpResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("Get")]
        public async Task<IActionResult> GetCategorie(int CategorieId)
        {
            var result = await _categorieService.GetCategorie(CategorieId);

            return result.Status switch
            {
                OperationStatus.Success => Ok(result),
                OperationStatus.BadRequest => BadRequest(result),
                OperationStatus.NotFound => NotFound(result),
                OperationStatus.ServerError => StatusCode(500, result),
                _ => StatusCode(500, result)
            };
        }
        [AllowAnonymous]
        [HttpGet("GetAllCategoriesAsync")]
        public async Task<IActionResult> GetAllCategoriesPagedAsync(int PageNumber,int PageSize)
        {
            var result = await _categorieService.GetAllCategoriesPagedAsync(PageNumber, PageSize);

            return result.Status switch
            {
                OperationStatus.Success => Ok(result),
                OperationStatus.BadRequest => BadRequest(result),
                OperationStatus.NotFound => NotFound(result),
                OperationStatus.ServerError => StatusCode(500, result),
                _ => StatusCode(500, result)
            };
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateCategorie(CategorieDto categorieDto)
        {
            var result = await _categorieService.UpdateCategorie(categorieDto);

            return this.ToHttpResponse(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCategorie(int CategorieId)
        {
            var result = await _categorieService.DeleteCategorie(CategorieId);

            return this.ToHttpResponse(result);
        }


    }
}
