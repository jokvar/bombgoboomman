using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SignalRWebPack.Models;
using SignalRWebPack.Logic;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SignalRWebPack.Patterns.Singleton;

namespace SignalRWebPack.Hubs
{
    public class ChatHub : Hub
    {

        public async Task NewMessage(string username, Message messageContainer)
        
        {
            string message = messageContainer.content;
            if (message == "test")
            {
                string ligma = "ligma";
                await Clients.Client(Context.ConnectionId).SendAsync("test2", ligma);
            }
            await Clients.All.SendAsync("messageReceived", username, new Message(message, 0));
        }

        public async Task CreateSession(string mapName)
        {
            Session session = SessionManager.GetSession(); //creates and stores new session and returns it
            string roomCode = session.roomCode;
            session.RegisterPlayer(Context.ConnectionId, true);
            //session.SetMap();
        }

        public async Task JoinSession(string roomCode)
        {
            Session session = SessionManager.GetSession(roomCode);
            if (session.RegisterPlayer(Context.ConnectionId)) //if 4 or more players ()
            {
                //this method v should deal with the posibility of >4 players
                //just discard the last one from the session lmao based
                //GameLogic.EnablePlaying(session);
            }
        }

        public async Task SendInput(PlayerAction input)
        {
            InputQueueManager.Instance.AddToInputQueue(Context.ConnectionId, input);
        }
    }
}