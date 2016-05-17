namespace NServiceBus.Hosting.Azure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Config;
    using Config.ConfigurationSource;
    using Helpers;
    using Profiles;
    using Integration.Azure;
    using Logging;
    using NServiceBus.Azure;

    class GenericHost : IHost
    {
        public GenericHost(IConfigureThisEndpoint specifier, string[] args, List<Type> defaultProfiles,
            IEnumerable<string> scannableAssembliesFullName = null)
        {
            this.specifier = specifier;

            endpointNameToUse = specifier.GetType().Namespace ?? specifier.GetType().Assembly.GetName().Name;

            List<Assembly> assembliesToScan;

            if (scannableAssembliesFullName == null || !scannableAssembliesFullName.Any())
            {
                var assemblyScanner = new AssemblyScanner();
                assembliesToScan = assemblyScanner
                    .GetScannableAssemblies()
                    .Assemblies;
            }
            else
            {
                assembliesToScan = scannableAssembliesFullName
                    .Select(Assembly.Load)
                    .ToList();
            }

            args = AddProfilesFromConfiguration(args);

            profileManager = new ProfileManager(assembliesToScan, args, defaultProfiles);
        }

        public void Start()
        {
            try
            {
                var startableEndpoint = PerformConfiguration().GetAwaiter().GetResult();

                bus = startableEndpoint.Start().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(GenericHost)).Fatal("Exception when starting endpoint.", ex);
                throw;
            }
        }

        public void Stop()
        {
            if (bus != null)
            {
                bus.Stop().GetAwaiter().GetResult();

                bus = null;
            }
        }

        public void Install(string username)
        {
            PerformConfiguration(builder => builder.EnableInstallers(username)).GetAwaiter().GetResult();
        }

        Task<IStartableEndpoint> PerformConfiguration(Action<EndpointConfiguration> moreConfiguration = null)
        {
            var loggingConfigurers = profileManager.GetLoggingConfigurer();
            foreach (var loggingConfigurer in loggingConfigurers)
            {
                loggingConfigurer.Configure(specifier);
            }

            var configuration = new EndpointConfiguration(endpointNameToUse);
            configuration.DefineCriticalErrorAction(OnCriticalError);

            if (SafeRoleEnvironment.IsAvailable)
            {
                if (!IsHostedIn.ChildHostProcess())
                {
                    configuration.AzureConfigurationSource();
                }

                var host = SafeRoleEnvironment.CurrentRoleName;
                var instance = SafeRoleEnvironment.CurrentRoleInstanceId;
                var hostId = DeterministicGuid(instance, host);
                configuration
                    .UniquelyIdentifyRunningInstance()
                    .UsingCustomIdentifier(hostId)
                    .UsingNames(instance, host);
            }

            moreConfiguration?.Invoke(configuration);

            specifier.Customize(configuration);
            RoleManager.TweakConfigurationBuilder(specifier, configuration);
            return Endpoint.Create(configuration);
        }

        // Windows hosting behavior when critical error occurs is suicide.
        Task OnCriticalError(ICriticalErrorContext context)
        {
            if (Environment.UserInteractive)
            {
                Thread.Sleep(10000); // so that user can see on their screen the problem
            }

            var message = $"The following critical error was encountered by NServiceBus:\n{context.Error}\nNServiceBus is shutting down.";
            LogManager.GetLogger(typeof(GenericHost)).Fatal(message);
            Environment.FailFast(message, context.Exception);

            return Task.FromResult(0);
        }

        string[] AddProfilesFromConfiguration(IEnumerable<string> args)
        {
            var list = new List<string>(args);

            var configSection = ((IConfigurationSource)new AzureConfigurationSource(new AzureConfigurationSettings())).GetConfiguration<AzureProfileConfig>();

            if (configSection != null)
            {
                var configuredProfiles = configSection.Profiles.Split(',');
                Array.ForEach(configuredProfiles, s => list.Add(s.Trim()));
            }

            return list.ToArray();
        }

        static Guid DeterministicGuid(params object[] data)
        {
            // use MD5 hash to get a 16-byte hash of the string
            using (var provider = new MD5CryptoServiceProvider())
            {
                var inputBytes = Encoding.Default.GetBytes(String.Concat(data));
                var hashBytes = provider.ComputeHash(inputBytes);
                // generate a guid from the hash:
                return new Guid(hashBytes);
            }
        }
        
        ProfileManager profileManager;
        IConfigureThisEndpoint specifier;
        IEndpointInstance bus;

        string endpointNameToUse;
    }
}
