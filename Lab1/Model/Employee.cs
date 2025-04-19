using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model
{
    public class Employee
    {
        private string name {  get; set; }
        private int yearOfEmployment { get; set; }
        private double skillLevel { get; set; }
        private ObservableCollection<Employee> subordinates { get; set; }

        public Employee(string name, int yearOfEmployment, double skillLevel, 
            ObservableCollection<Employee> subordinates) {
            this.name = name;
            this.yearOfEmployment = yearOfEmployment;
            this.skillLevel = skillLevel;
            this.subordinates = subordinates;
        }

        public override string ToString()
        {
            return "Employee: name=" + name + ", year of employment=" + yearOfEmployment + ", skill level=" + skillLevel;
        }
    }
}
