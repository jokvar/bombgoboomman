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

        public Map(int[] mapData)
        {
            int index = 0;
            tiles = new Tile[225];
            foreach (int data in mapData)
            {
                int x = index % 15;
                int y = index / 15;

                if (data == 1)
                {
                    tiles[index] = new Wall() { x = x, y = y, texture = "#000000" };
                }
                else if (data == 2)
                {
                    tiles[index] = new Box() { x = x, y = y, texture = "#ff661a" };
                }
                else
                {
                    tiles[index] = new EmptyTile() { x = x, y = y, texture = "#ffffff" };
                }
                index++;
            }
        }
    }
}
