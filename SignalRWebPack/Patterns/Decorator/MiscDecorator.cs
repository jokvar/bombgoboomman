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
            var cast = (ForegroundDecorator)gameObject;
            _texture = cast.textures.FirstOrDefault();
        }
    }
}
