using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetsManagment.Models
{
    public class StockGroup
    {
     public void SaveTourViewModel()
        {
            data = new List<Group>();
        }


        public double SL1 { get; set; }
        public string NAME2 { get; set; }
        public string PARENT2 { get; set; }

         public List<Group> data { get; set; }
    }


    public class Group {
        public double SERIAL { get; set; }
        public string NAME { get; set; }
        public string PARENT { get; set; }
        public string ONE_DOWN { get; set; }
        public string PRIMARY { get; set; }
        public string NAME_DEFAULT { get; set; }
        public double OPENING_BALANCE { get; set; }
        public double CLOSING_BALANCE { get; set; }
        public double INWARDQUANTITY { get; set; }
        public double OUTWARDQUANTITY { get; set; }
        public double OPENING_VALUE { get; set; }
        public double DEBIT_CLOSING_BAL { get; set; }
        public int LEVEL { get; set; }
        public int SEQUENCES { get; set; }
        public int PRIMARY_TYPE { get; set; }
        public int SECONDARY_TYPE { get; set; }
        public int DEFAULT { get; set; }
        public int USE_PACK_SIZE { get; set; }
        public string GR_NAME { get; set; }
        public DateTime INSERT_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public int G_STATUS { get; set; }
        public int FG_STATUS { get; set; }
        public int DIL_EFFECT_INF { get; set; }
    
    }

}