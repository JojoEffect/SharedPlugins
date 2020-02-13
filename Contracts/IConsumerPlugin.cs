// //////////////////////////////////////////////////////////////////////////////////
// /                  Copyright (c) 2020 TRUMPF Laser GmbH                          /
// /        All Rights Reserved, see LICENSE.TXT for further details                /
// //////////////////////////////////////////////////////////////////////////////////

namespace Contracts
{
    public interface IConsumerPlugin : IPlugin
    {
        void SetProducer(IProducerPlugin producer);
    }
}