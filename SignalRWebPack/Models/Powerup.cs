using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Powerup : GameObject
    {
        public Powerup_type type { get; set; }
        public int existDuration { get; set; }
        public DateTime plantedAt { get; set; }
        public Powerup(Powerup_type type, int x, int y)
        {
            this.type = type;
            this.x = x;
            this.y = y;
            existDuration = 10; //seconds
            plantedAt = DateTime.Now;
            texture = "powerup";
            textures = new List<string>();
            //textures.Add(texture);
        }

        public Powerup()
        {
            existDuration = 10; //seconds
            plantedAt = DateTime.Now;
            texture = "powerup";
        }

        public void SetValues(Powerup_type type, int x, int y)
        {
            this.type = type;
            this.x = x;
            this.y = y;
        }

        public override List<string> GetTextures()
        {
            return textures;
        }

    }
}
