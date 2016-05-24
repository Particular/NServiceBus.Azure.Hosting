namespace NServiceBus
{
    /// <summary>
    /// Called in order to configure logging.
    /// </summary>
    /// <remarks>
    /// If you want logging configured regardless of profiles, do not use this interface,
    /// instead configure logging before you call <see cref="Endpoint.Create"/> if you self hosting or configure logging in <see cref="IConfigureThisEndpoint.Customize"/>.
    /// Implementors should work against the generic version of this interface in the host.
    /// </remarks>
    [ObsoleteEx(Message = "Configure Azure Diagnostics through Visual Studio or .wadcfgx instead.", RemoveInVersion = "8", TreatAsErrorFromVersion = "7")]
    public interface IConfigureLogging
    {
        /// <summary>
        /// Performs all logging configuration.
        /// </summary>
        // ReSharper disable once UnusedParameter.Global            
        void Configure(IConfigureThisEndpoint specifier);
    }

    /// <summary>
    /// Called in order to configure logging for the given profile type.
    /// If an implementation isn't found for a given profile, then the search continues
    /// recursively up that profile's inheritance hierarchy.
    /// </summary>
    [ObsoleteEx(Message = "Configure Azure Diagnostics through Visual Studio or .wadcfgx instead.", RemoveInVersion = "8", TreatAsErrorFromVersion = "7")]
    public interface IConfigureLoggingForProfile<T> : IConfigureLogging where T : IProfile {}
}