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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

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

            var deserializeItem = new MenuItem { Header = "Deserialize" };
            deserializeItem.Click += (s, ev) => DeserializeEmployees();
            contextMenu.Items.Add(deserializeItem);

            var yearItem = new MenuItem { Header = "Find unique year in XML" };
            yearItem.Click += (s, ev) => FindUniqueYearOfEmployment();
            contextMenu.Items.Add(yearItem);

            var tableItem = new MenuItem { Header = "Generate XHTML table" };
            tableItem.Click += (s, ev) => GenerateXHTML();
            contextMenu.Items.Add(tableItem);

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
            var xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Employee>));
            using(var streamWriter = new StreamWriter("../../../Serialized/employees.xml"))
            {
                xmlSerializer.Serialize(streamWriter, employees);
            }
            EmployeeDetails.Text = "Data serialized successfully to file employees.xml\n\n";
        }

        public void DeserializeEmployees()
        {
            var xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Employee>));
            using (var streamReader = new StreamReader("../../../Serialized/employees.xml"))
            {
                var deserializeEmployees = xmlSerializer.Deserialize(streamReader);
                if(deserializeEmployees is ObservableCollection<Employee> validEmployees)
                {
                    this.employees.Clear();
                    foreach(var employee in validEmployees)
                    {
                        employee.SetIdFromTime();
                        this.employees.Add(employee);
                    }
                    EmployeeDetails.Text = "Data deserialized successfully from file employees.xml\n\n";
                }
                else
                {
                    EmployeeDetails.Text = "Error occured while deserializing data\n\n";
                }
            }
            EmployeeListView.Items.Refresh();
        }

        public void FindUniqueYearOfEmployment()
        {
            XElement xmlDocument = XElement.Load("../../../Serialized/employees.xml");
            IEnumerable<XElement> queryResult = xmlDocument.XPathSelectElements("//Employee[" +
                "not(employeeInfo/yearOfEmployment = preceding::Employee/employeeInfo/yearOfEmployment)" +
                "and not(employeeInfo/yearOfEmployment = following::Employee/employeeInfo/yearOfEmployment)]");
            EmployeeDetails.Text = "Employees with unique year of employment:\n\n";
            if (!queryResult.Any())
            {
                EmployeeDetails.Text += "NONE";
                return;
            } 
            foreach(var result in queryResult)
            {
                EmployeeDetails.Text += result.ToString() + "\n\n";
            }
        }

        public void GenerateXHTML()
        {
            XNamespace xmlns = XNamespace.Get("http://www.w3.org/1999/xhtml");
            var document = new XDocument(
                new XDeclaration("1.0", "utf-8", null), // encoding
                new XDocumentType("html",
                    "-//W3C//DTD XHTML 1.0 Strict//EN",
                    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd",
                    null
                ), // doctype
                new XElement(xmlns + "html",
                    new XElement(xmlns + "head",
                        new XElement(xmlns + "title", "Table of employees")
                    ),
                    new XElement(xmlns + "body",
                        new XElement(xmlns + "table",
                            new XAttribute("border", "1"),
                            new XElement(xmlns + "tr",
                                new XElement(xmlns + "th", "ID"),
                                new XElement(xmlns + "th", "Name"),
                                new XElement(xmlns + "th", "Year of employment"),
                                new XElement(xmlns + "th", "Skill level"),
                                new XElement(xmlns + "th", "Status")
                            ),
                            this.employees.Select(employee =>
                                new XElement(xmlns + "tr",
                                    new XElement(xmlns + "td", employee.id),
                                    new XElement(xmlns + "td", employee.name),
                                    new XElement(xmlns + "td", employee.employeeInfo.yearOfEmployment),
                                    new XElement(xmlns + "td", employee.employeeInfo.skillLevel),
                                    new XElement(xmlns + "td", employee.employeeInfo.status)
                                )
                            )
                        )
                    )
                )
            );
            document.Save("../../../Table/table.xhtml");
            EmployeeDetails.Text = "A table of employees were saved to file table.xhtml";
        }
    }
}