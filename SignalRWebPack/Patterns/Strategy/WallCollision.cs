using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Strategy
{
    class WallCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            //do nothing
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<GameObject> collisionList)
        {
            //do nothing
        }
    }
}
