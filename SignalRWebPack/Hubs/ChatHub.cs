using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRWebPack.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(long username, string message)
        {
            var x = Context.ConnectionId;
            await Clients.All.SendAsync("messageReceived", username, message);
        }
    }
}