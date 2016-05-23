// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace NServiceBus.Hosting
{
    using Microsoft.WindowsAzure.Storage;

    /// <summary>
    /// Holds host settings.
    /// </summary>
    public class HostingSettings
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HostingSettings"/>.
        /// </summary>
        /// <param name="connectionString">Connection string for the Azure storage account.</param>
        public HostingSettings(string connectionString)
        {
            StorageAccount = CloudStorageAccount.Parse(connectionString);
            LocalResource = DefaultLocalResource;
            Container = DefaultContainer;
            UpdateInterval = DefaultUpdateInterval;
            TimeToWaitUntilProcessIsKilled = DefaultTimeToWaitUntilProcessIsKilled;
        }

        /// <summary>
        /// Azure storage account information.
        /// </summary>
        public CloudStorageAccount StorageAccount { get; private set; }

        /// <summary>
        /// Azure blob storage container name.
        /// </summary>
        public string Container { get; set; }

        /// <summary>
        /// Local storage for endpoint binaries.
        /// </summary>
        public string LocalResource { get; set; }

        /// <summary>
        /// Indicates if role instance should be recycled when an error occurs.
        /// </summary>
        public bool RecycleRoleOnError { get; set; }

        /// <summary>
        /// Indicates if host should monitor endpoint storage for changes.
        /// </summary>
        public bool AutoUpdate { get; set; }

        /// <summary>
        /// Delay between endpoint storage checks.
        /// </summary>
        public int UpdateInterval { get; set; }

        /// <summary>
        /// Time the host should wait for the endpoint host process to exit.
        /// </summary>
        public int TimeToWaitUntilProcessIsKilled { get; set; }

        const string DefaultContainer = "endpoints";
        const string DefaultLocalResource = "endpoints";
        const int DefaultUpdateInterval = 600000;
        const int DefaultTimeToWaitUntilProcessIsKilled = 10000;
    }
}