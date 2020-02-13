// //////////////////////////////////////////////////////////////////////////////////
// /                  Copyright (c) 2020 TRUMPF Laser GmbH                          /
// /        All Rights Reserved, see LICENSE.TXT for further details                /
// //////////////////////////////////////////////////////////////////////////////////

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
            ConsoleLog.WriteAssemblyInformation("ProviderPlugin - PluginManifest", serviceCollectionType, typeof(IProducerPlugin), typeof(ProducerPlugin));
            services.AddTransient<IProducerPlugin, ProducerPlugin>();
        }
    }
}