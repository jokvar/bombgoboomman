﻿using Newtonsoft.Json;
using SignalRWebPack.Patterns.Iterator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Message : IIterable
    {
        public string Content { get; set; }
        public string Class { get; set; }
        [NonSerialized]
        public string Username;
    }
}
