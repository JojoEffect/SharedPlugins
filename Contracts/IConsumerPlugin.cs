namespace Contracts
{
    public interface IConsumerPlugin : IPlugin
    {
        void SetProducer(IProducerPlugin producer);
    }
}