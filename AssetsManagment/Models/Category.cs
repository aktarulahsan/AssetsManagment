using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetsManagment.Models
{
    public class Category
    {


        public void SaveTourViewModels()
        {
            data = new List<StockCategory>();
        }


        public List<StockCategory> data { get; set; }

        
    }


    public class StockCategory
    {
        public double SERIAL { get; set; }
        public string CategoryName { get; set; }
        public string CategoryUnder { get; set; }
        public long lngslNo { get; set; }
        public string strPrimary { get; set; }
        public string strDefaultFroup { get; set; }
    }
}




    