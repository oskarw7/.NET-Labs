using Lab1.Model;
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

namespace Lab1.View
{
    /// <summary>
    /// Interaction logic for CreateEmployeeWindow.xaml
    /// </summary>
    public partial class CreateEmployeeWindow : Window
    {
        public Employee employee { get; private set; }
        private string[] names = new string[]
        {
            "Adam", "Aleksandra", "Andrzej", "Antoni", "Barbara", "Beata", "Bogdan",
            "Bogumiła", "Damian", "Dorota", "Edward", "Elżbieta", "Filip",
            "Grażyna", "Hubert", "Iwona", "Jakub", "Jan", "Joanna", "Karolina",
            "Krzysztof", "Łukasz", "Małgorzata", "Michał", "Monika", "Natalia",
            "Paweł", "Piotr", "Zofia"
        };
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

            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(year) || string.IsNullOrEmpty(skill))
            {
                MessageBox.Show("Please provide all parameters.", "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(year, out int yearNum))
            {
                MessageBox.Show("Year of employment must be a number.", "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if(yearNum < 1950 || yearNum > DateTime.Now.Year)
            {
                MessageBox.Show("Year of employment out of valid range.", "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(skill, out double skillNum))
            {
                MessageBox.Show("Skill level must be a number.", "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(skillNum < 0.0 || skillNum > 10.0)
            {
                MessageBox.Show("Skill level out of valid range.", "Create", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.employee = new Employee(name, yearNum, skillNum, new ObservableCollection<Employee>());
            DialogResult = true;
            Close();
        }

        private void Autofill_Click(object sender, RoutedEventArgs e)
        {
            string name = names[random.Next(names.Length)];
            int year = random.Next(1950, DateTime.Now.Year);
            double skill = Math.Round(random.NextDouble() * 10, 1);

            NameBox.Text = name;
            YearBox.Text = year.ToString();
            SkillBox.Text = skill.ToString();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
