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
using SignalRWebPack.Patterns.Interpreter;

namespace SignalRWebPack.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(Message messageContainer)
        {
            List<IExpression> expressions = new List<IExpression>();
            expressions.Add(new CheckExpression());
            expressions.Add(new CommandExpression());
            expressions.Add(new MessageExpression());

            Message response = messageContainer;
            foreach (var exp in expressions)
            {
                response = exp.InterpretMessage(response, Context.ConnectionId);
            }

            string sessionCode = SessionManager.Instance.ActiveSessionCode;
            Session session = SessionManager.Instance.GetSession(sessionCode);
            //edge case: if anonymous tries to use /setname it says that he's changed his name
            //but it doesn't actually work
            if (response.IsGlobal)
            {
                await Clients.All.SendAsync("messageReceived", response.Username, response)
                    .ConfigureAwait(false);
            }
            else
            {
                var name = session.Players.Where(p => p.name == response.Username).FirstOrDefault();
                if (name == null)
                {
                    throw new Exception("This really do be a bruh moment.");
                }
                await Clients.Client(name.id).SendAsync("messageReceived", response.Username, response)
                    .ConfigureAwait(false);
            }
            

        }

        public async Task ReviveInput(PlayerAction input)
        {
            Session __session = SessionManager.Instance.GetPlayerSession(Context.ConnectionId);
            if (!SessionManager.Instance.IsPlayerAlive(Context.ConnectionId))
            {
                Player p = __session.Players[__session.MatchId(Context.ConnectionId)];
                __session.AddMessage("Game", new Message() { Content = "<b>" + p.name + "</b> has cheated! ", Class = "table-danger" });
                p.RestoreMemento();
            }
        }
        public async Task SendInput(PlayerAction input)
        {
            TemplateInputManager<PlayerAction>.Instance.AddById(Context.ConnectionId, input, forceThreadSafe: false, logResult: false);
        }
    }
}
