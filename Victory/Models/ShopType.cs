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
    public class ShopType
    {
        
        
        public int typeId { set; get; }
        
        public string typeName { set; get; }
        
        public int fatherId { set; get; }
        
        public string note { set; get; }
        
    }
}