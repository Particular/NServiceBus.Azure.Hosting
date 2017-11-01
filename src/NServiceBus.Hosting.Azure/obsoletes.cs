#pragma warning disable CS1591
namespace NServiceBus
{
    using System;

    public static class ConfigureAzureIntegration
    {
        [ObsoleteEx(Message = "Use code-based configuration instead.", RemoveInVersion = "9", TreatAsErrorFromVersion = "8")]
        public static void AzureConfigurationSource(this EndpointConfiguration config, string configurationPrefix = null)
        {
            throw new NotImplementedException();
        }
    }

    [ObsoleteEx(
       TreatAsErrorFromVersion = "8.0",
       Message = "The AsA_Worker role is obsoleted. Manually configure the EndpointConfiguration object via IConfigureThisEndpoint.Customize(EndpointConfiguration endpointConfiguration)",
       RemoveInVersion = "9.0")]
    public interface AsA_Worker { }
}
#pragma warning restore CS1591