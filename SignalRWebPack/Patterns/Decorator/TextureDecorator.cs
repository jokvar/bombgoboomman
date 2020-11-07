using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Decorator
{
    //Decorator class
    abstract class TextureDecorator : GameObject
    {
        protected GameObject gameObject;

        protected string _texture = "undefined texture";
        public TextureDecorator(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public override List<string> GetTextures()
        {
            if(gameObject.GetTextures() != null)
            {
                if (!gameObject.GetTextures().Contains(_texture) && _texture != null)
                {
                    gameObject.GetTextures().Add(_texture);
                }
            }
            return gameObject.GetTextures();
        }
    }
}
