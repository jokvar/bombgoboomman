using SignalRWebPack.Logic;
using SignalRWebPack.Models;
using SignalRWebPack.Models.TransportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using Microsoft.Extensions.Logging;
using SignalRWebPack.Patterns.Singleton;
using SignalRWebPack.Patterns.FactoryMethod;
using SignalRWebPack.Patterns.Strategy;
using SignalRWebPack.Patterns.Builder;
using SignalRWebPack.Patterns.Command;
using SignalRWebPack.Patterns.Decorator;

namespace SignalRWebPack.Logic
{
    public interface IGameLogic
    {
        Task GameLoop(CancellationToken cancellationToken);
    }
    public class GameLogic : IGameLogic
    {
        private readonly IHubContext<ChatHub> _hub;

        private object _sessionLock = new object();
        private Session session = null;
        private List<Player> players;// = session.Players;
        private Map gameMap;// = session.Map;
        private List<Bomb> bombs = new List<Bomb>();
        private List<ExplosionCell> explosions = new List<ExplosionCell>();
        private List<Powerup> powerups;// = session.powerups;
        private int mapDimensions = 15;

        //-------FactoryMethod--------------
        // Creators: 
        BombTransportCreator bombCreator;
        PowerupTransportCreator powerupCreator; 
        ExplosionTransportCreator explosionCreator;
        //-------Command (Invoker)--------------
        PowerupInvoker powerupInvoker;
        private readonly ILogger _logger;
 
        public GameLogic(IHubContext<ChatHub> hub, ILogger<GameLogic> logger)
        {
            _hub = hub;
            _logger = logger;
            bombCreator = new BombTransportCreator();
            powerupCreator = new PowerupTransportCreator();
            explosionCreator = new ExplosionTransportCreator();
        }
        public void SpawnPlayers()
        {
            players[0].x = 1;
            players[0].y = 1;

            players[1].x = mapDimensions - 1;
            players[1].y = 1;

            players[2].x = 1;
            players[2].y = mapDimensions - 1;

            players[3].x = mapDimensions - 1;
            players[3].y = mapDimensions - 1;
        }

        public async Task GameLoop(CancellationToken cancellationToken)
        {
            string ActiveSessionCode;
            while (true)
            {
                lock (_sessionLock)
                {
                    if (SessionManager.Instance.ActiveSessionCode != null)
                    {
                        ActiveSessionCode = SessionManager.Instance.ActiveSessionCode;
                        break;
                    }
                }
            }
            while (true)
            {
                lock (_sessionLock)
                {
                    if (SessionManager.Instance.GetSession(ActiveSessionCode).GameLoopEnabled)
                    {
                        session = SessionManager.Instance.GetSession(SessionManager.Instance.ActiveSessionCode);
                        break;
                    }
                }
            }

            powerupInvoker = session.powerupInvoker;
            List<Message> messages = new List<Message>();
            gameMap = session.Map;
            players = session.Players;
            powerups = session.powerups;

            while (!cancellationToken.IsCancellationRequested)
            {
                //_logger.LogInformation("iteration");

                //example action dequeing
                Tuple<string, PlayerAction> tuple;
                while ((tuple = InputQueueManager.Instance.ReadOne()) != null) //deleted when read
                {
                    string playerId = tuple.Item1;
                    PlayerAction action = tuple.Item2;
                    ProcessAction(action, playerId);
                }
                //end example
                //explosions[0].x = (i++ % 5) + 1;
                //_logger.LogInformation("sending draw data");

                FormDrawingObjectLists();
                await StoreDrawData(session.PlayerIDs, gameMap, players, bombs, session.powerups, explosions, messages);
                await Task.Delay(60); // 😎😎😎😎
                explosions = new List<ExplosionCell>();
                bombs = new List<Bomb>();
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].CheckBombTimers();
                    players[i].CheckInvulnerabilityPeriods();
                    for (int j = 0; j < players[i].bombs.Count; j++)
                    {
                        if(players[i].bombs[j].explosion != null)
                        {
                            players[i].bombs[j].explosion.CheckExplosionTimers();
                            if (players[i].bombs[j].explosion.isExpired)
                            {
                                players[i].RefreshBombList(players[i].bombs[j]);
                            }
                        }
                    }
                }



