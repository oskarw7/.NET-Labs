using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace Lab2.Model
{
    public class Employee
    {
        private static int idGenerator = 0;
        public int id { get; private set; }
        public string name {  get; private set; }
        public EmployeeInfo employeeInfo { get; private set; }

        public Employee(string name, EmployeeInfo employeeInfo) {
            this.id = idGenerator++;
            this.name = name;
            this.employeeInfo = employeeInfo;
        }

        public override string ToString()
        {
            return "Employee: name = " + name + ", " + employeeInfo;
        }
    }
}
