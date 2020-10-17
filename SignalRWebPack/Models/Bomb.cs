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
        public Player placedBy { get; set; }


        public Bomb(int x, int y, Player placedBy)
        {
            this.placedBy = placedBy;
            tickDuration = placedBy.bombTickDuration; //seconds
            plantedAt = DateTime.Now;
            explodesAt = plantedAt.AddSeconds(tickDuration);
            preExplodeTexture = "#0d0d0d";
            this.x = x;
            this.y = y;
        }
    }
}
