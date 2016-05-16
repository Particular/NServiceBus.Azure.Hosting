namespace NServiceBus
{
    /// <summary>
    ///     Indicates this endpoint is a host that merely loads other processes.
    /// </summary>
    [ObsoleteEx(ReplacementTypeOrMember = nameof(AsA_DynamicController), RemoveInVersion = "8", TreatAsErrorFromVersion = "7")]
    public interface AsA_Host
    {
    }
}