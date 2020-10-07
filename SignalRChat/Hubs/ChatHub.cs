using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            message += "a"; 
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMap()
        {
            char[] map = "nig".ToCharArray(); 
            await Clients.All.SendAsync("UpdateMap", map);
        }
    }
}
