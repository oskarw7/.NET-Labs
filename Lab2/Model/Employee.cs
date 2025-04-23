using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using System.Xml.Serialization;

namespace Lab2.Model
{
    public class Employee
    {
        // deserializacja wymaga publicznych setterów
        [XmlIgnore]
        private static int idGenerator = 0;
        [XmlIgnore]
        public int id { get; set; }
        public string name {  get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public Employee(string name, EmployeeInfo employeeInfo) {
            this.id = idGenerator++;
            this.name = name;
            this.employeeInfo = employeeInfo;
        }

        // wymog serializacji
        public Employee()
        {
            this.id = idGenerator++;
            this.name = "Unknown";
            this.employeeInfo = new EmployeeInfo();
        }

        public override string ToString()
        {
            return "Employee: name = " + name + ", " + employeeInfo;
        }

        public void SetIdFromTime()
        {
            this.id = (int)DateTimeOffset.Now.ToUnixTimeSeconds() % int.MaxValue + idGenerator++;
        }
    }
}
