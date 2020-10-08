using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Models
{
    public class Bomb : GameObject
    {
        public int tickDuration { get; set; }
        public DateTime plantedAt { get; set; }
        public string preExplodeTexture { get; set; }
        public double chanceToDud { get; set; }
        public string dudTexture { get; set; }
    }
}
