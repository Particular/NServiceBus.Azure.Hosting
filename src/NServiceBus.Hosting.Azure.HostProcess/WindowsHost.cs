namespace NServiceBus.Hosting.Azure.HostProcess
{
    using System;
    using System.Collections.Generic;

    class WindowsHost : MarshalByRefObject
    {
        GenericHost genericHost;

        public WindowsHost(Type endpointType, string[] args, IEnumerable<string> scannableAssembliesFullName)
        {
            var specifier = (IConfigureThisEndpoint)Activator.CreateInstance(endpointType);

            Program.EndpointId = Program.GetEndpointId(specifier);

            genericHost = new GenericHost(specifier, args, new List<Type> { typeof(Development) }, scannableAssembliesFullName);
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