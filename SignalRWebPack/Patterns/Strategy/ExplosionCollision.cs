using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Logic;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;

namespace SignalRWebPack.Patterns.Strategy
{
    class ExplosionCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionObject, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            //do nothing
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            var explosion = collisionTarget as ExplosionCell;
            player.x = explosion.x;
            player.y = explosion.y;
            if (!player.invulnerable)
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
    }
}
