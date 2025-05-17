using Lab3.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Lab3.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SortableSearchableCollection<Employee> employees;
        private Random random;
        private CollectionViewSource collectionViewSource;
        public MainWindow()
        {
            InitializeComponent();
            employees = new SortableSearchableCollection<Employee>();

            collectionViewSource = new CollectionViewSource { Source = employees };
            collectionViewSource.Filter += CollectionViewSource_Filter;

            EmployeeDataGrid.ItemsSource = collectionViewSource.View;

            random = new Random();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (e.Item is not Employee emp)
            {
                e.Accepted = false;
                return;
            }

            string inputText = SearchTextBox.Text.ToLower();

            e.Accepted = string.IsNullOrWhiteSpace(inputText) || emp.name.ToLower().Contains(inputText);
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

        private void EmployeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEmployee = EmployeeDataGrid.SelectedItem as Employee;
            if (selectedEmployee == null) return;
            string employeesText = selectedEmployee.ToString();
            EmployeeDetails.Text = employeesText;
        }

        private void EmployeeDataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var src = e.OriginalSource as DependencyObject;

            while (src != null && src is not DataGridRow)
            {
                src = VisualTreeHelper.GetParent(src);
            }
            var dataGridRow = src as DataGridRow;

            Employee? employee = null;

            if (dataGridRow != null)
            {
                dataGridRow.Focus();
                employee = dataGridRow.Item as Employee;
            }

            var contextMenu = new ContextMenu();

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
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            string? selected = (SortComboBox.SelectedItem as ComboBoxItem)?.Content as string;

            switch (selected)
            {
                case "Name":
                    employees.SortBy(emp => emp.name);
                    break;
                case "Year":
                    employees.SortBy(emp => emp.employeeInfo.yearOfEmployment);
                    break;
                case "Skill":
                    employees.SortBy(emp => emp.employeeInfo.skillLevel);
                    break;
                case "Status":
                    employees.SortBy(emp => emp.employeeInfo.status.ToString());
                    break;
                case "EmployeeInfo":
                    employees.SortBy(emp => emp.employeeInfo);
                    break;
                default:
                    return;
            }

            collectionViewSource.View.Refresh();
        }

        public void CreateEmployee()
        {
            var createEmployeeWindow = new CreateEmployeeWindow();
            var result = createEmployeeWindow.ShowDialog();
            if (result == true)
            {
                this.employees.Add(createEmployeeWindow.employee);
            }
        }

        public void DeleteEmployee(Employee employee)
        {
            if (employee == null)
            {
                return;
            }
            this.employees.Remove(employee);
            EmployeeDataGrid.Items.Refresh();
            EmployeeDetails.Text = "";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //collectionViewSource.View.Refresh();
            string input = SearchTextBox.Text.ToLower();

            employees.SearchBy(emp =>
            string.IsNullOrWhiteSpace(input) ||
            emp.name.ToLower().Contains(input) ||
            emp.employeeInfo.status.ToString().ToLower().Contains(input)
            );
        }
    }
}