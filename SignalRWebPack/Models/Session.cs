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

        private List<IObserver> Observers = new List<IObserver>();
        public Session()
        {
            Players = new List<Player>();
            //commented out because right now, session has no player array, unless hard coded
            //var livesObserver = new LivesObserver();
            //this.Attach(livesObserver);
        }

        private object _playerRegistryLock = new object();
        public bool RegisterPlayer(string id, bool isHost = false)
        {
            lock (_playerRegistryLock)
            {
                int index = Players.Count;
                Player player;
                switch (index)
                {
                    case (0):
                        player = new Player("player1", id, 1, 1);
                        break;
                    case (1):
                        player = new Player("player2", id, 3, 1);
                        break;
                    case (2):
                        player = new Player("player3", id, 3, 3);
                        break;
                    case (3):
                        player = new Player("player4", id, 1, 3);
                        break;
                    default:
                        return true;
                }
                if (isHost)
                {
                    Host = player;
                }
                Players.Add(player);
                return Players.Count >= 4;
            }   
        }

        public int MatchId(string id) => Players.IndexOf(Players.Where(p => p.id == id).First());

        public void SetMap(string mapName) => Map = new Map(mapName);

        public void Attach(IObserver observer) => Observers.Add(observer);

        public void Detach(IObserver observer) => this.Observers.Remove(observer);
        public void Notify()
        {
            foreach (var observer in Observers)
            {
                observer.Update(this);
            }
        }
    }
}