using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Order.Services.Hub
{
    public class AppHub : Hub<IAppHub>
    {
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Main App");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Main App");
            await base.OnDisconnectedAsync(exception);
        }
    }
}