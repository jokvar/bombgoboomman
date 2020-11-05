using SignalRWebPack.Models;
using System;

namespace SignalRWebPack.Patterns.FactoryMethod
{
    public class ExplosionTransport : ITransportObject
    {
        public double x { get; set; }
        public double y { get; set; }
        public string texture { get; set; }

        public void Pack(GameObject gameObject)
        {
            if (gameObject.GetType() != typeof(Explosion))
            {
                throw new ArgumentException(
                    string.Format("Attempted to Pack {0} as {1}.",
                    gameObject.GetType().ToString(),
                    GetType().ToString()
                    )
                );
            }
            x = gameObject.x;
            y = gameObject.y;
            texture = gameObject.texture;
        }
    }
}
