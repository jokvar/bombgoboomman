using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Logic
{
    public class SessionManager
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
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("this will never happen");
            }
            return Sessions.Values.Where(s => s.PlayerIDs.Contains(id)).FirstOrDefault();
        }

        public bool IsPlayerAlive(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("this will never happen");
            }
            Session __session = GetPlayerSession(id);
            return __session == null ? false : __session.Players[__session.MatchId(id)].IsAlive;
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
                if (string.IsNullOrWhiteSpace(code))
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

        public void FlushSessions()
        {
            Sessions = new Dictionary<string, Session>();
            _activeSessionCode = null;
        }
        public static string GenerateRoomCode()
        {
            return "6969";
        }

    }
}