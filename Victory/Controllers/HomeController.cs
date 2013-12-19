using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Victory.Controllers
{
    public class HomeController : Controller
    {
        Dao.ShopDao _shopDao = new Dao.ShopDao(MvcApplication.NHibernateSessionFactory.GetCurrentSession());
        public JsonResult Index()
        {
            
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            /*_shopDao.CreateShop(new NHibernateMappings.Shop
            {

                shopName = "" + new DateTime().ToShortDateString()
            });
            List<NHibernateMappings.Shop> list = _shopDao.QueryShop();
            foreach (NHibernateMappings.Shop test in list)
            {
                ViewBag.Message += test.shopName;
            }
            */
            return Json(new { success = true, msg = ViewBag.Message }, JsonRequestBehavior.AllowGet); 
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
