// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace NServiceBus.Hosting
{
    using Microsoft.WindowsAzure.Storage;

    public class HostingSettings
    {
        public HostingSettings(string connectionString)
        {
            StorageAccount = CloudStorageAccount.Parse(connectionString);
            LocalResource = DefaultLocalResource;
            Container = DefaultContainer;
            UpdateInterval = DefaultUpdateInterval;
            TimeToWaitUntilProcessIsKilled = DefaultTimeToWaitUntilProcessIsKilled;
        }

        public CloudStorageAccount StorageAccount { get; private set; }

        public string Container { get; set; }

        public string LocalResource { get; set; }

        public bool RecycleRoleOnError { get; set; }

        public bool AutoUpdate { get; set; }

        public int UpdateInterval { get; set; }

        public int TimeToWaitUntilProcessIsKilled { get; set; }

        const string DefaultContainer = "endpoints";
        const string DefaultLocalResource = "endpoints";
        const int DefaultUpdateInterval = 600000;
        const int DefaultTimeToWaitUntilProcessIsKilled = 10000;
    }
}