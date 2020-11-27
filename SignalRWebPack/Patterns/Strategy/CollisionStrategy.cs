using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;

namespace SignalRWebPack.Patterns.Strategy
{
    //Strategy abstract class
    public abstract class CollisionStrategy
    {
        //How explosion collision with collisionTarget is resolved
        public abstract void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> powerupList);

        //How player collision with collisionTarget is resolved
        public abstract void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker);

    }
}
