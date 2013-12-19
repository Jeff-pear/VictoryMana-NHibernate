using System.Collections.Generic;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using Victory.Models;
using System;
using System.IO;
using System.Data.OleDb;
namespace Victory.Controllers
{
    public class ShopController : Controller
    {
        Dao.ShopDao _shopDao = new Dao.ShopDao(MvcApplication.NHibernateSessionFactory.GetCurrentSession());
        private OleDbConnection buildConn()
        {

            OleDbConnectionStringBuilder connectStringBuilder = new OleDbConnectionStringBuilder();
            connectStringBuilder.DataSource = System.Web.Configuration.WebConfigurationManager.AppSettings["accessFileLocation"].ToString();
            connectStringBuilder.Provider = System.Web.Configuration.WebConfigurationManager.AppSettings["provider"].ToString();
            OleDbConnection cn = new OleDbConnection(connectStringBuilder.ConnectionString);
            return cn;

        }

        /*
         * 店铺列表
         * */
        public JsonResult ShopListGet(string shopName, string userId, string condition, string start, string limit)
        {


            OleDbConnection mycn = buildConn();
            mycn.Open();
            string sqlcondition = "";
            if (!string.IsNullOrEmpty(shopName))
            {
                sqlcondition += " and s.shopname like '%" + shopName + "%'";
            }
            if (!string.IsNullOrEmpty(condition))
            {
                sqlcondition += " and s.shopname like '%" + condition + "%'";
            }
            if (string.IsNullOrEmpty(start))
            {
                start = "0";
            }
            if (string.IsNullOrEmpty(limit))
            {
                limit = "10";
            }
            int curPage = (Convert.ToInt16(start) / 10);
            int pag = curPage * Convert.ToInt16(limit);
            string sql = "";
            if (start == "0")
            {
                sql = "select top " + Convert.ToInt16(limit) + " s.*,u.username from shop s,sys_user u where s.userid = u.id " + sqlcondition + " order by s.shopid desc";//+ "order by shopid desc LIMIT " + start + ", " + limit + "";
            }
            else
            {
                sql = "select top " + Convert.ToInt16(limit) + " s.*,u.username from shop s,sys_user u where s.userid = u.id " + sqlcondition + " and s.shopid not in (select top " + pag + " shopid from shop where 1=1 " + sqlcondition + " ) order by s.shopid desc";//+ "order by shopid desc LIMIT " + start + ", " + limit + "";
            }

            //string sql = "select s.*,u.username from shop s,sys_user u where 1=1 and s.userid = u.id " + sqlcondition;// +" LIMIT " + start + ", " + limit + "";
            OleDbCommand mycm = new OleDbCommand(sql, mycn);
            OleDbDataReader msdr = mycm.ExecuteReader();
            List<Shop> list = new List<Shop>();
            while (msdr.Read())
            {
                if (msdr.HasRows)
                {

                    string[] readstring = new string[msdr.FieldCount];
                    for (int i = 0; i < msdr.FieldCount; i++)
                    {
                        if (msdr.IsDBNull(i))
                        {
                            continue;
                        }
                    }

                    Shop u = new Shop();
                    u.shopId = (int)msdr["shopid"];
                    u.shopName = Convert.ToString(msdr["shopname"]);
                    u.userId = (int)msdr["userid"];
                    u.typeId = (int)msdr["typeid"];
                    u.address = Convert.ToString(msdr["address"]);
                    u.areaId = (int)msdr["areaid"];
                    if (!string.IsNullOrEmpty((string)msdr["addtime"]))
                    {
                        u.addtime = Convert.ToDateTime((string)msdr["addtime"]);
                    }
                    if (!string.IsNullOrEmpty((string)msdr["opentime"]))
                    {
                        u.opentime = Convert.ToDateTime((string)msdr["opentime"]);
                    }
                    if (!string.IsNullOrEmpty((string)msdr["modifytime"]))
                    {
                        u.modifytime = Convert.ToDateTime((string)msdr["modifytime"]);
                    }

                    u.introduction = Convert.ToString(msdr["introduction"]);
                    u.status = Convert.ToString(msdr["status"]);
                    u.username = (string)msdr["username"];
                    u.piclist = (string)msdr["piclist"];
                    u.picdetail = Convert.ToString(msdr["picdetail"]);
                    u.piclogo = Convert.ToString(msdr["piclogo"]);
                    u.favorabledeals1 = Convert.ToString(msdr["favorabledeals1"]);
                    u.favorabledeals2 = Convert.ToString(msdr["favorabledeals2"]);
                    u.favorabledeals3 = Convert.ToString(msdr["favorabledeals3"]);
                    u.favorabledeals4 = Convert.ToString(msdr["favorabledeals4"]);
                    u.favorabledeals5 = Convert.ToString(msdr["favorabledeals5"]);
                    //SELECT * FROM post WHERE FIND_IN_SET('123', tags)
                    u.typeIds = Convert.ToString(msdr["type_ids"]);
                    list.Add(u);
                }
            }
            msdr.Close();
            mycn.Close();
            return Json(new { data = list, success = true }, JsonRequestBehavior.AllowGet);
        }
        /*
         * 店铺添加 
         * */
        public JsonResult ShopAdd(string shopname, string address, string userid, string typeid, string introduction, string tel, string email)
        {


            OleDbConnection mycn = buildConn();
            mycn.Open();
            OleDbCommand mycm = new OleDbCommand();
            string field = "";
            string value = "";

            if (Request.Files.Count != 0)
            {
                string uploadsFolder = HttpContext.Server.MapPath("~/Upload");
                Guid identifier = Guid.NewGuid();
                var uploadsPath = Path.Combine(uploadsFolder, identifier.ToString());
                var httpfile = Request.Files["shopdetailpic"];
                string Extension = Path.GetExtension(httpfile.FileName);
                if (httpfile.ContentLength > 0)
                {
                    httpfile.SaveAs(uploadsPath + Extension);
                    if (!string.IsNullOrEmpty(httpfile.FileName))
                    {
                        field += " picdetail,";
                        value += "@picdetail,";

                        mycm.Parameters.Add("@picdetail", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "/Upload/" + identifier.ToString() + Extension;
                    }
                }

                Guid identifier1 = Guid.NewGuid();
                var uploadsPath1 = Path.Combine(uploadsFolder, identifier1.ToString());
                var httpfile1 = Request.Files["shoplistpic"];
                string Extension1 = Path.GetExtension(httpfile1.FileName);
                if (httpfile1.ContentLength > 0)
                {
                    httpfile1.SaveAs(uploadsPath1 + Extension1);
                    if (!string.IsNullOrEmpty(httpfile1.FileName))
                    {
                        field += " piclist,";
                        value += "@piclist,";
                        mycm.Parameters.Add("@piclist", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "/Upload/" + identifier1.ToString() + Extension1;
                    }
                }


                Guid identifier2 = Guid.NewGuid();
                var uploadsPath2 = Path.Combine(uploadsFolder, identifier2.ToString());
                var httpfile2 = Request.Files["shoplogopic"];
                string Extension2 = Path.GetExtension(httpfile2.FileName);
                if (httpfile2.ContentLength > 0)
                {
                    httpfile2.SaveAs(uploadsPath2 + Extension2);
                    if (!string.IsNullOrEmpty(httpfile2.FileName))
                    {
                        field += " piclogo,";
                        value += "@piclogo,";
                        mycm.Parameters.Add("@piclogo", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "/Upload/" + identifier.ToString() + Extension2;
                    }
                }

            }

            if (!string.IsNullOrEmpty(shopname))
            {
                field += " shopname,";
                value += "@shopname,";
                mycm.Parameters.Add("@shopname", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = shopname;
            }
            if (!string.IsNullOrEmpty(userid))
            {
                field += " userid,";
                value += "@userid,";
                mycm.Parameters.Add("@userid", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = userid;
            }

            if (!string.IsNullOrEmpty(tel))
            {
                field += "tel,";
                value += "@tel,";
                mycm.Parameters.Add("@tel", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = tel;
            }
            if (!string.IsNullOrEmpty(email))
            {
                field += "email,";
                value += "@email,";
                mycm.Parameters.Add("@email", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = email;
            }
            if (!string.IsNullOrEmpty(introduction))
            {
                field += "introduction,";
                value += "@introduction,";
                mycm.Parameters.Add("@introduction", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = introduction;
            }
            if (!string.IsNullOrEmpty(address))
            {
                field += " address,";
                value += "@address,";
                mycm.Parameters.Add("@address", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = address;
            }


            field = field.Substring(0, field.Length - 1);
            value = value.Substring(0, value.Length - 1);

            mycm.Connection = mycn;
            string guid = Guid.NewGuid().ToString();


            string sql = "INSERT INTO shop (" + field + ",addtime,opentime,modifytime) VALUES(" + value + ",now(),now(),now()) ";
            mycm.CommandText = sql;

            object result = mycm.ExecuteScalar();
            sql = "SELECT @@identity as newId";
            mycm.CommandText = sql;
            OleDbDataReader msdr = mycm.ExecuteReader();
            int shopId = 0;
            while (msdr.Read())
            {
                if (msdr.HasRows)
                {
                    string[] readstring = new string[msdr.FieldCount];
                    for (int i = 0; i < msdr.FieldCount; i++)
                    {
                        if (msdr.IsDBNull(i))
                        {
                            continue;
                        }
                    }
                    shopId = Convert.ToInt16(msdr["newId"]);

                }
            }
            msdr.Close();
            if (shopId != 0)
            {
                sql = "INSERT INTO shop_type (shop_id,type_id) VALUES(" + shopId + "," + typeid + ") ";
                mycm.CommandText = sql;
                int result2 = mycm.ExecuteNonQuery();
            }

            mycn.Close();


            return Json(new { success = true, exec1 = result, msg = "成功" }, JsonRequestBehavior.AllowGet);

        }
        /*
         * 推荐商家 后台用
         * */
        public JsonResult RecShopListGet(string shopId)
        {

            OleDbConnection mycn = buildConn();
            mycn.Open();
            string condition = "";
            if (!string.IsNullOrEmpty(shopId))
            {
                condition += " and s.shopid= " + shopId + "";
            }

            string sql = "select s.* from shop s,recommend_shop r where 1=1 and r.shopid = s.shopid " + condition;
            OleDbCommand mycm = new OleDbCommand(sql, mycn);
            OleDbDataReader msdr = mycm.ExecuteReader();
            List<RecShop> list = new List<RecShop>();
            while (msdr.Read())
            {
                if (msdr.HasRows)
                {
                    string[] readstring = new string[msdr.FieldCount];
                    for (int i = 0; i < msdr.FieldCount; i++)
                    {
                        if (msdr.IsDBNull(i))
                        {
                            continue;
                        }
                    }
                    RecShop u = new RecShop();
                    u.shopId = (int)msdr["shopid"];
                    u.shopName = msdr.GetString(1);
                    u.introduction = msdr.GetString(2);
                    u.picurl = msdr.GetString(3);
                    u.attention = (int)msdr["attention"];

                    list.Add(u);
                }
            }
            msdr.Close();
            mycn.Close();
            return Json(new { data = list, success = true }, JsonRequestBehavior.AllowGet);
        }
        /*
         * 1会员商家 2新店推荐 3品牌推荐 4胜利特价
         * 店铺列表
         * */
        public JsonResult ShopMidListGet(string type, string start, string limit)
        {

            OleDbConnection mycn = buildConn();
            mycn.Open();
            string condition = "";
            if (!string.IsNullOrEmpty(type))
            {
                condition += " and r.type= '" + type + "'";
            }

            int curPage = (Convert.ToInt16(start) / 10);
            int pag = curPage * Convert.ToInt16(limit);
            string sql = "";
            if (start == "0")
            {
                sql = "select top " + Convert.ToInt16(limit) + " s.*,r.type from shop s,recommend_shop r where 1=1 and r.shopid = s.shopid " + condition + " order by s.shopid desc ";
            }
            else
            {
                sql = "select top " + Convert.ToInt16(limit) + " s.*,r.type from shop s,recommend_shop r where 1=1 and r.shopid = s.shopid " + condition + " and s.shopid not in (select top " + pag + " shopid from shop s,type r where 1=1 and r.shopid= s.shopid " + condition + " ) order by s.shopid desc ";
            }

            OleDbCommand mycm = new OleDbCommand(sql, mycn);
            OleDbDataReader msdr = mycm.ExecuteReader();
            List<RecShop> list = new List<RecShop>();
            while (msdr.Read())
            {
                if (msdr.HasRows)
                {
                    string[] readstring = new string[msdr.FieldCount];
                    for (int i = 0; i < msdr.FieldCount; i++)
                    {
                        if (msdr.IsDBNull(i))
                        {
                            continue;
                        }
                    }
                    RecShop u = new RecShop();
                    u.shopId = Convert.ToInt32(msdr["shopid"]);
                    u.shopName = Convert.ToString(msdr["shopname"]);
                    u.introduction = Convert.ToString(msdr["introduction"]);
                    u.picurl = Convert.ToString(msdr["piclist"]);
                    u.attention = Convert.ToInt32(msdr["attention"]);
                    if (System.DBNull.Value != msdr["type"])
                    {
                        u.type = Convert.ToString(msdr["type"]);
                    }
                    list.Add(u);
                }
            }
            msdr.Close();
            mycn.Close();
            return Json(new { data = list, success = true }, JsonRequestBehavior.AllowGet);
        }
        /*
         * 天天美食、玩乐生活等
         * type为必录项
         */
        public JsonResult ShopNorTypeListGet(string type, string start, string limit)
        {

            OleDbConnection mycn = buildConn();
            mycn.Open();

            if (string.IsNullOrEmpty(type))
            {
                return Json(new { success = false, msg = "传入的type值为空！" }, JsonRequestBehavior.AllowGet);
            }

            string sql = "select s.shopid,s.shopname,s.introduction,s.piclist,s.attention  from shop s where  s.shopid in (select shop_id from shop_type where type_id = " + type + ")";// ) m  left join type f on f.type_id = " + type + "" +" LIMIT " + start + ", " + limit + "";
            OleDbCommand mycm = new OleDbCommand(sql, mycn);
            OleDbDataReader msdr = mycm.ExecuteReader();
            List<RecShop> list = new List<RecShop>();
            while (msdr.Read())
            {
                if (msdr.HasRows)
                {
                    string[] readstring = new string[msdr.FieldCount];
                    for (int i = 0; i < msdr.FieldCount; i++)
                    {
                        if (msdr.IsDBNull(i))
                        {
                            continue;
                        }
                    }
                    RecShop u = new RecShop();
                    u.shopId = Convert.ToInt32(msdr["shopid"]);
                    u.shopName = Convert.ToString(msdr["shopname"]);
                    u.introduction = Convert.ToString(msdr["introduction"]);
                    u.picurl = Convert.ToString(msdr["piclist"]);
                    u.attention = Convert.ToInt32(msdr["attention"]);


                    //u.type = Convert.ToString(msdr["type_name"]);

                    list.Add(u);
                }
            }
            msdr.Close();
            mycn.Close();
            return Json(new { data = list, success = true }, JsonRequestBehavior.AllowGet);
        }
        /*
         * 店铺详细
         * */
        public JsonResult ShopDetailsGet(string id)
        {

            OleDbConnection mycn = buildConn();
            mycn.Open();

            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, msg = "传入的id值为空！" }, JsonRequestBehavior.AllowGet);
            }

            string sql = "select * from shop where 1=1 and shopid = " + id + "";
            OleDbCommand mycm = new OleDbCommand(sql, mycn);
            OleDbDataReader msdr = mycm.ExecuteReader();
            Shop u = new Shop();
            while (msdr.Read())
            {
                if (msdr.HasRows)
                {
                    string[] readstring = new string[msdr.FieldCount];
                    for (int i = 0; i < msdr.FieldCount; i++)
                    {
                        if (msdr.IsDBNull(i))
                        {
                            continue;
                        }
                    }

                    u.shopId = (int)msdr["shopid"];
                    u.shopName = (string)msdr["shopname"];
                    u.userId = (int)msdr["userid"];
                    u.typeId = (int)msdr["typeid"];
                    u.address = (string)msdr["address"];
                    u.areaId = (int)msdr["areaid"];
                    if (!string.IsNullOrEmpty((string)msdr["addtime"]))
                    {
                        u.addtime = Convert.ToDateTime((string)msdr["addtime"]);
                    }
                    if (!string.IsNullOrEmpty((string)msdr["opentime"]))
                    {
                        u.opentime = Convert.ToDateTime((string)msdr["opentime"]);
                    }
                    if (!string.IsNullOrEmpty((string)msdr["modifytime"]))
                    {
                        u.modifytime = Convert.ToDateTime((string)msdr["modifytime"]);
                    }

                    u.introduction = Convert.ToString(msdr["introduction"]);
                    u.status = Convert.ToString(msdr["status"]);
                    u.piclist = Convert.ToString(msdr["piclist"]);
                    u.picdetail = (string)msdr["picdetail"];
                    u.piclogo = (string)msdr["piclogo"];
                    u.favorabledeals1 = Convert.ToString(msdr["favorabledeals1"]);
                    u.favorabledeals2 = Convert.ToString(msdr["favorabledeals2"]);
                    u.favorabledeals3 = Convert.ToString(msdr["favorabledeals3"]);
                    u.favorabledeals4 = Convert.ToString(msdr["favorabledeals4"]);
                    u.favorabledeals5 = Convert.ToString(msdr["favorabledeals5"]);

                }
            }
            msdr.Close();
            mycn.Close();
            return Json(new { data = u, success = true }, JsonRequestBehavior.AllowGet);
        }
        /*
         * 我的收藏
         * */
        public JsonResult ShopFavorListGet(string userName, string start, string limit)
        {


            OleDbConnection mycn = buildConn();
            mycn.Open();
            string condition = "";
            if (!string.IsNullOrEmpty(userName))
            {
                condition += " and u.username= '" + userName + "'";
            }



            string sql = "select s.*,r.*,t.type_id from shop s,favorite r,shop_type t,sys_user u where 1=1 and u.id = r.user_id and t.shop_id = s.shopid and r.shop_id = s.shopid " + condition + " order by s.shopid";
            OleDbCommand mycm = new OleDbCommand(sql, mycn);
            OleDbDataReader msdr = mycm.ExecuteReader();
            List<RecShop> list = new List<RecShop>();
            List<RecShop> listResult = new List<RecShop>();
            while (msdr.Read())
            {
                if (msdr.HasRows)
                {
                    string[] readstring = new string[msdr.FieldCount];
                    for (int i = 0; i < msdr.FieldCount; i++)
                    {
                        if (msdr.IsDBNull(i))
                        {
                            continue;
                        }
                    }
                    RecShop u = new RecShop();
                    u.shopId = Convert.ToInt16(msdr["shopid"]);
                    u.shopName = Convert.ToString(msdr["shopname"]);
                    u.introduction = Convert.ToString(msdr["introduction"]);
                    u.picurl = Convert.ToString(msdr["piclist"]);
                    u.attention = Convert.ToInt16(msdr["attention"]);
                    u.type = Convert.ToString((int)msdr["type_id"]);
                    list.Add(u);
                }
            }
            int s = Convert.ToInt16(start);
            int p = Convert.ToInt16(limit);
            if (p > list.Count - s - (p * (s / p)))
            {
                p = list.Count;
            }
            for (int i = s; i < p; i++)
            {
                listResult.Add(list[i]);
            }
            msdr.Close();
            mycn.Close();
            return Json(new { data = listResult, success = true }, JsonRequestBehavior.AllowGet);
        }
        /**
        * 店铺删除 同时删除关联表
        * */
        public JsonResult ShopDel(string id)
        {


            OleDbConnection mycn = buildConn();
            mycn.Open();
            OleDbCommand mycm = new OleDbCommand();
            mycm.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = id;
            mycm.Connection = mycn;
            string sql = "DELETE FROM shop WHERE shopid = @id";
            mycm.CommandText = sql;
            mycm.ExecuteNonQuery();
            mycn.Close();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }
    }

}
