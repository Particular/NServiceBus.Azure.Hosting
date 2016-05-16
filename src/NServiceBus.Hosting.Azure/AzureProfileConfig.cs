namespace NServiceBus.Config
{
    using System;
    using System.Configuration;
    using NServiceBus.Features;

    /// <summary>
    ///     Configuration section for Azure host.
    /// </summary>
    [ObsoleteEx(Message = "The profiles configured via AzureProfileConfig are no longer supported", TreatAsErrorFromVersion = "7", RemoveInVersion = "8")]
    public class AzureProfileConfig : ConfigurationSection
    {
        /// <summary>
        ///     A comma separated list of profile names
        /// </summary>
        [ConfigurationProperty("Profiles", IsRequired = false)]
        public string Profiles
        {
            get { return this["Profiles"] as string; }
            set { this["Profiles"] = value; }
        }
    }

    internal sealed class DetectObsoleteConfigurationSettings : Feature
    {
        public DetectObsoleteConfigurationSettings()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var azureProfileConfig = context.Settings.GetConfigSection<AzureProfileConfig>();

            if (!string.IsNullOrWhiteSpace(azureProfileConfig?.Profiles))
            {
                throw new NotSupportedException($"The {nameof(AzureProfileConfig.Profiles)} attribute in the {nameof(AzureProfileConfig)} configuration section is no longer supported.");
            }
        }
    }
}