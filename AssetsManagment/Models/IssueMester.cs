using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetsManagment.Models
{
    public class IssueMester
    {
 
            public  int issue_no   { get; set; } 
            public  string department_name   { get; set; }
            public  string card_no   { get; set; }
            public  string emp_name   { get; set; }
            public  DateTime issue_date   { get; set; }
            public  string created_by   { get; set; }
            public DateTime create_date { get; set; }
            public  string updated_by   { get; set; }
            public DateTime update_date { get; set; }

            public List<IssueDetails> gIssueDetailsList { get; set; }

            public void InitialList()
            {
                gIssueDetailsList = new List<IssueDetails>();
            }
    }



    public class IssueDetails
    {

    public int department_code { get; set; }
    public int issue_d_no { get; set; }
    public int issue_no { get; set; }
    public string item_code { get; set; }
    public string item_name { get; set; }
    public int item_qty { get; set; }
    public string create_by { get; set; }
    public DateTime create_date { get; set; }
    }
}


//CREATE TABLE ISSUE_MASTER (
//ISSUE_NO int IDENTITY(1,1) PRIMARY KEY,
//DEPARTMENT_NAME varchar(45) default NULL,
//CARD_NO varchar(10) default NULL,
//EMP_NAME varchar(10) default NULL,
//ISSUE_DATE date default NULL,
//CREATED_BY varchar(50) default NULL,
//CREATE_DATE datetime default NULL,
//UPDATED_BY varchar(45) default NULL,
//UPDATE_DATE date default NULL,
//) 



//        CREATE TABLE ISSUE_DETAILS (
//        ISSUE_D_NO int IDENTITY(1,1) PRIMARY KEY,
//        ISSUE_NO int ,
//        ITEM_CODE varchar(50) default NULL,
//        ITEM_NAME varchar(50) default NULL,
//        ITEM_QTY NUMERIC(10) default 0 NOT NULL,
//        CREATE_BY varchar(45) default NULL,
//        CREATE_DATE datetime default NULL,
//        ) 