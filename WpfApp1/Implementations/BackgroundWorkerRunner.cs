using System;
using System.ComponentModel;
using System.Threading;
using WpfApp1.Helpers;

namespace WpfApp1.Implementations
{
    internal class BackgroundWorkerRunner
    {
        private readonly FibonacciTaskManager manager;
        private readonly Action updateProgress;

        public BackgroundWorkerRunner(FibonacciTaskManager m, Action progressUpdate) => (manager, updateProgress) = (m, progressUpdate);

        public void Start(int threadCount, CancellationToken token)
        {
            for (int i = 0; i < threadCount; i++)
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        long task = manager.GetNextTask();

                        if (task == -1) break;

                        var (result, time) = FibonacciThread.Calculate(task);

                        manager.LogResult((short)Thread.CurrentThread.ManagedThreadId, task, result, time);
                        updateProgress();
                    }
                };
                worker.RunWorkerAsync();
            }
        }
    }

}
