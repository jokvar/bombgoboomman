using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.Observer;

namespace SignalRWebPack.Patterns.ChainOfResponsibility
{
    //This looks empty but most of the logic is in LivesObserver
    public abstract class Handler
    {
        protected Handler next;

        public void SetNext(Handler next)
        {
            this.next = next;
        }

        public abstract void Update(ISubject subject);
    }
}
