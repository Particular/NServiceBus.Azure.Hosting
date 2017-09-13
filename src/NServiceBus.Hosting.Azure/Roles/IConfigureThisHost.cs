namespace NServiceBus
{
    /// <summary>
    /// Indicates that the implementing class will configure the host to dynamically
    /// load and run multiple endpoints from Azure cloud storage.
    /// </summary>
    [ObsoleteEx(Message = "The azure cloud services host will be deprecated in the next major version. See upgrade guide and documentation for alternatives.",
        RemoveInVersion = "10.0")]
#pragma warning disable 1591
    public interface IConfigureThisHost
    {
        /// <summary>
        /// Allows to override host settings.
        /// </summary>
        /// <returns>Host settings.</returns>
        HostingSettings Configure();
    }
#pragma warning restore 1591
}