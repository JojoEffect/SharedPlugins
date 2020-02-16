namespace SharedPlugins
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Loader;
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;

    public class PluginRegistry
    {
        private readonly AssemblyLoadRouter loadRouter = new AssemblyLoadRouter();
        private readonly Dictionary<string, (Type pluginType, PluginLoadContext loadContext)> registry = new Dictionary<string, (Type pluginType, PluginLoadContext loadContext)>();
        private readonly Type[] DefaultSharedTypes = {
            typeof(IServiceCollection),
            typeof(IPluginManifest),
            typeof(IPlugin),
            typeof(IProducerPlugin),
            typeof(IConsumerPlugin),
            typeof(PluginManifestBase),
            typeof(ConsoleLog),
            typeof(PluginExtensions),
            typeof(PluginManifestBase)
        };

        public PluginRegistry(string producerPluginsPath, string consumerPluginsPath)
        {
            AssemblyLoadContext.Default.Resolving += this.SharedPluginsContextOnResolving;

            this.RegisterPlugins(producerPluginsPath);
            this.RegisterPlugins(consumerPluginsPath);
        }

        private Assembly? SharedPluginsContextOnResolving(AssemblyLoadContext arg1, AssemblyName arg2)
        {
            ConsoleLog.WriteResolvingEvent(arg1, arg2);

            if (arg1.Name == "SharedPlugins")
            {
                return Assembly.LoadFrom("C:\\Users\\TauberJo\\source\\repos\\SharedPlugins\\SharedPlugins\\bin\\Debug\\netcoreapp3.0\\plugins\\producer\\ProducerPlugin.ProducerPlugin\\Newtonsoft.Json.dll");
            }

            return null;
        }

        public IPlugin GetPlugin(string typeName)
        {
            return this.GetPlugin<IPlugin>(typeName);
        }

        public TPlugin GetPlugin<TPlugin>(string typeName) where TPlugin : class, IPlugin
        {
            if (!this.registry.TryGetValue(typeName, out (Type pluginType, PluginLoadContext loadContext) entry))
                throw new Exception($"No plugin found with type name '{typeName}'.");

            IServiceProvider pluginServiceProvider = this.CreatePluginServiceProvider(entry.loadContext);

            object resolvedObject = pluginServiceProvider.GetService(entry.pluginType);

            ConsoleLog.WriteAssemblyInformation("PluginRegistry - GetPlugin",resolvedObject.GetType());

            if (!(resolvedObject is TPlugin resolvedPlugin))
                throw new Exception($"Plugin type name '{typeName}' is not of requested type '{typeof(TPlugin)}'.");

            return resolvedPlugin;
        }

        private IServiceProvider CreatePluginServiceProvider(PluginLoadContext loadContext)
        {
            Assembly pluginAssembly = loadContext.LoadPluginAssembly();
            Type[] allTypes = pluginAssembly.GetTypes();
            Type pluginManifestType = allTypes.FirstOrDefault(t => typeof(IPluginManifest).IsAssignableFrom(t) && !t.IsAbstract);

            if (pluginManifestType is null) throw new TypeLoadException($"Exactly one '{nameof(IPluginManifest)}' has to be defined in plugin assembly '{pluginAssembly.FullName}'.");

            if (!(Activator.CreateInstance(pluginManifestType) is IPluginManifest pluginManifest))
                throw new Exception($"The implementation of '{typeof(IPluginManifest)}' must have a parameter-less constructor.");

            IServiceCollection hostServices = new ServiceCollection();

            //Add host services

            return pluginManifest.GetServiceProvider(hostServices);
        }

        private void RegisterPlugins(string pluginFolder)
        {
            foreach (string pluginDirectory in Directory.GetDirectories(pluginFolder))
            {
                string? pluginName = Path.GetFileName(pluginDirectory);
                string assemblyFile = Path.Combine(pluginDirectory, pluginName + ".dll");

                PluginLoadContext loadContext = this.CreatePluginContext(assemblyFile, out Assembly pluginAssembly);

                this.loadRouter.TryRegisterLoadContext(pluginAssembly.GetName(), loadContext);

                Type[] allTypes = pluginAssembly.GetTypes();
                Type foundPluginType = allTypes.FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract);

                ConsoleLog.WriteAssemblyInformation("PLUGIN LOAD CONTEXT PLUGIN", foundPluginType);

                this.registry.Add(foundPluginType.FullName, (foundPluginType, loadContext));
            }
        }

        private PluginLoadContext CreatePluginContext(string assemblyFile, out Assembly pluginAssembly)
        {
            PluginLoadContext loadContext = new PluginLoadContext(
                assemblyFile, 
                true, 
                this.loadRouter, 
                this.DefaultSharedTypes, 
                AssemblyLoadContext.Default, 
                true);
            pluginAssembly = loadContext.LoadPluginAssembly();
            return loadContext;
        }
    }
}