using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Memento
{
    public class Memento
    {
        private int lives;
        private String texture;

        public Memento(int lives, String texture)
        {
            this.lives = lives;
            this.texture = texture;
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public String Texture
        {
            get { return texture; }
            set { texture = value; }
        }
    }
}
