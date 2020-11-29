using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Logic;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;

namespace SignalRWebPack.Patterns.Strategy
{
    public class ExplosionCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            if (collisionTarget == null || collisionTarget.GetType() != typeof(ExplosionCell))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'ExplosionCell'");
            }

            if(explosions == null)
            {
                throw new ArgumentNullException("The reference to the 'explosions' list cannot be null");
            }

            var emptyTile = collisionTarget as ExplosionCell;
            ExplosionCell exp1 = new ExplosionCell(explodedAt, emptyTile.x, emptyTile.y);
            explosions.Add(exp1);
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            if (collisionTarget == null || collisionTarget.GetType() != typeof(ExplosionCell))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'ExplosionCell'");
            }

            if(player == null)
            {
                throw new ArgumentNullException("This method cannot be called when 'player' is null");
            }

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
