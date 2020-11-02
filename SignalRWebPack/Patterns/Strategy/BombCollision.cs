using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Strategy
{
    class BombCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> powerupList)
        {
            var bomb = collisionTarget as Bomb;
            bomb.Explode();
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList)
        {
            throw new NotImplementedException();
        }
    }
}
