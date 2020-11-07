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
        private readonly IHubContext<ChatHub> _hub;
        public LivesObserver(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        public void Update(ISubject subject)
        {
            Player p = (subject as Session).LastPlayerDamaged;

            if (p.IsAlive)
            {

            }

            //List<Player> players = (subject as Session).Players;

            //foreach (Player p in players)
            //{
            //    if (p.lives == 0)
            //    {
            //        (subject as Session).Players.Remove(p);
            //    }
            //}

            int count = (subject as Session).Players.Where(p => p.IsAlive).ToList().Count;

            if (count < 2)
            {
                
            }
        }
    }
}
