using AssetsManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetsManagment.Controllers.distribute
{
    public class DistributeController : Controller
    {
        //
        // GET: /Distribute/
        public ActionResult Index()
        {
            return View();
        }



        //public List<Location> LocationList()
        //{

        //    string strSQL;
        //    SqlDataReader drGetGroup;
        //    List<Location> oogrp = new List<Location>();
        //    LocationResponse response = new LocationResponse();
        //    string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        //    strSQL = "SELECT GODOWNS_SERIAL,GODOWNS_NAME,BRANCH_ID,GODOWNS_DEFAULT,GODOWNS_PHONE,GODOWNS_ADDRESS1 FROM INV_GODOWNS ";

        //    //if (searchTerm != null)
        //    //{
        //    //    strSQL = "SELECT GODOWNS_SERIAL,GODOWNS_NAME,BRANCH_ID,GODOWNS_DEFAULT,GODOWNS_PHONE,GODOWNS_ADDRESS1 FROM INV_GODOWNS WHERE GODOWNS_NAME=" + "'" + searchTerm + "'";
        //    //}
        //    using (SqlConnection gcnMain = new SqlConnection(connectionString))
        //    {
        //        if (gcnMain.State == ConnectionState.Open)
        //        {
        //            gcnMain.Close();
        //        }
        //        gcnMain.Open();

        //        SqlCommand cmd = new SqlCommand(strSQL, gcnMain);
        //        drGetGroup = cmd.ExecuteReader();
        //        while (drGetGroup.Read())
        //        {
        //            Location ogrp = new Location();
        //            ogrp.lngSlNo = Convert.ToInt64(drGetGroup["GODOWNS_SERIAL"].ToString());
        //            ogrp.strLocation = drGetGroup["GODOWNS_NAME"].ToString();
        //            ogrp.strPhone = drGetGroup["GODOWNS_PHONE"].ToString();
        //            ogrp.strAddres1 = drGetGroup["GODOWNS_ADDRESS1"].ToString();

        //            if (drGetGroup["BRANCH_ID"].ToString() != "")
        //            {
        //                ogrp.strBranch = Utility.gstrGetBranchName("0001", drGetGroup["BRANCH_ID"].ToString());
        //            }
        //            else
        //            {
        //                ogrp.strBranch = "";
        //            }
        //            ogrp.lngDefault = Convert.ToInt32(drGetGroup["GODOWNS_DEFAULT"]);
        //            oogrp.Add(ogrp);
        //        }
        //        drGetGroup.Close();
        //        gcnMain.Dispose();
        //        return oogrp;
        //    }

        //}



	}
}