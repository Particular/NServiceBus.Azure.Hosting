namespace NServiceBus.Hosting
{
    public class HostingSettings
    {
        public const string DefaultConnectionString = "UseDevelopmentStorage=true";
        public const string DefaultContainer = "endpoints";
        public const string DefaultLocalResource = "endpoints";
        public const int DefaultUpdateInterval = 600000;
        public const int DefaultTimeToWaitUntilProcessIsKilled = 10000;

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
    }
}