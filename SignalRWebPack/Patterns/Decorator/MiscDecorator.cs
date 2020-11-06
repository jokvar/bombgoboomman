using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Decorator
{
    //Concrete decorator class
    class MiscDecorator : TextureDecorator
    {
        public MiscDecorator(GameObject gameObject)
            : base(gameObject)
        {
            //var cast = gameObject as Powerup;
            //PowerupDecoration();// (cast.type);
        }

        public GameObject PowerupDecoration()//(Powerup_type type)
        {
            Powerup powerup = (Powerup)gameObject;
            powerup.misc = "powerup_plus";
            //var powerup = gameObject as Powerup;
            //switch (powerup.type)
            //{
            //    case Powerup_type.AdditionalBomb:
            //        gameObject.misc = "powerup_plus";
            //        break;
            //    case Powerup_type.ExplosionSize:
            //        gameObject.misc = "powerup_plus";
            //        break;
            //    case Powerup_type.BombTickDuration:
            //        gameObject.misc = "powerup_time";
            //        break;
            //    default:
            //        gameObject.misc = null;
            //        break;
            //}
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
