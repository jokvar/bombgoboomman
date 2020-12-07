using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Visitor
{
    public interface IVisitor
    {
        void Visit(GameObject gameObject);
    }
}
