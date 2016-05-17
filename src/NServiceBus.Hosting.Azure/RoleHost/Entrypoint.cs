namespace NServiceBus.Hosting.Azure
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using NServiceBus.Config;
    using NServiceBus.Hosting.Helpers;
    using NServiceBus.Integration.Azure;

    public class NServiceBusRoleEntrypoint
    {
        const string ProfileSetting = "AzureProfileConfig.Profiles";
        const string EndpointConfigurationType = "EndpointConfigurationType";

        static List<Assembly> scannedAssemblies;
        IHost host;

        public NServiceBusRoleEntrypoint()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            var azureSettings = new AzureConfigurationSettings();

            var requestedProfiles = GetRequestedProfiles(azureSettings);
            var endpointConfigurationType = GetEndpointConfigurationType(azureSettings);

            AssertThatEndpointConfigurationTypeHasDefaultConstructor(endpointConfigurationType);

            var specifier = Activator.CreateInstance(endpointConfigurationType);

            var controller = specifier as IConfigureThisHost;
            if (controller != null)
            {
                var controllerSettings = controller.Configure();
                host = new DynamicHostController(controllerSettings, requestedProfiles, new List<Type>
                {
                    typeof(Development)
                });
            }
            else
            {
                scannedAssemblies = scannedAssemblies ?? new List<Assembly>();
                host = new GenericHost((IConfigureThisEndpoint) specifier, requestedProfiles,
                    new List<Type>
                    {
                        typeof(Development)
                    }, scannedAssemblies.Select(s => s.ToString()));
            }
        }

        static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Trace.WriteLine("Unhandled exception occured: " + e.ExceptionObject);
        }

        public void Start()
        {
            host.Start();
        }

        public void Stop()
        {
            host.Stop();
        }

        static void AssertThatEndpointConfigurationTypeHasDefaultConstructor(Type type)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);

            if (constructor == null)
                throw new InvalidOperationException(
                    "Endpoint configuration type needs to have a default constructor: " + type.FullName);
        }

        static string[] GetRequestedProfiles(IAzureConfigurationSettings azureSettings)
        {
            string requestedProfileSetting;
            if (azureSettings.TryGetSetting(ProfileSetting, out requestedProfileSetting))
            {
                var requestedProfiles = requestedProfileSetting.Split(' ');
                requestedProfiles = AddProfilesFromConfiguration(requestedProfiles);
                return requestedProfiles;
            }
            return new string[0];
        }

        static Type GetEndpointConfigurationType(AzureConfigurationSettings settings)
        {
            string endpoint;
            if (settings.TryGetSetting(EndpointConfigurationType, out endpoint))
            {
                var endpointType = Type.GetType(endpoint, false);
                if (endpointType == null)
                    throw new ConfigurationErrorsException(
                        $"The 'EndpointConfigurationType' entry in the role config has specified to use the type '{endpoint}' but that type could not be loaded.");

                return endpointType;
            }

            var endpoints = ScanAssembliesForEndpoints().ToList();

            ValidateEndpoints(endpoints);

            return endpoints.First();
        }

        static IEnumerable<Type> ScanAssembliesForEndpoints()
        {
            if (scannedAssemblies == null)
            {
                var assemblyScanner = new AssemblyScanner
                {
                    ThrowExceptions = false
                };

                scannedAssemblies = assemblyScanner.GetScannableAssemblies().Assemblies;
            }
            return scannedAssemblies.SelectMany(
                assembly => assembly.GetTypes().Where(
                    t => typeof(IConfigureThisEndpoint).IsAssignableFrom(t)
                         && t != typeof(IConfigureThisEndpoint)
                         && !t.IsAbstract));
        }

        static void ValidateEndpoints(IList<Type> endpointConfigurationTypes)
        {
            var count = endpointConfigurationTypes.Count();
            if (count == 0)
            {
                throw new InvalidOperationException("No endpoint configuration found in scanned assemblies. " +
                                                    "This usually happens when NServiceBus fails to load your assembly containing IConfigureThisEndpoint." +
                                                    " Try specifying the type explicitly in the roles config using the appsetting key: EndpointConfigurationType, " +
                                                    "Scanned path: " + AppDomain.CurrentDomain.BaseDirectory);
            }

            if (count > 1)
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

        static string[] AddProfilesFromConfiguration(IEnumerable<string> args)
        {
            var list = new List<string>(args);

            var configSection = ConfigurationManager.GetSection("AzureProfileConfig") as AzureProfileConfig;

            if (configSection != null)
            {
                var configuredProfiles = configSection.Profiles.Split(',');
                Array.ForEach(configuredProfiles, s => list.Add(s.Trim()));
            }

            return list.ToArray();
        }
    }
}