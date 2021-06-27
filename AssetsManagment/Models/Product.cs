using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetsManagment.Models
{
    public class Product
    {
        public int prod_id { get; set; }
        public string prod_name { get; set; }
        public int prod_cId { get; set; }
        public string prod_unit { get; set; }
        public int prod_op_qty { get; set; }
        public byte cat_status { get; set; }
        public int prod_catId { get; set; }

        public virtual Category category { get; set; }
    }
}