namespace SignalRWebPack.Patterns.FactoryMethod
{
    public class ExplosionTransportCreator : TransportObjectCreator
    {
        public override ITransportObject FactoryMethod()
        {
            return new ExplosionTransport();
        }
    }
}
