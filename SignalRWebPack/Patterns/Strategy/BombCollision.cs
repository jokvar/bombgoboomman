using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Strategy
{
    class BombCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionTarget, List<Explosion> explosions, DateTime explodedAt, List<GameObject> collisionList)
        {
            throw new NotImplementedException();
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<GameObject> collisionList)
        {
            throw new NotImplementedException();
        }
    }
}
