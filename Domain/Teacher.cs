using System.Collections.Generic;

namespace Domain
{
    public class Teacher
    {
        public virtual int Number { get; set; }
        public virtual int Experience { get; set; }
        public virtual IList<FlightUnit> FlightUnits { get; set; }
    }
}