﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Tile : GameObject
    {
        public override List<string> GetTextures()
        {
            return textures;
        }
    }
}
