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
            var cast = (BackgroundDecorator)gameObject;
            _texture = cast.textures.FirstOrDefault();
            textures = new List<string>();

            switch (_texture)
            {
                case "powerup_bomb_naked":
                    textures.Add("powerup_plus");
                    break;

                case "powerup_explosion_naked":
                    textures.Add("powerup_plus");
                    break;

                case "powerup_bombTick_naked":
                    textures.Add("powerup_time");
                    break;

                default:
                    throw new NotImplementedException();
            }

        }
    }
}
