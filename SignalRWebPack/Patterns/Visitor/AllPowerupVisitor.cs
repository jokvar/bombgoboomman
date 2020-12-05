using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Logic;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;

namespace SignalRWebPack.Patterns.Visitor
{
    public class AllPowerupVisitor : IVisitor
    {
        private static Session session = SessionManager.Instance.GetSession(SessionManager.Instance.ActiveSessionCode);
        public void Visit(GameObject gameObject)
        {
            if (gameObject == null || gameObject.GetType() != typeof(Player))
            {
                throw new ArgumentException("Type of gameObject has to be 'Player'");
            }
            var playerCast = gameObject as Player;
            var invoker = session.powerupInvoker;

            PowerupCommand command = new DecreaseBombTickDuration(playerCast);
            invoker.ExecuteCommand(command);

            command = new IncreaseExplosionSize(playerCast);
            invoker.ExecuteCommand(command);

            command = new IncreaseBombCount(playerCast);
            invoker.ExecuteCommand(command);
        }
    }
}
