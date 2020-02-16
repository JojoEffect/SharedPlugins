namespace ProducerPlugin
{
    using System;
    using Contracts;
    using Newtonsoft.Json;

    public class ProducerPlugin : IProducerPlugin
    {
        public ProducerPlugin()
        {
            Type providerType = this.GetType();

            ConsoleLog.WriteAssemblyInformation("ProducerPlugin - Constructor", providerType, typeof(JsonConvert));
        }
    }
}
