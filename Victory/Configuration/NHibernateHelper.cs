using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Victory.Configuration
{
    public static class NHibernateHelper
    {
        public static ISessionFactory GetNHibernateSessionFactory()
        {
            var configure = new NHibernate.Cfg.Configuration();
            configure.DataBaseIntegration(delegate(NHibernate.Cfg.Loquacious.IDbIntegrationConfigurationProperties dbi) {
                dbi.ConnectionStringName = "mvcWithNHibernate";
                dbi.Dialect<NHibernate.Dialect.MySQLDialect>();
                dbi.Driver<NHibernate.Driver.MySqlDataDriver>();
                dbi.Timeout = 255;
            });
            configure.CurrentSessionContext<NHibernate.Context.WebSessionContext>();
            //configure.AddAssembly(typeof(NHibernateMappings.User).Assembly);
            configure.AddAssembly(typeof(NHibernateMappings.Shop).Assembly);
            return configure.BuildSessionFactory();
        }
    }
}