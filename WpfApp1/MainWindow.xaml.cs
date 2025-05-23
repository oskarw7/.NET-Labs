using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Helpers;
using WpfApp1.Implementations;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FibonacciTaskManager manager;
        private IEnumerable<long> tasks = Enumerable.Range(200000, 10000).Select(x => (long)x);
        private int completedTasks = 0;
        private CancellationTokenSource stopTokenSource;
        private Stopwatch timer;

        public MainWindow()
        {
            InitializeComponent();
            completedTasks = 0;
            manager = new FibonacciTaskManager(tasks);
            listBoxLog.ItemsSource = manager.Logs;
            progressBar.Minimum = 0;
            progressBar.Maximum = manager.TotalTasks;
            progressBar.Value = 0;
            stopTokenSource = new CancellationTokenSource();
            timer = Stopwatch.StartNew();
        }

        private void UpdateProgress()
        {
            Dispatcher.Invoke(() =>
            {
                completedTasks++;
                progressBar.Value = completedTasks;
                if (completedTasks == tasks.Count())
                {
                    timer.Stop();
                    MessageBox.Show($"All tasks completed in {timer.Elapsed.TotalSeconds:F2} seconds.",
                                    "Done", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            });
        }



        private void btnStartTask_Click(object sender, RoutedEventArgs e)
        {
            StartRunner(new TaskBasedRunner(manager, UpdateProgress));
        }

        private void btnStartDelegate_Click(object sender, RoutedEventArgs e)
        {
            StartRunner(new DelegateBasedRunner(manager, UpdateProgress));
        }

        private void btnStartAsyncAwait_Click(object sender, RoutedEventArgs e)
        {
            StartRunner(new AsyncAwaitRunner(manager, UpdateProgress));
        }

        private void btnStartBackground_Click(object sender, RoutedEventArgs e)
        {
            StartRunner(new BackgroundWorkerRunner(manager, UpdateProgress));
        }

        private void StartRunner(object runner)
        {

            switch (runner)
            {
                case TaskBasedRunner t: t.Start(4, stopTokenSource.Token); break;
                case DelegateBasedRunner d: d.Start(4, stopTokenSource.Token); break;
                case AsyncAwaitRunner a: a.Start(4, stopTokenSource.Token); break;
                case BackgroundWorkerRunner b: b.Start(4, stopTokenSource.Token); break;
            }
        }


        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            stopTokenSource.Cancel();
            completedTasks = 0;
            manager = new FibonacciTaskManager(tasks);
            listBoxLog.ItemsSource = manager.Logs;
            stopTokenSource = new CancellationTokenSource();
            progressBar.Value = 0;
            timer = Stopwatch.StartNew();
        }
    }
}