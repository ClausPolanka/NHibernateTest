using Domain;
using FluentNHibernate.Mapping;

namespace DataAccess
{
    public class AircraftMap : ClassMap<Aircraft>
    {
        public AircraftMap()
        {
            Table("Flugzeug");
            Id(x => x.Id, "id");
            Map(x => x.Name, "name");
            Map(x => x.Manufacturer, "hersteller");
            Map(x => x.Model, "modell");
            Map(x => x.Wingspan, "spannweite");

            DiscriminateSubClassesOnColumn("id");
        }
    }
}
