using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    class Bomb : GameObject
    {
        public int tickDuration { get; set; }
        public DateTime plantedAt { get; set; }
        public DateTime explodesAt { get; set; }
        public string preExplodeTexture { get; set; }
        public int explosionSizeMultiplier { get; set; }
        public Explosion explosion { get; set; }
        public bool hasExploded { get; set; }

        public Bomb(int x, int y, int tickDuration, int sizeMultiplier)
        {
            this.tickDuration = tickDuration; //seconds
            plantedAt = DateTime.Now;
            explodesAt = plantedAt.AddSeconds(tickDuration);
            preExplodeTexture = "#101010";
            this.texture = "#a0a0a0";
            this.x = x;
            this.y = y;
            explosionSizeMultiplier = sizeMultiplier;
        }

        public Bomb()
        {
            plantedAt = DateTime.Now;
            explodesAt = plantedAt.AddSeconds(tickDuration);
            preExplodeTexture = "#0d0d0d";
        }

        public Explosion GetExplosion()
        {
            
            return explosion;
        }

        public void NullExplosion()
        {
            explosion = null;
        }

        public void SetValues(int x, int y)
        {
            //this.placedBy = placedBy;
            //tickDuration = placedBy.bombTickDuration; //seconds
            this.x = x;
            this.y = y;
        }

        //method for resolving bomb explosions
        public void Explode()
        {
            explosion = new Explosion(x, y, false, explosionSizeMultiplier);
            explosion.SpawnExplosions(x, y);
            hasExploded = true;
        }

        
    }
}
