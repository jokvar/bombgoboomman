using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Proxy
{
    interface ISessionManager
    {
        public Session GetPlayerSession(string id);
        public bool IsPlayerAlive(string id);
        public List<string> AllSessionCodes();
        public string ActiveSessionCode { get; set; }
        public void FlushSessions();
        public Session GetSession(string code, bool newSession = true);
        public void CreateSession(string mapName, string connectionId);
        public void JoinSession(string roomCode, string connectionId);
    }
}
