using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.Command;
using SignalRWebPack.Patterns.Strategy;

namespace SignalRWebPack.Models
{
    class Player : GameObject
    {
        private CollisionStrategy _collisionStrategy;

        public DefaultPowerupValues Defaults = new DefaultPowerupValues();

        public int lives { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public bool invulnerable { get; set; }
        public double speedMultiplier { get; set; }
        public bool ready { get; set; }
        public DateTime invulnerableSince { get; set; }
        public DateTime invulnerableUntil { get; set; }
        public int invulnerabilityDuration { get; set; }
        public int maxBombs { get; set; }
        public int activeBombCount { get; set; }
        public int explosionSizeMultiplier { get; set; }
        public int bombTickDuration { get; set; }
        public List<Bomb> bombs { get; set; }
        public bool IsAlive { get { return lives > 0; } }
        public Player(string name, string id, int x, int y)
        {
            lives = 3;
            this.name = name;
            this.id = id;
            invulnerable = false;
            speedMultiplier = 1;
            texture = "player";
            ready = false;
            invulnerableSince = DateTime.Now;
            invulnerableUntil = DateTime.MinValue;
            invulnerabilityDuration = 3; //seconds
            bombTickDuration = 3; //seconds
            maxBombs = 1;
            this.x = x;
            this.y = y;
            explosionSizeMultiplier = 2;
            bombs = new List<Bomb>();
            
        }

        public void SetCollisionStrategy(CollisionStrategy collisionStrategy)
        {
            _collisionStrategy = collisionStrategy;
        }

        public void ResolvePlayerCollision(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            _collisionStrategy.PlayerCollisionStrategy(player, collisionTarget, collisionList, powerupInvoker);
        }

        public void PlaceBomb()
        {
            if (activeBombCount + 1 <= maxBombs)
            {
                activeBombCount++;
                Bomb bomb = new Bomb(x, y, bombTickDuration, explosionSizeMultiplier);
                bombs.Add(bomb);
            }
        }

        public int GetBombCount()
        {
            return bombs.Count;
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
                        if (!bombs[i].hasExploded)
                        {
                            bombs[i].Explode();
                            bombs[i].hasExploded = true;
                        }
                        
                        //activeBombCount--;
                    }
                }
            }

        }

        public void CheckInvulnerabilityPeriods()
        {
            if (invulnerable)
            {
                if (invulnerableUntil <= DateTime.Now)
                {
                    invulnerable = false;
                    invulnerableUntil = DateTime.MinValue;
                }
            }
            
        }

        public void RefreshBombList(Bomb bomb)
        {
            bomb.NullExplosion();
            bombs.Remove(bomb);
            activeBombCount--;
        }

        public Bomb GetBomb(int x, int y) => bombs.Where(bomb => bomb.x == x && bomb.y == y).FirstOrDefault();

        public class DefaultPowerupValues
        {
            public readonly int MaxBombTickDuration = 3;
            public readonly int MinBombTickDuration = 1;

            public readonly int MaxPlayerSpeed = 1;
            public readonly int MinPlayerSpeed = 1;

            public readonly int MaxExplosionDamageBombs = 2;
            public readonly int MinExplosionDamageBombs = 1;

            public readonly int MaxExplosionSize = 8;
            public readonly int MinExplosionSize = 2;

            public readonly int MaxBombs = 8;
            public readonly int MinBombs = 1;
        }
    }
}
