using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Models
{
    public abstract class GameObject
    {
        //background image
        public string texture { get; set; }
        //List of textures for decorating gameobjects (limited to powerups for the time being)
        public List<string> textures { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public abstract List<string> GetTextures();

    }
}
