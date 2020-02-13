namespace AlcTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Loader;
    using Contracts;
    using McMaster.NETCore.Plugins;

    class Program
    {
        static void Main(string[] args)
        {
            string assemblyFile = args[0];

            if (!Path.IsPathRooted(assemblyFile))
            {
                assemblyFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyFile);
            }

            AssemblyLoadContext sharedAssembliesContext = CreateSharedContext(assemblyFile, out IEnumerable<Type> assemblyTypes);
            ConsoleLog.WriteAssemblyInformation($"SHARED TYPES in ALC '{sharedAssembliesContext.Name}'", assemblyTypes.ToArray());

            PluginLoader pluginLoader = CreatePluginLoader(assemblyFile, sharedAssembliesContext, out IEnumerable<Type> loaderTypes);
            ConsoleLog.WriteAssemblyInformation("PLUGIN LOADER TYPES", loaderTypes.ToArray());
        }

        private static AssemblyLoadContext CreateSharedContext(string assemblyFile, out IEnumerable<Type> sharedTypes)
        {
            AssemblyLoadContext sharedAssembliesContext = new AssemblyLoadContext("SharedAssemblies");
            sharedAssembliesContext.LoadFromAssemblyPath(assemblyFile);

            Assembly sharedAssembly = sharedAssembliesContext.Assemblies.First();
            sharedTypes = sharedAssembly.GetTypes();

            return sharedAssembliesContext;
        }

        private static PluginLoader CreatePluginLoader(string assemblyFile, AssemblyLoadContext sharedAssembliesContext, out IEnumerable<Type> sharedTypes)
        {
            PluginLoader loader = PluginLoader.CreateFromAssemblyFile(
                assemblyFile: assemblyFile,
                true,
                sharedTypes: Array.Empty<Type>(),
                config =>
                {
                    config.DefaultContext = sharedAssembliesContext;
                    config.PreferSharedTypes = true;
                });

            Assembly pluginAssembly = loader.LoadDefaultAssembly();
            sharedTypes = pluginAssembly.GetTypes();

            return loader;
        }
    }
}
