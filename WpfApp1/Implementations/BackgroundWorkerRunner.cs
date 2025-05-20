using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Helpers;

namespace WpfApp1.Implementations
{
    internal class BackgroundWorkerRunner
    {
        private readonly FibonacciTaskManager manager;
        private readonly Action updateProgress;

        public BackgroundWorkerRunner(FibonacciTaskManager m, Action progressUpdate) =>
            (manager, updateProgress) = (m, progressUpdate);

        public void Start(int threadCount)
        {
            for (int i = 0; i < threadCount; i++)
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    while (true)
                    {
                        long task = manager.GetNextTask();
                        if (task == -1) break;

                        var (result, time) = FibonacciHelper.Calculate(task);
                        manager.LogResult(task, result, time);
                        updateProgress();
                    }
                };
                worker.RunWorkerAsync();
            }
        }
    }

}
