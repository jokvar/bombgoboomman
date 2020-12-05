using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Interpreter
{
    interface IExpression
    {
        Message InterpretMessage(Message message, string connectionId);
    }
}
