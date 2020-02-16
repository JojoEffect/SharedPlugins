// //////////////////////////////////////////////////////////////////////////////////
// /                  Copyright (c) 2020 TRUMPF Laser GmbH                          /
// /        All Rights Reserved, see LICENSE.TXT for further details                /
// //////////////////////////////////////////////////////////////////////////////////

namespace SharedPlugins
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Loader;
    using Contracts;

    public class PluginLoadContext : AssemblyLoadContext
    {
        private readonly string pluginAssemblyPath;
        private readonly AssemblyLoadRouter assemblyLoadRouter;
        private readonly AssemblyLoadContext defaultLoadContext;
        private readonly HashSet<string> sharedAssemblies = new HashSet<string>();
        private readonly bool preferDefaultContext;

        public PluginLoadContext(
            string pluginAssemblyPath, 
            bool isCollectible, 
            AssemblyLoadRouter assemblyLoadRouter,
            Type[] sharedTypes,
            AssemblyLoadContext defaultLoadContext,
            bool preferDefaultContext)
            : base(Path.GetFileNameWithoutExtension(pluginAssemblyPath), isCollectible)
        {
            this.pluginAssemblyPath = pluginAssemblyPath;
            this.assemblyLoadRouter = assemblyLoadRouter;
            this.defaultLoadContext = defaultLoadContext;
            this.preferDefaultContext = preferDefaultContext;

            foreach (Type type in sharedTypes)
            {
                this.PreferDefaultLoadContextAssembly(type.Assembly.GetName());
            }
        }

        public Assembly LoadPluginAssembly()
        {
            return this.LoadFromAssemblyPath(this.pluginAssemblyPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            ConsoleLog.WriteLoadContextLoadCall(this, assemblyName);

            Assembly? routerAssembly = this.assemblyLoadRouter.RouteAssemblyLoad(assemblyName);
            if (routerAssembly != null) return routerAssembly;

            if (this.preferDefaultContext && this.sharedAssemblies.Contains(assemblyName.Name))
            {
                Assembly? defaultAssembly = this.defaultLoadContext.LoadFromAssemblyName(assemblyName);

                if (defaultAssembly != null) return defaultAssembly;
            }

            string? baseDirectory = Path.GetDirectoryName(this.pluginAssemblyPath);

            string resourcePath = Path.Combine(baseDirectory, assemblyName.CultureName, assemblyName.Name + ".dll");

            if (File.Exists(resourcePath))
            {
                return this.LoadFromAssemblyPath(resourcePath);
            }

            return null;
        }

        public void PreferDefaultLoadContextAssembly(AssemblyName assemblyName)
        {
            if (assemblyName.Name == null || this.sharedAssemblies.Contains(assemblyName.Name)) return;

            this.sharedAssemblies.Add(assemblyName.Name);
        }
    }
}