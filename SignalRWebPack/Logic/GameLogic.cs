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

namespace SignalRWebPack.Logic
{
    public interface IGameLogic
    {
        Task GameLoop(CancellationToken cancellationToken);
    }
    public class GameLogic : IGameLogic
    {
        private readonly IHubContext<ChatHub> _hub;

        private static Session session = SessionManager.GetSession();
        private List<Player> players = session.Players;
        private Map gameMap = session.Map;
        private List<Bomb> bombs = new List<Bomb>();
        private List<Explosion> explosions = new List<Explosion>();
        private List<Powerup> powerups = new List<Powerup>();
        private int mapDimensions = 15;

        //-------FactoryMethod--------------
        // Creators: 
        BombTransportCreator bombCreator;
        PowerupTransportCreator powerupCreator; 
        ExplosionTransportCreator explosionCreator;
        //-------FactoryMethod--------------
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
        int[] mapData = {   1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                            1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1,
                            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                            1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1,
                            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                            1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1,
                            1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                            1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1,
                            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                            1, 0, 1, 0, 1, 0, 1, 0, 1, 2, 1, 0, 1, 0, 1,
                            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                            1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1,
                            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            gameMap = new Map(mapData);
            players = new List<Player> { new Player("vardas", "lol koks dar id", 1, 1) };
            bombs = new List<Bomb> { new Bomb(1, 2, players.Last()) };
            powerups = new List<Powerup> { new Powerup(Powerup_type.AdditionalBomb, 2, 1) };
            //explosions = new List<Explosion> { new Explosion(DateTime.Now, 1, 5) };
            List<Message> messages = new List<Message>();

            while (!cancellationToken.IsCancellationRequested)
            {
                //_logger.LogInformation("iteration");

                //example action dequeing
                Tuple<string, PlayerAction> tuple;
                while ((tuple = InputQueueManager.Instance.ReadOne()) != null) //deleted when read
                {
                    string playerId = tuple.Item1;
                    PlayerAction action = tuple.Item2;
                    _logger.LogInformation("analyzed tuple");
                    ProcessAction(action, playerId);
                }
                //end example
                //explosions[0].x = (i++ % 5) + 1;
                //_logger.LogInformation("sending draw data");
                await StoreDrawData(session.PlayerIDs, gameMap, players, bombs, powerups, explosions, messages);
                await Task.Delay(60); // 😎😎😎😎
                //await Broadcast(new Message("ligma lol", 1)); 

                CheckBombTimers();
                CheckExplosionTimers();
                //CheckInvulnerabilityPeriods();
                //CheckPowerupTimers();
                //client.StoreDrawData(session.PlayerIDs, gameMap, players, bombs, powerups, explosions, messages); ; 
            } 
        }

        public void ProcessAction(PlayerAction playerAction, string id)
        {
            int requestIndex = 0;//session.MatchId(id);
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
                    if(players[requestIndex].activeBombCount + 1 < players[requestIndex].maxBombs)
                    {
                        players[requestIndex].activeBombCount++;
                        PlaceBomb(requestIndex);
                    }
                    break;

            }
        }

