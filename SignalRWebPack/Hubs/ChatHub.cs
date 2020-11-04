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
            else if (message == "create")
            {
                await CreateSession("test");
            }
            else if (message == "join")
            {
                await JoinSession("test");
            }
            await Clients.All.SendAsync("messageReceived", username, new Message(message, 0));
        }

        public async Task CreateSession(string mapName)
        {
            Session session = SessionManager.Instance.GetSession(null);
            session.RegisterPlayer(Context.ConnectionId, true);
            session.SetMap(mapName); 
        }

        public async Task JoinSession(string roomCode)
        {
            //hardcode
            roomCode = SessionManager.GenerateRoomCode();
            //enmd hardcode
            Session session = SessionManager.Instance.GetSession(roomCode);
            if (session.RegisterPlayer(Context.ConnectionId)) //if 4 or more players ()
            {
                //the following method has literally no way of existing, current 
                //workaround is setting a single active session in sessionmanager
                //GameLogic.Instance.EnablePlaying(session);
                SessionManager.Instance.ActiveSessionCode = roomCode;
            }
        }

        public async Task SendInput(PlayerAction input)
        {
            InputQueueManager.Instance.AddToInputQueue(Context.ConnectionId, input);
        }
    }
}