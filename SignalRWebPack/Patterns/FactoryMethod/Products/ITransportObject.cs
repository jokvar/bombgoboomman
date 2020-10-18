using SignalRWebPack.Models;


namespace SignalRWebPack.Patterns.FactoryMethod
{
    public interface ITransportObject
    {
        double x { get; set; }
        double y { get; set; }
        string texture { get; set; }

        void Pack(GameObject gameobject);
    }
}
