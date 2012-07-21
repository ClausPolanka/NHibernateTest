using System;

namespace Domain
{
    public class FlightUnit
    {
        public virtual int StudentNumber { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int Duration { get; set; }
        public virtual string LectureContent { get; set; }
        public virtual int TeacherNumber { get; set; }
        public virtual int AircraftId { get; set; }

        public virtual bool Equals(FlightUnit other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return other.StudentNumber == StudentNumber &&
                   other.Date.Equals(Date) &&
                   other.Duration == Duration &&
                   Equals(other.LectureContent, LectureContent) &&
                   other.TeacherNumber == TeacherNumber &&
                   other.AircraftId == AircraftId;
        }

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

        public override int GetHashCode()
        {
            unchecked
            {
                int result = StudentNumber;
                result = (result*397) ^ Date.GetHashCode();
                result = (result*397) ^ Duration;
                result = (result*397) ^ (LectureContent != null ? LectureContent.GetHashCode() : 0);
                result = (result*397) ^ TeacherNumber;
                result = (result*397) ^ AircraftId;
                return result;
            }
        }
    }
}