using System;
using System.Collections.Generic;

namespace Domain
{
    public class FlightUnit
    {
        public virtual int StudentNumber { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int Duration { get; set; }
        public virtual string LectureContent { get; set; }
        public virtual int AircraftId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof (FlightUnit))
                return false;
            return Equals((FlightUnit) obj);
        }

        public virtual bool Equals(FlightUnit other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other.StudentNumber == StudentNumber && other.Date.Equals(Date);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StudentNumber * 397) ^ Date.GetHashCode();
            }
        }
    }
}