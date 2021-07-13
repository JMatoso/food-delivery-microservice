using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Order.Services.Hub
{
    public class AppHub : Hub<IAppHub>
    {
        public async Task AddUserToGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task SendToGroup(string group, string message)
        {
            await Clients.Group(group).SendNotification(message);
        }

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