using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.Helpers;

namespace WpfApp1.Implementations
{
    internal class AsyncAwaitRunner
    {
        private readonly FibonacciTaskManager manager;
        private readonly Action updateProgress;

        public AsyncAwaitRunner(FibonacciTaskManager m, Action progressUpdate) =>
            (manager, updateProgress) = (m, progressUpdate);

        public async void Start(int threadCount)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (true)
                    {
                        long task = manager.GetNextTask();
                        if (task == -1) break;

                        await Task.Delay(1); // Simulate async work
                        var (result, time) = FibonacciHelper.Calculate(task);
                        manager.LogResult((short)Thread.CurrentThread.ManagedThreadId, task, result, time);
                        updateProgress();
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }
    }

}
