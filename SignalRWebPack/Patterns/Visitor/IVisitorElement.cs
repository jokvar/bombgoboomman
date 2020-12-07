using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Visitor
{
    public interface IVisitorElement
    {
        public  void Accept(IVisitor visitor);
    }
}
