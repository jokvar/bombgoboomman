using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SignalRWebPack.Models;
using SignalRWebPack.Logic;

namespace SignalRWebPack.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(long username, string message)
        {
            var x = Context.ConnectionId;
            await Clients.All.SendAsync("messageReceived", username, message);
        }

        public async Task CreateSession(string mapName)
        {
            Session session = SessionManager.GetSession(); //creates and stores new session and returns it
            string roomCode = session.roomCode;
            session.RegisterPlayer(Context.ConnectionId, true);
            session.SetMap(mapName);
            GameLogic.EnableDrawing(session);
        }



        public async Task JoinSession(string roomCode)
        {
            Session session = SessionManager.GetSession(roomCode);
            if (session.RegisterPlayer(Context.ConnectionId)) //if 4 or more players ()
            {
                //this method v should deal with the posibility of >4 players
                //just discard the last one from the session lmao based
                GameLogic.EnablePlaying(session);
            }
        }

        public async Task SendInput(object data)
        {
            //this queue needs to be thread safe; the send input requests are async (duh)
            GameLogic.AddToInputQueue(Context.ConnectionId, data);
        }
    }
}