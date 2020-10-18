namespace SignalRWebPack.Patterns.FactoryMethod
{
    public class PowerupTransportCreator : TransportObjectCreator
    {
        public override ITransportObject FactoryMethod()
        {
            return new PowerupTransport();
        }
    }
}
