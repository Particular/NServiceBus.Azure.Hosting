// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace NServiceBus.Hosting
{
    public class HostingSettings
    {
        public HostingSettings()
        {
            ConnectionString = DefaultConnectionString;
            LocalResource = DefaultLocalResource;
            Container = DefaultContainer;
            UpdateInterval = DefaultUpdateInterval;
            TimeToWaitUntilProcessIsKilled = DefaultTimeToWaitUntilProcessIsKilled;
        }

        public string ConnectionString { get; set; }
        
        public string Container { get; set; }

        public string LocalResource { get; set; }

        public bool RecycleRoleOnError { get; set; }

        public bool AutoUpdate { get; set; }

        public int UpdateInterval { get; set; }

        public int TimeToWaitUntilProcessIsKilled { get; set; }

        const string DefaultConnectionString = "UseDevelopmentStorage=true";
        const string DefaultContainer = "endpoints";
        const string DefaultLocalResource = "endpoints";
        const int DefaultUpdateInterval = 600000;
        const int DefaultTimeToWaitUntilProcessIsKilled = 10000;
    }
}