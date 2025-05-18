using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model
{
    public enum Status
    {
        Intern,
        Junior,
        Mid,
        Senior
    }

    public class EmployeeInfo : IComparable<EmployeeInfo>, IComparable
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
            return "year of employment = " + yearOfEmployment + ",\n skill level = " + skillLevel + ", status = " + status;
        }

        // wymog icomparable
        public int CompareTo(EmployeeInfo? other)
        {
            if (other == null) return 1;

            int statusCompare = this.status.CompareTo(other.status);
            if (statusCompare != 0)
            {
                return statusCompare;
            }

            // default 
            return this.yearOfEmployment.CompareTo(other.yearOfEmployment);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is EmployeeInfo other)
                return this.CompareTo(other);
            throw new ArgumentException("Object is not an EmployeeInfo");
        }
    }
}