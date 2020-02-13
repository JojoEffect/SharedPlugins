// //////////////////////////////////////////////////////////////////////////////////
// /                  Copyright (c) 2020 TRUMPF Laser GmbH                          /
// /        All Rights Reserved, see LICENSE.TXT for further details                /
// //////////////////////////////////////////////////////////////////////////////////

namespace Contracts
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IPluginManifest
    {
        void ConfigureServices(IServiceCollection services);
    }
}