using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Observer
{
    public class LivesObserver : IObserver
    {
        public void Update(ISubject subject)
        {
            List <Player> players = (subject as Session).Players;

            foreach (Player p in players)
            {
                if(p.lives == 0)
                {
                    (subject as Session).Players.Remove(p);
                }
            }

            int count = (subject as Session).Players.Where(p => p.lives > 0).ToList().Count;

            if (count == 1)
            {
                //write some code to indicate game ending
            }
        }
    }
}
