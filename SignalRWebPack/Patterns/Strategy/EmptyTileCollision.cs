using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Strategy
{
    class EmptyTileCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            var emptyTile = collisionTarget as EmptyTile;
            ExplosionCell exp1 = new ExplosionCell(explodedAt, emptyTile.x, emptyTile.y);
            explosions.Add(exp1);
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<GameObject> collisionList)
        {
            var emptyTile = collisionTarget as EmptyTile;
            player.x = emptyTile.x;
            player.y = emptyTile.y;
        }
    }
}
