namespace NServiceBus.Hosting.Azure
{
    interface IHost
    {
        void Start();

        void Stop();

        void Install(string username) ;
    }
}