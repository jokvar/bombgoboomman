using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;
using SignalRWebPack.Patterns.AbstractFactory;
using SignalRWebPack.Patterns.Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Strategy
{
    public class BoxCollision : CollisionStrategy
    {
        ObjectFactory oFactory = FactoryProducer.getFactory("ObjectFactory") as ObjectFactory;
        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            if(collisionTarget == null || collisionTarget.GetType() != typeof(Box))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'Box'");
            }
            var box = collisionTarget as Box;
            ExplosionCell exp1 = new ExplosionCell(explodedAt, box.x, box.y);
            explosions.Add(exp1);
            GeneratePowerup(box.x, box.y, collisionList);
            //no empty tile reinitilization
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            if (collisionTarget == null || collisionTarget.GetType() != typeof(Box))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'Box'");
            }
            if(player == null)
            {
                throw new ArgumentNullException("This method cannot be called when 'player' is null");
            }
        }

        public void GeneratePowerup(int x, int y, List<Powerup> powerups)
        {
            if(powerups == null)
            {
                throw new ArgumentNullException("This method cannot be called when 'powerups' is null");
            }
            var rand = new Random();
            //will be true 50% of the time
            if (rand.Next(100) < 50)
            {
                Powerup pow = oFactory.GetObject("powerup") as Powerup;
                int powerupIndex = rand.Next(0, 100);

                Powerup powerup;
                GameObject powerDecorator;
                //switch (powerupIndex)
                //{
                //    case 0:
                //        powerup = new Powerup(Powerup_type.BombTickDuration, x, y);
                //        break;
                //    case 2:
                //        powerup = new Powerup(Powerup_type.PowerDownX3, x, y);
                //        break;
                //    case 1:
                //        powerup = new Powerup(Powerup_type.PowerDown, x, y);
                //        break;
                //    case 3:
                //        powerup = new Powerup(Powerup_type.ExplosionSize, x, y);
                //        break;
                //    case 4:
                //        powerup = new Powerup(Powerup_type.AdditionalBomb, x, y);
                //        break;
                //    default:
                //        throw new NotImplementedException();

                //}
                if (powerupIndex > 50)
                {
                    powerup = new Powerup(Powerup_type.BombTickDuration, x, y);
                }
                else if (powerupIndex > 65)
                {                   
                    powerup = new Powerup(Powerup_type.ExplosionSize, x, y);
                }
                else if (powerupIndex > 80)
                {
                    powerup = new Powerup(Powerup_type.AdditionalBomb, x, y);
                }
                else if (powerupIndex > 90)
                {
                    powerup = new Powerup(Powerup_type.PowerDown, x, y);
                }
                else if (powerupIndex > 97)
                {
                    powerup = new Powerup(Powerup_type.PowerDownX3, x, y);
                }
                else
                {
                    return;
                }

                powerDecorator = new MiscDecorator(new ForegroundDecorator(new BackgroundDecorator(powerup)));
                powerDecorator.GetTextures();
                powerups.Add(powerup);
            }

        }
    }
}
