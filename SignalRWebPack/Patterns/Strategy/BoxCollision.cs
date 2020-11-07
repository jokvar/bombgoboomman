using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;
using SignalRWebPack.Patterns.AbstractFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Strategy
{
    class BoxCollision : CollisionStrategy
    {
        ObjectFactory oFactory = FactoryProducer.getFactory("ObjectFactory") as ObjectFactory;
        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            var box = collisionTarget as Box;
            ExplosionCell exp1 = new ExplosionCell(explodedAt, box.x, box.y);
            explosions.Add(exp1);
            GeneratePowerup(box.x, box.y, collisionList);
            //no empty tile reinitilization
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            //do nothing
        }

        public void GeneratePowerup(int x, int y, List<Powerup> powerups)
        {
            var rand = new Random();
            //will be true 50% of the time
            if (rand.Next(100) < 50)
            {
                Powerup pow = oFactory.GetObject("powerup") as Powerup;
                int powerupIndex = rand.Next(0, 6);
                switch (powerupIndex)
                {
                    case 0:
                        pow.x = x;
                        pow.y = y;
                        pow.type = Powerup_type.BombTickDuration;
                        powerups.Add(pow);
                        break;
                    case 2:
                        pow.x = x;
                        pow.y = y;
                        pow.type = Powerup_type.PowerDownX3;
                        powerups.Add(pow);
                        break;
                    case 1:
                        pow.x = x;
                        pow.y = y;
                        pow.type = Powerup_type.PowerDown;
                        powerups.Add(pow);
                        break;
                    case 3:
                        pow.x = x;
                        pow.y = y;
                        pow.type = Powerup_type.ExplosionSize;
                        powerups.Add(pow);
                        break;
                    case 4:
                        pow.x = x;
                        pow.y = y;
                        pow.type = Powerup_type.AdditionalBomb;
                        powerups.Add(pow);
                        break;
                    default:
                        break;

                }
            }

        }
    }
}
