namespace NServiceBus.Hosting.Azure.Tests
{
    using System.Configuration;
    using Config.ConfigurationSource;
    using NUnit.Framework;

    [TestFixture]
    [Category("Azure")]
    public class When_using_the_azure_configuration_source
    {
        [SetUp]
        public void SetUp()
        {
            azureSettings = new FakeAzureConfigurationSettings();
        }

        [Test]
        public void The_service_configuration_should_override_appconfig()
        {
            configSource = new AzureConfigurationSource(azureSettings, false);

            azureSettings.AddSetting("TestConfigSection.StringSetting", "test");

            Assert.AreEqual(configSource.GetConfiguration<TestConfigSection>().StringSetting, "test");
        }

        [Test]
        public void Overrides_should_be_possible_for_non_existing_sections()
        {
            configSource = new AzureConfigurationSource(azureSettings, false);

            azureSettings.AddSetting("SectionNotPresentInConfig.SomeSetting", "test");

            Assert.AreEqual(configSource.GetConfiguration<SectionNotPresentInConfig>().SomeSetting, "test");
        }

        [Test]
        public void No_section_should_be_returned_if_both_azure_and_app_configs_are_empty()
        {
            configSource = new AzureConfigurationSource(azureSettings, false);

            Assert.Null(configSource.GetConfiguration<SectionNotPresentInConfig>());
        }

        [Test]
        public void Value_types_should_be_converted_from_string_to_its_native_type()
        {
            configSource = new AzureConfigurationSource(azureSettings, false);

            azureSettings.AddSetting("TestConfigSection.IntSetting", "23");

            Assert.AreEqual(configSource.GetConfiguration<TestConfigSection>().IntSetting, 23);
        }

        [Test]
        public void Should_cache_previous_lookups_by_default()
        {
            configSource = new AzureConfigurationSource(azureSettings);

            azureSettings.AddSetting("TestConfigSection.StringSetting", "test");

            Assert.AreEqual(configSource.GetConfiguration<TestConfigSection>(), configSource.GetConfiguration<TestConfigSection>());
        }

        FakeAzureConfigurationSettings azureSettings;
#pragma warning disable 618
        IConfigurationSource configSource;
#pragma warning restore 618
    }

    public class TestConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("StringSetting", IsRequired = true)]
        public string StringSetting
        {
            get { return (string) this["StringSetting"]; }
            set { this["StringSetting"] = value; }
        }

        [ConfigurationProperty("IntSetting", IsRequired = false)]
        public int IntSetting
        {
            get { return (int) this["IntSetting"]; }
            set { this["IntSetting"] = value; }
        }
    }

    public class SectionNotPresentInConfig : ConfigurationSection
    {
        [ConfigurationProperty("SomeSetting", IsRequired = true)]
        public string SomeSetting
        {
            get { return (string) this["SomeSetting"]; }
            set { this["SomeSetting"] = value; }
        }
    }
}