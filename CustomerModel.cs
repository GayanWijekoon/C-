using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVCWEF.Models
{
    public class CustomerModel
    {
        public int id { get; set; }


        [DisplayName("Customer Name")]
        public String name { get; set; }


        [DisplayName("Customer Description")]
        public String description { get; set; }

        public bool RememberMe { get; set; }
    }
}