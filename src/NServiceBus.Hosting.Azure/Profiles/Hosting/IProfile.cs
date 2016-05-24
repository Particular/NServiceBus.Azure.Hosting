namespace NServiceBus
{
    /// <summary>
    /// Marker interface to indicate a run-time profile.
    /// Implementors must be concrete class - interfaces are not supported.
    /// </summary>
    [ObsoleteEx(ReplacementTypeOrMember = nameof(IConfigureThisEndpoint), RemoveInVersion = "8", TreatAsErrorFromVersion = "7")]
    public interface IProfile
    {
    }
}
