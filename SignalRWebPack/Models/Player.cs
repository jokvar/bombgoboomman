using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.Command;
using SignalRWebPack.Patterns.Strategy;
using SignalRWebPack.Patterns.AbstractFactory;
using SignalRWebPack.Patterns.Visitor;
using SignalRWebPack.Patterns.Mediator;
using SignalRWebPack.Patterns.Memento;
using SignalRWebPack.Patterns.State;

namespace SignalRWebPack.Models
{
    public class Player : GameObject, IVisitorElement
    {
        private CollisionStrategy _collisionStrategy;
        private ObjectFactory oFactory = FactoryProducer.getFactory("ObjectFactory") as ObjectFactory;

        public IMediator _mediator;
        private PlayerState _playerState;
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

        private Memento memento;
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
            playerState = new IdleState();
            switch(name)
            {
                case ("player1"):
                texture = "player";
                break;
                case ("player2"):
                texture = "playerTwo";
                break;
                case ("player3"):
                texture = "playerThree";
                break;
                case ("player4"):
                texture = "playerFour";
                break;
            }
            memento = new Memento(this.lives, this.texture);
        }

        public PlayerState playerState
        {
            get { return _playerState; }
            set { _playerState = value; }
        }

        public void ExecuteAction()
        {
            _playerState.Action(this);
        }

        public override List<string> GetTextures()
        {
            return textures;
        }

        public void SetCollisionStrategy(CollisionStrategy collisionStrategy)
        {
            _collisionStrategy = collisionStrategy;
        }

        public CollisionStrategy GetCollisionStrategy()
        {
            return _collisionStrategy;
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
                Bomb b = oFactory.GetObject("bomb") as Bomb;
                b.x = x;
                b.y = y;
                b.tickDuration = bombTickDuration;
                b.explodesAt = b.plantedAt.AddSeconds(b.tickDuration);
                b.explosionSizeMultiplier = explosionSizeMultiplier;
                b.texture = "bomb";
                bombs.Add(b);
            }
        }

        //Used exclusively for the visitor pattern
        public void SpawnBomb()
        {
            Bomb b = oFactory.GetObject("bomb") as Bomb;
            b.x = x;
            b.y = y;
            b.tickDuration = bombTickDuration;
            b.explodesAt = b.plantedAt.AddSeconds(b.tickDuration);
            b.explosionSizeMultiplier = explosionSizeMultiplier;
            b.texture = "bomb";
            bombs.Add(b);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
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

        public void SetMediator(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public void SaveMemento()
        {
            this.memento = new Memento(this.lives, this.texture);
        }

        public void RestoreMemento()
        {
            this.lives = memento.Lives;
            this.texture = memento.Texture;
        }



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
