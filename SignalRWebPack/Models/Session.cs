using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns;
using SignalRWebPack.Patterns.Observer;

namespace SignalRWebPack.Models
{
    public class Session : ISubject
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

        private List<IObserver> Observers = new List<IObserver>();
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

        public void SetMap(string mapName) => Map = new Map(mapName);

        public void Attach(IObserver observer)
        {
            this.Observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this.Observers.Remove(observer);
        }
        public void Notify()
        {
            foreach (var observer in Observers)
            {
                observer.Update(this);
            }
        }
    }
}