namespace NServiceBus
{
    using NServiceBus.Hosting;

    public interface IConfigureThisHost
    {
        HostingSettings Configure();
    }
}