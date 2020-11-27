using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Bomb : GameObject
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
            preExplodeTexture = "wall";
            this.texture = "bomb";
            this.x = x;
            this.y = y;
            explosionSizeMultiplier = sizeMultiplier;
        }

        public Bomb()
        {
            plantedAt = DateTime.Now;
            explodesAt = plantedAt.AddSeconds(tickDuration);
            preExplodeTexture = "bomb";
        }

        public override List<string> GetTextures()
        {
            return textures;
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
            if (!hasExploded)
            {
                hasExploded = true;
                explosion = new Explosion(x, y, false, explosionSizeMultiplier);
                explosion.SpawnExplosions(x, y);
            }
        }        
    }
}
