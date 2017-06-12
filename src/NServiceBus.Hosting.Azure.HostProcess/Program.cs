namespace NServiceBus.Hosting.Azure.HostProcess
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Helpers;

    class Program
    {
        static ManualResetEvent Wait = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            var endpointConfigurationType = GetEndpointConfigurationType();

            AssertThatEndpointConfigurationTypeHasDefaultConstructor(endpointConfigurationType);

            var endpointConfigurationFile = GetEndpointConfigurationFile(endpointConfigurationType);

            var endpointConfiguration = Activator.CreateInstance(endpointConfigurationType);

            var endpointId = GetEndpointId(endpointConfiguration);

            var settings = AppDomain.CurrentDomain.SetupInformation;
            settings.ShadowCopyFiles = "false";
            settings.AppDomainInitializerArguments = args;
            settings.ConfigurationFile = endpointConfigurationFile;
            var domain = AppDomain.CreateDomain(endpointId, null, settings);
            var windowsHost = domain.CreateInstanceAndUnwrap<WindowsHost>(endpointConfigurationType);

            windowsHost.Start();

            WaitHandle.WaitAny(new WaitHandle[]
            {
                Wait
            });
        }

        static void AssertThatEndpointConfigurationTypeHasDefaultConstructor(Type type)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);

            if (constructor == null)
                throw new InvalidOperationException("Endpoint configuration type needs to have a default constructor: " + type.FullName);
        }

        static string GetEndpointConfigurationFile(Type endpointConfigurationType)
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                endpointConfigurationType.Assembly.ManifestModule.Name + ".config");
        }

        static string GetEndpointId(object endpointConfiguration)
        {
            var endpointName = endpointConfiguration.GetType().FullName;
            return $"{endpointName}_v{endpointConfiguration.GetType().Assembly.GetName().Version}";
        }

        static Type GetEndpointConfigurationType()
        {
            var endpoints = ScanAssembliesForEndpoints();

            ValidateEndpoints(endpoints);

            return endpoints.First();
        }

        static IList<Type> ScanAssembliesForEndpoints()
        {
            var assemblyScanner = new AssemblyScanner
            {
                ThrowExceptions = false
            };

            var scanResult = assemblyScanner.GetScannableAssemblies();

            return scanResult.Types.Where(
                t => (typeof(IConfigureThisEndpoint).IsAssignableFrom(t) || typeof(IConfigureThisHost).IsAssignableFrom(t))
                     && t != typeof(IConfigureThisEndpoint)
                     && !t.IsAbstract).ToList();
        }

        static void ValidateEndpoints(IList<Type> endpointConfigurationTypes)
        {
            if (!endpointConfigurationTypes.Any())
            {
                throw new InvalidOperationException("No endpoint configuration found in scanned assemblies. " +
                                                    "This usually happens when NServiceBus fails to load your assembly containing IConfigureThisEndpoint." +
                                                    " Try specifying the type explicitly in the NServiceBus.Host.exe.config using the appsetting key: EndpointConfigurationType, " +
                                                    "Scanned path: " + AppDomain.CurrentDomain.BaseDirectory);
            }

            if (endpointConfigurationTypes.Count() > 1)
            {
                throw new InvalidOperationException("Host doesn't support hosting of multiple endpoints. " +
                                                    "Endpoint classes found: " +
                                                    string.Join(", ",
                                                                endpointConfigurationTypes.Select(
                                                                    e => e.AssemblyQualifiedName).ToArray()) +
                                                    " You may have some old assemblies in your runtime directory." +
                                                    " Try right-clicking your VS project, and selecting 'Clean'."
                    );

            }
        }
    }
}
