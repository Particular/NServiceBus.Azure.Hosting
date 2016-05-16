namespace NServiceBus.Hosting
{
    using System;
    using System.Configuration;
    using NServiceBus.Features;

    [ObsoleteEx(Message = "The profiles configured via AzureProfileConfig are no longer supported",
        TreatAsErrorFromVersion = "7",
        RemoveInVersion = "8")]
    public class DynamicHostControllerConfig : ConfigurationSection
    {
        [ConfigurationProperty("ConnectionString", IsRequired = false, DefaultValue = "UseDevelopmentStorage=true")]
        public string ConnectionString
        {
            get { return (string) this["ConnectionString"]; }
            set { this["ConnectionString"] = value; }
        }

        [ConfigurationProperty("Container", IsRequired = false, DefaultValue = "endpoints")]
        public string Container
        {
            get { return (string) this["Container"]; }
            set { this["Container"] = value; }
        }

        [ConfigurationProperty("LocalResource", IsRequired = false, DefaultValue = "endpoints")]
        public string LocalResource
        {
            get { return (string) this["LocalResource"]; }
            set { this["LocalResource"] = value; }
        }

        [ConfigurationProperty("RecycleRoleOnError", IsRequired = false, DefaultValue = false)]
        public bool RecycleRoleOnError
        {
            get { return (bool) this["RecycleRoleOnError"]; }
            set { this["RecycleRoleOnError"] = value; }
        }

        [ConfigurationProperty("AutoUpdate", IsRequired = false, DefaultValue = false)]
        public bool AutoUpdate
        {
            get { return (bool) this["AutoUpdate"]; }
            set { this["AutoUpdate"] = value; }
        }

        [ConfigurationProperty("UpdateInterval", IsRequired = false, DefaultValue = 600000)]
        public int UpdateInterval
        {
            get { return (int) this["UpdateInterval"]; }
            set { this["UpdateInterval"] = value; }
        }

        [ConfigurationProperty("TimeToWaitUntilProcessIsKilled", IsRequired = false, DefaultValue = 10000)]
        public int TimeToWaitUntilProcessIsKilled
        {
            get { return (int) this["TimeToWaitUntilProcessIsKilled"]; }
            set { this["TimeToWaitUntilProcessIsKilled"] = value; }
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
            var cfg = context.Settings.GetConfigSection<DynamicHostControllerConfig>();
            if (cfg == null)
            {
                return;
            }

            foreach (PropertyInformation property in cfg.ElementInformation.Properties)
            {
                if (property.ValueOrigin != PropertyValueOrigin.Default)
                {
                    throw new NotSupportedException(BuildConfigExceptionMessage(property.Name));
                }
            }
        }

        private static string BuildConfigExceptionMessage(string attributeName)
        {
            return $"The {attributeName} attribute in the {nameof(DynamicHostControllerConfig)} configuration section is no longer supported. Use {nameof(DynamicHostControllerSettings)}.{attributeName}.";
        }
    }
}