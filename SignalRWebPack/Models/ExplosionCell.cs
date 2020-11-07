using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.Strategy;

namespace SignalRWebPack.Models
{
    class ExplosionCell : GameObject
    {
        private CollisionStrategy _collisionStrategy { get; set; }
        public int damage { get; set; }
        public int explosionDuration { get; set; }
        public DateTime explodedAt { get; set; }
        public DateTime expiresAt { get; set; }
        public bool isExpired { get; set; }

        public ExplosionCell(DateTime explodedAt, int x, int y)
        {
            damage = 1;
            explosionDuration = 2; //explosion persists for 3 seconds
            this.explodedAt = explodedAt;
            expiresAt = explodedAt.AddSeconds(explosionDuration);
            this.x = x;
            this.y = y;
            texture = "explosion";
        }

        public ExplosionCell()
        {
            damage = 1;
            explosionDuration = 3; //explosion persists for 3 seconds
            texture = "explosion";
        }

        public override List<string> GetTextures()
        {
            return textures;
        }

        public void SetValues(DateTime explodedAt, int x, int y)
        {
            this.explodedAt = explodedAt;
            this.x = x;
            this.y = y;
        }

        public void SetCollisionStrategy(CollisionStrategy collisionStrategy)
        {
            _collisionStrategy = collisionStrategy;
        }

        public void ResolveExplosionCollision(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            _collisionStrategy.ExplosionCollisionStrategy(collisionTarget, explosions, explodedAt, collisionList);
        }
    }
}
