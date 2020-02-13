namespace ConsumerPlugin
{
    using System;
    using Contracts;
    using ProducerPlugin;

    public class ConsumerPlugin : IConsumerPlugin
    {
        private static readonly Type expectedProducerType = typeof(ProducerPlugin);

        public ConsumerPlugin(IProducerPlugin producer)
        {
            Type consumerType = this.GetType();
            Type providerType = producer.GetType();

            ConsoleLog.WriteAssemblyInformation("ConsumerPlugin - Constructor", consumerType, providerType);
        }


        public void SetProducer(IProducerPlugin producer)
        {
            Type newProducerType = producer.GetType();

            bool typeCheckResult = newProducerType == expectedProducerType;

            ConsoleLog.WriteAssemblyInformation(
                $"ConsumerPlugin - SetProducer - Type check result: {typeCheckResult} - [expected, provided]",
                expectedProducerType,
                newProducerType);
        }
    }
}
