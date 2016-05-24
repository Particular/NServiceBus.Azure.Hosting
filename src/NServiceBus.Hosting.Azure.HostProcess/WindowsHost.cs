namespace NServiceBus.Hosting.Azure.HostProcess
{
    using System;
    using System.Collections.Generic;

    class WindowsHost : MarshalByRefObject
    {
        GenericHost genericHost;

        public WindowsHost(Type endpointType)
        {
            var specifier = (IConfigureThisEndpoint)Activator.CreateInstance(endpointType);

            Program.EndpointId = Program.GetEndpointId(specifier);

            genericHost = new GenericHost(specifier);
        }

        public void Start()
        {
            genericHost.Start();
        }

        public void Stop()
        {
            genericHost.Stop();
        }
    }
}