using SignalRWebPack.Logic;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Strategy
{
    public class PlayerCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            if (collisionTarget == null || collisionTarget.GetType() != typeof(Player))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'Player'");
            }

            if(explosions == null)
            {
                throw new ArgumentNullException("This method cannot be called when the list 'explosions' is null");
            }

            var player = collisionTarget as Player;
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

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            if (collisionTarget == null || collisionTarget.GetType() != typeof(Player))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'Player'");
            }

            if(player == null)
            {
                throw new ArgumentNullException("This method cannot be called when 'player' is null");
            }
        }
    }
}
