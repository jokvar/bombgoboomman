using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Builder
{
    class MapDirector
    {
        public void Construct(MapBuilder builder)
        {
            builder.BuildWalls();
            builder.BuildBoxes();
            builder.BuildEmptyTiles();
        }
    }
}
