namespace NServiceBus
{
    /// <summary>
    /// Abstraction for code that will be called that will take dependent action based upon
    /// the Profile(s) that are active. Useful for implementing special functionality if
    /// a specific profile is activated, and implementing default functionality otherwise.
    /// </summary>
    [ObsoleteEx(ReplacementTypeOrMember = nameof(IConfigureThisEndpoint), RemoveInVersion = "8", TreatAsErrorFromVersion = "7")]
    public interface IHandleAnyProfile : IHandleProfile<IProfile>, IWantTheListOfActiveProfiles
    {
    }
}