                //CheckExplosionTimers();
                //CheckInvulnerabilityPeriods();
                //CheckPowerupTimers();
                //client.StoreDrawData(session.PlayerIDs, gameMap, players, bombs, powerups, explosions, messages);
                
                //await Broadcast(new Message("ligma lol", 1)); 
            }

            throw new NotImplementedException("Reached end of game loop");
        }

        public void ProcessAction(PlayerAction playerAction, string id)
        {
            int requestIndex = session.MatchId(id);
            //storing current coordinates
            int x = players[requestIndex].x;
            int y = players[requestIndex].y;
            switch (playerAction.action)
            {
                case ActionEnums.Up:
                    PlayerCheckAdjacentTiles(x, y - 1, requestIndex);
                    break;

                case ActionEnums.Down:
                    PlayerCheckAdjacentTiles(x, y + 1,requestIndex);
                    break;

                case ActionEnums.Right:
                    PlayerCheckAdjacentTiles(x + 1, y, requestIndex);
                    break;

                case ActionEnums.Left:
                    PlayerCheckAdjacentTiles(x - 1, y, requestIndex);
                    break;

                case ActionEnums.PlaceBomb:
                    players[requestIndex].PlaceBomb();
                    break;

            }
        }

        public void PlayerCheckAdjacentTiles(int x, int y, int index)
        {
            int movementIndex = 0;
            //Collision playerCollision = new Collision();

            //converting player coordinates to map tile index
            movementIndex = ConvertCoordsToIndex(x, y);
            //retrieving every type of gameobject that could exist on the tile the player is trying to move towards
            Bomb bombCheck = bombs.Where(e => e.x == x && e.y == y).FirstOrDefault();
            Powerup powerupCheck = powerups.Where(e => e.x == x && e.y == y).FirstOrDefault();
            ExplosionCell explosionCheck = null;// = explosions.Where(e => e.x == x && e.y == y).FirstOrDefault();
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < players[i].bombs.Count; j++)
                {
                    if (explosionCheck == null && players[i].bombs[j].explosion != null)
                    {
                        explosionCheck = players[i].bombs[j].explosion.GetExplosionCells().Where(e => e.x == x && e.y == y).FirstOrDefault();
                    }
                    //players[i].bombs[j].explosion.GetExplosionCells
                }
            }
            if (gameMap.tiles[movementIndex] is Wall)
            {
                players[index].SetCollisionStrategy(new WallCollision());
                players[index].ResolvePlayerCollision(players[index], gameMap.tiles[movementIndex], new List<Powerup>(), null);
            }
            else if (bombCheck != null)
            {
                //I sleep
            }
            else if (gameMap.tiles[movementIndex] is Box)
            {
                players[index].SetCollisionStrategy(new BoxCollision());
                players[index].ResolvePlayerCollision(players[index], gameMap.tiles[movementIndex], new List<Powerup>(), null);
            }
            else if (powerupCheck != null)
            {
                players[index].SetCollisionStrategy(new PowerupCollision());
                players[index].ResolvePlayerCollision(players[index], powerupCheck, powerups, powerupInvoker);
            }
            else if (explosionCheck != null)
            {
                players[index].SetCollisionStrategy(new ExplosionCollision());
                players[index].ResolvePlayerCollision(players[index], explosionCheck, new List<Powerup>(), null);
            }
            else if (gameMap.tiles[movementIndex] is EmptyTile)
            {
                players[index].SetCollisionStrategy(new EmptyTileCollision());
                players[index].ResolvePlayerCollision(players[index], gameMap.tiles[movementIndex], new List<Powerup>(), null);
            }
        }

        public int ConvertCoordsToIndex(int x, int y)
        {
            return 15 * y + x;
        }

        public void CheckInvulnerabilityPeriods()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].invulnerableSince != null)
                {
                    DateTime invulnExpiration = players[i].invulnerableSince;
                    invulnExpiration.AddSeconds(players[i].invulnerabilityDuration);

                    if (invulnExpiration <= DateTime.Now)
                    {
                        players[i].invulnerable = false;
                    }
                }
            }
            
        }

        public void CheckPowerupTimers()
        {
            if (powerups.Any())
            {
                for (int i = 0; i < powerups.Count; i++)
                {
                    DateTime powerupExpiration = powerups[i].plantedAt;
                    powerupExpiration.AddSeconds(powerups[i].existDuration);

                    if (powerupExpiration <= DateTime.Now)
                    {
                        powerups.RemoveAt(i);
                    }
                }
            }
            
        }

        public void FormDrawingObjectLists()
        {
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < players[i].GetBombCount(); j++)
                {
                   // if (!bombs.Contains(players[i].bombs[j]))
                    //{
                        bombs.Add(players[i].bombs[j]);
                   // }
                    if(players[i].bombs[j].GetExplosion() != null)
                    {
                        int cellCount = players[i].bombs[j].GetExplosion().GetExplosionCells().Count;
                        for (int k = 0; k < cellCount; k++)
                        {
                            //if (!explosions.Contains(players[i].bombs[j].GetExplosion().GetExplosionCells()[k]))
                            //{
                                explosions.Add(players[i].bombs[j].GetExplosion().GetExplosionCells()[k]);
                           // }
                            
                        }
                    }
                    
                }
            }
        }

        private async Task StoreDrawData(string[] playerIDs, Map _map, List<Player> _players, List<Bomb> _bombs, List<Powerup> _powerups, List<ExplosionCell> _explosions, List<Message> _messages)
        {
            TTile[] tiles = new TTile[_map.tiles.Length];
            for (int i = 0; i < _map.tiles.Length; i++)
            {
                tiles[i] = new TTile() { x = _map.tiles[i].x, y = _map.tiles[i].y, texture = _map.tiles[i].texture };
            }
            TMap map = new TMap() { tiles = tiles };
            // ------
            TPlayer[] players = new TPlayer[_players.Count];
            for (int i = 0; i < _players.Count; i++)
            {
                players[i] = new TPlayer() { x = _players[i].x, y = _players[i].y, texture = _players[i].texture };
            }
            // ------
            BombTransport[] bombs = _bombs.Select(bomb => (BombTransport) bombCreator.Pack(bomb)).ToArray();
            PowerupTransport[] powerups = new PowerupTransport[_powerups.Count];
            for (int i = 0; i < _powerups.Count; i++)
            {
                powerups[i] = (PowerupTransport)powerupCreator.Pack(_powerups[i]);
            }

            ExplosionTransport[] explosions = _explosions.Select(explosion => (ExplosionTransport) explosionCreator.Pack(explosion)).ToArray();
            Message[] messages = _messages.ToArray();
            if (playerIDs.Length == 4)
            {
                await _hub.Clients.Clients(playerIDs[0], playerIDs[1], playerIDs[2], playerIDs[3]).SendAsync("StoreDrawData", map, players, bombs, powerups, explosions, messages);
            }
            else
            {
                await _hub.Clients.All.SendAsync("StoreDrawData", map, players, bombs, powerups, explosions, messages);
            }
        }

        public async Task StartPlaying(string[] playerIDs)
        {
            await _hub.Clients.Clients(playerIDs[0], playerIDs[1], playerIDs[2], playerIDs[3]).SendAsync("StartPlaying");
        }

        public async Task Broadcast(Message message)
        {
            await _hub.Clients.All.SendAsync("messageReceived", "admin", message);
        }

    }
}
