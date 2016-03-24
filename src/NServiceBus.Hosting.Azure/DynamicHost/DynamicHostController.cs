namespace NServiceBus.Hosting.Azure
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Configuration.AdvanceExtensibility;
    using Profiles;

    class DynamicHostController : IHost
    {
        public DynamicHostController(IConfigureThisEndpoint specifier, string[] requestedProfiles, List<Type> defaultProfiles)
        {
            this.specifier = specifier;

            var assembliesToScan = new List<Assembly> { GetType().Assembly };

            profileManager = new ProfileManager(assembliesToScan, requestedProfiles, defaultProfiles);
        }

        public void Start()
        {

            var endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.AzureConfigurationSource();
            var configSection = endpointConfiguration.GetSettings().GetConfigSection<DynamicHostControllerConfig>() ?? new DynamicHostControllerConfig();
            
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            profileManager.ActivateProfileHandlers(endpointConfiguration);
            specifier.Customize(endpointConfiguration);

            endpointConfiguration.SendOnly();
            endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            loader = new DynamicEndpointLoader
            {
                ConnectionString = configSection.ConnectionString,
                Container = configSection.Container
            };
            provisioner = new DynamicEndpointProvisioner
            {
                LocalResource = configSection.LocalResource,
                RecycleRoleOnError = configSection.RecycleRoleOnError
            };

            runner = new DynamicEndpointRunner
            {
                RecycleRoleOnError = configSection.RecycleRoleOnError,
                TimeToWaitUntilProcessIsKilled = configSection.TimeToWaitUntilProcessIsKilled
            };

            var endpointsToHost = loader.LoadEndpoints();

            if (endpointsToHost == null) return;

            runningServices = new List<EndpointToHost>(endpointsToHost);

            provisioner.Provision(runningServices);

            runner.Start(runningServices);

            if (!configSection.AutoUpdate) return;

            monitor = new DynamicHostMonitor
            {
                Loader = loader,
                Interval = configSection.UpdateInterval
            };
            monitor.UpdatedEndpoints += UpdatedEndpoints;
            monitor.NewEndpoints += NewEndpoints;
            monitor.RemovedEndpoints += RemovedEndpoints;
            monitor.Monitor(runningServices);
            monitor.Start();
        }

        public void Stop()
        {
            endpoint?.Stop();
            monitor?.Stop();
            runner?.Stop(runningServices);
        }

        public void Install(string username)
        {
            //todo -yves
        }

        public void UpdatedEndpoints(object sender, EndpointsEventArgs e)
        {
            runner.Stop(e.Endpoints);
            provisioner.Remove(e.Endpoints);
            provisioner.Provision(e.Endpoints);
            runner.Start(e.Endpoints);
        }

        public void NewEndpoints(object sender, EndpointsEventArgs e)
        {
            provisioner.Provision(e.Endpoints);
            runner.Start(e.Endpoints);
            monitor.Monitor(e.Endpoints);
            runningServices.AddRange(e.Endpoints);
        }

        public void RemovedEndpoints(object sender, EndpointsEventArgs e)
        {
            monitor.StopMonitoring(e.Endpoints);
            runner.Stop(e.Endpoints);
            provisioner.Remove(e.Endpoints);
            foreach (var endpoint in e.Endpoints)
                runningServices.Remove(endpoint);
        }

        IConfigureThisEndpoint specifier;
        ProfileManager profileManager;
        DynamicEndpointLoader loader;
        DynamicEndpointProvisioner provisioner;
        DynamicEndpointRunner runner;
        DynamicHostMonitor monitor;
        List<EndpointToHost> runningServices;
        IEndpointInstance endpoint;
    }
}
