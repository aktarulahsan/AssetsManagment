using AssetsManagment.Controllers.setting;
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

namespace AssetsManagment.Controllers
{
    public class ItemController : Controller
    {
        //
        // GET: /Item/

        LocationController con = new LocationController();


        public ActionResult Index()
        {

            ViewBag.reslist = LocationList( );
            ViewBag.groupList = groupList();

            ViewBag.res = con.showGrid();
            
            return PartialView("ItemsView");




        }
        public List<Location> LocationList( )
        {

            string strSQL;
            SqlDataReader drGetGroup;
            List<Location> oogrp = new List<Location>();
            LocationResponse response = new LocationResponse();
            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            strSQL = "SELECT GODOWNS_SERIAL,GODOWNS_NAME,BRANCH_ID,GODOWNS_DEFAULT,GODOWNS_PHONE,GODOWNS_ADDRESS1 FROM INV_GODOWNS ";

            //if (searchTerm != null)
            //{
            //    strSQL = "SELECT GODOWNS_SERIAL,GODOWNS_NAME,BRANCH_ID,GODOWNS_DEFAULT,GODOWNS_PHONE,GODOWNS_ADDRESS1 FROM INV_GODOWNS WHERE GODOWNS_NAME=" + "'" + searchTerm + "'";
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
                return oogrp;
            }

        }

        public List<Group> groupList()
        {

            SqlDataReader drGetGroup;
            List<Group> oogrp = new List<Group>();
            StockGroup sp = new StockGroup();
            Response response = new Response();
            string strSQL;


            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            strSQL = "SELECT STOCKGROUP_SERIAL, STOCKGROUP_NAME, STOCKGROUP_PARENT,STOCKGROUP_ONE_DOWN, STOCKGROUP_PRIMARY,STOCKGROUP_NAME_DEFAULT ,G_STATUS ";
            strSQL = strSQL + "FROM INV_STOCKGROUP ORDER BY STOCKGROUP_NAME ASC ";
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
                    Group ogrp = new Group();
                    ogrp.SERIAL = Convert.ToInt64(drGetGroup["STOCKGROUP_SERIAL"].ToString());
                    ogrp.NAME = drGetGroup["STOCKGROUP_NAME"].ToString();
                    ogrp.PARENT = drGetGroup["STOCKGROUP_PARENT"].ToString();
                    ogrp.PRIMARY = drGetGroup["STOCKGROUP_PRIMARY"].ToString();
                    ogrp.G_STATUS = Convert.ToInt16(drGetGroup["G_STATUS"].ToString());
                    if (drGetGroup["STOCKGROUP_ONE_DOWN"].ToString() != "")
                    {
                        ogrp.ONE_DOWN = drGetGroup["STOCKGROUP_ONE_DOWN"].ToString();
                    }
                    else
                    {
                        ogrp.ONE_DOWN = "";
                    }

                    oogrp.Add(ogrp);

                }
                drGetGroup.Close();
                gcnMain.Dispose();
                sp.data = oogrp;



