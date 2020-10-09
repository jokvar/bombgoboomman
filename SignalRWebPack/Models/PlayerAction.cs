using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class PlayerAction
    {
        public string direction { get; set; }
        public bool bombPlaced { get; set; }

        public PlayerAction(string direction, bool bombPlaced)
        {
            this.direction = direction;
            this.bombPlaced = bombPlaced;
        }
    }
}
