using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(long username, string message)
        {
            var x = Context.ConnectionId;
            await Clients.All.SendAsync("messageReceived", username, message);
        }

        public async Task CreateSession()
        {

        }

        public async Task JoinSession(string roomCode)
        {

        }

        public async Task SendInput(object data)
        {
            
        }

        public void Test()
        {
            
        }
    }
}