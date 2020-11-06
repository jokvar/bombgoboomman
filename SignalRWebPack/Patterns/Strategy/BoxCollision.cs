using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;
using SignalRWebPack.Patterns.Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Strategy
{
    class BoxCollision : CollisionStrategy
    {
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
                int powerupIndex = rand.Next(0, 6);
                Powerup powerup;
                //MiscDecorator misc;
                //BackgroundDecorator bg;
                //ForegroundDecorator fg;
                switch (powerupIndex)
                {
                    case 0:
                        powerup = new Powerup(Powerup_type.BombTickDuration, x, y);
                        //bg = new BackgroundDecorator(powerup);
                        //bg.PowerupDecoration();
                        //fg = new ForegroundDecorator(powerup);
                        //fg.PowerupDecoration(Powerup_type.BombTickDuration);
                        //misc = new MiscDecorator(powerup);
                        
                        powerups.Add(powerup);
                        break;
                    case 2:
                        powerup = new Powerup(Powerup_type.PowerDownX3, x, y);
                        powerups.Add(powerup);
                        break;
                    case 1:
                        powerup = new Powerup(Powerup_type.PowerDown, x, y);
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
                    default:
                        break;

                }
            }

        }
    }
}
