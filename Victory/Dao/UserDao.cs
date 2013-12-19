using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Victory.Dao
{
    public class UserDao
    {
        private readonly ISession _nhibernateSession;
        public UserDao(ISession nhibernateSession) { this._nhibernateSession = nhibernateSession; }
        public NHibernateMappings.User CreateUser(NHibernateMappings.User user)
        {
            using (ITransaction tran = _nhibernateSession.BeginTransaction()) {
                _nhibernateSession.Save(user);
                tran.Commit();
                return user;
            }
        }
        public List<NHibernateMappings.User> QueryUser()
        {
            using (ITransaction tran = _nhibernateSession.BeginTransaction())
            {

                ISQLQuery query = _nhibernateSession.CreateSQLQuery("select * from user c").AddEntity("NHibernateMappings.User");

                IList<NHibernateMappings.User> c = query.List<NHibernateMappings.User>();
                return c.ToList();
            }
        }
    }
}