﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.Builder;
using SignalRWebPack.Patterns;
using SignalRWebPack.Patterns.Observer;
using SignalRWebPack.Patterns.Command;

namespace SignalRWebPack.Models
{
    public class Session : ISubject
    {
        public List<Player> Players { get; set; }
        public List<Powerup> powerups { get; set; }
        public Player Host { get; set; }
        public Player LastPlayerDamaged { get; set; }
        public Map Map { get; set; }
        public List<Tuple<string, Message>> Messages { get; set; }
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

        private List<IObserver> Observers = new List<IObserver>();
        public Session()
        {
            powerups = new List<Powerup>();
            Players = new List<Player>();
            powerupInvoker = new PowerupInvoker();
            MapDirector director = new MapDirector();
            MapBuilder b1 = new ClassicBuilder();
            //MapBuilder b1 = new MLGBuilder();
            director.Construct(b1);
            Map = b1.GetResult();
            Messages = new List<Tuple<string, Message>>();
            var livesObserver = new LivesObserver();
            Attach(livesObserver);

        }

        private object _playerRegistryLock = new object();

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
        public void Attach(IObserver observer) => Observers.Add(observer);

        public void Detach(IObserver observer) => this.Observers.Remove(observer);
        public void Notify()
        {
            foreach (var observer in Observers)
            {
                observer.Update(this);
            }
        }

        public void AddMessage(string username, Message message)
        {
            Messages.Add(new Tuple<string, Message>(username, message));
        }

        public Tuple<string, Message> ReadOneMessage()
        {      
            if (Messages.Count == 0) //if empty
            {
                return null;
            }
            Tuple<string, Message> message = Messages[0];
            Messages.RemoveAt(0);
            return message;
        }

        public List<Tuple<string, Message>> ReadAllMessages()
        {
            if (Messages.Count == 0) //if empty
            {
                return null;
            }
            List<Tuple<string, Message>> messages = new List<Tuple<string, Message>> (Messages);
            Messages.Clear();
            return messages;
        }
    }
}