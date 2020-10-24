using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Builder
{
    abstract class MapBuilder
    {
        public abstract void BuildWalls();

        public abstract void BuildBoxes();

        public abstract void BuildEmptyTiles();

        public abstract Map GetResult();
    }
}
