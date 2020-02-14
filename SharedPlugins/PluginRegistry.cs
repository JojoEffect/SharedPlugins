﻿namespace SharedPlugins
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Contracts;
    using McMaster.NETCore.Plugins;
    using Microsoft.Extensions.DependencyInjection;

    public class PluginRegistry
    {
        private readonly List<Type> sharedTypes = new List<Type>();
        private readonly Dictionary<string, (Type pluginType, PluginLoader pluginLoader)> registry = new Dictionary<string, (Type pluginType, PluginLoader pluginLoader)>();

        public PluginRegistry(string producerPluginsPath, string consumerPluginsPath)
        {
            this.RegisterPlugins(producerPluginsPath);
            this.RegisterPlugins(consumerPluginsPath);
        }

        public IPlugin GetPlugin(string typeName)
        {
            return this.GetPlugin<IPlugin>(typeName);
        }

        public TPlugin GetPlugin<TPlugin>(string typeName) where TPlugin : class, IPlugin
        {
            if (!this.registry.TryGetValue(typeName, out (Type pluginType, PluginLoader pluginLoader) entry))
                throw new Exception($"No plugin found with type name '{typeName}'.");

            IServiceProvider pluginServiceProvider = this.CreatePluginServiceProvider(entry.pluginLoader);

            object resolvedObject = pluginServiceProvider.GetService(entry.pluginType);

            ConsoleLog.WriteAssemblyInformation("PluginRegistry - GetPlugin",resolvedObject.GetType());

            if (!(resolvedObject is TPlugin resolvedPlugin))
                throw new Exception($"Plugin type name '{typeName}' is not of requested type '{typeof(TPlugin)}'.");

            return resolvedPlugin;
        }

        private IServiceProvider CreatePluginServiceProvider(PluginLoader pluginLoader)
        {
            Assembly pluginAssembly = pluginLoader.LoadDefaultAssembly();
            Type[] allTypes = pluginAssembly.GetTypes();
            Type pluginManifestType = allTypes.FirstOrDefault(t => typeof(IPluginManifest).IsAssignableFrom(t) && !t.IsAbstract);

            if (pluginManifestType is null) throw new TypeLoadException($"Exactly one '{nameof(IPluginManifest)}' has to be defined in plugin assembly '{pluginAssembly.FullName}'.");

            if (!(Activator.CreateInstance(pluginManifestType) is IPluginManifest pluginManifest))
                throw new Exception($"The implementation of '{typeof(IPluginManifest)}' must have a parameter-less constructor.");

            IServiceCollection pluginServices = new ServiceCollection();
            pluginManifest.ConfigureServices(pluginServices);

            return pluginServices.BuildServiceProvider();
        }

        private void RegisterPlugins(string pluginFolder)
        {
            foreach (string pluginDirectory in Directory.GetDirectories(pluginFolder))
            {
                string? pluginName = Path.GetFileName(pluginDirectory);
                string assemblyFile = Path.Combine(pluginDirectory, pluginName + ".dll");

                PluginLoader pluginLoader = this.CreatePluginLoader(assemblyFile, out IEnumerable<Type> foundPluginTypes);
                this.sharedTypes.AddRange(foundPluginTypes.Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract));

                foreach (Type pluginType in foundPluginTypes)
                {
                    this.registry.Add(pluginType.FullName, (pluginType, pluginLoader));
                }
            }
        }

        private PluginLoader CreatePluginLoader(string assemblyFile, out IEnumerable<Type> foundPluginTypes)
        {
            PluginLoader loader = PluginLoader.CreateFromAssemblyFile(
                assemblyFile: assemblyFile,
                true,
                // Here we share all producer plugin types
                sharedTypes: this.sharedTypes.ToArray(),
                config =>
                {
                    config.PreferSharedTypes = true;
                });

            Assembly pluginAssembly = loader.LoadDefaultAssembly();
            Type[] allTypes = pluginAssembly.GetTypes();

            foundPluginTypes = allTypes.Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract);

            ConsoleLog.WriteAssemblyInformation("PLUGIN LOADER PLUGINS", foundPluginTypes.ToArray());

            return loader;
        }
    }
}