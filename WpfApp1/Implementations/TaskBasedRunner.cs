using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Helpers;

namespace WpfApp1.Implementations
{
    internal class TaskBasedRunner
    {
        private readonly FibonacciTaskManager manager;
        private readonly Action updateProgress;
        public TaskBasedRunner(FibonacciTaskManager m, Action progressUpdate) =>
            (manager, updateProgress) = (m, progressUpdate);

        public void Start(int threadCount)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    while (true)
                    {
                        long task = manager.GetNextTask();
                        if (task == -1) break;

                        var (result, time) = FibonacciHelper.Calculate(task);
                        manager.LogResult(task, result, time);
                        updateProgress();
                    }
                }));
            }

            Task.WhenAll(tasks); // not awaited in fire-and-forget
        }
    }

}
