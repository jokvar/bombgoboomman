using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Box : Tile
    {
        public Box()
        {

        }

        public void SetValues(int x, int y, string texture)
        {
            this.x = x;
            this.y = y;
            this.texture = texture;
        }
    }
}
