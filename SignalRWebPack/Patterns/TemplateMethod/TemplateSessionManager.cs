using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Iterator;
using SignalRWebPack.Patterns.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.TemplateMethod
{
    public class TemplateSessionManager<T> : TemplateClass<Session>, ISessionManager where T : Session
    {
        public static TemplateSessionManager<Session> Instance { get; } = new TemplateSessionManager<Session>();
        public int StackSize { get { return queue.GetCount(); } }
        public string ActiveSessionCode
        {
            get { lock (__lock) { return ActiveSessionCode;  } }
            set { lock (__lock) { ActiveSessionCode = value; } }
        }

        private readonly object __lock;

        public TemplateSessionManager()
        {
            queue = new SessionIterator();
            __lock = queue.GetLock();
        }
        public override bool ItemIsValid(Session item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            return true;
        }

        public override void InjectConnectionIntoItem(string code, Session item)
        {
            item.roomCode = code;
        }

        public override bool IdIsValid(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                return false;
            }
            return connectionId.Length == 4 && connectionId.All(char.IsLetterOrDigit);
        }

        public override void Log(bool idIsValid, bool itemIsValid)
        {
            if (!idIsValid)
            {
                Console.WriteLine($"Session id was invalid.");
            }
            if (!itemIsValid)
            {
                Console.WriteLine($"Session was invalid.");
            }
            if (idIsValid && itemIsValid)
            {
                Console.WriteLine($"Added to Sessions.");
            }
        }
        public override Session ReadOne()
        {
            Lock();
            if (StackSize == 0)
            {
                Unlock();
                return null;
            }
            Session input = queue.First();
            Unlock();
            return input;
        }

        public override void Lock()
        {
            Monitor.Enter(__lock);
        }

        public override void Unlock()
        {
            Monitor.PulseAll(__lock);
            Monitor.Exit(__lock);
        }
        public override Session Find(string code)
        { 
            for (SessionIterator i = queue.SessionIterator(); i.HasNext;)
            {
                Session session = i.Next();
                if (session == null)
                {
                    break;
                }
                var id = session.roomCode;
                if (code == id)
                {
                    return session;
                }
            }
            return null;
        }

        public Session GetSession(string code, bool newSession = true)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                code = GenerateRoomCode();
            }
            Session session = FindById(code);
            if (newSession)
            {
                if (session == null)
                {
                    session = new Session();
                    ActiveSessionCode = code;
                    session.roomCode = code;
                    AddById(session.roomCode, session, forceThreadSafe: true, logResult: false);
                }
            }
            return session;
        }
            
        public override IIterator<Session> Iterator()
        {
            return queue.SessionIterator();
        }

        public Session GetPlayerSession(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("this will never happen");
            }
            for (SessionIterator i = queue.SessionIterator(); i.HasNext;)
            {
                Session session = i.Next();
                if (session == null)
                {
                    break;
                }
                if (session.PlayerIDs.Contains(id))
                {
                    return session;
                }
            }
            return null;
        }

        public bool IsPlayerAlive(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("this will never happen");
            }
            Session __session = GetPlayerSession(id);
            return __session != null && __session.Players[__session.MatchId(id)].IsAlive;
        }

        public void FlushSessions()
        {
            FlushQueue(0);
        }
        public static string GenerateRoomCode()
        {
            return "6969";
        }

        public List<string> AllSessionCodes()
        {
            List<string> codes = new List<string>();
            Lock();
            for (SessionIterator i = queue.SessionIterator(); i.HasNext;)
            {
                Session session = i.Next();
                if (session == null)
                {
                    break;
                }
                codes.Add(session.roomCode);
            }
            Unlock();
            return codes;
        }
    }
}
