namespace SharedPlugins
{
    using System;
    using System.IO;
    using Contracts;

    class Program
    {
        static void Main(string[] args)
        {
            string producerPluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins/producer");
            string consumerPluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins/consumer");
            PluginRegistry pluginRegistry = new PluginRegistry(producerPluginsPath, consumerPluginsPath);

            IProducerPlugin producerPlugin = pluginRegistry.GetPlugin<IProducerPlugin>("ProducerPlugin.ProducerPlugin");
            //IConsumerPlugin consumerPlugin = pluginRegistry.GetPlugin<IConsumerPlugin>("ConsumerPlugin.ConsumerPlugin");

            //consumerPlugin.SetProducer(producerPlugin);
        }
    }
}
