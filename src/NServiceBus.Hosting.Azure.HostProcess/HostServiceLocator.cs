namespace NServiceBus.Hosting.Azure.HostProcess
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;

    class HostServiceLocator : ServiceLocatorImplBase
    {
        public static string[] Args;

        protected override object DoGetInstance(Type serviceType, string key)
        {
            var endpoint = Type.GetType(key,true);

            var scannableString = Args.First(a => a.StartsWith("/scannedAssemblies="));
            var scannableAssembliesFullName = scannableString.Replace("/scannedAssemblies=","").Split(';');

            return new WindowsHost(endpoint, Args, scannableAssembliesFullName);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}