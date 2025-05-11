using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Model
{
    public enum Status
    {
        Intern,
        Junior,
        Mid,
        Senior
    }

    public class EmployeeInfo
    {
        // deserializacja wymaga publicznych setterów
        public int yearOfEmployment { get; set; }
        public int skillLevel { get; set; } 
        public Status status { get; set; }

        public EmployeeInfo(int yearOfEmployment, int skillLevel, Status status)
        {
            this.yearOfEmployment = yearOfEmployment;
            if (skillLevel < 1) this.skillLevel = 1;
            else if (skillLevel > 10) this.skillLevel = 10;
            else this.skillLevel = skillLevel;
            this.status = status;
        }

        // wymog serializacji
        public EmployeeInfo()
        {
            this.yearOfEmployment = 0;
            this.skillLevel = 0;
            this.status = Status.Intern;
        }

        public override string ToString()
        {
            return "year of employment = " + yearOfEmployment + ", skill level = " + skillLevel + ", status = " + status;
        }
    }
}
