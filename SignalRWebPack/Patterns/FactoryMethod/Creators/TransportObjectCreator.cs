using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.FactoryMethod
{
    public abstract class TransportObjectCreator
    {
        public abstract ITransportObject FactoryMethod();

        public ITransportObject Pack(GameObject gameObject)
        {
            var product = FactoryMethod();
            product.Pack(gameObject);
            return product;
        }
    }
}
