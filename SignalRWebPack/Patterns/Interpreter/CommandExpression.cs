using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;
using SignalRWebPack.Logic;

namespace SignalRWebPack.Patterns.Interpreter
{
    public class CommandExpression : IExpression
    {
        private static SessionManager manager = SessionManager.Instance;
        public Message InterpretMessage(Message message, string connectionId)
        {
            if (message == null)
            {
                throw new ArgumentNullException("Message instance cannot be null");
            }
            if (message.IsCommand)
            {
                if (message.Content[0].ToString() == "/")
                {
                    string content = message.Content.Substring(1);
                    var parts = content.Split(' ');
                    Message response = new Message();
                    if (parts.Length == 1)
                    {
                        if (parts[0] == "create")
                        {
                            SessionManager.Instance.CreateSession("test", connectionId);
                            string sessionCode = SessionManager.Instance.ActiveSessionCode;
                            Session session = SessionManager.Instance.GetSession(sessionCode);
                            var username = session.Username(connectionId);
                            response = new Message()
                            {
                                Content = "has created a new Session [" + sessionCode + "].", Class = "table-success",
                                Username = username
                            };
                        }
                        else if (parts[0] == "join")
                        {
                            SessionManager.Instance.JoinSession("test", connectionId);
                            string sessionCode = SessionManager.Instance.ActiveSessionCode;
                            Session session = SessionManager.Instance.GetSession(sessionCode);
                            var username = session.Username(connectionId);
                            response = new Message()
                            {
                                Content = "has connected to Session [" + sessionCode + "].", Class = "table-info",
                                Username = username
                            };
                        }
                        //TO DO at some point maybe, probably not
                        //else if (parts[0] == "help")
                        //{
                        //    
                        //}
                        //else if (parts[0] == "dump")
                        //{
                        //    
                        //}
                        else
                        {
                            string sessionCode = SessionManager.Instance.ActiveSessionCode;
                            Session session = SessionManager.Instance.GetSession(sessionCode);
                            var username = session.Username(connectionId);
                            response = new Message()
                            {
                                Content = "The command " + message.Content + " is invalid.",
                                Class = "table-warning",
                                IsCommand = false, IsGlobal = false, Username = username
                            };
                        }
                    }
                    else if(parts.Length > 1)
                    {
                        if (parts[0] == "setname")
                        {
                            string sessionCode = SessionManager.Instance.ActiveSessionCode;
                            if (sessionCode != null)
                            {
                                Session session = SessionManager.Instance.GetSession(sessionCode);
                                var username = session.Username(connectionId);

                                foreach (var player in session.Players)
                                {
                                    if (player.name == parts[1])
                                    {
                                        response = new Message()
                                        {
                                            Content = "This name is already taken.", IsGlobal = false,
                                            Class = "table-warning",
                                            Username = username
                                        };
                                        return response;
                                    }
                                }
                                session.SetUsername(connectionId, parts[1]);
                                response = new Message()
                                {
                                    Content = "<b>" + username + "</b> has changed their name to <b>" + parts[1] +
                                              "</b>.",
                                    Class = "table-warning",
                                    Username = "System"
                                };
                            }
                        }
                    }
                    else
                    {
                        string sessionCode = SessionManager.Instance.ActiveSessionCode;
                        Session session = SessionManager.Instance.GetSession(sessionCode);
                        var username = session.Username(connectionId);
                        response = new Message()
                        {
                            Content = "The command \"" + message.Content + "\" is invalid.",
                            Class = "table-warning",
                            IsCommand = false, IsGlobal = false, Username = username
                        };
                    }

                    return response;
                }
            }
            return new Message() {Content = "Oops, this shouldn't have happened.", IsCommand = false, Username = "System"};
        }
        
    }
}
