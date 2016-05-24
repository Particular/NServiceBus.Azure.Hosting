namespace NServiceBus.Hosting.Azure
{
    using System.Diagnostics;

    static class IsHostedIn
    {
        static string HostProcessName = "NServiceBus.Hosting.Azure.HostProcess";

        public static bool ChildHostProcess()
        {
            var currentProcess = Process.GetCurrentProcess();
            return currentProcess.ProcessName == HostProcessName;
        }
    }
}