using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Command
{
    class IncreaseExplosionSize : PowerupCommand
    {
        public IncreaseExplosionSize(Player _player) : base(_player) { }

        public override void Execute()
        {
            if (_player.explosionSizeMultiplier < _player.Defaults.MaxExplosionSize)
            {
                _player.explosionSizeMultiplier++;
            }
        }

        public override void Undo()
        {
            if (_player.explosionSizeMultiplier > _player.Defaults.MinExplosionSize)
            {
                _player.explosionSizeMultiplier--;
            }
        }
    }
}
