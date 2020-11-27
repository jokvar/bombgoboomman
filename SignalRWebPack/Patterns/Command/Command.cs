using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Command
{
    public abstract class PowerupCommand
    {
        protected readonly Player _player;
        public abstract void Execute();
        public abstract void Undo();
        public bool IsPlayerAlive { get { return _player.IsAlive; } }

        public PowerupCommand(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException("Player was null");
            }
            _player = player;
        }
    }
}
