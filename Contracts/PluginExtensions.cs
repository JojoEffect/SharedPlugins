namespace Contracts
{
    using Microsoft.Extensions.DependencyInjection;

    public static class PluginExtensions
    {
        public static IServiceCollection AddProducer<TPlugin>(this IServiceCollection serviceCollection) where TPlugin : IProducerPlugin
        {
            serviceCollection.AddTransient(typeof(TPlugin));
            serviceCollection.AddTransient(typeof(IPlugin), typeof(TPlugin));
            serviceCollection.AddTransient(typeof(IProducerPlugin), typeof(TPlugin));

            return serviceCollection;
        }

        public static IServiceCollection AddConsumer<TPlugin>(this IServiceCollection serviceCollection) where TPlugin : IConsumerPlugin
        {
            serviceCollection.AddTransient(typeof(TPlugin));
            serviceCollection.AddTransient(typeof(IPlugin), typeof(TPlugin));
            serviceCollection.AddTransient(typeof(IConsumerPlugin), typeof(TPlugin));

            return serviceCollection;
        }
    }
}