using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.Helpers;

namespace WpfApp1.Implementations
{
    internal class TaskRunner
    {
        private readonly FibonacciTaskManager manager;
        private readonly Action updateProgress;
        public TaskRunner(FibonacciTaskManager m, Action progressUpdate) => (manager, updateProgress) = (m, progressUpdate);

        public void Start(int threadCount, CancellationToken token)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        long task = manager.GetNextTask();

                        if (task == -1) break;

                        var (result, time) = FibonacciThread.Calculate(task);
                        manager.LogResult((short)Thread.CurrentThread.ManagedThreadId, task, result, time);

                        updateProgress();
                    }
                }));
            }

            Task.WhenAll(tasks);
        }
    }

}
