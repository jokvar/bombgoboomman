using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Logic;
using SignalRWebPack.Patterns.Strategy;

namespace SignalRWebPack.Models
{
    class Explosion : GameObject
    {
        private List<ExplosionCell> explosions { get; set; }
        private static Session session = SessionManager.Instance.GetSession(SessionManager.Instance.ActiveSessionCode);
        private Map gameMap = session.Map;
        private List<Powerup> powerups = session.powerups;
        private List<Player> players = session.Players;

        public bool isExpired { get; set; }
        public int explosionSizeMultiplier { get; set; }
        public int size { get; set; }

        public Explosion(int x, int y, bool isExpired, int explosionSizeMultiplier)
        {
            //x and y represent the center of the explosion
            this.x = x;
            this.y = y;
            this.isExpired = isExpired;
            size = 1;
            this.explosionSizeMultiplier = explosionSizeMultiplier;
            explosions = new List<ExplosionCell>();
            
        }

        public List<ExplosionCell> GetExplosionCells()
        {
            return explosions;
        }

        //spawns gunpowder with the center of the explosion being the coordinates x and y (where the bomb was initially placed)
        public void SpawnExplosions(int x, int y)
        {
            DateTime explodedAt = DateTime.Now;
            ExplosionCell explosion = new ExplosionCell(explodedAt, x, y);
            DateTime expiresAt = explodedAt.AddSeconds(explosion.explosionDuration);

            //make 4 flags for each direction of the explosion to track whether it continues to spread or not
            bool xPlusStopped = false;
            bool xMinusStopped = false;
            bool yPlusStopped = false;
            bool yMinusStopped = false;

            //calculating explosion coordinates
            //replacing the initial bomb with an explosion tile
            explosions.Add(explosion);
            int explosionSize = size * explosionSizeMultiplier;
            for (int i = 1; i <= explosionSize; i++)
            {
                
                xPlusStopped = ExplosionCheckAdjacentTiles(x + i, y, xPlusStopped, explodedAt, explosion);

                xMinusStopped = ExplosionCheckAdjacentTiles(x - i, y, xMinusStopped, explodedAt, explosion);

                yPlusStopped = ExplosionCheckAdjacentTiles(x, y + i, yPlusStopped, explodedAt, explosion);

                yMinusStopped = ExplosionCheckAdjacentTiles(x, y - i, yMinusStopped, explodedAt, explosion);
            }
        }

        bool ExplosionCheckAdjacentTiles(int x, int y, bool explosionStopped, DateTime explodedAt, ExplosionCell explosion)
        {
            int explosionIndex = 0;

            explosionIndex = ConvertCoordsToIndex(x, y);
            if (explosionIndex > 225 || explosionIndex < 0 || explosionStopped)
            {
                return true;
            }

            //checking whether an explosion already exists at the given coordinates
            ExplosionCell explosionCheck = explosions.Where(e => e.x == x && e.y == y).FirstOrDefault();

            //checking whether an explosion already exists at the given coordinates
            Bomb bombCheck = null;
            for (int i = 0; i < players.Count; i++)
            {
                if(bombCheck == null)
                {
                    bombCheck = players[i].GetBomb(x, y);
                }
                
            }

            //checking whether a player is standing at the given coordinates
            Player playerCheck = players.Where(e => e.x == x && e.y == y).FirstOrDefault();

            //checking whether a powerup exists at the given coordinates
            Powerup powerupCheck = powerups.Where(e => e.x == x && e.y == y).FirstOrDefault();

            if (gameMap.tiles[explosionIndex] is Wall)
            {
                explosion.SetCollisionStrategy(new WallCollision());
                explosion.ResolveExplosionCollision(gameMap.tiles[explosionIndex], explosions, explodedAt, powerups);
                explosionStopped = true;
            }
            //explosionCheck might not be null - confirm during testing
            else if (explosionCheck != null)
            {
                explosion.SetCollisionStrategy(new ExplosionCollision());
                explosion.ResolveExplosionCollision(explosionCheck, explosions, explodedAt, powerups);
                explosionStopped = true;
            }
            else if (bombCheck != null)
            {
                explosion.SetCollisionStrategy(new BombCollision());
                explosion.ResolveExplosionCollision(bombCheck, explosions, explodedAt, powerups);
            }
            else if (powerupCheck != null)
            {
                explosion.SetCollisionStrategy(new PowerupCollision());
                explosion.ResolveExplosionCollision(powerupCheck, explosions, explodedAt, powerups);
            }
            //if explosion spawns on player
            else if (playerCheck != null)
            {
                explosion.SetCollisionStrategy(new PlayerCollision());
                explosion.ResolveExplosionCollision(playerCheck, explosions, explodedAt, powerups);
            }
            else if (gameMap.tiles[explosionIndex] is Box && !explosionStopped)
            {
                explosion.SetCollisionStrategy(new BoxCollision());
                explosion.ResolveExplosionCollision(gameMap.tiles[explosionIndex], explosions, explodedAt, powerups);
                explosionStopped = true;
                gameMap.tiles[explosionIndex] = new EmptyTile { x = x, y = y, texture = "blank" };
            }
            else if (gameMap.tiles[explosionIndex] is EmptyTile && !explosionStopped)
            {
                explosion.SetCollisionStrategy(new EmptyTileCollision());
                explosion.ResolveExplosionCollision(gameMap.tiles[explosionIndex], explosions, explodedAt, powerups);
            }
            return explosionStopped;
        }

        public void CheckExplosionTimers()
        {
            if (explosions.Any())
            {
                int counter = 0;
                for (int i = 0; i < explosions.Count; i++)
                {
                    if (explosions[i].expiresAt <= DateTime.Now)
                    {
                        //explosions[i] = null;
                        explosions[i].isExpired = true;
                        counter++;
                    }
                }
                if (counter == explosions.Count)
                {
                    isExpired = true;
                }
            }
        }



        private int ConvertCoordsToIndex(int x, int y)
        {
            return 15 * y + x;
        }
    }
}
