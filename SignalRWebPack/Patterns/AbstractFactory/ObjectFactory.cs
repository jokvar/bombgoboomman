using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.AbstractFactory
{
    public class ObjectFactory : AbstractFactory
    {
        public override GameObject GetObject(String objectType)
       {
           if (objectType.Equals("explosion"))
           {
               return new ExplosionCell();
           }
           if (objectType.Equals("bomb"))
           {
               return new Bomb();
           }
           if (objectType.Equals("powerup"))
           {
               return new Powerup();
           }
           return null;
       }
    }
}
