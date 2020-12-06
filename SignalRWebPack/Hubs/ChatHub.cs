using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SignalRWebPack.Models;
using SignalRWebPack.Logic;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SignalRWebPack.Patterns.Singleton;
using System;
using SignalRWebPack.Patterns.Interpreter;

namespace SignalRWebPack.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(Message messageContainer)
        {

            CheckExpression typeCheck = new CheckExpression();
            CommandExpression commandCheck = new CommandExpression();
            MessageExpression messageCheck = new MessageExpression();

            var response = typeCheck.InterpretMessage(messageContainer, Context.ConnectionId);
            if (response.IsCommand)
            {
                response = commandCheck.InterpretMessage(messageContainer, Context.ConnectionId);
            }
            else
            {
                response = messageCheck.InterpretMessage(messageContainer, Context.ConnectionId);
            }

            string sessionCode = SessionManager.Instance.ActiveSessionCode;
            Session session = SessionManager.Instance.GetSession(sessionCode);
            if (response.IsGlobal)
            {
                foreach (string id in session.PlayerIDs)
                {
                    await Clients.Client(id).SendAsync("messageReceived", response.Username, response)
                        .ConfigureAwait(false);
                }
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

        public async Task SendInput(PlayerAction input)
        {
            InputQueueManager.Instance.AddToInputQueue(Context.ConnectionId, input);
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
    }
}
