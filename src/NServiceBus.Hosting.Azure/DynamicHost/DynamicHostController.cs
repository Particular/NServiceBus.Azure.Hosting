namespace NServiceBus.Hosting.Azure
{
    using System.Collections.Generic;

    class DynamicHostController : IHost
    {
        public DynamicHostController(HostingSettings settings)
        {
            this.settings = settings;
        }

        public void Start()
        {
            var endpointConfiguration = new EndpointConfiguration("DynamicHostController");
            endpointConfiguration.AzureConfigurationSource();

            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            endpointConfiguration.SendOnly();
            endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            loader = new DynamicEndpointLoader(settings.StorageAccount,settings.Container);

            provisioner = new DynamicEndpointProvisioner
            {
                LocalResource = settings.LocalResource,
                RecycleRoleOnError = settings.RecycleRoleOnError
            };

            runner = new DynamicEndpointRunner
            {
                RecycleRoleOnError = settings.RecycleRoleOnError,
                TimeToWaitUntilProcessIsKilled = settings.TimeToWaitUntilProcessIsKilled
            };

            var endpointsToHost = loader.LoadEndpoints();

            if (endpointsToHost == null) return;

            runningServices = new List<EndpointToHost>(endpointsToHost);

            provisioner.Provision(runningServices);

            runner.Start(runningServices);

            if (!settings.AutoUpdate) return;

            monitor = new DynamicHostMonitor
            {
                Loader = loader,
                Interval = settings.UpdateInterval
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

        void UpdatedEndpoints(object sender, EndpointsEventArgs e)
        {
            runner.Stop(e.Endpoints);
            provisioner.Remove(e.Endpoints);
            provisioner.Provision(e.Endpoints);
            runner.Start(e.Endpoints);
        }

        void NewEndpoints(object sender, EndpointsEventArgs e)
        {
            provisioner.Provision(e.Endpoints);
            runner.Start(e.Endpoints);
            monitor.Monitor(e.Endpoints);
            runningServices.AddRange(e.Endpoints);
        }

        void RemovedEndpoints(object sender, EndpointsEventArgs e)
        {
            monitor.StopMonitoring(e.Endpoints);
            runner.Stop(e.Endpoints);
            provisioner.Remove(e.Endpoints);
            foreach (var endpoint in e.Endpoints)
                runningServices.Remove(endpoint);
        }

        IEndpointInstance endpoint;
        DynamicEndpointLoader loader;
        DynamicHostMonitor monitor;
        DynamicEndpointProvisioner provisioner;
        DynamicEndpointRunner runner;
        List<EndpointToHost> runningServices;

        HostingSettings settings;
    }
}