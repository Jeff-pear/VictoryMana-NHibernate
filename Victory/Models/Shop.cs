using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Victory.Models
{
    public class Shop
    {
        
        
        public int shopId { set; get; }
        
        public string shopName { set; get; }
        
        public int userId { set; get; }
        
        public string username { set; get; }
        
        public int typeId { set; get; }
        
        public string typeIds { set; get; }
        
        public string address { set; get; }
        
        public int areaId { set; get; }
        
        public string status { set; get; }
        
        public DateTime addtime { set; get; }
        
        public DateTime opentime { set; get; }
        
        public int adduser { set; get; }       
        
        public DateTime modifytime { set; get; }
        
        public int modifyuser { set; get; } 
        
        public string introduction { set; get; }
        
        public string weixincode { set; get; }
        
        public double squre { set; get; }
        
        public string representproducts { set; get; }
        
        public string pricerange { set; get; }
        
        public string favorabledeals1 { set; get; }
        
        public string favorabledeals2 { set; get; }
        
        public string favorabledeals3 { set; get; }
        
        public string favorabledeals4 { set; get; }
        
        public string favorabledeals5 { set; get; }
        
        public string piclist { set; get; }
        
        public string picdetail { set; get; }
        
        public string piclogo { set; get; }
        
        public string picback1 { set; get; }
        
        public string picback2 { set; get; }
        
        public int attention { set; get; }
        
 
       
    }
}