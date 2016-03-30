﻿namespace NServiceBus
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Hosting;
    using Logging;

    class StartableAndStoppableRunner
    {
        public StartableAndStoppableRunner(IEnumerable<IWantToRunWhenEndpointStartsAndStops> wantToRunWhenBusStartsAndStops)
        {
            this.wantToRunWhenBusStartsAndStops = wantToRunWhenBusStartsAndStops;
        }

        public Task Start(IMessageSession session)
        {
            var startableTasks = new List<Task>();
            foreach (var startable in wantToRunWhenBusStartsAndStops)
            {
                var task = startable.Start(session).ThrowIfNull();

                var startable1 = startable;

                /* 
                    We can't use the await keyword because of the conditional logging. 
                    Since we want to start them concurrently and log per instance there is not much else we can do.
                */
                task.ContinueWith(t =>
                {
                    thingsRanAtStartup.Add(startable1);
                    Log.DebugFormat("Started {0}.", startable1.GetType().AssemblyQualifiedName);
                }, TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
                task.ContinueWith(t =>
                {
                    Log.Error($"Startup task {startable1.GetType().AssemblyQualifiedName} failed to complete.", t.Exception);
                }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
                
                startableTasks.Add(task);
            }

            return Task.WhenAll(startableTasks.ToArray());
        }

        public async Task Stop(IMessageSession session)
        {
            var stoppables = Interlocked.Exchange(ref thingsRanAtStartup, new ConcurrentBag<IWantToRunWhenEndpointStartsAndStops>());
            if (!stoppables.Any())
            {
                return;
            }

            var stoppableTasks = new List<Task>();
            foreach (var stoppable in stoppables)
            {
                try
                {
                    var task = stoppable.Stop(session).ThrowIfNull();

                    var stoppable1 = stoppable;

                    /* 
                        We can't use the await keyword because of the conditional logging. 
                        Since we want to start them concurrently and log per instance there is not much else we can do.
                    */
                    task.ContinueWith(t =>
                    {
                        thingsRanAtStartup.Add(stoppable1);
                        Log.DebugFormat("Stopped {0}.", stoppable1.GetType().AssemblyQualifiedName);
                    }, TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously).Ignore();
                    task.ContinueWith(t =>
                    {
                        Log.Fatal($"Startup task {stoppable1.GetType().AssemblyQualifiedName} failed to stop.", t.Exception);
                        t?.Exception?.Flatten().Handle(e => true);
                    }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously).Ignore();

                    stoppableTasks.Add(task);
                }
                catch (Exception e)
                {
                    Log.Fatal("Startup task failed to stop.", e);
                }
            }

            try
            {
                await Task.WhenAll(stoppableTasks.ToArray()).ConfigureAwait(false);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // ignore because we want to shutdown no matter what.
            }
        }

        IEnumerable<IWantToRunWhenEndpointStartsAndStops> wantToRunWhenBusStartsAndStops;
        ConcurrentBag<IWantToRunWhenEndpointStartsAndStops> thingsRanAtStartup = new ConcurrentBag<IWantToRunWhenEndpointStartsAndStops>();
        static ILog Log = LogManager.GetLogger<StartableAndStoppableRunner>();
    }
}
