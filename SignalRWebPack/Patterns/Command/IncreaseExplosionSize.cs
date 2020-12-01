using SignalRWebPack.Logic;
using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Command
{
    public class IncreaseExplosionSize : PowerupCommand
    {
        public IncreaseExplosionSize(Player _player) : base(_player) { }

        public override void Execute()
        {
            Session session = SessionManager.Instance.GetPlayerSession(_player.id);
            if (_player.explosionSizeMultiplier < DefaultPowerupValues.MaxExplosionSize)
            {
                _player.explosionSizeMultiplier++;
                session.AddMessage("Game", new Message() { Content = "<b>" + _player.name + "</b> has picked up a <b>Increase Explosion Size</b> powerup!", Class = "table-info" });
            }
            else
            {
                session.AddMessage("Game", new Message() { Content = "<b>" + _player.name + "</b> already has the <b>Biggest Explosion Size</b> allowed.", Class = "table-warning" });
            }
        }
        public override void Undo()
        {
            Session session = SessionManager.Instance.GetPlayerSession(_player.id);
            if (_player.explosionSizeMultiplier > DefaultPowerupValues.MinExplosionSize)
            {
                session.AddMessage("Game", new Message() { Content = "<b>" + _player.name + "</b> has had their <b>Increase Explosion Size</b> powerup <b>undone</b>!", Class = "table-warning" });
                _player.explosionSizeMultiplier--;
            }
        }
    }
}
