namespace NServiceBus.Hosting.Azure
{
    /// <summary>
    /// Identifies a host
    /// </summary>
    internal interface IHost
    {
        /// <summary>
        /// Does startup work.
        /// </summary>
        void Start();

        /// <summary>
        /// Does shutdown work.
        /// </summary>
        void Stop();

        /// <summary>
        /// Performs necessary installation
        /// </summary>
        void Install(string username) ;
    }
}