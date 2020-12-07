using SignalRWebPack.Patterns.Iterator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class PlayerAction : IIterable
    {
        public ActionEnums action { get; set; }
        public string PlayerId { get; set; }
        public PlayerAction(ActionEnums action, string playerId = null)
        {
            this.action = action;
            PlayerId = playerId;
        }
    }
}
