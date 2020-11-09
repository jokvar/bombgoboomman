using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.SignalR;
using SignalRWebPack.Hubs;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Observer
{
    public class LivesObserver : IObserver
    {
        public void Update(ISubject subject)
        {
            Player player = (subject as Session).LastPlayerDamaged;
            Session session = (subject as Session);
            if (player.IsAlive)
            {
                session.AddMessage("Game", new Message() { Content = "<b>" + player.name + "</b> has taken damage! Remaining lives: <b>" + player.lives+ "</b>", Class = "table-warning"});
            }
            else
            {
                player.texture = "blank";
                session.AddMessage("Game", new Message() { Content = "<b>" + player.name + "</b> has died!", Class = "table-danger" });
            }

            List<Player> alive = (subject as Session).Players.Where(p => p.IsAlive).ToList();

            if (alive.Count == 1)
            {
                session.AddMessage("Game", new Message() { Content = "<b>" + alive[0].name + "</b> has won!", Class = "table-success" });
                session.HasGameEnded = true;
            }
            else if (alive.Count == 0)
            {
                session.AddMessage("Game", new Message() { Content = "<b>Epic draw!</b>", Class = "table-success" });
                session.HasGameEnded = true;
            }
        }
    }
}
