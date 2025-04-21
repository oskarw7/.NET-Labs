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
        // Public, zeby TreeView widzialo zmienne do bindingu
        public string name {  get; private set; }
        public int yearOfEmployment { get; private set; }
        public double skillLevel { get; private set; }
        public ObservableCollection<Employee> subordinates { get; private set; }

        public Employee supervisor { get; set; }

        public Employee(string name, int yearOfEmployment, double skillLevel, 
            ObservableCollection<Employee> subordinates) {
            this.name = name;
            this.yearOfEmployment = yearOfEmployment;
            this.skillLevel = skillLevel;
            this.subordinates = subordinates;
            if(subordinates != null)
            {
                foreach(var subordinate in subordinates)
                {
                    subordinate.supervisor = this;
                }
            }
        }

        public override string ToString()
        {
            return "Employee: name=" + name + ", year of employment=" + yearOfEmployment + ", skill level=" + skillLevel;
        }

        public void AddSubordinate(Employee subordinate)
        {
            subordinates.Add(subordinate);
            subordinate.supervisor = this;
        }

        public string PrintRecursive(int depth = 0)
        {
            string result = new string('\t', depth) + this + "\n\n";
            foreach (var subordinate in subordinates)
            {
                result += subordinate.PrintRecursive(depth + 1);
            }
            return result;
        }
    }
}
