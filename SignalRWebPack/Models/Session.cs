using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public class Session
    {
        public List<Player> Players { get; set; }
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
            Player host = new Player("wtf name here?", id);
            if (isHost)
            {
                Host = host;
            }
            Players.Add(host);
            return (Players.Count >= 4);
        }

        public void SetMap(string mapName) => Map = new Map(mapName);
    }
}
