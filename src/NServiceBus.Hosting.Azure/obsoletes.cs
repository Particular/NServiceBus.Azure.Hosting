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
}