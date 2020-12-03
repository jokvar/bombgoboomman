using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Iterator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.TemplateMethod
{
    public class TemplateInputManager<T> : TemplateClass<PlayerAction> where T : PlayerAction
    {
        public static TemplateInputManager<PlayerAction> Instance { get; } = new TemplateInputManager<PlayerAction>();
        public int StackSize { get { return queue.GetCount(); } }
        public TemplateInputManager()
        {
            queue = new InputIterator();
        }

        public override bool ItemIsValid(PlayerAction item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            return true;
        }
        public override void InjectConnectionIntoItem(string connectionId, PlayerAction item)
        {
            item.PlayerId = connectionId;
        }

        public override void Log(bool idIsValid, bool itemIsValid)
        {
            if (!idIsValid)
            {
                Console.WriteLine($"PlayerAction id was invalid.");
            }
            if (!itemIsValid)
            {
                Console.WriteLine($"PlayerAction action was invalid.");
            }
        }

        public override PlayerAction ReadOne()
        {
            Lock();
            if (StackSize == 0)
            {
                return null;
            }
            PlayerAction input = queue.RemoveFirst();
            Unlock();
            return input;
        }
        public override void Lock()
        {
            Monitor.Enter(queue.GetLock());
        }

        public override void Unlock()
        {
            Monitor.Exit(queue.GetLock());
        }

        public override PlayerAction Find(string connectionId)
        {
            Lock();
            for (InputIterator i = queue.InputIterator(); i.HasNext;)
            {
                PlayerAction action = i.Next();
                if (action == null)
                {
                    break;
                }
                var id = action.PlayerId;
                if (connectionId == id)
                {
                    Unlock();
                    return action;
                }
            }
            Unlock();
            return null;
        }

        public override IIterator<PlayerAction> Iterator()
        {
            return queue.InputIterator();
        }
    }
}
