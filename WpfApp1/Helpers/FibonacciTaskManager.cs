using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Helpers
{
    internal class FibonacciTaskManager
    {
        private readonly object _lock = new object();
        private Queue<long> taskQueue;
        public ObservableCollection<string> Logs { get; }

        public int TotalTasks => originalCount;
        private int originalCount;

        public FibonacciTaskManager(IEnumerable<long> tasks)
        {
            taskQueue = new Queue<long>(tasks);
            originalCount = taskQueue.Count;
            Logs = new ObservableCollection<string>();
        }

        public long GetNextTask()
        {
            lock (_lock)
            {
                if (taskQueue.Count > 0)
                    return taskQueue.Dequeue();
                return -1;
            }
        }

        public void LogResult(long input, long result, long timeNs)
        {
            string entry = $"Fib({input}) = {result} in {timeNs / 1000} μs";
            App.Current.Dispatcher.Invoke(() => Logs.Add(entry));
        }
    }

}
