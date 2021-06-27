using AssetsManagment.Models;
using Dutility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace AssetsManagment.Controllers.setting
{
    public class LocationController : Controller
    {

        private string connstring;
        //
        // GET: /Location/
        public ActionResult Index()
        {
            return PartialView("Location");
        }

        public ActionResult create(Location regObject)
        {

            long lngStockType;
            string vstrBranch = "";
            string strPrimary = "", vstrUnder = "", strDeComID = "";

            //vstrLocation = "", vstrAddress1 = "", vstrAddress2 = "", vstrCity = "", vstrPhone = "", vstrFax = "", intsection="";
            string strSQL, strBranchId = "", strString = "";
            connstring = Utility.SQLConnstringComSwitch(strDeComID);

            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


            using (SqlConnection gcnMain = new SqlConnection(connectionString))
            {
                if (gcnMain.State == ConnectionState.Open)
                {
                    gcnMain.Close();
                }

                try
                {
                    gcnMain.Open();
                    SqlDataReader dr;
                    //strBranchId = Utility.gstrGetBranchID(strDeComID, vstrBranch);
                    strBranchId = "0001";
                    regObject.intSection = 0;
                    SqlCommand cmdInsert = new SqlCommand();
                    SqlTransaction myTrans;
                    myTrans = gcnMain.BeginTransaction();
                    cmdInsert.Connection = gcnMain;
                    cmdInsert.Transaction = myTrans;
                    strSQL = "INSERT INTO INV_GODOWNS(BRANCH_ID,GODOWNS_NAME,";
                    strSQL = strSQL + "GODOWNS_ADDRESS1, ";
                    strSQL = strSQL + "GODOWNS_PHONE,SECTION_STATUS) ";
                    strSQL = strSQL + "VALUES('" + strBranchId + "','" + regObject.strLocation + "', ";
                    strSQL = strSQL + "'" + regObject.strAddres1.Replace("'", "''") + "', ";
                    //strSQL = strSQL + "'" + regObject.strAddres2.Replace("'", "''") + "',";
                    //strSQL = strSQL + "'" + regObject.strCity.Replace("'", "''") + "', ";
                    strSQL = strSQL + "'" + regObject.strPhone.Replace("'", "''") + "', ";

                    //strSQL = strSQL + "'" + regObject.strFax.Replace("'", "''") + "' ";

                    strSQL = strSQL  + regObject.intSection + " ";
                    strSQL = strSQL + ")";
                    cmdInsert.CommandText = strSQL;
                    cmdInsert.ExecuteNonQuery();

                    //strSQL = "INSERT INTO INV_STOCKITEM_CLOSING(STOCKITEM_NAME,GODOWNS_NAME) ";
                    //strSQL = strSQL + "SELECT STOCKITEM_NAME,'" + regObject.strLocation + "'  FROM INV_STOCKITEM ";
                    //cmdInsert.CommandText = strSQL;
                    //cmdInsert.ExecuteNonQuery();

                    cmdInsert.Transaction.Commit();
                    gcnMain.Close();

                    return Json("Ok", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Save fail !!", JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    gcnMain.Close();
                }
            }
        }


        public ActionResult updateData(Location regObject)
        {
            string strSQL;
            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection gcnMain = new SqlConnection(connectionString))
            {
                if (gcnMain.State == ConnectionState.Open)
                {
                    gcnMain.Close();
                }

                try
                {
                    gcnMain.Open();
                    SqlDataReader dr;
                    //strBranchId = Utility.gstrGetBranchID(strDeComID, vstrBranch);
                    regObject.strBranch = "0001";
                    SqlCommand cmdInsert = new SqlCommand();
                    SqlTransaction myTrans;
                    myTrans = gcnMain.BeginTransaction();
                    cmdInsert.Connection = gcnMain;
                    cmdInsert.Transaction = myTrans;

                    strSQL = "UPDATE INV_GODOWNS SET GODOWNS_NAME = '" + regObject.strLocation + "',";
                    strSQL = strSQL + "BRANCH_ID = '" + regObject.strBranch + "',";
                    strSQL = strSQL + "GODOWNS_PARENT_GROUP = '" + regObject.strUnder + "',";
                    strSQL = strSQL + "GODOWNS_ADDRESS1 = '" + regObject.strAddres1.Replace("'", "''") + "', ";
                    //strSQL = strSQL + "GODOWNS_ADDRESS2 = '" + regObject.strAddres2.Replace("'", "''") + "',";
                    //strSQL = strSQL + "GODOWNS_CITY = '" + regObject.strCity.Replace("'", "''") + "', ";
                    strSQL = strSQL + "GODOWNS_PHONE = '" + regObject.strPhone.Replace("'", "''") + "', ";
                    //strSQL = strSQL + "GODOWNS_FAX = '" + regObject.strFax.Replace("'", "''") + "',";
                    strSQL = strSQL + "SECTION_STATUS = " + regObject.intSection + " ";
                    strSQL = strSQL + "WHERE GODOWNS_SERIAL = " + regObject.lngSlNo + " ";
                    cmdInsert.CommandText = strSQL;
                    cmdInsert.ExecuteNonQuery();


                    cmdInsert.Transaction.Commit();
                    gcnMain.Close();
                    return Json("Ok", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Update fail !!", JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    gcnMain.Close();
                }
            }
        
        
        }

        public JsonResult showGrid()
        {

            string strSQL;
            SqlDataReader drGetGroup;
            List<Location> oogrp = new List<Location>();
            LocationResponse response = new LocationResponse();
            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            //connstring = Utility.SQLConnstringComSwitch(strDeComID);
            //if (vblngAccessControl == true)
            //{
            //    strSQL = "SELECT PRI_LOCATION_SERIAL AS GODOWNS_SERIAL,GODOWNS_NAME,'' BRANCH_ID,0 GODOWNS_DEFAULT FROM USER_PRIVILEGES_LOCATION ";
            //    strSQL = strSQL + " WHERE USER_LOGIN_NAME='" + vstrUserName + "' ";

            //}
            //else
            //{
            strSQL = "SELECT GODOWNS_SERIAL,GODOWNS_NAME,BRANCH_ID,GODOWNS_DEFAULT,GODOWNS_PHONE,GODOWNS_ADDRESS1 FROM INV_GODOWNS ";
            //}





            using (SqlConnection gcnMain = new SqlConnection(connectionString))
            {
                if (gcnMain.State == ConnectionState.Open)
                {
                    gcnMain.Close();
                }
                gcnMain.Open();

                SqlCommand cmd = new SqlCommand(strSQL, gcnMain);
                drGetGroup = cmd.ExecuteReader();
                while (drGetGroup.Read())
                {
                    Location ogrp = new Location();
                    ogrp.lngSlNo = Convert.ToInt64(drGetGroup["GODOWNS_SERIAL"].ToString());
                    ogrp.strLocation = drGetGroup["GODOWNS_NAME"].ToString();
                    ogrp.strPhone = drGetGroup["GODOWNS_PHONE"].ToString();
                    ogrp.strAddres1 = drGetGroup["GODOWNS_ADDRESS1"].ToString();

                    if (drGetGroup["BRANCH_ID"].ToString() != "")
                    {
                        ogrp.strBranch = Utility.gstrGetBranchName("0001", drGetGroup["BRANCH_ID"].ToString());
                    }
                    else
                    {
                        ogrp.strBranch = "";
                    }
                    ogrp.lngDefault = Convert.ToInt32(drGetGroup["GODOWNS_DEFAULT"]);
                    oogrp.Add(ogrp);
                }
                drGetGroup.Close();
                gcnMain.Dispose();
                response.data = oogrp;

                return Json(response, JsonRequestBehavior.AllowGet);


            }

        }

        public JsonResult LocationList(string searchTerm)
        {

            string strSQL;
            SqlDataReader drGetGroup;
            List<Location> oogrp = new List<Location>();
            LocationResponse response = new LocationResponse();
            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            strSQL = "SELECT GODOWNS_SERIAL,GODOWNS_NAME,BRANCH_ID,GODOWNS_DEFAULT,GODOWNS_PHONE,GODOWNS_ADDRESS1 FROM INV_GODOWNS ";

            if (searchTerm != null)
            {
                strSQL = "SELECT GODOWNS_SERIAL,GODOWNS_NAME,BRANCH_ID,GODOWNS_DEFAULT,GODOWNS_PHONE,GODOWNS_ADDRESS1 FROM INV_GODOWNS WHERE GODOWNS_NAME="+"'"+searchTerm+"'";
            }




            using (SqlConnection gcnMain = new SqlConnection(connectionString))
            {
                if (gcnMain.State == ConnectionState.Open)
                {
                    gcnMain.Close();
                }
                gcnMain.Open();

                SqlCommand cmd = new SqlCommand(strSQL, gcnMain);
                drGetGroup = cmd.ExecuteReader();
                while (drGetGroup.Read())
                {
                    Location ogrp = new Location();
                    ogrp.lngSlNo = Convert.ToInt64(drGetGroup["GODOWNS_SERIAL"].ToString());
                    ogrp.strLocation = drGetGroup["GODOWNS_NAME"].ToString();
                    ogrp.strPhone = drGetGroup["GODOWNS_PHONE"].ToString();
                    ogrp.strAddres1 = drGetGroup["GODOWNS_ADDRESS1"].ToString();

                    if (drGetGroup["BRANCH_ID"].ToString() != "")
                    {
                        ogrp.strBranch = Utility.gstrGetBranchName("0001", drGetGroup["BRANCH_ID"].ToString());
                    }
                    else
                    {
                        ogrp.strBranch = "";
                    }
                    ogrp.lngDefault = Convert.ToInt32(drGetGroup["GODOWNS_DEFAULT"]);
                    oogrp.Add(ogrp);
                }
                drGetGroup.Close();
                gcnMain.Dispose();
                response.data = oogrp;

                var modifiedData = oogrp.Select(x => new
                {
                    id = x.lngSlNo,
                    text = x.strLocation
                });



                return Json(modifiedData, JsonRequestBehavior.AllowGet);


            }

        }

    }
}