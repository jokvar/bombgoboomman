using SignalRWebPack.Models;
using SignalRWebPack.Patterns.AbstractFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Builder
{
    class ClassicBuilder : MapBuilder
    {
        private Map _map = new Map();
        TileFactory tFactory = FactoryProducer.getFactory("TileFactory") as TileFactory;
        

        public override void BuildWalls()
        {
            _map.tiles = new Tile[225];
            //Coord conversion to tile index = 15 * y + x
            for (int y = 0; y < 15; y++)
            {
                for (int x = 0; x < 15; x++)
                {
                    int index = 15 * y + x;
                    //populating border walls
                    if (y == 0 || y == 14 || x == 0 || x == 14)
                    {
                        Wall w = tFactory.GetObject("wall") as Wall;
                        w.x = x;
                        w.y = y;
                        w.texture = "wall";
                        _map.tiles[index] = w;
                    }
                    //populating walls every second row and cell
                    else if(y % 2 == 0)
                    {
                        if(x % 2 == 0)
                        {
                            Wall w = tFactory.GetObject("wall") as Wall;
                            w.x = x;
                            w.y = y;
                            w.texture = "wall";
                            _map.tiles[index] = w;
                        }
                    }

                }
            }
        }

        public override void BuildBoxes()
        {
            //initialize empty tiles on every corner of the map where players will be spawned
            _map.tiles[16] = new EmptyTile() { x = 1, y = 1, texture = "blank" };
            _map.tiles[17] = new EmptyTile() { x = 2, y = 1, texture = "blank" };
            _map.tiles[31] = new EmptyTile() { x = 1, y = 2, texture = "blank" };

            _map.tiles[27] = new EmptyTile() { x = 12, y = 1, texture = "blank" };
            _map.tiles[28] = new EmptyTile() { x = 13, y = 1, texture = "blank" };
            _map.tiles[43] = new EmptyTile() { x = 13, y = 2, texture = "blank" };

            _map.tiles[181] = new EmptyTile() { x = 1, y = 12, texture = "blank" };
            _map.tiles[196] = new EmptyTile() { x = 1, y = 13, texture = "blank" };
            _map.tiles[197] = new EmptyTile() { x = 2, y = 13, texture = "blank" };

            _map.tiles[193] = new EmptyTile() { x = 13, y = 12, texture = "blank" };
            _map.tiles[207] = new EmptyTile() { x = 12, y = 13, texture = "blank" };
            _map.tiles[208] = new EmptyTile() { x = 13, y = 13, texture = "blank" };

            for (int y = 0; y < 15; y++)
            {
                for (int x = 0; x < 15; x++)
                {
                    int index = 15 * y + x;
                    if(_map.tiles[index] == null)
                    {
                        var rand = new Random();
                        if(rand.Next(100) < 70)
                        {
                            Box b = tFactory.GetObject("box") as Box;
                            b.x = x;
                            b.y = y;
                            b.texture = "box";
                            _map.tiles[index] = b;
                        }
                    }
                    
                }
            }
        }

        public override void BuildEmptyTiles()
        {
            for (int y = 0; y < 15; y++)
            {
                for (int x = 0; x < 15; x++)
                {
                    int index = 15 * y + x;
                    if(_map.tiles[index] == null)
                    {
                        EmptyTile e = tFactory.GetObject("empty") as EmptyTile;
                        e.x = x;
                        e.y = y;
                        e.texture = "blank";
                        _map.tiles[index] = e;
                    }
                }
            }
        }

        public override Map GetResult()
        {
            return _map;
        }
    }
}
