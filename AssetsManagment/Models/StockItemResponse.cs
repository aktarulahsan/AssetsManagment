using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetsManagment.Models
{
    public class StockItemResponse
    {
        public void InitialList()
        {
            data = new List<StockItem>();
        }

        public List<StockItem> data { get; set; }

       
    }

    public class GLocation{

        public double intQty { get; set; }
        public double dblBranchRate { get; set; }
        public double dblBranchAmnout { get; set; }
        public string strBatch { get; set; }
        public string strBranchName { get; set; }
    
    
    }


    public class StockItem
    {
        public string strInOUT { get; set; }
        
        public string strItemName { get; set; }
        public string strItemNameBangla { get; set; }
        public string strItemcode { get; set; }
        public string strIOldtemcode { get; set; }
        public string strItemDescription { get; set; }
        public string strItemGroup { get; set; }
        public string strItemCategory { get; set; }
        public string strUnit { get; set; }
        public string strAltUnit { get; set; }
        public string strWhere { get; set; }
        public string strToUnit { get; set; }
        public string strBatch { get; set; }
        public double dblMinimumStock { get; set; }
        public double dblReorderQty { get; set; }
        public string strManufacturer { get; set; }
        public string strStatus { get; set; }
        public int intAltUnit { get; set; }
        public int intSPItem { get; set; }
        public int intMaintainBatch { get; set; }
        public double dblOpnQty { get; set; }
        public double dblOpnRate { get; set; }
        public double dblOpnValue { get; set; }
        public string strBranchName { get; set; }
        public double dblBranchQty { get; set; }
        public double dblBranchRate { get; set; }
        public double dblBranchAmnout { get; set; }
        public long lngSlNo { get; set; }
        public long lngVtype { get; set; }
        public string strLedgerName { get; set; }
        public string strConversion { get; set; }
        public string strDenominator { get; set; }
        public string strParentGroup { get; set; }
        public string strLocation { get; set; }
        public double dblClsBalance { get; set; }
        public string strMatType { get; set; }
        public string strPowerClass { get; set; }
        public string strRefNo { get; set; }
        public string strDate { get; set; }
        public string strNarration { get; set; }
        public string strToLocation { get; set; }
        public string strFromLocation { get; set; }
        public string strBillKey { get; set; }
        public string strProcess { get; set; }
        public string strAgnstRefNo { get; set; }
        public string strPreserveSQL { get; set; }
        public List<GLocation> gLocationList { get; set; }

        public void InitialList()
        {
            gLocationList = new List<GLocation>();
        }

    }
}