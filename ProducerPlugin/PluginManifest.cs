namespace ProducerPlugin
{
    using System;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    public class PluginManifest : IPluginManifest
    {
        public void ConfigureServices(IServiceCollection services)
        {
            Type serviceCollectionType = services.GetType();
            ConsoleLog.WriteAssemblyInformation("ProducerPlugin - PluginManifest", serviceCollectionType, typeof(IProducerPlugin), typeof(ProducerPlugin));
            services.AddProducer<ProducerPlugin>();
        }
    }
}