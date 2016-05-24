namespace NServiceBus
{
    /// <summary>
    /// Indicates that the implementing class will configure the host to dynamically
    /// load and run multiple endpoints from Azure cloud storage.
    /// </summary>
    public interface IConfigureThisHost
    {
        /// <summary>
        /// Allows to override host settings.
        /// </summary>
        /// <returns>Host settings.</returns>
        HostingSettings Configure();
    }
}