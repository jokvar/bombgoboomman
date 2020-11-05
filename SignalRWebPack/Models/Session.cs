using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.Builder;
using SignalRWebPack.Patterns;
using SignalRWebPack.Patterns.Observer;
using SignalRWebPack.Patterns.Command;

namespace SignalRWebPack.Models
{
    class Session : ISubject
    {
        public List<Player> Players { get; set; }
        public List<Powerup> powerups { get; set; }
        public Player Host { get; set; }
        public Map Map { get; set; }
        //-------Command (Invoker)--------------
        public PowerupInvoker powerupInvoker;
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

            powerups = new List<Powerup>();
            Players = new List<Player>();// { new Player("vardas", "lol koks dar id", 1, 1) };
            powerupInvoker = new PowerupInvoker();
            MapDirector director = new MapDirector();
            //MapBuilder b1 = new ClassicBuilder();
            MapBuilder b1 = new MLGBuilder();
            director.Construct(b1);
            Map = b1.GetResult();

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
                        player = new Player("player2", id, 13, 1);
                        break;
                    case (2):
                        player = new Player("player3", id, 1, 13);
                        break;
                    case (3):
                        player = new Player("player4", id, 13, 13);
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

        //public void SetMap(string mapName) => Map = new Map(mapName);

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