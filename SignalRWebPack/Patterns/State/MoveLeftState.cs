using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.State
{
    public class MoveLeftState : PlayerState
    {
        public override void Action(Player player)
        {
            player.x--;
            player.playerState = new IdleState();
        }
    }
}
