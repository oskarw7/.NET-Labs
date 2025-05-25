using System;
using System.Threading;
using WpfApp1.Helpers;

namespace WpfApp1.Implementations
{
    internal class DelegateRunner
    {
        private delegate void WorkDelegate();
        private readonly FibonacciTaskManager manager;
        private readonly Action updateProgress;

        public DelegateRunner(FibonacciTaskManager m, Action progressUpdate) => (manager, updateProgress) = (m, progressUpdate);

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

                var (result, time) = FibonacciThread.Calculate(task);

                manager.LogResult((short)Thread.CurrentThread.ManagedThreadId, task, result, time);
                updateProgress();
            }
        }
    }

}
