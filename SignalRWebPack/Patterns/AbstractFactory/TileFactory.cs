using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.AbstractFactory
{
    public class TileFactory : AbstractFactory
    {
        public override GameObject GetObject(String type)
        {
            if(type != null)
            {
                if (type.Equals("wall"))
                {
                    return new Wall();
                }
                if (type.Equals("box"))
                {
                    return new Box();
                }
                if (type.Equals("empty"))
                {
                    return new EmptyTile();
                }
                throw new ArgumentNullException();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
