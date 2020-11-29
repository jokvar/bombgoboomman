using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Strategy
{
    public class EmptyTileCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            if (collisionTarget == null || collisionTarget.GetType() != typeof(EmptyTile))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'EmptyTile'");
            }
            var emptyTile = collisionTarget as EmptyTile;

            if(explosions == null)
            {
                throw new ArgumentNullException("This method cannot be called when the list of 'explosions' is null");
            }

            ExplosionCell exp1 = new ExplosionCell(explodedAt, emptyTile.x, emptyTile.y);
            explosions.Add(exp1);
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            if (collisionTarget == null || collisionTarget.GetType() != typeof(EmptyTile))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'EmptyTile'");
            }

            if(player == null)
            {
                throw new ArgumentNullException("This method cannot be called when 'player' is null");
            }
            var emptyTile = collisionTarget as EmptyTile;
            player.x = emptyTile.x;
            player.y = emptyTile.y;
        }
    }
}
