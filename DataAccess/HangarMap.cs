using Domain;
using FluentNHibernate.Mapping;

namespace DataAccess
{
    public class HangarMap : ClassMap<Hangar>
    {
        public HangarMap()
        {
            Id(x => x.Name).Column("name");
            
            HasManyToMany(x => x.Mechanics)
                .Table("verwaltet")
                .ParentKeyColumn("name")
                .ChildKeyColumn("nummer")
                .Inverse();

            HasManyToMany(x => x.Aircrafts)
                .Table("abgestellt")
                .ParentKeyColumn("name")
                .ChildKeyColumn("id")
                .FetchType.Join();
        }
    }
}
