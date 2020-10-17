using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Explosion : GameObject
    {
        public int damage { get; set; }
        public int size { get; set; }
        public int explosionDuration { get; set; }
        public DateTime explodedAt { get; set; }

        public Explosion(DateTime explodedAt, int x, int y)
        {
            damage = 1;
            size = 1;
            explosionDuration = 3; //explosion persists for 3 seconds
            this.explodedAt = explodedAt;
            this.x = x;
            this.y = y;
            texture = "#ff0000";
        }
    }
}
