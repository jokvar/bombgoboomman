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

            int i = 0;
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
                    PlayerCheckAdjacentTiles("y", x, y, -1, requestIndex);
                    break;

                case ActionEnums.Down:
                    PlayerCheckAdjacentTiles("y", x, y, 1, requestIndex);
                    break;

                case ActionEnums.Right:
                    PlayerCheckAdjacentTiles("x", x, y, 1, requestIndex);
                    break;

                case ActionEnums.Left:
                    PlayerCheckAdjacentTiles("x", x, y, -1, requestIndex);
                    break;

                case ActionEnums.PlaceBomb:
                    if(players[requestIndex].activeBombCount < players[requestIndex].maxBombs)
                    {
                        players[requestIndex].activeBombCount++;
                        PlaceBomb(requestIndex);
                    }
                    break;

            }
        }

        public void PlayerCheckAdjacentTiles(string axis, int x, int y, int increment, int index)
        {
            Bomb bombCheck;
            Powerup powerupCheck;
            Explosion explosionCheck;
            int movementIndex = 0;
            switch (axis)
            {
                case "x":
                    //converting player coordinates to map tile index
                    movementIndex = ConvertCoordsToIndex(x + increment, y);
                    //retrieving every type of gameobject that could exist on the tile the player is trying to move towards
                    bombCheck = bombs.Where(e => e.x == x + increment && e.y == y).FirstOrDefault();
                    powerupCheck = powerups.Where(e => e.x == x + increment && e.y == y).FirstOrDefault();
                    explosionCheck = explosions.Where(e => e.x == x + increment && e.y == y).FirstOrDefault();
                    if (gameMap.tiles[movementIndex] is Wall)
                    {
                        //I sleep
                    }
                    else if (bombCheck != null)
                    {
                        //I sleep
                    }
                    else if (gameMap.tiles[movementIndex] is Box)
                    {
                        //I sleep
                    }
                    else if (powerupCheck != null)
                    {
                        players[index].x += increment;
                        ResolvePowerup(players[index]);
                        powerups.RemoveAt(GetPowerupIndex(powerupCheck));
                    }
                    else if (explosionCheck != null)
                    {
                        players[index].x += increment;
                        players[index].lives--;
                        players[index].invulnerableSince = DateTime.Now;
                        players[index].invulnerable = true;
                    }
                    else if (gameMap.tiles[movementIndex] is EmptyTile)
                    {
                        players[index].x += increment;
                    }
                    
                    break;

                case "y":
                    movementIndex = ConvertCoordsToIndex(x, y + increment);
                    bombCheck = bombs.Where(e => e.x == x && e.y == y + increment).FirstOrDefault();
                    powerupCheck = powerups.Where(e => e.x == x && e.y == y + increment).FirstOrDefault();
                    explosionCheck = explosions.Where(e => e.x == x && e.y == y + increment).FirstOrDefault();
                    if (gameMap.tiles[movementIndex] is Wall)
                    {
                        //I sleep
                    }
                    else if (bombCheck != null)
                    {
                        //I sleep
                    }
                    else if (gameMap.tiles[movementIndex] is Box)
                    {
                        //I sleep
                    }
                    else if (powerupCheck != null)
                    {
                        players[index].y += increment;
                        ResolvePowerup(players[index]);
                        powerups.RemoveAt(GetPowerupIndex(powerupCheck));
                    }
                    else if (explosionCheck != null)
                    {
                        players[index].y += increment;
                        players[index].lives--;
                        players[index].invulnerableSince = DateTime.Now;
                        players[index].invulnerable = true;
                    }
                    else if (gameMap.tiles[movementIndex] is EmptyTile)
                    {
                        players[index].y += increment;
                    }
                    
                    break;
            }
        }

        //resolves powerup pick ups
        public void ResolvePowerup(Player playerReference)
        {
            Powerup powerupCheck = powerups.Where(e => e.x == playerReference.x && e.y == playerReference.y).FirstOrDefault();
            switch (powerupCheck.type)
            {
                case Powerup_type.AdditionalBomb:
                    if(playerReference.maxBombs < 8)
                    {
                        playerReference.maxBombs++;
                    }
                    break;

                case Powerup_type.ExplosionSize:
                    playerReference.explosionSizeMultiplier++;
                    break;

                case Powerup_type.BombTickDuration:
                    if(playerReference.bombTickDuration > 2)
                    {
                        playerReference.bombTickDuration--;
                    }
                    break;

                    //TODO: possibly reconsider use cases for these
                case Powerup_type.ExplosionDamage:
                    break;

                case Powerup_type.PlayerSpeed:
                    break;
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
                xPlusStopped = ExplosionCheckAdjacentTiles("x", x, y, i, xPlusStopped, explodedAt);

                yPlusStopped = ExplosionCheckAdjacentTiles("y", x, y, i, yPlusStopped, explodedAt);

                xMinusStopped = ExplosionCheckAdjacentTiles("x", x, y, -i, xMinusStopped, explodedAt);

                yMinusStopped = ExplosionCheckAdjacentTiles("y", x, y, -i, yMinusStopped, explodedAt);
            }
        }

        public bool ExplosionCheckAdjacentTiles(string axis, int x, int y, int increment, bool explosionStopped, DateTime explodedAt)
        {
            int explosionIndex = 0;
            Explosion explosionCheck;
            Bomb bombCheck;
            Player playerCheck;
            Powerup powerupCheck;
            switch (axis)
            {
                case "x":
                    explosionIndex = ConvertCoordsToIndex(x + increment, y);
                    if (explosionIndex > 225 || explosionIndex < 0)
                    {
                        break;
                    }

                    //checking whether an explosion already exists at the given coordinates
                    explosionCheck = explosions.Where(e => e.x == x + increment && e.y == y).FirstOrDefault();

                    //checking whether an explosion already exists at the given coordinates
                    bombCheck = bombs.Where(e => e.x == x + increment && e.y == y).FirstOrDefault();

                    //checking whether a player is standing at the given coordinates
                    playerCheck = players.Where(e => e.x == x + increment && e.y == y).FirstOrDefault();

                    //checking whether a powerup exists at the given coordinates
                    powerupCheck = powerups.Where(e => e.x == x + increment && e.y == y).FirstOrDefault();

                    if (gameMap.tiles[explosionIndex] is Wall)
                    {
                        explosionStopped = true;
                    }
                    //explosionCheck might not be null - confirm during testing
                    else if (explosionCheck != null)
                    {
                        explosionStopped = true;
                    }
                    else if (bombCheck != null)
                    {
                        explosionStopped = true;
                        BombExplosion(GetBombIndex(bombCheck));
                    }
                    else if (powerupCheck != null)
                    {
                        Explosion exp1 = new Explosion(explodedAt, x + increment, y);
                        explosions.Add(exp1);
                        powerups.RemoveAt(GetPowerupIndex(powerupCheck));
                    }
                    //if explosion spawns on player
                    else if (playerCheck != null)
                    {
                        Explosion exp1 = new Explosion(explodedAt, x + increment, y);
                        explosions.Add(exp1);
                        playerCheck.lives--;
                        playerCheck.invulnerableSince = DateTime.Now;
                        playerCheck.invulnerable = true;
                    }
                    else if (gameMap.tiles[explosionIndex] is Box && !explosionStopped)
                    {
                        Explosion exp1 = new Explosion(explodedAt, x + increment, y);
                        explosions.Add(exp1);
                        GeneratePowerup(x + increment, y, explosionIndex);
                        explosionStopped = true;
                        gameMap.tiles[explosionIndex] = new EmptyTile { x = x + increment, y = y, texture = "#ffffff" };
                    }
                    else if (gameMap.tiles[explosionIndex] is EmptyTile && !explosionStopped)
                    {
                        Explosion exp1 = new Explosion(explodedAt, x + increment, y);
                        explosions.Add(exp1);
                    }
                    
                    break;

                case "y":
                    explosionIndex = ConvertCoordsToIndex(x, y + increment);
                    if(explosionIndex > 225 || explosionIndex < 0)
                    {
                        break;
                    }
                    //checking whether an explosion already exists at the given coordinates
                    explosionCheck = explosions.Where(e => e.x == x && e.y == y + increment).FirstOrDefault();

                    //checking whether an explosion already exists at the given coordinates
                    bombCheck = bombs.Where(e => e.x == x && e.y == y + increment).FirstOrDefault();

                    //checking whether a player is standing at the given coordinates
                    playerCheck = players.Where(e => e.x == x && e.y == y + increment).FirstOrDefault();

                    //checking whether a powerup exists at the given coordinates
                    powerupCheck = powerups.Where(e => e.x == x && e.y == y + increment).FirstOrDefault();

                    if (gameMap.tiles[explosionIndex] is Wall)
                    {
                        explosionStopped = true;
                    }
                    else if (explosionCheck != null)
                    {
                        explosionStopped = true;
                    }
                    else if (bombCheck != null)
                    {
                        explosionStopped = true;
                        BombExplosion(GetBombIndex(bombCheck));
                    }
                    else if (powerupCheck != null)
                    {
                        Explosion exp1 = new Explosion(explodedAt, x, y + increment);
                        explosions.Add(exp1);
                        powerups.RemoveAt(GetPowerupIndex(powerupCheck));
                    }
                    else if (playerCheck != null)
                    {
                        Explosion exp1 = new Explosion(explodedAt, x, y + increment);
                        explosions.Add(exp1);
                        playerCheck.lives--;
                        playerCheck.invulnerableSince = DateTime.Now;
                        playerCheck.invulnerable = true;
                    }
                    else if (gameMap.tiles[explosionIndex] is Box && !explosionStopped)
                    {
                        Explosion exp = new Explosion(explodedAt, x, y + increment);
                        explosions.Add(exp);
                        GeneratePowerup(x, y + increment, explosionIndex);
                        explosionStopped = true;
                        gameMap.tiles[explosionIndex] = new EmptyTile {x = x, y = y + increment, texture = "#ffffff" };
                    }
                    else if (gameMap.tiles[explosionIndex] is EmptyTile && !explosionStopped)
                    {
                        Explosion exp = new Explosion(explodedAt, x, y + increment);
                        explosions.Add(exp);
                    }
                    
                    break;
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

        public void GeneratePowerup(int x, int y, int boxIndex)
        {
            var rand = new Random();
            //will be true 50% of the time
            if(rand.Next(100) < 50)
            {
                int powerupIndex = rand.Next(0, 6);
                Powerup powerup;
                switch (powerupIndex)
                {
                    case 0:
                        powerup = new Powerup(Powerup_type.BombTickDuration, x, y);
                        powerups.Add(powerup);
                        break;

                    case 1:
                        powerup = new Powerup(Powerup_type.PlayerSpeed, x, y);
                        powerups.Add(powerup);
                        break;

                    case 2:
                        powerup = new Powerup(Powerup_type.ExplosionDamage, x, y);
                        powerups.Add(powerup);
                        break;

                    case 3:
                        powerup = new Powerup(Powerup_type.ExplosionSize, x, y);
                        powerups.Add(powerup);
                        break;

                    case 4:
                        powerup = new Powerup(Powerup_type.AdditionalBomb, x, y);
                        powerups.Add(powerup);
                        break;

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
            BombTransport[] bombs = (BombTransport[])_bombs.Select(bomb => bombCreator.Pack(bomb)).ToArray();
            PowerupTransport[] powerups = (PowerupTransport[])_powerups.Select(powerup => powerupCreator.Pack(powerup)).ToArray();
            ExplosionTransport[] explosions = (ExplosionTransport[])_explosions.Select(explosion => explosionCreator.Pack(explosion)).ToArray();
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
