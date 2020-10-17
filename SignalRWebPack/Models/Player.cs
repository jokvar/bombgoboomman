using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Player
    {
        public int lives { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public bool invulnerable { get; set; }
        public double speedMultiplier { get; set; }
        public string texture { get; set; }
        public bool ready { get; set; }
        public DateTime invulnerableSince { get; set; }
        public int invulnerabilityDuration { get; set; }
        public int maxBombs { get; set; }
        public int activeBombCount { get; set; }
        public int explosionSizeMultiplier { get; set; }
        public int bombTickDuration { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public Player(string name, string id, int x, int y)
        {
            lives = 3;
            this.name = name;
            this.id = id;
            invulnerable = false;
            speedMultiplier = 1;
            texture = "#66ff99";
            ready = false;
            invulnerableSince = DateTime.Now;
            invulnerabilityDuration = 3; //seconds
            bombTickDuration = 5; //seconds
            maxBombs = 1;
            this.x = x;
            this.y = y;
        }
    }
}
