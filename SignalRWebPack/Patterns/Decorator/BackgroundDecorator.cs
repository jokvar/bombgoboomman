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
            if(gameObject is Powerup)
            {
                _texture = "powerup";
                var cast = (Powerup)gameObject;
                textures = new List<string>();

                switch (cast.type)
                {
                    case Powerup_type.AdditionalBomb:
                        textures.Add("powerup_bomb_naked");
                        //foreground = "powerup_bomb_naked";
                        break;

                    case Powerup_type.ExplosionSize:
                        textures.Add("powerup_explosion_naked");
                        break;

                    case Powerup_type.BombTickDuration:
                        textures.Add("powerup_bomb_naked");
                        break;

                    case Powerup_type.PowerDown:
                        textures.Add("powerup_powerdown");
                        break;
                        
                    case Powerup_type.PowerDownX3:
                        textures.Add("powerup_powerdownX3");
                        break;

                    default:
                        //textures.Add("undefined foreground");
                        break;
                }
            }
            
            
        }

        //public GameObject PowerupDecoration()
        //{
        //    //gameObject.texture = "powerup";
        //    _type = "powerup";
        //    Powerup powerup = (Powerup)gameObject;
        //    powerup.foreground = "TEST";
        //    return powerup;

        //}

        //public void PlayerDecoration()
        //{

        //}

        //public void BombDecoration()
        //{

        //}


    }
}
