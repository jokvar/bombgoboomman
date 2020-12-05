using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Proxy;
using SignalRWebPack.Patterns.TemplateMethod;

namespace SignalRWebPack.Logic
{
    public class SessionManager : ISessionManager
    {
        public static SessionManager Instance { get; } = new SessionManager();
        public bool EnableLogging { get; set; } = false;

        private static TemplateSessionManager<Session> _Instance = TemplateSessionManager<Session>.Instance;
        public string ActiveSessionCode {
            get
            {
                Log($"Accessed {nameof(ActiveSessionCode)}");
                return _Instance.ActiveSessionCode;
            }
            set
            {
                Log($"Set {nameof(ActiveSessionCode)}");
                _Instance.ActiveSessionCode = value;
            }
        }
        

        public List<string> AllSessionCodes()
        {
            Log($"Accessed {nameof(AllSessionCodes)}");
            return _Instance.AllSessionCodes();
        }

        public Session GetPlayerSession(string id)
        {
            Log($"Accessed {nameof(GetPlayerSession)}");
            return _Instance.GetPlayerSession(id);
        }

        public bool IsPlayerAlive(string id)
        {
            Log($"Accessed {nameof(IsPlayerAlive)}");
            return _Instance.IsPlayerAlive(id);
        }

        public Session GetSession(string code, bool newSession = true)
        {
            Log($"Accessed {nameof(GetSession)}");
            return _Instance.GetSession(code, newSession);
        }

        public void FlushSessions()
        {
            Log($"Accessed {nameof(FlushSessions)}");
            _Instance.FlushSessions();
        }

        public static string GenerateRoomCode()
        {
            return TemplateSessionManager<Session>.GenerateRoomCode();
        }

        public void Log(string Message)
        {
            if (EnableLogging)
            {
                //Logger.LogInformation(Message);
                Console.WriteLine(Message);
            }
        }
    }
}