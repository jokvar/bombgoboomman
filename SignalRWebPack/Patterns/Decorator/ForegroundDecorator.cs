using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Decorator
{
    //Concrete decorator class
    class ForegroundDecorator : TextureDecorator
    {
        public ForegroundDecorator(GameObject gameObject)
            : base(gameObject)
        {
            //var cast = gameObject as Powerup;
            //PowerupDecoration();//(cast.type);
            //var cast = gameObject as BackgroundDecorator;
            
        }
        public GameObject PowerupDecoration()//(Powerup_type type)
        {
            Powerup powerup = (Powerup)gameObject;
            powerup.foreground = "powerup_bomb_naked";
            //var powerup = gameObject as Powerup;
            //switch (powerup.type)
            //{
            //    case Powerup_type.AdditionalBomb:
            //        gameObject.foreground = "powerup_bomb_naked";
            //        break;
            //    case Powerup_type.ExplosionSize:
            //        gameObject.foreground = "powerup_explosion_naked";
            //        break;
            //    case Powerup_type.BombTickDuration:
            //        gameObject.foreground = "powerup_bomb_naked";
            //        break;
            //    default:
            //        gameObject.foreground = null;
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
