namespace NServiceBus.Hosting.Azure.Tests
{
    using System.Collections.Generic;

    class FakeAzureConfigurationSettings : IAzureConfigurationSettings
    {
        public string GetSetting(string name)
        {
            return settings[name];
        }

        public bool TryGetSetting(string name, out string setting)
        {
            return settings.TryGetValue(name, out setting);
        }

        public void AddSetting(string name, string setting)
        {
            settings.Add(name, setting);
        }

        Dictionary<string, string> settings = new Dictionary<string, string>();
    }
}