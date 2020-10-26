using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Strategy
{
    class ExplosionCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionObject, List<Explosion> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            //do nothing
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList)
        {
            var explosion = collisionTarget as Explosion;
            player.x = explosion.x;
            player.y = explosion.y;
            player.lives--;
            player.invulnerableSince = DateTime.Now;
            player.invulnerable = true;
        }
    }
}
