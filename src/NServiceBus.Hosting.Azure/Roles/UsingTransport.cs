namespace NServiceBus
{
    using Transport;

    /// <summary>
    /// Role used to specify the desired transport to use
    /// </summary>
    /// <typeparam name="T">The <see cref="TransportDefinition"/> to use.</typeparam>
    [ObsoleteEx(ReplacementTypeOrMember = nameof(UseTransportExtensions), RemoveInVersion = "8", TreatAsErrorFromVersion = "7")]
    public interface UsingTransport<T> where T : TransportDefinition
    {
    }
}