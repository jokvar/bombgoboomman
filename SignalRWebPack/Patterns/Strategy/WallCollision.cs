using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.State;

namespace SignalRWebPack.Patterns.Strategy
{
    public class WallCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            if (collisionTarget == null || collisionTarget.GetType() != typeof(Wall))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'Wall'");
            }

            if(explosions == null)
            {
                throw new ArgumentNullException("This method cannot be called if 'explosions' is null");
            }
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            if (collisionTarget == null || collisionTarget.GetType() != typeof(Wall))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'Wall'");
            }

            if(player == null)
            {
                throw new ArgumentNullException("This method cannot be called when 'player' is null");
            }
            player.playerState = new IdleState();
        }
    }
}
