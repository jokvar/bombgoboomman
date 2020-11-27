using SignalRWebPack.Logic;
using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Command
{
    public class DecreaseBombTickDuration : PowerupCommand
    {
        public DecreaseBombTickDuration(Player _player) : base(_player) { }

        public override void Execute()
        {
            Session session = SessionManager.Instance.GetPlayerSession(_player.id);
            if (_player.bombTickDuration > _player.Defaults.MinBombTickDuration)
            {
                _player.bombTickDuration--;               
                session.AddMessage("Game", new Message() { Content = "<b>" + _player.name + "</b> has picked up a <b>Decrease Bomb Tick Duration</b> powerup!", Class="table-info"});
            }
            else
            {
                session.AddMessage("Game", new Message() { Content = "<b>" + _player.name + "</b> already has the <b>Shortest Bomb Tick Duration</b> allowed.", Class="table-warning"});
            }
        }

        public override void Undo()
        {
            Session session = SessionManager.Instance.GetPlayerSession(_player.id);
            if (_player.bombTickDuration < _player.Defaults.MaxBombTickDuration)
            {
                session.AddMessage("Game", new Message() { Content = "<b>" + _player.name + "</b> has had their <b>Decrease Bomb Tick Duration</b> powerup <b>undone</b>!", Class="table-warning" });
                _player.bombTickDuration++;
            }
        }
    }
}
