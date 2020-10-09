using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Map
    {
        public string name { get; set; }
        public string author { get; set; }
        public DateTime creationDate { get; set; }
        public string thumbnail { get; set; }
        public Tile[] tiles { get; set; }

        public Map(string mapName)
        {
            //soemthing like the following:
            //return database.Where(Map => Map.name == mapName).First();
        }
    }
}
