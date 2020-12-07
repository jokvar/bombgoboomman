using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Visitor
{

    public class PlayersStructure
    {
        private List<Player> _players = new List<Player>();

        public void Attach(Player player)
        {
            _players.Add(player);
        }

        public void Detach(Player player)
        {
            _players.Remove(player);
        }

        public void Accept(IVisitor visitor)
        {
            foreach (Player p in _players)
            {
                p.Accept(visitor);
            }
        }
    }
}
