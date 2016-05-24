namespace NServiceBus
{
    using Hosting.Azure;

    /// <summary>
    /// Extension methods that configure the the host.
    /// </summary>
    public static class ConfigureAzureIntegration
    {
        /// <summary>
        /// Overrides settings from App.config file with settings from the service configuration file.
        /// </summary>
        /// <param name="config">The <see cref="EndpointConfiguration" /> instance to apply the settings to.</param>
        /// <param name="configurationPrefix">Prefix to use for override settings keys.</param>
        public static void AzureConfigurationSource(this EndpointConfiguration config, string configurationPrefix = null)
        {
            var azureConfigSource = new AzureConfigurationSource(new AzureConfigurationSettings())
            {
                ConfigurationPrefix = configurationPrefix
            };
            
            config.CustomConfigurationSource(azureConfigSource);
        }
    }
}