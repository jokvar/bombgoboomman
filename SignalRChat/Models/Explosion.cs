using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Models
{
    public class Explosion
    {
        public int damage { get; set; }
        public int size { get; set; }
        public int explosionDuration { get; set; }
        public DateTime explodedAt { get; set; }

    }
}
