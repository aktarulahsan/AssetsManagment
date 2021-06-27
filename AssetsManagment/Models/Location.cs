using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetsManagment.Models
{

    public class LocationResponse
    {


        public void InitialList()
        {
            data = new List<Location>();
        }


        public List<Location> data { get; set; }


    }



    public class Location
    {


        public int SERIAL { get; set; }
        public long lngSlNo { get; set; }
        public string strLocation { get; set; }
        public string strUnder { get; set; }
        public string strBranch { get; set; }
        public string strAddres1 { get; set; }
        public string strAddres2 { get; set; }
        public string strCity { get; set; }
        public string strPhone { get; set; }
        public string strFax { get; set; }
        public string strParentGroup { get; set; }
      
        public int lngDefault { get; set; }
        public int intSection { get; set; }



    }
}