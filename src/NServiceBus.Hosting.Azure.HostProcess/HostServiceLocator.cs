namespace NServiceBus.Hosting.Azure.HostProcess
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;

    class HostServiceLocator : ServiceLocatorImplBase
    {
        protected override object DoGetInstance(Type serviceType, string key)
        {
            var endpoint = Type.GetType(key,true);

            return new WindowsHost(endpoint);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}