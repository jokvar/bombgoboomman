using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SignalRWebPack.Models;
using SignalRWebPack.Logic;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SignalRWebPack.Patterns.Singleton;
using System;
using SignalRWebPack.Patterns.TemplateMethod;

namespace SignalRWebPack.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(Message messageContainer)
        {
            string username = "System";
            if (messageContainer.Content == "/create")
            {
                await CreateSession("test");
                string sessionCode = SessionManager.Instance.ActiveSessionCode;
                Session session = SessionManager.Instance.GetSession(sessionCode);
                username = session.Username(Context.ConnectionId);
                Message response = new Message() { Content = "has created a new Session [" + sessionCode + "].", Class = "table-success" };
                foreach (string id in session.PlayerIDs)
                {
                    await Clients.Client(id).SendAsync("messageReceived", username, response);
                }
            }
            else if (messageContainer.Content == "/join")
            {
                JoinSession("test");
                string sessionCode = SessionManager.Instance.ActiveSessionCode;
                Session session = SessionManager.Instance.GetSession(sessionCode);
                username = session.Username(Context.ConnectionId);
                Message response = new Message() { Content = "has connected to Session [" + sessionCode + "].", Class = "table-info" };
                foreach (string id in session.PlayerIDs)
                {
                    await Clients.Client(id).SendAsync("messageReceived", username, response).ConfigureAwait(false);
                }
            }
            else if (messageContainer.Content.Split(' ')[0] == "/setname")
            {
                string[] newName = messageContainer.Content.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                if (newName.Length == 2)
                {
                    string sessionCode = SessionManager.Instance.ActiveSessionCode;
                    if (sessionCode != null)
                    {
                        Session session = SessionManager.Instance.GetSession(sessionCode);
                        username = session.Username(Context.ConnectionId);
                        session.SetUsername(Context.ConnectionId, newName[1]);
                        Message response = new Message() { Content = "<b>" + username + "</b> has changed their name to <b>" + newName[1] + "</b>.", Class = "table-warning" };
                        foreach (string id in session.PlayerIDs)
                        {
                            await Clients.Client(id).SendAsync("messageReceived", "System", response).ConfigureAwait(false);
                        }
                    }
                }
                
            }
            else if (messageContainer.Content == "/dump")
            {
                await Clients.All.SendAsync("messageReceived", username, new Message() { Content = "this is where diagnostic information would be dumped - if we HAD any", Class = "table-danger" });
            }
            else
            {
                string sessionCode = SessionManager.Instance.ActiveSessionCode;
                if (sessionCode == null)
                {
                    username = "anonymous";
                }
                else
                {
                    username = SessionManager.Instance.GetSession(sessionCode).Username(Context.ConnectionId);
                }            
                Message response = new Message() { Content = messageContainer.Content, Class = "table-secondary" };
                await Clients.All.SendAsync("messageReceived", username, response).ConfigureAwait(false);
            }  
        }

        public async Task CreateSession(string mapName)
        {
            Session session = SessionManager.Instance.GetSession(null);
            session.RegisterPlayer(Context.ConnectionId, true);
        }

        public void JoinSession(string roomCode)
        {
            //hardcode
            roomCode = SessionManager.GenerateRoomCode();
            //enmd hardcode
            Session session = SessionManager.Instance.GetSession(roomCode);
            if (session.RegisterPlayer(Context.ConnectionId)) //if 4 or more players ()
            {
                //the following method has literally no way of existing, current 
                //workaround is setting a single active session in sessionmanager
                //GameLogic.Instance.EnablePlaying(session);
                SessionManager.Instance.ActiveSessionCode = roomCode;
            }
        }

        public async Task SendInput(PlayerAction input)
        {
            //deprecating inputQueueManager
            //InputQueueManager.Instance.AddToInputQueue(Context.ConnectionId, input);
            TemplateInputManager<PlayerAction>.Instance.AddById(Context.ConnectionId, input, forceThreadSafe: false, logResult: false);
        }
    }
}