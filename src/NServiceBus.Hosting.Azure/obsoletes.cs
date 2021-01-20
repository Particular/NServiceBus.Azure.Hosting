#pragma warning disable 1591
namespace NServiceBus
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;

    class Const
    {
        public const string ObsoletionMessage = "The Azure cloud services host has been deprecated. It is recommended to switch to self-hosting or the generic host instead. See the upgrade guide and docs for alternatives.";
    }

    // Internal only type referenced by the APIApprovals test
    class AzureHostingInternalType
    {
    }

    [ObsoleteEx(
        Message = Const.ObsoletionMessage,
        TreatAsErrorFromVersion = "9.0",
        RemoveInVersion = "10.0")]
    public class NServiceBusRoleEntrypoint
    {
        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public NServiceBusRoleEntrypoint()
        {
            throw new NotImplementedException();
        }

        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public void Start()
        {
            throw new NotImplementedException();
        }

        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }

    [ObsoleteEx(
        Message = Const.ObsoletionMessage,
        TreatAsErrorFromVersion = "9.0",
        RemoveInVersion = "10.0")]
    public class HostingSettings
    {
        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public HostingSettings(string connectionString)
        {
            throw new NotImplementedException();
        }

        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public CloudStorageAccount StorageAccount => throw new NotImplementedException();

        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public string Container
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public string LocalResource
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public bool RecycleRoleOnError
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public bool AutoUpdate
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public int UpdateInterval
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public int TimeToWaitUntilProcessIsKilled
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }

    [ObsoleteEx(
        Message = Const.ObsoletionMessage,
        TreatAsErrorFromVersion = "9.0",
        RemoveInVersion = "10.0")]
    public interface IConfigureThisHost
    {
        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        HostingSettings Configure();
    }

    [ObsoleteEx(
        Message = Const.ObsoletionMessage,
        TreatAsErrorFromVersion = "9.0",
        RemoveInVersion = "10.0")]
    public static class EndpointStartableAndStoppableExtensions
    {
        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public static void RunWhenEndpointStartsAndStops(this EndpointConfiguration configuration, IWantToRunWhenEndpointStartsAndStops startableAndStoppable)
        {
            throw new NotImplementedException();
        }
    }

    [ObsoleteEx(
        Message = Const.ObsoletionMessage,
        TreatAsErrorFromVersion = "9.0",
        RemoveInVersion = "10.0")]
    public interface IWantToRunWhenEndpointStartsAndStops
    {
        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        Task Start(IMessageSession session);

        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        Task Stop(IMessageSession session);
    }

    [ObsoleteEx(
        Message = Const.ObsoletionMessage,
        TreatAsErrorFromVersion = "9.0",
        RemoveInVersion = "10.0")]
    public static class EndpointConfigurationExtensions
    {
        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        public static void DefineEndpointName(this EndpointConfiguration configuration, string endpointName)
        {
            throw new NotImplementedException();
        }
    }

    [ObsoleteEx(
        Message = Const.ObsoletionMessage,
        TreatAsErrorFromVersion = "9.0",
        RemoveInVersion = "10.0")]
    public interface IConfigureThisEndpoint
    {
        [ObsoleteEx(
            Message = Const.ObsoletionMessage,
            TreatAsErrorFromVersion = "9.0",
            RemoveInVersion = "10.0")]
        void Customize(EndpointConfiguration configuration);
    }
}
#pragma warning restore 1591