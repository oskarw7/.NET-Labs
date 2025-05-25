using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.Helpers;

namespace WpfApp1.Implementations
{
    internal class AsyncRunner
    {
        private readonly FibonacciTaskManager manager;
        private readonly Action updateProgress;

        public AsyncRunner(FibonacciTaskManager m, Action progressUpdate) => (manager, updateProgress) = (m, progressUpdate);

        public async void Start(int threadCount, CancellationToken token)
        {
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        long task = manager.GetNextTask();

                        if (task == -1) break;

                        var (result, time) = await Task.Run(() => FibonacciThread.Calculate(task));
                        manager.LogResult((short)Thread.CurrentThread.ManagedThreadId, task, result, time);

                        updateProgress();
                    }
                }));

            }

            await Task.WhenAll(tasks);
        }
    }
}