                return oogrp;

            }
        }

        public JsonResult getStockItemList()
        {

            SqlDataReader drGetItems;
            List<StockItem> oogrp = new List<StockItem>();
            StockItemResponse sp = new StockItemResponse();
            Response response = new Response();
            string strSQL;

            //STOCKITEM_NAME
            //INSERT INTO INV_STOCKITEM  STOCKITEM_OPENING_BALANCE  STOCKGROUP_NAME

            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            strSQL = "SELECT STOCKITEM_SERIAL, STOCKITEM_NAME, STOCKGROUP_NAME, STOCKITEM_OPENING_BALANCE,STOCKITEM_STATUS ";
            strSQL = strSQL + "FROM INV_STOCKITEM ORDER BY STOCKITEM_NAME ASC ";
            using (SqlConnection gcnMain = new SqlConnection(connectionString))
            {
                if (gcnMain.State == ConnectionState.Open)
                {
                    gcnMain.Close();
                }
                gcnMain.Open();
                SqlCommand cmd = new SqlCommand(strSQL, gcnMain);
                drGetItems = cmd.ExecuteReader();
                while (drGetItems.Read())
                {
                    StockItem ogrp = new StockItem();
                    ogrp.lngSlNo = Convert.ToInt64(drGetItems["STOCKITEM_SERIAL"].ToString());
                    ogrp.strItemName = drGetItems["STOCKITEM_NAME"].ToString();
                    ogrp.strItemGroup = drGetItems["STOCKGROUP_NAME"].ToString();
                    //ogrp.strItemDescription = drGetItems["STOCKITEM_DESCRIPTION"].ToString();
                    ogrp.dblOpnQty = Convert.ToDouble(drGetItems["STOCKITEM_OPENING_BALANCE"].ToString());
                    ogrp.strStatus = drGetItems["STOCKITEM_STATUS"].ToString();
                    //if (drGetItems["STOCKGROUP_ONE_DOWN"].ToString() != "")
                    //{
                    //    ogrp.ONE_DOWN = drGetGroup["STOCKGROUP_ONE_DOWN"].ToString();
                    //}
                    //else
                    //{
                    //    ogrp.ONE_DOWN = "";
                    //}

                    oogrp.Add(ogrp);

                }
                drGetItems.Close();
                gcnMain.Dispose();
                sp.data = oogrp;



                return Json(sp, JsonRequestBehavior.AllowGet);

            }
        }


        public JsonResult getStockItemListByGroup(string group)
        {

            SqlDataReader drGetItems;
            List<StockItem> oogrp = new List<StockItem>();
            StockItemResponse sp = new StockItemResponse();
            Response response = new Response();
            string strSQL;


            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            strSQL = "SELECT STOCKITEM_SERIAL, STOCKITEM_NAME, STOCKGROUP_NAME, STOCKITEM_OPENING_BALANCE,STOCKITEM_STATUS ";
            strSQL = strSQL + "FROM INV_STOCKITEM  ";
            strSQL = strSQL + " where STOCKGROUP_NAME='" + group + "' ";
             strSQL = strSQL + "ORDER BY STOCKITEM_NAME ASC";
            using (SqlConnection gcnMain = new SqlConnection(connectionString))
            {
                if (gcnMain.State == ConnectionState.Open)
                {
                    gcnMain.Close();
                }
                gcnMain.Open();
                SqlCommand cmd = new SqlCommand(strSQL, gcnMain);
                drGetItems = cmd.ExecuteReader();
                while (drGetItems.Read())
                {
                    StockItem ogrp = new StockItem();
                    ogrp.lngSlNo = Convert.ToInt64(drGetItems["STOCKITEM_SERIAL"].ToString());
                    ogrp.strItemName = drGetItems["STOCKITEM_NAME"].ToString();
                    ogrp.strItemGroup = drGetItems["STOCKGROUP_NAME"].ToString();
                    //ogrp.strItemDescription = drGetItems["STOCKITEM_DESCRIPTION"].ToString();
                    ogrp.dblOpnQty = Convert.ToDouble(drGetItems["STOCKITEM_OPENING_BALANCE"].ToString());
                    ogrp.strStatus = drGetItems["STOCKITEM_STATUS"].ToString();
                    

                    oogrp.Add(ogrp);

                }
                drGetItems.Close();
                gcnMain.Dispose();
                //sp.data = oogrp;



                return Json(oogrp, JsonRequestBehavior.AllowGet);

            }
        }


        public JsonResult locationListByItemId(string id)
        {

            string strSQL;
            SqlDataReader drGetGroup;
            List<GLocation> oogrp = new List<GLocation>();
            LocationResponse response = new LocationResponse();
            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            strSQL = "SELECT INV_TRAN_KEY, INV_TRAN_POSITION, BRANCH_ID, STOCKITEM_NAME, INV_TRAN_QUANTITY, INV_TRAN_RATE, INV_TRAN_AMOUNT, GODOWNS_NAME FROM INV_TRAN ";
            strSQL = strSQL + " where STOCKITEM_NAME='" + id + "' ";

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
                    GLocation location = new GLocation();
                    location.intQty = Convert.ToDouble(drGetGroup["INV_TRAN_QUANTITY"].ToString());
                    location.dblBranchRate = Convert.ToDouble(drGetGroup["INV_TRAN_RATE"].ToString());
                    location.dblBranchAmnout = Convert.ToDouble(drGetGroup["INV_TRAN_AMOUNT"].ToString());
                    location.strBranchName = drGetGroup["GODOWNS_NAME"].ToString();



                    oogrp.Add(location);
                }
                drGetGroup.Close();
                gcnMain.Dispose();
                return Json(oogrp, JsonRequestBehavior.AllowGet);
                //return oogrp;
            }
        
        
        
        
        }

        public JsonResult updateData(StockItem resObje)
        {
            StockItem objstok = new StockItem();

            long lngStockStatus = 0, lngloop = 1;
            string strSQL, strPrimary, strStockItem, strParent, strCategory = "", strGroupParent = "", strfiled = "",
            strGodown = "", strdate = "01/01/1900", strBranchID = "", strGodownSerial = "", strItemSerial = "", strRefNo = "", strInOutFlg = "";
            string vstrItemName;
            string vstrCatgory;
            objstok = resObje;
            List<GLocation> gLocationLists = resObje.gLocationList;

            string vstrAltUnit = objstok.strUnit;

            string fstrSQL = "";

            fstrSQL = "SELECT * FROM  INV_STOCKITEM WHERE STOCKITEM_NAME = '" + objstok.strItemName.Replace("'", "''") + "'";

            var a = resObje;
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


                    SqlCommand cmd = new SqlCommand(fstrSQL, gcnMain);
                    var fResult = cmd.ExecuteReader();
                    if (fResult != null)
                    {

                      

                        strPrimary = Utility.gstrGetPrimary("0003", objstok.strItemGroup.Replace("'", "''"));
                        if (objstok.strStatus == "Yes")
                        {
                            lngStockStatus = 1;
                        }

                        strStockItem = objstok.strItemName.Replace("'", "''");
                        vstrItemName = objstok.strItemName.Replace("'", "''");
                        strParent = objstok.strItemGroup.Replace("'", "''");



                        SqlDataReader dr;
                        SqlCommand cmdInsert = new SqlCommand();
                        SqlTransaction myTrans;
                        myTrans = gcnMain.BeginTransaction();
                        cmdInsert.Connection = gcnMain;
                        cmdInsert.Transaction = myTrans;

                        string strAlias = "";

                        strAlias = Utility.GetsItemCode("0003", "Finished Goods").ToString();


                        strRefNo = Utility.vtSTOCK_OPENING_STR + "0001" + strItemSerial + "-OPN" + lngloop + strGodownSerial;

                        strSQL = "UPDATE INV_STOCKITEM ";
                        strSQL = strSQL + "SET ";
                        strSQL = strSQL + "STOCKITEM_NAME = '" + strStockItem + "',";
                        strSQL = strSQL + "STOCKITEM_ALIAS = '" + strAlias + "',";
                        strSQL = strSQL + "STOCKITEM_DESCRIPTION = '" + objstok.strItemDescription + "',";
                        strSQL = strSQL + "STOCKGROUP_NAME = '" + objstok.strItemGroup + "',";
                        strSQL = strSQL + "STOCKITEM_BASEUNITS = '" + objstok.strUnit + "',";
                        strSQL = strSQL + "STOCKITEM_OPENING_BALANCE = '" + objstok.dblOpnQty + "',";
                        strSQL = strSQL + "STOCKITEM_OPENING_RATE = '" + objstok.dblOpnRate + "',";
                        strSQL = strSQL + "STOCKITEM_OPENING_VALUE = '" + objstok.dblOpnValue + "',";
                        strSQL = strSQL + "STOCKITEM_STATUS = '" + lngStockStatus + "',";

                        strSQL = strSQL + "WHERE LEDGER_SERIAL = " + objstok.strIOldtemcode + " ";



                        cmdInsert.CommandText = strSQL;
                        cmdInsert.ExecuteNonQuery();


                        //for (int i = 0; i < gLocationLists.length; i++)
                        //{

                        //}
                        if (gLocationLists.Count > 0)
                        {

                            foreach (var item in gLocationLists)
                            {



                                strSQL = "INSERT INTO INV_TRAN ";
                                strSQL = strSQL + "(INV_TRAN_KEY,INV_TRAN_POSITION,BRANCH_ID,INV_REF_NO,INV_DATE,STOCKITEM_NAME,";
                                strSQL = strSQL + "INV_TRAN_QUANTITY,INV_TRAN_RATE,INV_TRAN_AMOUNT,GODOWNS_NAME";

                                strSQL = strSQL + ") ";
                                strSQL = strSQL + "VALUES('" + strRefNo + lngloop.ToString().PadLeft(4, '0') + "',";
                                strSQL = strSQL + "" + lngloop + ",'0001',";
                                strSQL = strSQL + "'" + strRefNo + "'," + Utility.cvtSQLDateString(strdate) + ",";
                                strSQL = strSQL + "'" + vstrItemName + "'," + item.intQty + ",";
                                strSQL = strSQL + " " + item.dblBranchRate + "," + item.dblBranchAmnout + ",";
                                strSQL = strSQL + "'" + item.strBranchName.Replace("'", "''") + "'";
                                //strSQL = strSQL + 0 + ",1";
                                //if (strBatch != "")
                                //{
                                //    strSQL = strSQL + ",'" + strBatch.Replace("'", "''") + "'";
                                //}
                                strSQL = strSQL + ") ";



                                cmdInsert.CommandText = strSQL;
                                cmdInsert.ExecuteNonQuery();
                                lngloop += 1;

                            }

                        }






                        cmdInsert.Transaction.Commit();
                        gcnMain.Close();
                        return Json("OK", JsonRequestBehavior.AllowGet);


                    }
                    else { 
                        
                        //item not found
                    
                    }

                   
                }
                catch (Exception ex)
                {
                    return Json(ex.ToString(), JsonRequestBehavior.AllowGet); 
                }
                finally
                {
                    gcnMain.Close();
                }
            }
            return Json("OK", JsonRequestBehavior.AllowGet);
           
        }




        public JsonResult saveDatad(StockItem resObje)
        {
            StockItem objstok = new StockItem();
            //objstok.gLocationList.ToList();
            long lngStockStatus = 0, lngloop = 1;
            string strSQL, strPrimary, strStockItem, strParent, strCategory = "", strGroupParent = "", strfiled = "",
            strGodown = "", strdate = "01/01/1900", strBranchID = "", strGodownSerial = "", strItemSerial = "", strRefNo = "", strInOutFlg = "";
            string vstrItemName;
            string vstrCatgory;
            objstok = resObje;
            List<GLocation> gLocationLists = resObje.gLocationList;

            string vstrAltUnit = objstok.strUnit;



            var a = resObje;
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

                    strPrimary = Utility.gstrGetPrimary("0003", objstok.strItemGroup.Replace("'", "''"));
                    if (objstok.strStatus == "Yes")
                    {
                        lngStockStatus = 1;
                    }

                    strStockItem = objstok.strItemName.Replace("'", "''");
                    vstrItemName = objstok.strItemName.Replace("'", "''");
                    strParent = objstok.strItemGroup.Replace("'", "''");
      


                    SqlDataReader dr;
                    SqlCommand cmdInsert = new SqlCommand();
                    SqlTransaction myTrans;
                    myTrans = gcnMain.BeginTransaction();
                    cmdInsert.Connection = gcnMain;
                    cmdInsert.Transaction = myTrans;

                    string strAlias = "";
           
                        strAlias = Utility.GetsItemCode("0003", "Finished Goods").ToString();


                   

                    strSQL = "INSERT INTO INV_STOCKITEM";
                    strSQL = strSQL + "(STOCKITEM_NAME,STOCKITEM_ALIAS,STOCKITEM_DESCRIPTION,STOCKGROUP_NAME,";
                    strSQL = strSQL + "STOCKITEM_BASEUNITS,STOCKITEM_OPENING_BALANCE,STOCKITEM_OPENING_RATE,STOCKITEM_OPENING_VALUE,";
                    strSQL = strSQL + "STOCKITEM_STATUS)";
                    strSQL = strSQL + "VALUES(";
                    strSQL = strSQL + "'" + strStockItem + "'";
                    strSQL = strSQL + "," + strAlias + "";
                    strSQL = strSQL + ",'" + objstok.strItemDescription + "'";
                    strSQL = strSQL + ",'" + objstok.strItemGroup   + "'";
                    strSQL = strSQL + ",'" + objstok.strUnit    + "'";
                    strSQL = strSQL + ",'" + objstok.dblOpnQty   + "'";
                    strSQL = strSQL + ",'" + objstok.dblOpnRate  + "'";
                    strSQL = strSQL + ",'" + objstok.dblOpnValue + "'";
                    strSQL = strSQL + ",'" + lngStockStatus + "'";
                    strSQL = strSQL + ")";
                

                    cmdInsert.CommandText = strSQL;
                    cmdInsert.ExecuteNonQuery();

                    if (gLocationLists.Count > 0) {

                        foreach (var item in gLocationLists)
                        {

                            strSQL = "SELECT GODOWNS_SERIAL,BRANCH_ID FROM INV_GODOWNS WHERE GODOWNs_NAME = 'Main Location' ";
                            cmdInsert.CommandText = strSQL;
                            dr = cmdInsert.ExecuteReader();
                            if (dr.Read())
                            {
                                strGodownSerial = dr["GODOWNS_SERIAL"].ToString();
                                strBranchID = dr["BRANCH_ID"].ToString();
                            }
                            dr.Close();
                            strSQL = "SELECT max(INV_TRAN_SERIAL)+1 as STOCKITEM_SERIAL FROM INV_TRAN ";
                            cmdInsert.CommandText = strSQL;
                            dr = cmdInsert.ExecuteReader();
                            if (dr.Read())
                            {
                                strItemSerial = dr["STOCKITEM_SERIAL"].ToString();
                            }
                            dr.Close();



                            strRefNo = Utility.vtSTOCK_OPENING_STR + "0001" + strItemSerial + "-OPN" + lngloop + strGodownSerial;
                            strSQL = "INSERT INTO INV_TRAN ";
                            strSQL = strSQL + "(INV_TRAN_KEY,INV_TRAN_POSITION,BRANCH_ID,INV_REF_NO,INV_DATE,STOCKITEM_NAME,";
                            strSQL = strSQL + "INV_TRAN_QUANTITY,INV_TRAN_RATE,INV_TRAN_AMOUNT,GODOWNS_NAME";
                          
                            strSQL = strSQL + ") ";
                            strSQL = strSQL + "VALUES('" + strRefNo + lngloop.ToString().PadLeft(4,'0') + "',";
                            strSQL = strSQL + "" + lngloop + ",'0001',";
                            strSQL = strSQL + "'" + strRefNo + "'," + Utility.cvtSQLDateString(strdate) + ",";
                            strSQL = strSQL + "'" + vstrItemName + "'," + item.intQty + ",";
                            strSQL = strSQL + " " + item.dblBranchRate + "," + item.dblBranchAmnout + ",";
                            strSQL = strSQL + "'" + item.strBranchName.Replace("'", "''") + "'";
                         
                            strSQL = strSQL + ") ";



                            cmdInsert.CommandText = strSQL;
                            cmdInsert.ExecuteNonQuery();
                            lngloop += 1;

                        }
                        
                    }
                   
                  




                    cmdInsert.Transaction.Commit();
                    gcnMain.Close();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return  Json(ex, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    gcnMain.Close();
                }
            }


            //return Json("OK", JsonRequestBehavior.AllowGet);

        }
 


        public StockGroup mFillStockGroupList(string strDeComID)
        {
            string strSQL = null;
            SqlDataReader drGetGroup;
            List<Group> oogrp = new List<Group>();
            StockGroup sp = new StockGroup();
            Response response = new Response();



            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            strSQL = "SELECT STOCKGROUP_SERIAL, STOCKGROUP_NAME, STOCKGROUP_PARENT,STOCKGROUP_ONE_DOWN, STOCKGROUP_PRIMARY,STOCKGROUP_NAME_DEFAULT ";
            strSQL = strSQL + "FROM INV_STOCKGROUP ORDER BY STOCKGROUP_NAME ASC ";
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
                    Group ogrp = new Group();
                    ogrp.SERIAL = Convert.ToInt64(drGetGroup["STOCKGROUP_SERIAL"].ToString());
                    ogrp.NAME = drGetGroup["STOCKGROUP_NAME"].ToString();
                    ogrp.PARENT = drGetGroup["STOCKGROUP_PARENT"].ToString();
                    ogrp.PRIMARY = drGetGroup["STOCKGROUP_PRIMARY"].ToString();
                    if (drGetGroup["STOCKGROUP_ONE_DOWN"].ToString() != "")
                    {
                        ogrp.ONE_DOWN = drGetGroup["STOCKGROUP_ONE_DOWN"].ToString();
                    }
                    else
                    {
                        ogrp.ONE_DOWN = "";
                    }

                    //if (drGetGroup["STOCKGROUP_NAME_DEFAULT"].ToString() != "")
                    //{
                    //    ogrp.DEFAULT = drGetGroup["STOCKGROUP_NAME_DEFAULT"].ToString();
                    //}
                    //else
                    //{
                    //    ogrp.DEFAULT = "";
                    //}
                    oogrp.Add(ogrp);

                }
                drGetGroup.Close();
                gcnMain.Dispose();
                sp.data = oogrp;



                return sp;

            }
        }



        [HttpPost]
        public ActionResult deleteItemById(int id)
        {
            string strSQL="";
        
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

                   
                    SqlCommand cmdDelete = new SqlCommand();
                    SqlTransaction myTrans;
                    myTrans = gcnMain.BeginTransaction();
                    cmdDelete.Connection = gcnMain;
                    cmdDelete.Transaction = myTrans;

                    strSQL = "DELETE FROM INV_STOCKITEM ";
                    strSQL = strSQL + "WHERE STOCKITEM_SERIAL = " + id + "";
                    cmdDelete.CommandText = strSQL;
                    cmdDelete.ExecuteNonQuery();
           
                    cmdDelete.Transaction.Commit();
                    gcnMain.Close();
                  
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }

                catch (Exception ex)
                {

                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    gcnMain.Close();
                }

            }

        }
        //
        // GET: /Item/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Item/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Item/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Item/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Item/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Item/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Item/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
