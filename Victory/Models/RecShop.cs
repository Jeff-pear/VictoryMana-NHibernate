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

namespace Victory.Models
{
    public class RecShop
    {
        
        public int shopId { set; get; }
        
        public string shopName { set; get; }
        
        public string type { set; get; }
        
        public string introduction { set; get; }
        
        public string picurl { set; get; }
        
        public int attention { set; get; }
    }
}