using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Interpreter
{
    public class CheckExpression : IExpression
    {
        public Message InterpretMessage(Message message, string connectionId)
        {
            var response = new Message();
            if (message.Content.StartsWith("/"))
            {
                message.IsCommand = true;
                //response = new Message() {Content = "This is a command", IsCommand = true};
            }
            else
            {
                message.IsCommand = false;
                //response = new Message() { Content = "This is not a command", IsCommand = false};
            }

            return message;
        }
    }
}
