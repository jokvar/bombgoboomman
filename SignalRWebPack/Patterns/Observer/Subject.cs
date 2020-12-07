using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.ChainOfResponsibility;

namespace SignalRWebPack.Patterns.Observer
{
    public interface ISubject
    {
        // Attach an observer to the subject.
        void Attach(Handler observer);

        // Detach an observer from the subject.
        void Detach(Handler observer);

        // Notify all observers about an event.
        void Notify();
    }
}
