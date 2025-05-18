using Lab3.Model;
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
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Lab3.View
{
    /// <summary>
    /// Interaction logic for CreateEmployeeWindow.xaml
    /// </summary>
    public partial class CreateEmployeeWindow : Window
    {
        public Employee employee { get; private set; }
        public static string[] names = new[] { "Jan", "Anna", "Piotr", "Kasia", "Marek", "Ewa", "Tomasz", "Agnieszka", "Paweł", "Magda",
                                "Krzysztof", "Monika", "Łukasz", "Beata", "Andrzej", "Joanna", "Marcin", "Aleksandra", "Dawid", "Natalia",
                                "Grzegorz", "Julia", "Michał", "Zuzanna", "Rafał", "Karolina", "Artur", "Sylwia", "Sebastian", "Patrycja"};
        private Random random;

        public CreateEmployeeWindow()
        {
            InitializeComponent();
            this.random = new Random();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var name = NameBox.Text;
            var year = YearBox.Text;
            var skill = SkillBox.Text;
            var status = StatusBox.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(year) || string.IsNullOrEmpty(skill) || string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Please provide all parameters.", "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(year, out int yearNum))
            {
                MessageBox.Show("Year of employment must be a number.", "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (yearNum < 1950 || yearNum > DateTime.Now.Year)
            {
                MessageBox.Show("Year of employment out of valid range.", "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(skill, out int skillNum))
            {
                MessageBox.Show("Skill level must be a number.", "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (skillNum < 0 || skillNum > 10)
            {
                MessageBox.Show("Skill level out of valid range.", "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var statusParsed = mapStatus(status);
            if (statusParsed == null)
            {
                MessageBox.Show("Such status is not allowed.", "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var employeeInfo = new EmployeeInfo(yearNum, skillNum, statusParsed.Value);
            this.employee = new Employee(name, employeeInfo);
            DialogResult = true;
            Close();
        }

        private void Autofill_Click(object sender, RoutedEventArgs e)
        {
            string name = names[random.Next(names.Length)];
            int year = random.Next(1950, DateTime.Now.Year);
            int skill = random.Next(1, 11);
            var statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToArray();
            Status status = statuses[random.Next(statuses.Length)];

            NameBox.Text = name;
            YearBox.Text = year.ToString();
            SkillBox.Text = skill.ToString();
            StatusBox.Text = status.ToString();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private Status? mapStatus(string status)
        {
            switch (status)
            {
                case "Intern":
                    return Status.Intern;
                case "Junior":
                    return Status.Junior;
                case "Mid":
                    return Status.Mid;
                case "Senior":
                    return Status.Senior;
                default:
                    return null;
            }
        }
    }
}
