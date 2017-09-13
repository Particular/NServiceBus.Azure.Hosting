namespace NServiceBus
{
    using System.Collections.Generic;
    using Configuration.AdvanceExtensibility;

    /// <summary>
    /// Extension methods for EndpointConfiguration.
    /// </summary>
    [ObsoleteEx(Message = "The azure cloud services host will be deprecated in the next major version. See upgrade guide and documentation for alternatives.",
        RemoveInVersion = "10.0")]
    public static class EndpointStartableAndStoppableExtensions
    {
        /// <summary>
        /// Register a specific instance of an IWantToRunWhenEndpointStartsAndStops implementation
        /// </summary>
        public static void RunWhenEndpointStartsAndStops(this EndpointConfiguration configuration, IWantToRunWhenEndpointStartsAndStops startableAndStoppable)
        {
            var settings = configuration.GetSettings();

            if (!settings.TryGet(out List<IWantToRunWhenEndpointStartsAndStops> instanceList))
            {
                instanceList = new List<IWantToRunWhenEndpointStartsAndStops>();
                settings.Set<List<IWantToRunWhenEndpointStartsAndStops>>(instanceList);
            }

            instanceList.Add(startableAndStoppable);
        }
    }
}