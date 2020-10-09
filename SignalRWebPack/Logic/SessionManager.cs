using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Logic
{
    public class SessionManager //possibly static
    {
        public Session GetSession() //creates and stores new session and returns it
        {
            Session sess = new Session();
            return sess;
        }

        public Session GetSession(string roomCode) //existing by its room code id
        {
            Session sess = new Session();
            return sess;
        }

        public string GenerateRoomCode()
        {
            return "6969";
        }

        public void RegisterPlayer(string id)
        {

        }

        public void RegisterHost(string id) //figure out the logic here but have Host be a refference to the first Player
        {

        }

    }
}
