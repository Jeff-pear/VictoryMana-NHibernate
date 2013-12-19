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

namespace NHibernateMappings
{
    public partial class Shop
    {
          
        public virtual int shopId { set; get; }
        
        public virtual string shopName { set; get; }

        public virtual int userId { set; get; }
        
        public virtual int typeId { set; get; }
        
        public virtual string typeIds { set; get; }
        
        public virtual string address { set; get; }
        
        public virtual int areaId { set; get; }
        
        public virtual string status { set; get; }
        
        public virtual string addtime { set; get; }
        
        public virtual string opentime { set; get; }
        
        public virtual int adduser { set; get; }       
        
        public virtual string modifytime { set; get; }
        
        public virtual int modifyuser { set; get; }
         
        public virtual string tel { set; get; }

        public virtual string email { set; get; }

        public virtual string introduction { set; get; }
        
        public virtual string weixincode { set; get; }
        
        public virtual string representproducts { set; get; }
        
        public virtual string pricerange { set; get; }
        
        public virtual string favorabledeals1 { set; get; }
        
        public virtual string favorabledeals2 { set; get; }
        
        public virtual string favorabledeals3 { set; get; }
        
        public virtual string favorabledeals4 { set; get; }
        
        public virtual string favorabledeals5 { set; get; }
        
        public virtual string piclist { set; get; }

        public virtual double squre { set; get; }
        
        public virtual string picdetail { set; get; }
        
        public virtual string piclogo { set; get; }
        
        public virtual string picback1 { set; get; }
        
        public virtual string picback2 { set; get; }
        
        public virtual int attention { set; get; }
                 
    }
    public partial class User
    {

        public virtual int id { set; get; }

        public virtual string username { set; get; }

        public virtual string password { set; get; }

        public virtual string encode_password { set; get; }

        public virtual string cnName { set; get; }

        public virtual string email { set; get; }

        public virtual string sex { set; get; }

        public virtual string mobilephone { set; get; }

        public virtual string isEnabled { set; get; }

        public virtual DateTime createDate { set; get; }

        public virtual string status { set; get; }

        public virtual string level { set; get; }

        public virtual string picUrl { set; get; }

        public virtual string dimensionalCodeUrl { set; get; }

        public virtual string type { set; get; }

        public virtual string notes { set; get; }


    }
}