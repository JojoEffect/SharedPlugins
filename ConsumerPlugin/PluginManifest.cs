namespace ConsumerPlugin
{
    using System;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using ProducerPlugin;

    public class PluginManifest : IPluginManifest
    {
        public void ConfigureServices(IServiceCollection services)
        {
            Type serviceCollectionType = services.GetType();

            ConsoleLog.WriteAssemblyInformation(
                "ConsumerPlugin - PluginManifest", 
                serviceCollectionType, 
                typeof(IProducerPlugin), 
                typeof(ConsumerPlugin), 
                typeof(ProducerPlugin));

            services.AddProducer<ProducerPlugin>();
            services.AddConsumer<ConsumerPlugin>();
        }
    }
}