using Lab2.Model;
using Lab2.View;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Serialization;

namespace Lab2.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Employee> employees;
        private Random random;
        public MainWindow()
        {
            InitializeComponent();
            employees = new ObservableCollection<Employee>();
            EmployeeListView.ItemsSource = employees;
            random = new Random();
        }

        private void GenerateData_Click(object sender, RoutedEventArgs e)
        {
            employees.Clear();

            var names = CreateEmployeeWindow.names;
            var statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToArray();

            for (int i = 0; i < 50; i++)
            {
                string name = names[random.Next(names.Length)];
                int yearOfEmployment = random.Next(1950, DateTime.Now.Year);
                int skillLevel = random.Next(1, 11);
                Status status = statuses[random.Next(statuses.Length)];
                var employeeInfo = new EmployeeInfo(yearOfEmployment, skillLevel, status);
                var employee = new Employee(name, employeeInfo);

                employees.Add(employee);
            }
        }

        private void Version_Click(object sender, RoutedEventArgs e)
        {
            var versionWindow = new VersionWindow();
            versionWindow.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EmployeeListView_SelectionChanged(object sender, SelectionChangedEventArgs e) { 
            var selectedEmployee = EmployeeListView.SelectedItem as Employee;
            if (selectedEmployee == null) return;
            string employeesText = selectedEmployee.ToString();
            EmployeeDetails.Text = employeesText;
        }

        private void EmployeeListView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var src = e.OriginalSource as DependencyObject;

            while (src != null && src is not ListViewItem)
            {
                src = VisualTreeHelper.GetParent(src);
            }
            var listItem = src as ListViewItem;

            Employee? employee = null;

            if (listItem != null)
            {
                listItem.Focus();
                employee = listItem.DataContext as Employee;
            }

            var contextMenu = new ContextMenu();

            var projectItem = new MenuItem { Header = "Execute first query" };
            projectItem.Click += (s, ev) => ProjectEmployees_Click();
            contextMenu.Items.Add(projectItem);

            var groupItem = new MenuItem { Header = "Execute second query" };
            groupItem.Click += (s, ev) => GroupProjectedEmployees_Click();
            contextMenu.Items.Add(groupItem);

            var serializeItem = new MenuItem { Header = "Serialize" };
            serializeItem.Click += (s, ev) => SerializeEmployees();
            contextMenu.Items.Add(serializeItem);

            var createItem = new MenuItem { Header = "Create" };
            createItem.Click += (s, ev) => CreateEmployee();
            contextMenu.Items.Add(createItem);

            if (employee != null)
            {
                var deleteItem = new MenuItem { Header = "Delete" };
                deleteItem.Click += (s, ev) => DeleteEmployee(employee);
                contextMenu.Items.Add(deleteItem);
            }

            e.Handled = true;           // przeciwdzialanie wyswietleniu defaultowemu context menu
            contextMenu.IsOpen = true;  // zamiast listItem.ContextMenu, zeby wyswietlic na pustym polu
        }

        public void CreateEmployee()
        {
            var createEmployeeWindow = new CreateEmployeeWindow();
            var result = createEmployeeWindow.ShowDialog();
            if (result==true)
            {
                this.employees.Add(createEmployeeWindow.employee);
            }
        }

        public void DeleteEmployee(Employee employee)
        {
            if(employee == null)
            {
                return;
            }
            this.employees.Remove(employee);
            EmployeeListView.Items.Refresh();
            EmployeeDetails.Text = "";
        }

        // w instrukcji jest wypisywanie query w konsoli, ja zmienilem config z WinExe na Exe w .csproj
        // wyniki byly w konsoli, ale dodatkowe okna (jak formularz) byly puste po pierwszym otwarciu
        // dlatego wyniki w EmployeeDetails
        public void ProjectEmployees_Click()
        {
            var queryResult = ProjectEmployees();

            EmployeeDetails.Text = "";
            foreach(var result in queryResult)
            {
                string printResult = "SUM_OF =  " + result.SUM_OF + ", UPPERCASE = " + result.UPPERCASE + "\n\n";
                Console.WriteLine(printResult);
                EmployeeDetails.Text += printResult;
            }
        }

        public void GroupProjectedEmployees_Click()
        {
            var queryResult = GroupProjectedEmployees();
            EmployeeDetails.Text = "";
            foreach(var result in queryResult)
            {
                string printResult = "UPPERCASE = " + result.UPPERCASE + ", AVERAGE_SUM_OF = " + result.AVERAGE_SUM_OF + "\n\n";
                Console.WriteLine(printResult);
                EmployeeDetails.Text += printResult;
            }
        }

        public IEnumerable<dynamic> ProjectEmployees()
        {
            var query = from employee in this.employees
                        where employee.id % 2 == 1
                        select new
                        {
                            SUM_OF = employee.employeeInfo.skillLevel + employee.employeeInfo.yearOfEmployment,
                            UPPERCASE = employee.employeeInfo.status.ToString().ToUpper() // raczej dodac pole tekstowe
                        };
            return query;
        }

        public IEnumerable<dynamic> GroupProjectedEmployees()
        {
            var projected = ProjectEmployees();
            var query = from employee in projected
                        group employee by employee.UPPERCASE into grouped
                        select new
                        {
                            UPPERCASE = grouped.Key,
                            AVERAGE_SUM_OF = grouped.Average(x => x.SUM_OF)
                        };
            return query;
        }

        public void SerializeEmployees()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Employee>));
            using(var streamWriter = new StreamWriter("../../../Serialized/employees.xml"))
            {
                xmlSerializer.Serialize(streamWriter, employees.ToList());
            }
        }
    }
}