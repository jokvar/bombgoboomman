﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.AbstractFactory
{
    public class FactoryProducer
    {
        public static AbstractFactory getFactory(string FactoryType)
        {
            if(FactoryType != null)
            {
                if (FactoryType.Equals("ObjectFactory"))
                {
                    return new ObjectFactory();
                }
                else if (FactoryType.Equals("TileFactory"))
                {
                    return new TileFactory();
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
