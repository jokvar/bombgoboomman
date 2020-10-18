namespace SignalRWebPack.Patterns.FactoryMethod
{
    public class BombTransportCreator : TransportObjectCreator
    {
        public override ITransportObject FactoryMethod()
        {
            return new BombTransport();
        }
    }
}
