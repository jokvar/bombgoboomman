using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Logic;

namespace SignalRWebPack.Patterns.Interpreter
{
    public class MessageExpression : IExpression
    {
        public Message InterpretMessage(Message message, string connectionId)
        {
            if (message == null)
            {
                throw new ArgumentNullException("Message instance cannot be null");
            }

            if (!message.IsCommand)
            {
                string sessionCode = SessionManager.Instance.ActiveSessionCode;
                string username;
                if (sessionCode == null)
                {
                    username = "anonymous";
                }
                else
                {
                    username = SessionManager.Instance.GetSession(sessionCode).Username(connectionId);
                }
                Message response = new Message() { Content = message.Content, Class = "table-secondary", Username = username};
                return response;
            }

            return new Message() {Content = "Oops, this shouldn't have happened", IsCommand = true, Username = "System"};
        }
    }
}
