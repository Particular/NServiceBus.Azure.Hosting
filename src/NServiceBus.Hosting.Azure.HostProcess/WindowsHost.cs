namespace NServiceBus.Hosting.Azure.HostProcess
{
    using System;

    class WindowsHost : MarshalByRefObject
    {
        GenericHost genericHost;

        public WindowsHost(Type endpointType)
        {
            var specifier = (IConfigureThisEndpoint)Activator.CreateInstance(endpointType);

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