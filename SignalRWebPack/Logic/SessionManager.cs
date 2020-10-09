using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Logic
{
    public static class SessionManager //possibly static
    {
        public static Session GetSession()
        {
            Session sess = new Session();
            return sess;
        }

        public static Session GetSession(string roomCode)
        {
            Session sess = new Session();
            return sess;
        }

    }
}
