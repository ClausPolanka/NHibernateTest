using Domain;
using FluentNHibernate.Mapping;

namespace DataAccess
{
    public class TeacherMap : ClassMap<Teacher>
    {
        public TeacherMap()
        {
            Table("Lehrer");

            Id(x => x.Number).Column("nummer");
            
            Map(x => x.Experience).Column("erfahrung");
            
            HasMany(x => x.FlightUnits)
                .KeyColumn("lnummer")
                .Not.LazyLoad();
        }
    }
}
