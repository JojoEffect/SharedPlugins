// //////////////////////////////////////////////////////////////////////////////////
// /                  Copyright (c) 2020 TRUMPF Laser GmbH                          /
// /        All Rights Reserved, see LICENSE.TXT for further details                /
// //////////////////////////////////////////////////////////////////////////////////

namespace Contracts
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    public abstract class PluginManifestBase : IPluginManifest
    {
        public IServiceProvider GetServiceProvider(IServiceCollection hostServices)
        {
            IServiceCollection services = new ServiceCollection();
            using IEnumerator<ServiceDescriptor> hostServicesEnumerator = hostServices.GetEnumerator();

            if (hostServicesEnumerator.Current != null)
            {
                do
                {
                    services.Add(hostServicesEnumerator.Current);
                } while (hostServicesEnumerator.MoveNext());
            }

            this.ConfigureServices(services);

            return services.BuildServiceProvider();
        }

        protected abstract void ConfigureServices(IServiceCollection services);
    }
}