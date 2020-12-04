using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.SignalR;
using SignalRWebPack.Hubs;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.ChainOfResponsibility;

namespace SignalRWebPack.Patterns.Observer
{
    public class LivesObserver : Handler
    {

        public override void Update(ISubject subject)
        {
            Player player = (subject as Session).LastPlayerDamaged;
            Session session = (subject as Session);
            if (player.IsAlive)
            {
                player.SaveMemento();
                session.AddMessage("Game", new Message() { Content = "<b>" + player.name + "</b> has taken damage! Remaining lives: <b>" + player.lives + "</b>", Class = "table-warning" });
                if (next != null)
                {
                    next.Update(subject);
                }
            }
            else
            {
                if (next != null)
                {
                    next.Update(subject);
                }
            }
        }
    }
}
