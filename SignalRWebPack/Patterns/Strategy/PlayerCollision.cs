using SignalRWebPack.Logic;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Strategy
{
    class PlayerCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionObject, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            var player = collisionObject as Player;
            ExplosionCell exp1 = new ExplosionCell(explodedAt, player.x, player.y);
            explosions.Add(exp1);
            if (!player.invulnerable && player.IsAlive)
            {
                player.lives--;
                Session s = SessionManager.Instance.GetPlayerSession(player.id);
                s.LastPlayerDamaged = player;
                s.Notify();
                player.invulnerableSince = DateTime.Now;
                player.invulnerableUntil = player.invulnerableSince.AddSeconds(player.invulnerabilityDuration);
                player.invulnerable = true;
            }
            
        }

        public override void PlayerCollisionStrategy(Player player, object collisionObject, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            //do nothing
        }
    }
}
