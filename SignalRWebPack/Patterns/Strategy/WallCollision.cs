using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;
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

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            //do nothing
        }
    }
}
