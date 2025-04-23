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
        public int yearOfEmployment { get; private set; }
        public int skillLevel { get; private set; } 
        public Status status { get; private set; }

        public EmployeeInfo(int yearOfEmployment, int skillLevel, Status status)
        {
            this.yearOfEmployment = yearOfEmployment;
            if (skillLevel < 1) this.skillLevel = 1;
            else if (skillLevel > 10) this.skillLevel = 10;
            else this.skillLevel = skillLevel;
            this.status = status;
        }

        public override string ToString()
        {
            return "year of employment = " + yearOfEmployment + ", skill level = " + skillLevel + ", status = " + status;
        }
    }
}
