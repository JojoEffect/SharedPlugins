// //////////////////////////////////////////////////////////////////////////////////
// /                  Copyright (c) 2020 TRUMPF Laser GmbH                          /
// /        All Rights Reserved, see LICENSE.TXT for further details                /
// //////////////////////////////////////////////////////////////////////////////////

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
            Console.WriteLine("ConsumerPlugin - PluginManifest");

            Type serviceCollectionType = services.GetType();

            ConsoleLog.WriteAssemblyInformation(
                "ConsumerPlugin - PluginManifest", 
                serviceCollectionType, 
                typeof(IProducerPlugin), 
                typeof(ConsumerPlugin), 
                typeof(ProducerPlugin));

            services.AddTransient<IConsumerPlugin, ConsumerPlugin>();
            services.AddTransient<IProducerPlugin, ProducerPlugin>();
        }
    }
}