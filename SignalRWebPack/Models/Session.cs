﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.Builder;
using SignalRWebPack.Patterns;
using SignalRWebPack.Patterns.Observer;
using SignalRWebPack.Patterns.Command;
using SignalRWebPack.Patterns.Iterator;
using SignalRWebPack.Patterns.ChainOfResponsibility;
using SignalRWebPack.Patterns.Mediator;

namespace SignalRWebPack.Models
{
    public class Session : ISubject, IIterable
    {
        protected IMediator _mediator;
        public List<Player> Players { get; set; }
        public List<Powerup> powerups { get; set; }
        public Player Host { get; set; }
        public Player LastPlayerDamaged { get; set; }
        public Map Map { get; set; }
        //public List<Tuple<string, Message>> Messages { get; set; }
        private MessageIterator Messages;
        public bool GameLoopEnabled { get { return Players.Count == 4; } }
        public bool HasGameEnded = false;
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

        private readonly List<Handler> Observers = new List<Handler>();
        public Session()
        {
            powerups = new List<Powerup>();
            Players = new List<Player>();
            powerupInvoker = new PowerupInvoker();
            MapDirector director = new MapDirector();
            //testing
            MapBuilder b2 = new MLGBuilder();
            director.Construct(b2);
            _ = b2.GetResult();
            //
            MapBuilder b1 = new ClassicBuilder();
            director.Construct(b1);
            Map = b1.GetResult();
            Messages = new MessageIterator();
            var livesObserver = new LivesObserver();
            var livesObserverChainTwo = new LivesObserverChainTwo();
            var livesObserverChainThree = new LivesObserverChainThree();
            var livesObserverChainFour = new LivesObserverChainFour();

            livesObserver.SetNext(livesObserverChainTwo);
            livesObserverChainTwo.SetNext(livesObserverChainThree);
            livesObserverChainThree.SetNext(livesObserverChainFour);

            Attach(livesObserver);
        }

        private readonly object _playerRegistryLock = new object();

        //only for development
        public bool RegisterPlayer(Player player, bool isHost = false)
        {
            lock (_playerRegistryLock)
            {
                Players.Add(player);
                if (isHost)
                {
                    Host = player;
                }
                return Players.Count >= 4;
            }  
        }
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
                PowerupSpawner spawner = new PowerupSpawner();
                PlayerMediator mediator = new PlayerMediator(player, this, spawner);
                player.SetMediator(mediator);
                Players.Add(player);
                return Players.Count >= 4;
            }
        }
        public int MatchId(string id) => Players.IndexOf(Players.FirstOrDefault(p => p.id == id));

        public string Username(string id)
        {
            int index = MatchId(id);
            if (index > -1)
            {
                return Players[index].name;
            }
            return "Anonymous";
        }

        public void SetUsername(string id, string username)
        {
            int index = MatchId(id);
            if (index > -1)
            {
                Players[index].name = username;
            }
        }
        public void Attach(Handler observer) => Observers.Add(observer);

        public void Detach(Handler observer) => this.Observers.Remove(observer);
        public void Notify()
        {
            foreach (var observer in Observers)
            {
                observer.Update(this);
            }
        }

        public void AddMessage(string username, Message message)
        {
            message.Username = username;
            Messages.Add(message);
        }

        public void Remove(Message message)
        {
            Messages.Remove(message);
        }
        public void SetMediator(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public MessageIterator MessageIterator()
        {
            return Messages.Iterator();
        }
    }
}