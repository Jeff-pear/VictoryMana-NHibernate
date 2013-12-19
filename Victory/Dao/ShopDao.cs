using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Victory.Dao
{
    public class ShopDao
    {
        private readonly ISession _nhibernateSession;
        public ShopDao(ISession nhibernateSession) { this._nhibernateSession = nhibernateSession; }
        public NHibernateMappings.Shop CreateShop(NHibernateMappings.Shop shop) {
            using (ITransaction tran = _nhibernateSession.BeginTransaction()) {
                _nhibernateSession.Save(shop);
                tran.Commit();
                return shop;
            }
        }
        public List<NHibernateMappings.Shop> QueryShop()
        {
            using (ITransaction tran = _nhibernateSession.BeginTransaction())
            {

                //var query = _nhibernateSession.QueryOver<NHibernateMappings.Test>().Where(p => shop.testname == "李永京").List();
                //IQuery query = _nhibernateSession.GetNamedQuery("GetDataSetTest");
                //query.List();


                ISQLQuery query = _nhibernateSession.CreateSQLQuery("select * from shop c").AddEntity("NHibernateMappings.Shop");

                IList<NHibernateMappings.Shop> c = query.List<NHibernateMappings.Shop>();
                return c.ToList();
            }
        }
    }
}