namespace NServiceBus
{
    using Hosting;

    public interface IConfigureThisHost
    {
        HostingSettings Configure();
    }
}