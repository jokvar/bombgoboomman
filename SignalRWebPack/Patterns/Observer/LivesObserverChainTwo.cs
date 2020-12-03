using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.ChainOfResponsibility;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Observer
{
    public class LivesObserverChainTwo : Handler
    {
        public override void Update(ISubject subject)
        {
            Player player = (subject as Session).LastPlayerDamaged;
            Session session = (subject as Session);
            if (!player.IsAlive)
            {
                player.texture = "blank";
                session.AddMessage("Game", new Message() { Content = "<b>" + player.name + "</b> has died!", Class = "table-danger" });
               
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
