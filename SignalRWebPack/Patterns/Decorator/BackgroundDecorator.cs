using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Decorator
{
    //Concrete decorator class
    class BackgroundDecorator : TextureDecorator
    {
        public BackgroundDecorator(GameObject gameObject)
            : base(gameObject)
        {
            //PowerupDecoration();
        }

        public GameObject PowerupDecoration()
        {
            //gameObject.texture = "powerup";
            _type = "powerup";
            Powerup powerup = (Powerup)gameObject;
            powerup.foreground = "TEST";
            return powerup;

        }

        public void PlayerDecoration()
        {

        }

        public void BombDecoration()
        {

        }


    }
}
