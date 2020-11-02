using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.Builder;

namespace SignalRWebPack.Models
{
    class Session
    {
        public List<Player> Players { get; set; }
        public List<Powerup> powerups { get; set; }
        public Player Host { get; set; }
        public Map Map { get; set; }
        public string[] PlayerIDs
        {
            get
            {
                if (Players != null)
                {
                    return Players.Select(p => p.id).ToArray();
                }
                return null;
            }
        }
        public string roomCode { get; set; }
        public string id { get; set; }
        public Session()
        {
            powerups = new List<Powerup>();
            Players = new List<Player> { new Player("vardas", "lol koks dar id", 1, 1) };

            MapDirector director = new MapDirector();
            MapBuilder b1 = new ClassicBuilder();
            //MapBuilder b1 = new MLGBuilder();
            director.Construct(b1);
            Map = b1.GetResult();
        }

        public string GenerateRoomCode()
        {
            return "6969";
        }

        public bool RegisterPlayer(string id, bool isHost = false)
        {
            if (Players == null)
            {
                Players = new List<Player>();
            }
            Player host = new Player("wtf name here?", id, 0, 0);
            if (isHost)
            {
                Host = host;
            }
            Players.Add(host);
            return (Players.Count >= 4);
        }

        public int MatchId(string id)
        {
            return Players.IndexOf(Players.Where(p => p.id == id).First());
            // -1 if not found
        }

        public void SetMap(string mapName) => Map = new Map();

        public void InstantiatePowerups() => powerups = new List<Powerup>();
    }
}