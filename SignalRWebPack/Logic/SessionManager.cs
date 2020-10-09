using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Logic
{
    public class SessionManager //possibly static
    {
        public Session GetSession()
        {
            Session sess = new Session();
            return sess;
        }

        public Session GetSession(string roomCode)
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

        public void RegisterHost(string id)
        {

        }

    }
}
