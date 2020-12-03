using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.ChainOfResponsibility;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Observer
{
    public class LivesObserverChainThree : Handler
    {
        public override void Update(ISubject subject)
        {
            Player player = (subject as Session).LastPlayerDamaged;
            Session session = (subject as Session);
            List<Player> alive = (subject as Session).Players.Where(p => p.IsAlive).ToList();

            if (alive.Count == 1)
            {
                session.AddMessage("Game", new Message() { Content = "<b>" + alive[0].name + "</b> has won!", Class = "table-success" });
                session.HasGameEnded = true;
            }
            else if (next != null)
            {
                 next.Update(subject);
            }
            
        }

    }
}
