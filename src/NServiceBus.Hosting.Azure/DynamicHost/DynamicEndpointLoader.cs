namespace NServiceBus.Hosting.Azure
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    class DynamicEndpointLoader
    {
        public DynamicEndpointLoader(CloudStorageAccount storageAccount, string container)
        {
            this.storageAccount = storageAccount;
            Container = container;
        }

        public IEnumerable<EndpointToHost> LoadEndpoints()
        {
            if (client == null)
            {
                client = storageAccount.CreateCloudBlobClient();
            }

            var blobContainer = client.GetContainerReference(Container);
            blobContainer.CreateIfNotExists();

            return from b in blobContainer.ListBlobs()
                where b.Uri.AbsolutePath.EndsWith(".zip")
                select new EndpointToHost((CloudBlockBlob) b);
        }

        readonly CloudStorageAccount storageAccount;
        readonly string Container;
        CloudBlobClient client;
    }
}