namespace NServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using Hosting.Azure;
    using Hosting.Helpers;

    /// <summary>
    /// Adapts endpoint host to interface that is easily integrated with Azure role entry point class.
    /// </summary>
    public class NServiceBusRoleEntrypoint
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NServiceBusRoleEntrypoint" />.
        /// </summary>
        public NServiceBusRoleEntrypoint()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            var azureSettings = new AzureConfigurationSettings();

            var endpointConfigurationType = GetEndpointConfigurationType(azureSettings);

            AssertThatEndpointConfigurationTypeHasDefaultConstructor(endpointConfigurationType);

            var specifier = Activator.CreateInstance(endpointConfigurationType);

            var controller = specifier as IConfigureThisHost;
            if (controller != null)
            {
                var controllerSettings = controller.Configure();
                host = new DynamicHostController(controllerSettings);
            }
            else
            {
                host = new GenericHost((IConfigureThisEndpoint) specifier);
            }
        }

        static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Trace.WriteLine("Unhandled exception occured: " + e.ExceptionObject);
        }

        /// <summary>
        /// Starts the endpoint host.
        /// </summary>
        public void Start()
        {
            host.Start();
        }

        /// <summary>
        /// Stops the endpoint host.
        /// </summary>
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
            var assemblyScanner = new AssemblyScanner
            {
                ThrowExceptions = false
            };

            var scanResult = assemblyScanner.GetScannableAssemblies();

            return scanResult.Types.Where(
                    t => (typeof(IConfigureThisEndpoint).IsAssignableFrom(t) || typeof(IConfigureThisHost).IsAssignableFrom(t))
                         && t != typeof(IConfigureThisEndpoint)
                         && !t.IsAbstract);
        }

        static void ValidateEndpoints(IList<Type> endpointConfigurationTypes)
        {
            var count = endpointConfigurationTypes.Count;
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

        IHost host;
        const string EndpointConfigurationType = "EndpointConfigurationType";
    }
}