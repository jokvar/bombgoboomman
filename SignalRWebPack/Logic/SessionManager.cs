using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Logic
{
    class SessionManager //possibly static
    {
        public static SessionManager Instance { get; } = new SessionManager();
        private Dictionary<string, Session> Sessions;
        private readonly object _sessionLock = new object();
        //hardcoded single session
        public List<string> AllSessionCodes { get { return new List<string>(Sessions.Keys); } }
        private string _activeSessionCode;
        public string ActiveSessionCode
        {
            get { lock (_sessionLock) { return _activeSessionCode; } }
            set { lock (_sessionLock) { _activeSessionCode = value; } }
        }
        public SessionManager()
        {
            Sessions = new Dictionary<string, Session>();
        }

        public Session GetSession(string code)
        {
            lock (_sessionLock)
            {
                if (code == null)
                {
                    code = GenerateRoomCode();
                    Session session = new Session();
                    session.roomCode = code;
                    Sessions.Add(code, session);
                    return session;
                }
                return Sessions[code];
            }
        }


        public static string GenerateRoomCode()
        {
            return "6969";
        }

    }
}