using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
            completedTasks = 0;
            var tasks = Enumerable.Range(20, 100000).Select(x => (long)x); // Example inputs
            manager = new FibonacciTaskManager(tasks);
            listBoxLog.ItemsSource = manager.Logs;
            progressBar.Minimum = 0;
            progressBar.Maximum = manager.TotalTasks;
            progressBar.Value = 0;

        }


        private int completedTasks = 0;

        private void UpdateProgress()
        {
            Dispatcher.Invoke(() =>
            {
                completedTasks++;
                progressBar.Value = completedTasks;
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
                case TaskBasedRunner t: t.Start(4); break;
                case DelegateBasedRunner d: d.Start(4); break;
                case AsyncAwaitRunner a: a.Start(4); break;
                case BackgroundWorkerRunner b: b.Start(4); break;
            }
        }
    }
}