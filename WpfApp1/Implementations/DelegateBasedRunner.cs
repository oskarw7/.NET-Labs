using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.Helpers;

namespace WpfApp1.Implementations
{
    internal class DelegateBasedRunner
    {
        private delegate void WorkDelegate();
        private readonly FibonacciTaskManager manager;
        private readonly Action updateProgress;

        public DelegateBasedRunner(FibonacciTaskManager m, Action progressUpdate) =>
            (manager, updateProgress) = (m, progressUpdate);

        public void Start(int threadCount, CancellationToken token)
        {
            for (int i = 0; i < threadCount; i++)
            {
                WorkDelegate del = () => Work(token);
                del.BeginInvoke(null, null);
            }
        }


        private void Work(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                long task = manager.GetNextTask();
                if (task == -1) break;

                var (result, time) = FibonacciHelper.Calculate(task);
                manager.LogResult((short)Thread.CurrentThread.ManagedThreadId, task, result, time);
                updateProgress();
            }
        }
    }

}
