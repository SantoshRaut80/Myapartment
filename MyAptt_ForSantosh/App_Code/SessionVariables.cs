using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
/// <summary>
/// Summary description for SessionsVariables
/// </summary>
public class SessionVariables
{
		//
		// TODO: Add constructor logic here
		//

          //public static HttpContext.Current.Session["User"] usersession;

    public static User User
    {
        get
        {
            return (User)HttpContext.Current.Session["User"];
        }

        set
        {
            HttpContext.Current.Session["User"] = value;
        }
    }

    public static string FName
    {
        get
        {
            return (string)HttpContext.Current.Session["FirstName"];
        }

        set
        {
            HttpContext.Current.Session["FirstName"] = value;
        }
    }


    public static string LName
    {
        get
        {
            return (string)HttpContext.Current.Session["LastName"];
        }

        set
        {
            HttpContext.Current.Session["LastName"] = value;
        }
    }


    public static string UserType
    {
        get
        {
            return (string)HttpContext.Current.Session["UserType"];
        }

        set
        {
            HttpContext.Current.Session["UserType"] = value;
        }
    }

    public static int ResiID
    {
        get
        {
            return (int)HttpContext.Current.Session["ResiID"];
        }

        set
        {
            HttpContext.Current.Session["ResiID"] = value;
        }
    }

    public static string UserLogin
    {
        get
        {
            return (string)HttpContext.Current.Session["UserLogin"];
        }

        set
        {
            HttpContext.Current.Session["UserLogin"] = value;
        }
    }

    public static string SocietyName
    {
        get
        {
            return (string)HttpContext.Current.Session["SocietyName"];
        }

        set
        {
            HttpContext.Current.Session["SocietyName"] = value;
        }
    }

    public static string FlatNumber
    {
        get
        {
            return (string)HttpContext.Current.Session["FlatNumber"];
        }

        set
        {
            HttpContext.Current.Session["FlatNumber"] = value;
        }
    }


    public static string Path
    {
        get
        {
            return (string)HttpContext.Current.Session["Path"];
        }

        set
        {
            HttpContext.Current.Session["Path"] = value;
        }
    }

    public static DataTable ExcelData
    {
        get
        {
            return (DataTable)HttpContext.Current.Session["ExcelData"];
        }

        set
        {
            HttpContext.Current.Session["ExcelData"] = value;
        }
    }

    public static DataTable SocietyBillPlans
    {
        get
        {
            return (DataTable)HttpContext.Current.Session["SocietyBillPlans"];
        }

        set
        {
            HttpContext.Current.Session["SocietyBillPlans"] = value;
        }
    }

    public static DataTable NewBills
    {
        get
        {
            return (DataTable)HttpContext.Current.Session["NewBills"];
        }

        set
        {
            HttpContext.Current.Session["NewBills"] = value;
        }
    }

    public static string PayID
    {
        get
        {
            return (string)HttpContext.Current.Session["PayID"];
        }

        set
        {
            HttpContext.Current.Session["PayID"] = value;
        }
    }

    public static string Rate
    {
        get
        {
            return (string)HttpContext.Current.Session["Rate"];
        }

        set
        {
            HttpContext.Current.Session["Rate"] = value;
        }
    }

    public static string ChargeType
    {
        get
        {
            return (string)HttpContext.Current.Session["ChargeType"];
        }

        set
        {
            HttpContext.Current.Session["ChargeType"] = value;
        }
    }

    public static string FlatArea
    {
        get
        {
            return (string)HttpContext.Current.Session["FlatArea"];
        }

        set
        {
            HttpContext.Current.Session["FlatArea"] = value;
        }
    }

    public static string UserID
    {
        get
        {
            return (string)HttpContext.Current.Session["UserID"];
        }

        set
        {
            HttpContext.Current.Session["UserID"] = value;
        }
    }

    public static DateTime Deactive
    {
        get
        {
            return (DateTime)HttpContext.Current.Session["Deactive"];
        }

        set
        {
            HttpContext.Current.Session["Deactive"] = value;
        }
    }

    public static int SelectedAnswerChart1
    {
        get
        {
            return (int)HttpContext.Current.Session["SelectedAnswerChart1"];
        }

        set
        {
            HttpContext.Current.Session["SelectedAnswerChart1"] = value;
        }
    }

    public static int SelectedAnswerChart2
    {
        get
        {
            return (int)HttpContext.Current.Session["SelectedAnswerChart2"];
        }

        set
        {
            HttpContext.Current.Session["SelectedAnswerChart2"] = value;
        }
    }

    public static string VendorID
    {
        get
        {
            return (string)HttpContext.Current.Session["VendorID"];
        }

        set
        {
            HttpContext.Current.Session["VendorID"] = value;
        }
    }

    public static int ID
    {
        get
        {
            return (int)HttpContext.Current.Session["ID"];
        }

        set
        {
            HttpContext.Current.Session["ID"] = value;
        }
    }

    public static DataTable ServerData
    {
        get
        {
            return (DataTable)HttpContext.Current.Session["ServerData"];
        }

        set
        {
            HttpContext.Current.Session["ServerData"] = value;
        }
    }

    public static DataTable Duplicate
    {
        get
        {
            return (DataTable)HttpContext.Current.Session["Duplicate"];
        }

        set
        {
            HttpContext.Current.Session["Duplicate"] = value;
        }
    }

    public static DataTable Newvalues
    {
        get
        {
            return (DataTable)HttpContext.Current.Session["Newvalues"];
        }

        set
        {
            HttpContext.Current.Session["Newvalues"] = value;
        }
    }
	
}