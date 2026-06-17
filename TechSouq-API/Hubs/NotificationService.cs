using Microsoft.AspNetCore.SignalR;
using TechSouq.Application.interfaces;

namespace TechSouq.API.Hubs
{
    public class NotificationService : INotificationService
    {

        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNewOrderNotificationAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNewOrder", new
            {
                message = message,
                date = DateTime.Now
            });
        }
    }
}
