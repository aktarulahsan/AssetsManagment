using AssetsManagment.Models;
using Dutility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace AssetsManagment.Controllers
{
    public class StockGroupController : Controller
    {
        string strSQL = null;
        //
        // GET: /StockGroup/
    
        public ActionResult Index()
        {
        
           
            return View();
        }

          public ActionResult SGview()
        {


            return PartialView("PartialStockGroupView");
        }

        


        [HttpPost]
        public ActionResult GetLeaveEditkey()
        {

            Response response = new Response();
          

            var datas = mFillStockGroupList("");

            //response.data = datas;

            return Json(datas, JsonRequestBehavior.AllowGet);
        }
    
         public StockGroup mFillStockGroupList(string strDeComID)
        {
  
            SqlDataReader drGetGroup;
            List<Group> oogrp = new List<Group>();
             StockGroup sp= new StockGroup();
            Response response = new Response();

             

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
               
                                      

                return sp;

            }
        }


         public JsonResult getUnderList(string strDeComID)
         {

             SqlDataReader drGetGroup;
             List<Group> oogrp = new List<Group>();
             StockGroup sp = new StockGroup();
             Response response = new Response();

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
                 //sp.data = oogrp;


                 return Json(oogrp, JsonRequestBehavior.AllowGet);
                 //return sp;

             }
         }

        

      
        // GET: /StockGroup/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

 

        // GET: /StockGroup/Create
        public ActionResult Create()
        {
            return View();
        }

    
        [HttpPost]
        public ActionResult Create(Group collection)
        {
          

            string strGroup,  strPrimary, strParent = "", strOneDown = "";
            long lngGrType = 0, lngManuFacPrimary = 0, lngManuFacSecondary = 0;
            string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection gcnMain = new SqlConnection(connectionString))
            {
                if (gcnMain.State == ConnectionState.Open)
                {
                    gcnMain.Close();
                }

                gcnMain.Open();

                //strPrimary = Utility.GetEndGroup(vstrUnder);
                strPrimary = collection.PARENT;
                strGroup =collection.NAME.Replace("'", "''");
                strParent = collection.PARENT.Replace("'", "''");

                SqlDataReader drGetGroup;
                SqlCommand cmdInsert = new SqlCommand();
                SqlTransaction myTrans;
                myTrans = gcnMain.BeginTransaction();
                cmdInsert.Connection = gcnMain;
                cmdInsert.Transaction = myTrans;

                strSQL = "SELECT STOCKGROUP_PRIMARY,STOCKGROUP_LEVEL,STOCKGROUP_PARENT,STOCKGROUP_ONE_DOWN,";
                strSQL = strSQL + "STOCKGROUP_PRIMARY_TYPE,STOCKGROUP_SECONDARY_TYPE FROM INV_STOCKGROUP ";
                strSQL = strSQL + "WHERE STOCKGROUP_NAME = '" + strParent + "' ";
                cmdInsert.CommandText = strSQL;
                drGetGroup = cmdInsert.ExecuteReader();
                if (drGetGroup.Read())
                {
                    lngGrType = Convert.ToInt32(drGetGroup["STOCKGROUP_LEVEL"]) + 1;
                    lngManuFacPrimary = Convert.ToInt32(drGetGroup["STOCKGROUP_PRIMARY_TYPE"].ToString());
                    lngManuFacSecondary = Convert.ToInt32(drGetGroup["STOCKGROUP_SECONDARY_TYPE"].ToString());
                    if (drGetGroup["STOCKGROUP_PRIMARY"].ToString() == strParent)
                    {
                        strOneDown = strGroup;
                        strPrimary = strParent;
                    }
                    else
                    {
                        strPrimary = drGetGroup["STOCKGROUP_PRIMARY"].ToString();
                        strOneDown = drGetGroup["STOCKGROUP_PARENT"].ToString();
                    }
                }
                drGetGroup.Close();
                if (collection.PARENT.ToUpper() == "PRIMARY")
                {
                    strParent = strGroup;
                    strPrimary = strGroup;
                    strOneDown = strGroup;
                    lngGrType = (long)Utility.GROUP_TYPE.gtMAIN_GROUP;
                }


                strSQL = "INSERT INTO INV_STOCKGROUP";
                strSQL = strSQL + "(STOCKGROUP_NAME,STOCKGROUP_PARENT,STOCKGROUP_ONE_DOWN,STOCKGROUP_PRIMARY,";
                strSQL = strSQL + "STOCKGROUP_LEVEL,STOCKGROUP_PRIMARY_TYPE,STOCKGROUP_SECONDARY_TYPE,";
                if (collection.NAME != "")
                {
                    strSQL = strSQL + "GR_NAME,";
                }
                strSQL = strSQL + "STOCKGROUP_USE_PACK_SIZE,G_STATUS,DIL_EFFECT_INF,STOCKGROUP_OPENING_VALUE) ";
                strSQL = strSQL + "VALUES(";
                strSQL = strSQL + "'" + strGroup + "','" + strParent + "',";
                strSQL = strSQL + "'" + strOneDown + "','" + strPrimary + "',";
                strSQL = strSQL + lngGrType + "," + lngManuFacPrimary + "," + lngManuFacSecondary + " ";
                if (collection.NAME != "")
                {
                    strSQL = strSQL + ",'" + collection.NAME + "'";
                }

                strSQL = strSQL + "," + collection.G_STATUS+ " ";
                strSQL = strSQL + "," + collection.G_STATUS + " ";
                strSQL = strSQL + "," + collection.G_STATUS + " ";
                strSQL = strSQL + ", 0 ";
                strSQL = strSQL + ")";
                cmdInsert.CommandText = strSQL;
                cmdInsert.ExecuteNonQuery();
                cmdInsert.Transaction.Commit();
                gcnMain.Close();
                //return "Ok";
                return Json("Ok", JsonRequestBehavior.AllowGet);
            
            }

        }




      
        //
        // GET: /StockGroup/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /StockGroup/Edit/5

        [HttpPost]
        public ActionResult Update(Group collection)
        {
            try
            {
                // TODO: Add update logic here


                string strGroup,  strPrimary, strParent = "", strOneDown = "";
                int gStatus;
                double gID;
                long lngGrType = 0, lngManuFacPrimary = 0, lngManuFacSecondary = 0;
                string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection gcnMain = new SqlConnection(connectionString))
                {
                    if (gcnMain.State == ConnectionState.Open)
                    {
                        gcnMain.Close();
                    }

                    gcnMain.Open();

                    //strPrimary = Utility.GetEndGroup(vstrUnder);
                    strPrimary = collection.PARENT;
                    strGroup = collection.NAME.Replace("'", "''");
                    strParent = collection.PARENT.Replace("'", "''");
                    gStatus = collection.G_STATUS;
                    gID = collection.SERIAL;

                    SqlDataReader drGetGroup;
                    SqlCommand cmdInsert = new SqlCommand();
                    SqlTransaction myTrans;
                    myTrans = gcnMain.BeginTransaction();
                    cmdInsert.Connection = gcnMain;
                    cmdInsert.Transaction = myTrans;

                    strSQL = "SELECT STOCKGROUP_PRIMARY,STOCKGROUP_LEVEL,STOCKGROUP_PARENT,STOCKGROUP_ONE_DOWN,";
                    strSQL = strSQL + "STOCKGROUP_PRIMARY_TYPE,STOCKGROUP_SECONDARY_TYPE FROM INV_STOCKGROUP ";
                    strSQL = strSQL + "WHERE STOCKGROUP_NAME = '" + strParent + "' ";
                    cmdInsert.CommandText = strSQL;
                    drGetGroup = cmdInsert.ExecuteReader();
                    if (drGetGroup.Read())
                    {
                        lngGrType = Convert.ToInt32(drGetGroup["STOCKGROUP_LEVEL"]) + 1;
                        lngManuFacPrimary = Convert.ToInt32(drGetGroup["STOCKGROUP_PRIMARY_TYPE"].ToString());
                        lngManuFacSecondary = Convert.ToInt32(drGetGroup["STOCKGROUP_SECONDARY_TYPE"].ToString());
                        if (drGetGroup["STOCKGROUP_PRIMARY"].ToString() == strParent)
                        {
                            strOneDown = strGroup;
                            strPrimary = strParent;
                        }
                        else
                        {
                            strPrimary = drGetGroup["STOCKGROUP_PRIMARY"].ToString();
                            strOneDown = drGetGroup["STOCKGROUP_PARENT"].ToString();
                        }
                    }
                    drGetGroup.Close();
                    if (collection.PARENT.ToUpper() == "PRIMARY")
                    {
                        strParent = strGroup;
                        strPrimary = strGroup;
                        strOneDown = strGroup;
                        lngGrType = (long)Utility.GROUP_TYPE.gtMAIN_GROUP;
                    }


                    strSQL = "UPDATE INV_STOCKGROUP SET ";
                    strSQL += "STOCKGROUP_NAME= '" + strGroup + "' ";
                    strSQL += ",STOCKGROUP_PRIMARY= '" + strPrimary + "' ";
                    strSQL += ",G_STATUS= " + gStatus + " ";
                    strSQL += "WHERE STOCKGROUP_SERIAL= " + Convert.ToInt16(gID) + " ";

                    cmdInsert.CommandText = strSQL;
                    cmdInsert.ExecuteNonQuery();
                    cmdInsert.Transaction.Commit();
                    gcnMain.Close();
                    //return "Ok";
                    return Json("Ok", JsonRequestBehavior.AllowGet);

                }

                //return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult DeleteG(int id)
        {

            string strGroupName = "", strResponse = "", strDefaultName = "";
            bool blnDelete = false;
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

                    SqlDataReader rsGet;
                    SqlCommand cmdDelete = new SqlCommand();
                    SqlTransaction myTrans;
                    myTrans = gcnMain.BeginTransaction();
                    cmdDelete.Connection = gcnMain;
                    cmdDelete.Transaction = myTrans;
                    strSQL = "SELECT STOCKGROUP_NAME, STOCKGROUP_PARENT,STOCKGROUP_NAME_DEFAULT, STOCKGROUP_PRIMARY FROM INV_STOCKGROUP WHERE STOCKGROUP_SERIAL = " + id + " ";
                    cmdDelete.CommandText = strSQL;
                    rsGet = cmdDelete.ExecuteReader();
                    if (rsGet.Read())
                    {
                        strGroupName = rsGet["STOCKGROUP_NAME"].ToString();
                        if (rsGet["STOCKGROUP_NAME_DEFAULT"].ToString() != "")
                        {
                            strDefaultName = rsGet["STOCKGROUP_NAME_DEFAULT"].ToString();
                        }
                        else
                        {
                            strDefaultName = "";
                        }
                    }
                    rsGet.Close();
                    strSQL = "SELECT STOCKGROUP_NAME FROM INV_STOCKGROUP WHERE STOCKGROUP_PARENT = '" + strGroupName + "' ";
                    strSQL = strSQL + "AND STOCKGROUP_NAME <> STOCKGROUP_PARENT ";
                    cmdDelete.CommandText = strSQL;
                    rsGet = cmdDelete.ExecuteReader();
                    if (rsGet.Read())
                    {
                        strResponse = "    Sub-Group/Transaction Found     ";
                        return Json("TF", JsonRequestBehavior.AllowGet);
                    }
                    rsGet.Close();

                    if (strDefaultName != "")
                    {
                        strResponse = "Default Group Can't Delete";
                        return Json("DF", JsonRequestBehavior.AllowGet);
                    }


                    //strSQL = "SELECT STOCKGROUP_NAME FROM INV_STOCKITEM WHERE STOCKGROUP_NAME = '" + strGroupName + "' ";
                    //cmdDelete.CommandText = strSQL;
                    //rsGet = cmdDelete.ExecuteReader();
                    //if (rsGet.Read())
                    //{
                    //    blnDelete = true;
                    //}
                    //rsGet.Close();
                    strGroupName = strGroupName.Replace("'", "''");
                    if (blnDelete == false)
                    {

                        //strSQL = "DELETE FROM USER_PRIVILEGES_STOCKGROUP ";
                        //strSQL = strSQL + "WHERE STOCKGROUP_NAME = '" + strGroupName + "'";
                        //cmdDelete.CommandText = strSQL;
                        //cmdDelete.ExecuteNonQuery();
                        strSQL = "DELETE FROM INV_STOCKGROUP ";
                        strSQL = strSQL + "WHERE STOCKGROUP_NAME = '" + strGroupName + "'";
                        cmdDelete.CommandText = strSQL;
                        cmdDelete.ExecuteNonQuery();
                        strResponse = "Deleted...";
                    }
                    cmdDelete.Transaction.Commit();
                    gcnMain.Close();
                    rsGet.Close();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }

                catch (Exception ex)
                {
                    strResponse = "Transaction Found Cannot Delete...";
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    gcnMain.Close();
                }

            }




        }

       
    }
}
