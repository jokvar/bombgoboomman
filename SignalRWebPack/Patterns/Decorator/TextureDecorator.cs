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

        //protected string _background = "undefined background";
        //protected string _foreground = "undefined foreground";
        //protected string _misc = "undefined misc";

        protected string _type = "undefined type";
        public TextureDecorator(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public string getType()
        {
            return _type;
        }
    }
}
