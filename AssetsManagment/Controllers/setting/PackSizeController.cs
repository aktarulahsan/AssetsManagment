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
    public class PackSizeController : Controller
    {
        string strSQL;
        //
        // GET: /PackSize/
        public ActionResult Index()
        {
            return PartialView("packView");
            //return View("packView");
        }

        [HttpPost]
        public ActionResult create( string vstrName)
        {
 //public string mInsertcategory(string strDeComID, string vstrName, string vstrUnder)
            long lngStockType;
            string strPrimary="", vstrUnder = "", strDeComID="";
    
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
                        //if (vstrUnder.ToUpper() == "PRIMARY")
                        //{
                        strPrimary = "Primary";
                        //    vstrUnder = vstrName.Replace("'", "''");
                        //    lngStockType = 1;
                        //}
                        //else
                        //{
                        //    strPrimary = Utility.mstrGetPrimary(strDeComID, vstrUnder.Replace("'", "''"));
                        //    vstrUnder = vstrUnder.Replace("'", "''");
                        lngStockType = 2;
                        //}
                        SqlCommand cmdInsert = new SqlCommand();
                        SqlTransaction myTrans;
                        myTrans = gcnMain.BeginTransaction();
                        cmdInsert.Connection = gcnMain;
                        cmdInsert.Transaction = myTrans;
                        vstrName = vstrName.Replace("'", "''");
                        strSQL = "INSERT INTO INV_STOCKCATEGORY";
                        strSQL = strSQL + "(STOCKCATEGORY_NAME,STOCKCATEGORY_PARENT,STOCKCATEGORY_PRIMARY,STOCKCATEGORY_TYPE) ";
                        strSQL = strSQL + "VALUES(";
                        strSQL = strSQL + "'" + vstrName + "','" + vstrUnder + "',";
                        strSQL = strSQL + "'" + strPrimary + "'," + lngStockType + " ";
                        strSQL = strSQL + ")";
                        cmdInsert.CommandText = strSQL;
                        cmdInsert.ExecuteNonQuery();

                        cmdInsert.Transaction.Commit();
                        //gcnMain.Close();
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

        public ActionResult upDateData(StockCategory regObej) {



            string strSQL, strGroupName, strParent, strPrimary, strOldLedgerGroup = "";
          
            
            string   vstrUnder = "", strDeComID = "";

            long mstrPrimaryKey = regObej.lngslNo;
            string vstrName = regObej.CategoryName;

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
                    strGroupName = vstrName.Replace("'", "''");
                    strParent = vstrUnder.Replace("'", "''");
                    //strPrimary = Utility.GetEndGroupStock(strDeComID, strParent);
                    strPrimary = "Primary";
                    SqlDataReader rsGet;
                    SqlCommand cmdInsert = new SqlCommand();
                    SqlTransaction myTrans;
                    myTrans = gcnMain.BeginTransaction();
                    cmdInsert.Connection = gcnMain;
                    cmdInsert.Transaction = myTrans;

                    strSQL = "SELECT STOCKCATEGORY_NAME FROM INV_STOCKCATEGORY ";
                    strSQL = strSQL + "WHERE STOCKCATEGORY_SERIAL = " + mstrPrimaryKey + " ";
                    cmdInsert.CommandText = strSQL;
                    rsGet = cmdInsert.ExecuteReader();
                    if (rsGet.Read())
                    {
                        strOldLedgerGroup = rsGet["STOCKCATEGORY_NAME"].ToString().Replace("'", "''");
                    }
                    rsGet.Close();

                    if (vstrUnder.ToUpper() == "PRIMARY")
                    {
                        strPrimary = vstrName.Replace("'", "''");
                        vstrUnder = vstrName.Replace("'", "''");

                    }
                    else
                    {
                        strPrimary = "Primary";
                        vstrUnder = vstrUnder.Replace("'", "''");

                    }

                    vstrName = vstrName.Replace("'", "''");
                    strSQL = "UPDATE INV_STOCKCATEGORY ";
                    strSQL = strSQL + "SET STOCKCATEGORY_NAME = '" + strGroupName + "',";
                    strSQL = strSQL + "STOCKCATEGORY_PARENT = '" + strParent + "', ";
                    strSQL = strSQL + "STOCKCATEGORY_PRIMARY = '" + strPrimary + "' ";
                    strSQL = strSQL + "WHERE STOCKCATEGORY_SERIAL = " + mstrPrimaryKey + " ";
                    cmdInsert.CommandText = strSQL;
                    cmdInsert.ExecuteNonQuery();
                    strSQL = "UPDATE INV_STOCKCATEGORY SET STOCKCATEGORY_PARENT = '" + strParent + "' ";
                    strSQL = strSQL + "WHERE STOCKCATEGORY_PARENT = '" + strOldLedgerGroup + "' ";
                    cmdInsert.CommandText = strSQL;
                    cmdInsert.ExecuteNonQuery();
                    strSQL = "UPDATE INV_STOCKCATEGORY SET STOCKCATEGORY_PRIMARY = '" + strGroupName + "' ";
                    strSQL = strSQL + "WHERE STOCKCATEGORY_PRIMARY = '" + strOldLedgerGroup + "' ";
                    cmdInsert.CommandText = strSQL;
                    cmdInsert.ExecuteNonQuery();
                    strSQL = "UPDATE INV_STOCKITEM SET STOCKITEM_PRIMARY_GROUP = '" + strGroupName + "' ";
                    strSQL = strSQL + "WHERE STOCKITEM_PRIMARY_GROUP = '" + strOldLedgerGroup + "' ";
                    cmdInsert.CommandText = strSQL;
                    cmdInsert.ExecuteNonQuery();
                    strSQL = "UPDATE INV_STOCKITEM SET STOCKCATEGORY_NAME = '" + strGroupName + "' ";
                    strSQL = strSQL + "WHERE STOCKCATEGORY_NAME = '" + strOldLedgerGroup + "' ";
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


        //public ActionResult deletePack() {
        //    long strSerialfNo;

        //    string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        //    using (SqlConnection gcnMain = new SqlConnection(connectionString))
        //    {
        //        if (gcnMain.State == ConnectionState.Open)
        //        {
        //            gcnMain.Close();
        //        }

        //        try
        //        {
        //            gcnMain.Open();

        //            SqlDataReader rsGet;
        //            SqlCommand cmdInsert = new SqlCommand();
        //            SqlTransaction myTrans;
        //            myTrans = gcnMain.BeginTransaction();
        //            cmdInsert.Connection = gcnMain;
        //            cmdInsert.Transaction = myTrans;

        //            strSQL = "SELECT STOCKCATEGORY_NAME, STOCKCATEGORY_PARENT, STOCKCATEGORY_PRIMARY FROM INV_STOCKCATEGORY ";
        //            strSQL = strSQL + "WHERE STOCKCATEGORY_SERIAL = " + strSerialfNo + " ";
        //            cmdInsert.CommandText = strSQL;
        //            rsGet = cmdInsert.ExecuteReader();
        //            if (rsGet.Read())
        //            {
        //                strGroupName = rsGet["STOCKCATEGORY_NAME"].ToString().Replace("'", "''");
        //            }
        //            rsGet.Close();


        //            strSQL = "DELETE FROM INV_STOCKCATEGORY ";
        //            strSQL = strSQL + "WHERE STOCKCATEGORY_NAME = '" + strGroupName.Replace("'", "''") + "'";
        //            cmdInsert.CommandText = strSQL;
        //            cmdInsert.ExecuteNonQuery();


        //            cmdInsert.Transaction.Commit();
        //            gcnMain.Close();
        //            return Json("Ok", JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json("Error", JsonRequestBehavior.AllowGet);
        //        }
        //        finally
        //        {
        //            gcnMain.Close();
        //        }
        //    }
        
        //}

       
        public ActionResult showGrid()
        {

            SqlDataReader drGetGroup;
            List<StockCategory> list = new List<StockCategory>();
            Category sp = new Category();



            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            strSQL = "SELECT STOCKCATEGORY_SERIAL, STOCKCATEGORY_NAME, STOCKCATEGORY_PARENT, STOCKCATEGORY_PRIMARY ";
            strSQL = strSQL + "FROM INV_STOCKCATEGORY ORDER BY STOCKCATEGORY_NAME ASC ";
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
                    StockCategory ogrp = new StockCategory();
                    ogrp.lngslNo = Convert.ToInt64(drGetGroup["STOCKCATEGORY_SERIAL"].ToString());
                    ogrp.CategoryName = drGetGroup["STOCKCATEGORY_NAME"].ToString();
                    ogrp.CategoryUnder = drGetGroup["STOCKCATEGORY_PARENT"].ToString();
                    ogrp.strPrimary = drGetGroup["STOCKCATEGORY_PRIMARY"].ToString();
                    list.Add(ogrp);


                }
                drGetGroup.Close();
                gcnMain.Dispose();

                sp.data = list;
                return Json(sp, JsonRequestBehavior.AllowGet);

            }


        }
	}
}