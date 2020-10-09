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
            string[] playerIDs = session.PlayerIDs;
            Clients.Clients(playerIDs[0], playerIDs[1], playerIDs[2], playerIDs[3])
                .SendAsync()
            

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