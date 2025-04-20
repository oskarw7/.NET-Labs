using Lab1.Model;
using Lab1.View;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Employee> employees;
        public MainWindow()
        {
            InitializeComponent();
            employees = new ObservableCollection<Employee>();
            EmployeeTreeView.ItemsSource = employees;
        }

        public void GenerateData_Click(object sender, RoutedEventArgs e)
        {
            employees.Clear();

            var ceoSubordinates = new ObservableCollection<Employee>
            {
                new Employee("Bob", 2015, 6.5, new ObservableCollection<Employee>()),
                new Employee("Charlie", 2018, 7.0, new ObservableCollection<Employee>())
            };

            var ceo = new Employee("Alice", 2010, 9.0, ceoSubordinates);

            ceoSubordinates[0].AddSubordinate(new Employee("Piotrek", 2022, 0.5, new ObservableCollection<Employee>()));

            employees.Add(ceo);
        }

        public void Version_Click(object sender, RoutedEventArgs e)
        {
            var versionWindow = new VersionWindow();
            versionWindow.ShowDialog();
        }

        public void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void EmployeeTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) { 
        
        }

        public void TreeItem_ContextMenuOpening(object sender, RoutedEventArgs e)
        {

        }
    }
}