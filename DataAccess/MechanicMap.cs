using Domain;
using FluentNHibernate.Mapping;

namespace DataAccess
{
    public class MechanicMap : ClassMap<Mechanic>
    {
        public MechanicMap()
        {
            Table("Mechaniker");
            
            Id(x => x.Number).Column("nummer");
            
            HasManyToMany(x => x.Hangars)
                .Table("verwaltet")
                .ParentKeyColumn("nummer")
                .ChildKeyColumn("name")
                .FetchType.Join();
        }
    }
}
