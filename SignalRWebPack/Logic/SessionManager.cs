using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.TemplateMethod;

namespace SignalRWebPack.Logic
{
    public class SessionManager
    {
        //public static SessionManager Instance { get; } = new SessionManager();
        //public static //public static TemplateSessionManager<Session> Instance { get; } = new TemplateSessionManager<Session>(); Instance { get; } = new TemplateSessionManager<Session>();
        public static TemplateSessionManager<Session> Instance => TemplateSessionManager<Session>.Instance;

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

        public void CreateSession(string mapName, string connectionId)
        {
            Session session = Instance.GetSession(null);
            session.RegisterPlayer(connectionId, true);
        }

        public void JoinSession(string roomCode, string connectionId)
        {
            //hardcode
            roomCode = GenerateRoomCode();
            //enmd hardcode
            Session session = Instance.GetSession(roomCode);
            if (session.RegisterPlayer(connectionId)) //if 4 or more players ()
            {
                //the following method has literally no way of existing, current 
                //workaround is setting a single active session in sessionmanager
                //GameLogic.Instance.EnablePlaying(session);
                Instance.ActiveSessionCode = roomCode;
            }
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
            return TemplateSessionManager<Session>.GenerateRoomCode();
        }

    }
}