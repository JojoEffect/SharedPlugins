namespace ProducerPlugin
{
    using System;
    using Contracts;

    public class ProducerPlugin : IProducerPlugin
    {
        public ProducerPlugin()
        {
            Type providerType = this.GetType();

            ConsoleLog.WriteAssemblyInformation("ProducerPlugin - Constructor", providerType);
        }
    }
}
