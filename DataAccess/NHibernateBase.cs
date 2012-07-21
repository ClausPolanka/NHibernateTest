using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace DataAccess
{
    public class NHibernateBase
    {
        private static ISessionFactory sessionFactory;

        protected static ISessionFactory SessionFactory { get; set; }

        public static ISessionFactory BuildSessionFactory()
        {
            if (sessionFactory == null)
            {
                sessionFactory = Fluently.Configure().Database(
                    MsSqlConfiguration.MsSql2008.ConnectionString(
                        @"Data Source=WIN7-VIAO-NB\SAGENIUZ;Initial Catalog=dm_SS_2008;Integrated Security=True")).
                    Mappings(m => m.FluentMappings.AddFromAssemblyOf<AircraftMap>())
                    .BuildSessionFactory();
            }
            return sessionFactory;
        }
    }
}