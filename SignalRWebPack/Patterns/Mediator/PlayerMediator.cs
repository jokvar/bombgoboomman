using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Mediator
{
    public class PlayerMediator : IMediator
    {
        private Player Player;
        private Session Session;
        private PowerupSpawner PowerupSpawner;


        public PlayerMediator(Player p, Session s, PowerupSpawner spawner)
        {
            this.Player = p;
            this.Player.SetMediator(this);
            this.Session = s;
            this.Session.SetMediator(this);
            this.PowerupSpawner = spawner;
            this.PowerupSpawner.SetMediator(this);

        }

        public void Notify(string ev)
        {
            if (ev == "PowerupSpawned")
            {
                Session.AddMessage("Game", new Message() { Content = "Powerup has spawned in the players place!", Class = "table-info" });
            }
            if (ev == "PlayerDied")
            {
                this.PowerupSpawner.SpawnPowerup(Player, Session.powerups);
            }
        }
    }
}
