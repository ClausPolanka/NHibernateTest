using Domain;
using FluentNHibernate.Mapping;

namespace DataAccess
{
    public class FlightUnitMap : ClassMap<FlightUnit>
    {
        public FlightUnitMap()
        {
            Table("Flugeinheit");
            
            CompositeId()
                .KeyProperty(x => x.StudentNumber, "snummer")
                .KeyProperty(x => x.Date, "datum");

            Map(x => x.Duration).Column("dauer");

            References(x => x.Teacher).Column("lnummer");
        }
    }
}
