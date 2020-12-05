using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Visitor
{
    public class BombDropVisitor : IVisitor
    {
        public void Visit(GameObject gameObject)
        {
            if (gameObject == null || gameObject.GetType() != typeof(Player))
            {
                throw new ArgumentException("Type of gameObject has to be 'Player'");
            }

            var playerCast = gameObject as Player;
            playerCast.SpawnBomb();
        }
    }
}
