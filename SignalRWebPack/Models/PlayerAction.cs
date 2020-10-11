using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class PlayerAction
    {
        public ActionEnums action { get; set; }

        public PlayerAction(ActionEnums action)
        {
            this.action = action;
        }
    }
}
