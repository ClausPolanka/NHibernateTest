using System;
using NHibernate;

namespace DataAccess
{
    public class TranactionContext
    {
        public static T Execute<T>(ISessionFactory sf, Func<ISession, T> block)
        {
            using (ISession s = sf.OpenSession())
            using (ITransaction tx = s.BeginTransaction())
            {
                try
                {
                    T t = block(s);
                    tx.Commit();
                    return t;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
        
        public static void Execute(ISessionFactory sf, Action<ISession> block)
        {
            using (ISession s = sf.OpenSession())
            using (ITransaction tx = s.BeginTransaction())
            {
                try
                {
                    block(s);
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}