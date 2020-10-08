using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Models
{
    public class Powerup : GameObject
    {
        public Powerup_type type { get; set; }
        public int existDuration { get; set; }
        public DateTime plantedAt { get; set; }
    }
}
