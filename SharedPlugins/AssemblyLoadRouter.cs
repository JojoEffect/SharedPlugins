// //////////////////////////////////////////////////////////////////////////////////
// /                  Copyright (c) 2020 TRUMPF Laser GmbH                          /
// /        All Rights Reserved, see LICENSE.TXT for further details                /
// //////////////////////////////////////////////////////////////////////////////////

namespace SharedPlugins
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Loader;

    public class AssemblyLoadRouter
    {
        private readonly Dictionary<string, AssemblyLoadContext> registry = new Dictionary<string, AssemblyLoadContext>();

        public AssemblyLoadRouter()
        {

        }

        public bool TryRegisterLoadContext(AssemblyName assemblyName, AssemblyLoadContext assemblyLoadContext)
        {
            if (assemblyLoadContext is null) throw new ArgumentNullException(nameof(assemblyLoadContext));

            return assemblyName.Name != null && this.registry.TryAdd(assemblyName.Name, assemblyLoadContext);
        }

        public Assembly? RouteAssemblyLoad(AssemblyName assemblyName)
        {
            if (assemblyName.Name != null && this.registry.TryGetValue(assemblyName.Name, out AssemblyLoadContext loadContext))
            {
                return loadContext.LoadFromAssemblyName(assemblyName);
            }

            return null;
        }
    }
}