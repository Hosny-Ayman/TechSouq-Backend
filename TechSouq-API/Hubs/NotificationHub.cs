using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TechSouq.API.Hubs
{
    [Authorize(Roles = "Admin")]
    public class NotificationHub:Hub
    {


    }
}
