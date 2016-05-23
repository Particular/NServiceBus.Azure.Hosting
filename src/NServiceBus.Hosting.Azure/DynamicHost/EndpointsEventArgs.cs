namespace NServiceBus.Hosting.Azure
{
    using System;
    using System.Collections.Generic;

    class EndpointsEventArgs : EventArgs
    {
        public IEnumerable<EndpointToHost> Endpoints { get; set; }
    }
}