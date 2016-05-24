namespace NServiceBus
{
    /// <summary>
    /// Generic abstraction for code which will be called when the given profile is active.
    /// </summary>
    [ObsoleteEx(ReplacementTypeOrMember = nameof(IConfigureThisEndpoint), RemoveInVersion = "8", TreatAsErrorFromVersion = "7")]
    public interface IHandleProfile<T> : IHandleProfile where T : IProfile {}

    /// <summary>
    /// Abstraction for code which will be called when the given profile is active.
    /// Implementors should implement IHandleProfile{T} rather than IHandleProfile.
    /// </summary>
    [ObsoleteEx(ReplacementTypeOrMember = nameof(IConfigureThisEndpoint), RemoveInVersion = "8", TreatAsErrorFromVersion = "7")]
    public interface IHandleProfile
    {
        /// <summary>
        /// Called when a given profile is activated.
        /// </summary>
// ReSharper disable UnusedParameter.Global
        void ProfileActivated(EndpointConfiguration config);
// ReSharper restore UnusedParameter.Global

    }
}