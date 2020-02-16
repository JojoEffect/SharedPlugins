namespace Contracts
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public interface IPluginManifest
    {
        IServiceProvider GetServiceProvider(IServiceCollection hostServices);
    }
}