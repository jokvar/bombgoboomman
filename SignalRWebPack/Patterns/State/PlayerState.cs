using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.State
{
    public abstract class PlayerState
    {
        public abstract void Action(Player player);
    }
}
