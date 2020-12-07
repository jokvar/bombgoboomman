using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.State
{
    public class PlaceBombState :PlayerState
    {
        public override void Action(Player player)
        {
            player.PlaceBomb();
            player.playerState = new IdleState();
        }
    }
}
