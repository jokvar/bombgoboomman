using SignalRWebPack.Logic;
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
            Session session = SessionManager.Instance.GetPlayerSession(_player.id);
            if (_player.maxBombs < _player.Defaults.MaxBombs)
            {
                _player.maxBombs++;
                session.AddMessage("Game", new Message() { Content = "<b>" + _player.name + "</b> has picked up a <b>Increase Bomb Count</b> powerup!", Class = "table-info" });
            }
            else
            {
                session.AddMessage("Game", new Message() { Content = "<b>" + _player.name + "</b> already has the <b>Max Bombs</b> allowed.", Class = "table-warning" });
            }
        }
        public override void Undo()
        {
            Session session = SessionManager.Instance.GetPlayerSession(_player.id);
            if (_player.maxBombs > _player.Defaults.MinBombs)
            {
                session.AddMessage("Game", new Message() { Content = "<b>" + _player.name + "</b> has had their <b>Increase Bomb Count</b> powerup <b>undone</b>!", Class = "table-warning" });
                _player.maxBombs--;
            }
        }
    }
}
