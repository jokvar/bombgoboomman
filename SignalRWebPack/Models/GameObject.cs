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
        //foreground image (e.g. player object, bomb object etc.)
        public string foreground { get; set; }
        //additional image such as a plus image for powerups
        public string misc { get; set; }
        public int x { get; set; }
        public int y { get; set; }

    }
}
