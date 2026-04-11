using Microsoft.AspNetCore.Authorization;
using TechSouq.API.Extensions;
namespace TechSouq.API.Policies
{
    public class ResourceOwnerHandler : AuthorizationHandler<ResourceOwnerRequirement, int>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOwnerRequirement requirement, int targetUserId)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var loggedInUserId = context.User.GetUserId();

            if (loggedInUserId == targetUserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}