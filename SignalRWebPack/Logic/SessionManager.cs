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

        public Session GetPlayerSession(string id)
        {
            return Sessions.Values.Where(s => s.PlayerIDs.Contains(id)).FirstOrDefault();
        }

        public SessionManager()
        {
            Sessions = new Dictionary<string, Session>();
            _activeSessionCode = null;
        }

        public Session GetSession(string code)
        {
            lock (_sessionLock)
            {
                if (code == null)
                {
                    code = GenerateRoomCode();
                }
                if (Sessions.ContainsKey(code))
                {
                    return Sessions[code];
                }
                Session session = new Session();
                session.roomCode = code;
                ActiveSessionCode = code;
                Sessions.Add(code, session);
                return session;
            }
        }


        public static string GenerateRoomCode()
        {
            return "6969";
        }

    }
}