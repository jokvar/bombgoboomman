using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Command
{
    class DecreaseBombTickDuration : PowerupCommand
    {
        public DecreaseBombTickDuration(Player _player) : base(_player) { }

        public override void Execute()
        {
            if (_player.bombTickDuration > _player.Defaults.MinBombTickDuration)
            {
                _player.bombTickDuration--;
            }
        }

        public override void Undo()
        {
            if (_player.bombTickDuration < _player.Defaults.MaxBombTickDuration)
            {
                _player.bombTickDuration++;
            }
        }
    }
}
