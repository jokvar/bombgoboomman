using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SignalRWebPack.Models;
using SignalRWebPack.Logic;
using System.Collections.Generic;
using System.Linq;

namespace SignalRWebPack.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(long username, string message)
        {
            if (message == "test")
            {
                string ligma = "ligma";
                //await Clients.Client(Context.ConnectionId).SendAsync("test", ligma);
                await Clients.Client(Context.ConnectionId).SendAsync("test2", ligma);
                //await Clients.All.SendAsync("test3", ligma);
            }
            await Clients.All.SendAsync("messageReceived", username, message);
        }

        public async Task CreateSession(string mapName)
        {
            Session session = SessionManager.GetSession(); //creates and stores new session and returns it
            string roomCode = session.roomCode;
            session.RegisterPlayer(Context.ConnectionId, true);
            session.SetMap(mapName);
            //GameLogic.EnableDrawing(session);
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
            //this queue needs to be thread safe; the send input requests are async (duh)
            //GameLogic.AddToInputQueue(Context.ConnectionId, input);
        }

        public async Task StoreDrawData(string[] playerIDs, Map map, List<Player> players, List<Bomb> bombs, List<Powerup> powerups, List<Explosion> explosions, List<Message> messages)
        {
            await Clients.Clients(playerIDs[0], playerIDs[1], playerIDs[2], playerIDs[3]).SendAsync("StoreDrawData", map, players, bombs, powerups, explosions, messages);
        }

        public async Task StartPlaying(string[] playerIDs)
        {
            await Clients.Clients(playerIDs[0], playerIDs[1], playerIDs[2], playerIDs[3]).SendAsync("StartPlaying");
        }
    }
}