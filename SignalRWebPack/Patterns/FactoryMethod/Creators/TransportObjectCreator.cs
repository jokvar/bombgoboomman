using SignalRWebPack.Models;
using System;

namespace SignalRWebPack.Patterns.FactoryMethod
{
    public abstract class TransportObjectCreator
    {
        public abstract ITransportObject FactoryMethod();

        public ITransportObject Pack(GameObject gameObject)
        {
            if (gameObject == null)
            {
                throw new ArgumentNullException();
            }
            var product = FactoryMethod();
            product.Pack(gameObject);
            return product;
        }
    }
}
