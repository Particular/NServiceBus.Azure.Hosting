namespace NServiceBus
{
    /// <summary>
    /// Indicate that the implementing class will specify configuration.
    /// </summary>
    [ObsoleteEx(Message = "The azure cloud services host will be deprecated in the next major version. See upgrade guide and documentation for alternatives.",
        RemoveInVersion = "10.0")]
#pragma warning disable 1591
    public interface IConfigureThisEndpoint
    {
        /// <summary>
        /// Allows to override default settings.
        /// </summary>
        /// <param name="configuration">Endpoint configuration builder.</param>
        void Customize(EndpointConfiguration configuration);
    }
#pragma warning restore 1591
}