        public void PlayerCheckAdjacentTiles(int x, int y, int index)
        {
            int movementIndex = 0;
            Collision playerCollision = new Collision();

            //converting player coordinates to map tile index
            movementIndex = ConvertCoordsToIndex(x, y);
            //retrieving every type of gameobject that could exist on the tile the player is trying to move towards
            Bomb bombCheck = bombs.Where(e => e.x == x && e.y == y).FirstOrDefault();
            Powerup powerupCheck = powerups.Where(e => e.x == x && e.y == y).FirstOrDefault();
            Explosion explosionCheck = explosions.Where(e => e.x == x && e.y == y).FirstOrDefault();
            if (gameMap.tiles[movementIndex] is Wall)
            {
                playerCollision.SetCollisionStrategy(new WallCollision());
                playerCollision.ResolvePlayerCollision(players[index], gameMap.tiles[movementIndex], new List<Powerup>());
            }
            else if (bombCheck != null)
            {
                //I sleep
            }
            else if (gameMap.tiles[movementIndex] is Box)
            {
                playerCollision.SetCollisionStrategy(new BoxCollision());
                playerCollision.ResolvePlayerCollision(players[index], gameMap.tiles[movementIndex], new List<Powerup>());
            }
            else if (powerupCheck != null)
            {
                playerCollision.SetCollisionStrategy(new PowerupCollision());
                playerCollision.ResolvePlayerCollision(players[index], powerupCheck, powerups);
            }
            else if (explosionCheck != null)
            {
                playerCollision.SetCollisionStrategy(new ExplosionCollision());
                playerCollision.ResolvePlayerCollision(players[index], explosionCheck, new List<Powerup>());
            }
            else if (gameMap.tiles[movementIndex] is EmptyTile)
            {
                playerCollision.SetCollisionStrategy(new EmptyTileCollision());
                playerCollision.ResolvePlayerCollision(players[index], gameMap.tiles[movementIndex], new List<Powerup>());
            }
        }


        public void PlaceBomb(int requestIndex)
        {
            int x = players[requestIndex].x;
            int y = players[requestIndex].y;
            Bomb bomb = new Bomb(x, y, players[requestIndex]);
            bombs.Add(bomb);
        }

        //method for resolving bomb explosions
        public void BombExplosion(int bombIndex)
        {
            int x = bombs[bombIndex].x;
            int y = bombs[bombIndex].y;
            Player playerReference = bombs[bombIndex].placedBy;
            bombs.RemoveAt(bombIndex);
            playerReference.activeBombCount--;
            SpawnExplosions(x, y, playerReference);
        }

        public int GetBombIndex(Bomb bomb)
        {
            for (int i = 0; i < bombs.Count; i++)
            {
                if(bombs[i] == bomb)
                {
                    return i;
                }
            }
            return 404; //not found
        }

        public int GetPowerupIndex(Powerup powerup)
        {
            for (int i = 0; i < powerups.Count; i++)
            {
                if (powerups[i] == powerup)
                {
                    return i;
                }
            }
            return 404; //not found
        }

        //spawns gunpowder with the center of the explosion being the coordinates x and y (where the bomb was initially placed)
        public void SpawnExplosions(int x, int y, Player playerReference)
        {
            DateTime explodedAt = DateTime.Now;
            Explosion explosion = new Explosion(explodedAt, x, y);
            DateTime expiresAt = explodedAt.AddSeconds(explosion.explosionDuration);

            //make 4 flags for each direction of the explosion to track whether it continues to spread or not
            bool xPlusStopped = false;
            bool xMinusStopped = false;
            bool yPlusStopped = false;
            bool yMinusStopped = false;

            //calculating explosion coordinates
            //replacing the initial bomb with an explosion tile
            explosions.Add(explosion);
            int explosionSize = explosion.size * playerReference.explosionSizeMultiplier;
            for (int i = 1; i <= explosionSize; i++)
            {
                xPlusStopped = ExplosionCheckAdjacentTiles(x + i, y, xPlusStopped, explodedAt);

                xMinusStopped = ExplosionCheckAdjacentTiles(x - i, y, xMinusStopped, explodedAt);

                yPlusStopped = ExplosionCheckAdjacentTiles(x, y + i, yPlusStopped, explodedAt);

                yMinusStopped = ExplosionCheckAdjacentTiles(x, y - i, yMinusStopped, explodedAt);
            }
        }

