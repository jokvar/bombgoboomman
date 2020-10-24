using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Strategy
{
    //Context class
    class Collision
    {
        private CollisionStrategy _collisionStrategy;

        public void SetCollisionStrategy(CollisionStrategy collisionStrategy)
        {
            _collisionStrategy = collisionStrategy;
        }

        public void ResolveExplosionCollision(object collisionTarget, List<Explosion> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            _collisionStrategy.ExplosionCollisionStrategy(collisionTarget, explosions, explodedAt, collisionList);
        }

        public void ResolvePlayerCollision(Player player, object collisionTarget, List<Powerup> collisionList)
        {
            _collisionStrategy.PlayerCollisionStrategy(player, collisionTarget, collisionList);
        }
    }
}
