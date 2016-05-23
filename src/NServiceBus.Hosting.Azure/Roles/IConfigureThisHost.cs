namespace NServiceBus
{
    /// <summary>
    /// Indicates that the implementing class will configure the host.
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