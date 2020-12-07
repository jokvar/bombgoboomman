using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Patterns.AbstractFactory;
using SignalRWebPack.Patterns.Decorator;
using SignalRWebPack.Patterns.Mediator;

namespace SignalRWebPack.Models
{
    public class PowerupSpawner
    {
        protected IMediator _mediator;

        public PowerupSpawner()
        {

        }

        public void SpawnPowerup(Player player, List<Powerup> powerups)
        {
            ObjectFactory oFactory = FactoryProducer.getFactory("ObjectFactory") as ObjectFactory;
            var rand = new Random();
            Powerup pow = oFactory.GetObject("powerup") as Powerup;
            int powerupIndex = rand.Next(0, 100);

            Powerup powerup = oFactory.GetObject("powerup") as Powerup;
            GameObject powerDecorator;

            int x = player.x;
            int y = player.y;

            if (powerupIndex < 30)
            {
                powerup = new Powerup(Powerup_type.BombTickDuration, x, y);
            }
            else if (powerupIndex < 60)
            {
                powerup = new Powerup(Powerup_type.ExplosionSize, x, y);
            }
            else if (powerupIndex < 101)
            {
                powerup = new Powerup(Powerup_type.AdditionalBomb, x, y);
            }

            powerDecorator = new MiscDecorator(new ForegroundDecorator(new BackgroundDecorator(powerup)));
            powerDecorator.GetTextures();
            powerups.Add(powerup);
            _mediator.Notify("PowerupSpawned");
        }

        public void SetMediator(IMediator mediator)
        {
            this._mediator = mediator;
        }
    }
}
