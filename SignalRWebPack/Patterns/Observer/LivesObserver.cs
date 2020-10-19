using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Observer
{
    public class LivesObserver : IObserver
    {
        
        LivesObserver()
        {
            
        }
        public void Update(ISubject subject)
        {
            if ((subject as Player).lives < 1)
            {
                Console.WriteLine("ConcreteObserverA: Reacted to the event.");
            }
        }
    }
}
