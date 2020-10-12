using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Message
    {
        [JsonProperty("content")]
        public string content { get; set; }
        [JsonProperty("code")]
        public double code { get; set; }
        public Message(string content, double code)
        {
            this.content = content;
            this.code = code;
        }
    }
}
