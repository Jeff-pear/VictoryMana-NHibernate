using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Victory
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static ISessionFactory NHibernateSessionFactory;
        public override void Init()
        {
            base.Init();
            this.BeginRequest += MvcApplication_BeginRequest;
            this.EndRequest += MvcApplication_EndRequest;
        }

        private void MvcApplication_EndRequest(object sender, EventArgs e)
        {
            var session = CurrentSessionContext.Unbind(NHibernateSessionFactory);
            session.Dispose();
        }

        private void MvcApplication_BeginRequest(object sender, EventArgs e)
        {
            var session = NHibernateSessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            NHibernateSessionFactory = Configuration.NHibernateHelper.GetNHibernateSessionFactory();
        }
    }
}