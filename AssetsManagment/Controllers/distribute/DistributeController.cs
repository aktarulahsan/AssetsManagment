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

        string strSQL;

        public JsonResult getGroupList(string strDeComID)
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



        public JsonResult saveIssue(IssueMester resObje)
        {
            IssueMester objIssue = new IssueMester();
            //objstok.gLocationList.ToList();


            objIssue = resObje;

            long LNGSLNO=0;

          
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
                    SqlCommand cmdInsert = new SqlCommand();
                    SqlTransaction myTrans;
                   
                    myTrans = gcnMain.BeginTransaction();
                    cmdInsert.Connection = gcnMain;
                    cmdInsert.Transaction = myTrans;

                    string strAlias = "";

                    DateTime createdate = DateTime.Now;

                    string stydate = Convert.ToString(createdate).ToString();
                    strSQL = "INSERT INTO ISSUE_MASTER";
                    strSQL = strSQL + "(DEPARTMENT_NAME ,CARD_NO,EMP_NAME,ISSUE_DATE,";
                    strSQL = strSQL + "CREATED_BY,CREATE_DATE)";
                    
                    strSQL = strSQL + "VALUES(";
                    strSQL = strSQL + "'" + objIssue.department_name + "'";
                    strSQL = strSQL + ", '" + objIssue.card_no + "'";
                    strSQL = strSQL + ",'" + objIssue.emp_name + "'";
                    strSQL = strSQL + "," + Utility.cvtSQLDateString(objIssue.issue_date.ToString()) + "";
                    strSQL = strSQL + ",'" + objIssue.created_by + "'";
                    strSQL = strSQL + "," + Utility.cvtSQLDateString(stydate) + "";
                    //strSQL = strSQL + ",'" + objIssue.updated_by + "'";
                    //strSQL = strSQL + "," + Utility.cvtSQLDateString(objIssue.update_date.ToString()) + " ";
                    strSQL = strSQL + ")";
                    cmdInsert.CommandText = strSQL;
                    cmdInsert.ExecuteNonQuery();

                    strSQL = "SELECT MAX(ISSUE_NO) ISSUE_NO FROM ISSUE_MASTER ";
                    cmdInsert.CommandText = strSQL;
                    dr = cmdInsert.ExecuteReader();
                    if (dr.Read())
                    {
                        LNGSLNO = Convert.ToInt64(dr["ISSUE_NO"].ToString());
                    }
                    dr.Close();

                    if (objIssue.gIssueDetailsList.Count > 0)
                    {
                        foreach (var item in objIssue.gIssueDetailsList)
                        {

                            strSQL = "INSERT INTO ISSUE_DETAILS ";
                            strSQL = strSQL + "(ISSUE_NO,ITEM_CODE,ITEM_NAME,ITEM_QTY,CREATE_BY,CREATE_DATE";
                           

                            strSQL = strSQL + ") ";
                            strSQL = strSQL + "VALUES('" + LNGSLNO + "',";
                            strSQL = strSQL + "" + item.item_code + ",";
                            strSQL = strSQL + "'" + item.item_name + "'," + item.item_qty + ",";
                            strSQL = strSQL + "'" + item.create_by + "'," + item.create_date + ",";
                  

                            strSQL = strSQL + ") ";



                            cmdInsert.CommandText = strSQL;
                            cmdInsert.ExecuteNonQuery();
                     

                        }

                    }






                    cmdInsert.Transaction.Commit();
                    gcnMain.Close();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    gcnMain.Close();
                }
            }

 

        }
	}
}