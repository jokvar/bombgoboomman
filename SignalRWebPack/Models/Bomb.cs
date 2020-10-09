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

        public Bomb(int x, int y)
        {
            tickDuration = 3; //seconds
            plantedAt = DateTime.Now;
            explodesAt = plantedAt.AddSeconds(tickDuration);
            preExplodeTexture = "black.jpeg";
            this.x = x;
            this.y = y;
        }
    }
}
