using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Wall : Tile
    {

        public Wall()
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
