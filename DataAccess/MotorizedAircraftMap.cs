using Domain;
using FluentNHibernate.Mapping;

namespace DataAccess
{
    public class MotorizedAircraftMap : ClassMap<MotorizedAircraft>
    {
        public MotorizedAircraftMap()
        {
            Table("Motorflugzeug");
            Id(x => x.Id, "id");
            Map(x => x.EnginePower, "leistung");
            HasOne(x => x.Aircraft);
        }
    }
}
