using Lab1.Model;
using Lab1.View;
using System.Collections.ObjectModel;
using System.Reflection.PortableExecutable;
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

namespace Lab1.View
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

        private void GenerateData_Click(object sender, RoutedEventArgs e)
        {
            employees.Clear();

            var ceoSubordinates = new ObservableCollection<Employee>
            {
                new Employee("Bob", 2015, 6.5, new ObservableCollection<Employee>()),
                new Employee("Charlie", 2018, 7.0, new ObservableCollection<Employee>()),
                new Employee("Robert", 2018, 1.0, new ObservableCollection<Employee>()),
                new Employee("Diana", 2020, 5.0, new ObservableCollection<Employee>()),
                new Employee("Ewa", 2019, 4.5, new ObservableCollection<Employee>())
            };

            var ceo = new Employee("Alice", 2010, 9.0, ceoSubordinates);

            ceoSubordinates[0].AddSubordinate(new Employee("Piotrek", 2022, 0.5, new ObservableCollection<Employee>()));
            ceoSubordinates[1].AddSubordinate(new Employee("Lukasz", 2019, 4.0, new ObservableCollection<Employee>()));
            ceoSubordinates[2].AddSubordinate(new Employee("Eustachy", 2018, 6.0, new ObservableCollection<Employee>()));
            ceoSubordinates[3].AddSubordinate(new Employee("Grzegorz", 2018, 6.0, new ObservableCollection<Employee>()));
            ceoSubordinates[3].AddSubordinate(new Employee("Ania", 2020, 4.5, new ObservableCollection<Employee>()));
            ceoSubordinates[4].AddSubordinate(new Employee("Katarzyna", 2017, 5.5, new ObservableCollection<Employee>()));

            var subordinatesOfFist = new ObservableCollection<Employee>
            {
                new Employee("Marta", 2021, 5.0, new ObservableCollection<Employee>()),
                new Employee("Marek", 1960, 5.5, new ObservableCollection<Employee>()),
                new Employee("Tomek", 2020, 5.0, new ObservableCollection<Employee>())
            };
            ceoSubordinates[0].AddSubordinate(new Employee("Franciszek", 2015, 5.5, subordinatesOfFist));

            subordinatesOfFist[0].AddSubordinate(new Employee("Szymon", 2022, 3.5, new ObservableCollection<Employee>()));

            var subordinatesOfTomek = new ObservableCollection<Employee>
            {
                new Employee("Julia", 2021, 4.5, new ObservableCollection<Employee>())
            };

            ceoSubordinates[1].AddSubordinate(new Employee("Szymon", 2021, 6.0, subordinatesOfTomek));

            var subordinatesOfEwa = new ObservableCollection<Employee>
            {
                new Employee("Kamil", 2020, 5.0, new ObservableCollection<Employee>())
            };

            ceoSubordinates[4].AddSubordinate(new Employee("Zuzanna", 2021, 4.0, subordinatesOfEwa));

            employees.Add(ceo);

            var ceo2Subordinates = new ObservableCollection<Employee>
            {
                new Employee("Ola", 2017, 6.5, new ObservableCollection<Employee>()),
                new Employee("Pawel", 2016, 7.5, new ObservableCollection<Employee>())
            };

            var secondRoot = new Employee("Henryk", 2008, 8.0, ceo2Subordinates);

            ceo2Subordinates[0].AddSubordinate(new Employee("Basia", 2019, 5.5, new ObservableCollection<Employee>()));
            ceo2Subordinates[1].AddSubordinate(new Employee("Janek", 2020, 6.0, new ObservableCollection<Employee>()));

            employees.Add(secondRoot);
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

        private void EmployeeTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedEmployee = EmployeeTreeView.SelectedItem as Employee;
            if (selectedEmployee == null) return;
            string employeesText = selectedEmployee.PrintRecursive();
            EmployeeDetails.Text = employeesText;
        }

        private void EmployeeTreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var src = e.OriginalSource as DependencyObject;

            while (src != null && src is not TreeViewItem)
            {
                src = VisualTreeHelper.GetParent(src);
            }
            var treeItem = src as TreeViewItem;

            if (treeItem == null)
            {
                e.Handled = true;
                var treeView = sender as TreeView;

                var contextMenu = new ContextMenu();

                var createItem = new MenuItem { Header = "Create" };
                createItem.Click += (s, ev) => CreateEmployee();
                contextMenu.Items.Add(createItem);

                treeView.ContextMenu = contextMenu;
            }
            else
            {
                treeItem.Focus();
                e.Handled = true; // przeciwdzialanie wyswietleniu defaultowemu context menu

                var employee = treeItem.DataContext as Employee;
                if (employee == null) return;

                var contextMenu = new ContextMenu();
                var createItem = new MenuItem { Header = "Create" };
                createItem.Click += (s, ev) => CreateEmployee(employee);
                contextMenu.Items.Add(createItem);
                var deleteItem = new MenuItem { Header = "Delete" };
                deleteItem.Click += (s, ev) => DeleteEmployee(employee);
                contextMenu.Items.Add(deleteItem);

                treeItem.ContextMenu = contextMenu;
            }
        }

        public void CreateEmployee(Employee superiorEmployee)
        {
            var createEmployeeWindow = new CreateEmployeeWindow();
            var result = createEmployeeWindow.ShowDialog();
            if (result == true)
            {
                superiorEmployee.AddSubordinate(createEmployeeWindow.employee);
            }
        }

        private void CreateEmployee()
        {
            var createEmployeeWindow = new CreateEmployeeWindow();
            var result = createEmployeeWindow.ShowDialog();
            if (result == true)
            {
                if (EmployeeTreeView.ItemsSource is ObservableCollection<Employee> employees)
                {
                    employees.Add(createEmployeeWindow.employee);
                }
                else
                {
                    var newList = new ObservableCollection<Employee>();
                    newList.Add(createEmployeeWindow.employee);
                    EmployeeTreeView.ItemsSource = newList;
                }
            }
        }

        public void DeleteEmployee(Employee employee)
        {
            var supervisor = employee.supervisor;
            if (supervisor == null)
            {
                employees.Remove(employee);
            }
            else
            {
                employee.supervisor.subordinates.Remove(employee);
            }
            EmployeeTreeView.Items.Refresh();
            EmployeeDetails.Text = "";
        }
    }
}