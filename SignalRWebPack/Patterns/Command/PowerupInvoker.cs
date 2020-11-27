using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Command
{
    public class PowerupInvoker
    {
        List<PowerupCommand> commandStack;

        public PowerupInvoker()
        {
            commandStack = new List<PowerupCommand>();
        }

        public void ExecuteCommand(PowerupCommand PowerupCommand)
        {        
            if (PowerupCommand.IsPlayerAlive)
            {
                commandStack.Add(PowerupCommand);
                PowerupCommand.Execute();
            }
        }

        public void Undo(int times)
        {
            for ( int i = 0; i < times; i++)
            {
                if (commandStack.Count > 0)
                {
                    PowerupCommand lastCommand = commandStack.Last();
                    lastCommand.Undo();
                    commandStack.RemoveAt(commandStack.Count - 1);
                }
            }
        }
    }
}
