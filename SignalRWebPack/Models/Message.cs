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
        public string Content { get; set; }
        [JsonProperty("class")]
        public string Class { get; set; }
    }
}
