using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Command
{
    internal class IncreaseBombCount : PowerupCommand
    {
        public IncreaseBombCount(Player _player) : base(_player) { }

        public override void Execute()
        {
            if (_player.maxBombs < _player.Defaults.MaxBombs)
            {
                _player.maxBombs++;
            }
        }

        public override void Undo()
        {
            if (_player.maxBombs > _player.Defaults.MinBombs)
            {
                _player.maxBombs--;
            }
        }
    }
}