        public bool ExplosionCheckAdjacentTiles(int x, int y, bool explosionStopped, DateTime explodedAt)
        {
            int explosionIndex = 0;
            Collision explosionCollision = new Collision();

            explosionIndex = ConvertCoordsToIndex(x, y);
            if (explosionIndex > 225 || explosionIndex < 0 || explosionStopped)
            {
                return true;
            }

            //checking whether an explosion already exists at the given coordinates
            Explosion explosionCheck = explosions.Where(e => e.x == x && e.y == y).FirstOrDefault();

            //checking whether an explosion already exists at the given coordinates
            Bomb bombCheck = bombs.Where(e => e.x == x && e.y == y).FirstOrDefault();

            //checking whether a player is standing at the given coordinates
            Player playerCheck = players.Where(e => e.x == x && e.y == y).FirstOrDefault();

            //checking whether a powerup exists at the given coordinates
            Powerup powerupCheck = powerups.Where(e => e.x == x && e.y == y).FirstOrDefault();

            if (gameMap.tiles[explosionIndex] is Wall)
            {
                explosionCollision.SetCollisionStrategy(new WallCollision());
                explosionCollision.ResolveExplosionCollision(gameMap.tiles[explosionIndex], explosions, explodedAt, powerups);
                explosionStopped = true;
            }
            //explosionCheck might not be null - confirm during testing
            else if (explosionCheck != null)
            {
                explosionCollision.SetCollisionStrategy(new ExplosionCollision());
                explosionCollision.ResolveExplosionCollision(explosionCheck, explosions, explodedAt, powerups);
                explosionStopped = true;
            }
            else if (bombCheck != null)
            {
                explosionStopped = true;
                BombExplosion(GetBombIndex(bombCheck));
            }
            else if (powerupCheck != null)
            {
                explosionCollision.SetCollisionStrategy(new PowerupCollision());
                explosionCollision.ResolveExplosionCollision(powerupCheck, explosions, explodedAt, powerups);
            }
            //if explosion spawns on player
            else if (playerCheck != null)
            {
                explosionCollision.SetCollisionStrategy(new PlayerCollision());
                explosionCollision.ResolveExplosionCollision(playerCheck, explosions, explodedAt, powerups);
            }
            else if (gameMap.tiles[explosionIndex] is Box && !explosionStopped)
            {
                explosionCollision.SetCollisionStrategy(new BoxCollision());
                explosionCollision.ResolveExplosionCollision(gameMap.tiles[explosionIndex], explosions, explodedAt, powerups);
                explosionStopped = true;
                gameMap.tiles[explosionIndex] = new EmptyTile { x = x, y = y, texture = "#ffffff" };
            }
            else if (gameMap.tiles[explosionIndex] is EmptyTile && !explosionStopped)
            {
                explosionCollision.SetCollisionStrategy(new EmptyTileCollision());
                explosionCollision.ResolveExplosionCollision(gameMap.tiles[explosionIndex], explosions, explodedAt, powerups);
            }
            return explosionStopped;
        }

        public int ConvertCoordsToIndex(int x, int y)
        {
            return 15 * y + x;
        }

        //repeatedly called method for checking whether any bomb timers in the bomb list have expired yet
        public void CheckBombTimers()
        {
            if (bombs.Any())
            {
                for (int i = 0; i < bombs.Count; i++)
                {
                    if (bombs[i].explodesAt <= DateTime.Now)
                    {
                        BombExplosion(i);
                    }
                }
            }
            
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

        public void CheckExplosionTimers()
        {
            if (explosions.Any())
            {
                for (int i = 0; i < explosions.Count; i++)
                {

                    if(explosions[i].expiresAt <= DateTime.Now)
                    {
                        explosions.RemoveAt(i);
                    }
                }
            }
        }

        public async void EnableDrawing()
        {

        }

        private async Task StoreDrawData(string[] playerIDs, Map _map, List<Player> _players, List<Bomb> _bombs, List<Powerup> _powerups, List<Explosion> _explosions, List<Message> _messages)
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
            PowerupTransport[] powerups = _powerups.Select(powerup => (PowerupTransport) powerupCreator.Pack(powerup)).ToArray();
            ExplosionTransport[] explosions = _explosions.Select(explosion => (ExplosionTransport) explosionCreator.Pack(explosion)).ToArray();
            Message[] messages = _messages.ToArray();
            //await _hub.Clients.Clients(playerIDs[0], playerIDs[1], playerIDs[2], playerIDs[3]).SendAsync("StoreDrawData", map, players, bombs, powerups, explosions, messages);
            await _hub.Clients.All.SendAsync("StoreDrawData", map, players, bombs, powerups, explosions, messages);
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
