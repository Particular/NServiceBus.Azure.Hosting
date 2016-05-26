namespace NServiceBus.Hosting.Azure
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Logging;

    class GenericHost : IHost
    {
        public GenericHost(IConfigureThisEndpoint specifier)
        {
            this.specifier = specifier;

            endpointNameToUse = specifier.GetType().Namespace ?? specifier.GetType().Assembly.GetName().Name;

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
                var displayName = $"{host}_{instance}";
                configuration
                    .UniquelyIdentifyRunningInstance()
                    .UsingNames(instance, host)
                    .UsingCustomDisplayName(displayName);
            }

            moreConfiguration?.Invoke(configuration);

            specifier.Customize(configuration);
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

        IEndpointInstance bus;

        string endpointNameToUse;

        IConfigureThisEndpoint specifier;
    }
}