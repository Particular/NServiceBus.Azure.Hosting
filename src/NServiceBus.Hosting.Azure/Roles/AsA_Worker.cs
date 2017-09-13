namespace NServiceBus
{
    [ObsoleteEx(
        TreatAsErrorFromVersion = "8.0",
        Message = "The AsA_Worker role is obsoleted. Manually configure the EndpointConfiguration object via IConfigureThisEndpoint.Customize(EndpointConfiguration endpointConfiguration)",
        RemoveInVersion = "9.0")]
    public interface AsA_Worker {}
}