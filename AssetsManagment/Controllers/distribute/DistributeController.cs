using AssetsManagment.Models;
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


	}
}