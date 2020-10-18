using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.AbstractFactory
{
    public abstract class AbstractFactory
    {
        public abstract GameObject GetObject(String objectType);
    }
}
