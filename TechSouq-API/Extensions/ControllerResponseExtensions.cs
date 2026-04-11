using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TechSouq.Application;

namespace TechSouq.API.Extensions
{
    public static class ControllerResponseExtensions
    {
        public static IActionResult ToHttpResponse<T>(this ControllerBase controller, OperationResult<T> result)
        {
            return result.Status switch
            {
                OperationStatus.Success => controller.Ok(result),
                OperationStatus.BadRequest => controller.BadRequest(result),
                OperationStatus.NotFound => controller.NotFound(result),
                _ => controller.StatusCode(500, result)
            };
        }

    }
}
