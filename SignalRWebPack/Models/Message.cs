using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Message
    {
        public string content;
        public Message(string content)
        {
            this.content = content;
        }
    }
}
