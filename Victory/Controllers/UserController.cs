using System.Collections.Generic;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using NHibernateMappings;
using System;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace Victory.Controllers
{
    public class UserController : Controller
    {
        private OleDbConnection buildConn()
        {
            OleDbConnectionStringBuilder connectStringBuilder = new OleDbConnectionStringBuilder();
            connectStringBuilder.DataSource = System.Web.Configuration.WebConfigurationManager.AppSettings["accessFileLocation"].ToString();
            connectStringBuilder.Provider = System.Web.Configuration.WebConfigurationManager.AppSettings["provider"].ToString();
            OleDbConnection cn = new OleDbConnection(connectStringBuilder.ConnectionString);
            return cn;
        }
        public Boolean isNullSession() {
            string u = Session["user"] as string;
            if (string.IsNullOrEmpty(u))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*
         * 登陆*/
        public JsonResult Login(string username, string password)
        {


            OleDbConnection mycn = buildConn();
            mycn.Open();
            string condition = "";
            if (string.IsNullOrEmpty(username))
            {
                return Json(new { success = false, msg = "用户名未登陆" }, JsonRequestBehavior.AllowGet);
            }
            else {
                condition += " and username = '" + username + "'";
            }
            if (string.IsNullOrEmpty(password))
            {
                return Json(new { success = false, msg = "密码为空" }, JsonRequestBehavior.AllowGet);
            }
            else {
                condition += " and password = '" + password + "'";
            }
            OleDbCommand mycm = null;
            OleDbDataReader msdr = null;
            try
            {
                string sql = "select * from sys_user where 1=1 " + condition;
                mycm = new OleDbCommand(sql, mycn);
                msdr = mycm.ExecuteReader();

                if (msdr.HasRows)
                {
                    msdr.Close();
                    mycn.Close();
                    Session["user"] = username;
                    string session = Session["user"] as string;
                    return Json(new { success = true, msg = session }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    msdr.Close();
                    mycn.Close();
                    return Json(new { success = false, msg = "用户不存在" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                msdr.Close();
                mycn.Close();
                return Json(new { success = false, msg = "未知错误" }, JsonRequestBehavior.AllowGet);
            }
            finally {
                msdr.Close();
                mycn.Close();
            }
        }
        /*
         * 用户列表*/
        public JsonResult UserListGet(string username, string cnName, string type, string email)
        {
            /*
            if (isNullSession())
            {
                return Json(new { data = "Session为空，请重新登陆", success = false }, JsonRequestBehavior.AllowGet);
            }
             * */

            OleDbConnection mycn = buildConn();
            mycn.Open();
            string condition = "";
            if (!string.IsNullOrEmpty(username))
            {
                condition += " and username = '" + username + "'";
            }
            if (!string.IsNullOrEmpty(type))
            {
                condition += " and type = '" + type + "'";
            }
            if (!string.IsNullOrEmpty(email))
            {
                condition += " and email = '" + email + "'";
            }
            if (!string.IsNullOrEmpty(cnName))
            {
                condition += " and cn_name = '" + cnName + "'";
            }
            string sql = "select * from sys_user where 1=1 " + condition;
            OleDbCommand mycm = new OleDbCommand(sql, mycn);
            OleDbDataReader msdr = mycm.ExecuteReader();
            List<User> list = new List<User>();
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
                    User u = new User();
                    u.id = Convert.ToInt32(msdr["id"]);
                    u.username = Convert.ToString( msdr["username"]);
                    u.password = Convert.ToString(msdr["password"]);
                    u.cnName = Convert.ToString(msdr["cn_name"]);
                    u.sex = Convert.ToString(msdr["sex"]); 
                    u.email = Convert.ToString(msdr["email"]);
                    u.mobilephone = Convert.ToString(msdr["mobilephone"]);


                    u.isEnabled = Convert.ToString(msdr["is_enabled"]);
                    //u.createDate = DateTime.Parse(Convert.ToString(msdr["create_date"]));
                    if (Convert.ToString(msdr["create_date"]).Length > 0) {
                            u.createDate = DateTime.ParseExact(Convert.ToString(msdr["create_date"]), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    u.status = Convert.ToString(msdr["status"]);
                    //u.picUrl = msdr.GetString(12);
                    //u.dimensionalCodeUrl = msdr.GetString(13);
                    u.type = Convert.ToString(msdr["type"]);
                    list.Add(u);
                }
            }
            msdr.Close();
            mycn.Close();
            return Json(new { data = list, success = true, msg = Session["user"] }, JsonRequestBehavior.AllowGet);
        }
        /*
         * 添加用户*/
        public JsonResult UserAdd(string username, string password, string cnName, string type, string email, string mobilephone)
        {


            OleDbConnection mycn = buildConn();
            mycn.Open();
            string field = "";
            string value = "";
            string sql = "INSERT INTO sys_user ( [username] ,[password],[cn_name],[type],[email],[mobilephone]) VALUES (@username,@password,@cn_name,@type,@email,@mobilephone) ";
            OleDbCommand mycm = new OleDbCommand(sql,mycn);
            
            
                field += " username,";
                value += "@password ,";
                mycm.Parameters.AddWithValue("@username", username);
                //mycm.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;

                field += " cn_name,";
                value += "@cn_name ,";
                mycm.Parameters.AddWithValue("@cn_name", cnName);

                field += " password,";
                value += "@password,";
                mycm.Parameters.AddWithValue("@password", password);
                //mycm.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
            
           
                field += "type,";
                value += "@type,";
                mycm.Parameters.AddWithValue("@type", type);
                //mycm.Parameters.Add("@type", SqlDbType.NVarChar).Value = type;
           
           
                field += " email,";
                value += "@email,";
                mycm.Parameters.AddWithValue("@email", email);
                //mycm.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
            
           
                field += "mobilephone,";
                value += "@mobilephone,";
                mycm.Parameters.AddWithValue("@mobilephone", mobilephone);
                //mycm.Parameters.Add("@mobilephone", SqlDbType.NVarChar).Value = mobilephone;


                
                
            
            
            
            
            mycm.ExecuteNonQuery();
            mycn.Close();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
          
        }
        /*
         * 用户验证
         * */
        public JsonResult LoginValidate(string userName) {

            OleDbConnection mycn = buildConn();
            mycn.Open();
            string condition = " and username = '" + userName + "'";
            OleDbCommand mycm = null;
            OleDbDataReader msdr = null;
            if (string.IsNullOrEmpty(userName)) {
                return Json(new { success = false, msg = "用户未登陆" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                string sql = "select * from sys_user where 1=1 " + condition;
                mycm = new OleDbCommand(sql, mycn);
                msdr = mycm.ExecuteReader();

                if (msdr.HasRows)
                {
                    while (msdr.Read())
                    {
                        if ((string)Session["user"] != (string)msdr["username"])
                        {
                            msdr.Close();
                            mycn.Close();
                            return Json(new { success = false, msg = "用户未登陆" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            User u = new User();
                            u.id = (int)msdr["id"];
                            u.username = (string)msdr["username"];
                            u.password = (string)msdr["password"];
                            u.cnName = (string)msdr["cn_name"];
                            u.status = (string)msdr["status"];
                            u.picUrl = (string)msdr["pic_url"];
                            u.dimensionalCodeUrl = (string)msdr["dimensional_code_url"];
                            u.type = msdr.GetString(12);
                            msdr.Close();
                            mycn.Close();
                            return Json(new { result = u, success = true, msg = "成功" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    msdr.Close();
                    mycn.Close();
                    return Json(new { success = false, msg = "用户不存在" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                msdr.Close();
                mycn.Close();
                return Json(new { success = false, msg = "未知错误" }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                msdr.Close();
                mycn.Close();
            }

            return Json(new { success = true,msg = "未知错误"  }, JsonRequestBehavior.AllowGet);
        }
        /*
          * 用户删除
          * */
        public JsonResult UserDel(string id)
        {


            OleDbConnection mycn = buildConn();
            mycn.Open();
            OleDbCommand mycm = new OleDbCommand();
            mycm.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = id;
            mycm.Connection = mycn;
            string sql = "DELETE FROM sys_user WHERE id = @id";
            mycm.CommandText = sql;
            mycm.ExecuteNonQuery();

            sql = "DELETE FROM shop_type WHERE shop_id = @id";
            mycm.CommandText = sql;
            mycm.ExecuteNonQuery();

            sql = "DELETE FROM recommend_shop WHERE shopid = @id";
            mycm.CommandText = sql;
            mycm.ExecuteNonQuery();

            mycn.Close();

            return Json(new { success = true, sql = sql }, JsonRequestBehavior.AllowGet);

        }
    }

}
