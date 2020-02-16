namespace Contracts
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IPluginManifest
    {
        void ConfigureServices(IServiceCollection services);
    }
}