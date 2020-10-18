using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Strategy
{
    //Strategy abstract class
    abstract class CollisionStrategy
    {
        //How explosion collision with collisionTarget is resolved
        public abstract void ExplosionCollisionStrategy(object collisionTarget, List<Explosion> explosions, DateTime explodedAt, List<GameObject> collisionList);

        //How player collision with collisionTarget is resolved
        public abstract void PlayerCollisionStrategy(Player player, object collisionTarget, List<GameObject> collisionList);

    }
}
