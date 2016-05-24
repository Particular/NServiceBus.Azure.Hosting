namespace NServiceBus.Hosting.Azure.Tests
{
    using System;
    using System.Reflection;
    using NUnit.Framework;

    public class When_using_obsoleted_config_sections
    {
        static object BuildConfig()
        {
            return Activator.CreateInstance(ConfigSectionType);
        }

        [Test]
        public void Should_not_throw_if_default()
        {
            var config = BuildConfig();
            Validate(config);
        }

        [Test]
        public void Should_throw_if_not_default()
        {
            var config = BuildConfig();

            var autoUpdateProperty = ConfigSectionType.GetProperty("AutoUpdate");
            autoUpdateProperty.SetValue(config, true);

            var ex = Assert.Throws<NotSupportedException>(() => { Validate(config); });
            Assert.True(ex.Message.Contains("AutoUpdate"));
        }

        static void Validate(object config)
        {
            try
            {
                typeof(DetectObsoleteDynamicHostControllerConfig).GetMethod("Validate").Invoke(null, new[]
                {
                    config
                });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        static readonly Type ConfigSectionType = typeof(DetectObsoleteDynamicHostControllerConfig).Assembly.GetType("NServiceBus.Hosting.Azure.DynamicHostControllerConfig");
    }
}