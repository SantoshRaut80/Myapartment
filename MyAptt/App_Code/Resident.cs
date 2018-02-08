using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Resident
/// </summary>
public class Resident
{
    String ViewName = "dbo.ViewResidents";
    string Table_Name = "dbo.Resident";
    string Table_TotalUser = "dbo.TotalUsers";

    public Resident()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public DataSet GetResidentAll()
    {
        DataSet dsResident = null;
        try
        {
            DataAccess dacess = new DataAccess();
            String UserSearchQuery = "select * from " + ViewName;
            dsResident = dacess.GetData(UserSearchQuery);

        }
        catch (Exception ex)
        {
        }
        return dsResident;
    }

    public DataSet GetResidentUserFlat(string UserName, String FlatNumber)
    {
        DataSet dsResidentUserFlat = null;
        try
        {
            String flatCond = "", userNameCond = "";
            if (FlatNumber == "")
            {
                flatCond = "FlatNumber is not null";
            }
            else
            {
                flatCond = "FlatNumber = '" + FlatNumber + "'";
            }

            if (UserName == "Select")
            {
                userNameCond = "FirstName is not null";
            }
            else
            {
                userNameCond = "FirstName like '" + UserName + "%'";
            }


            DataAccess dacess = new DataAccess();
            String UserSearchQuery = "select * from " + ViewName + " Where " + userNameCond + " and " + flatCond;
            dsResidentUserFlat = dacess.GetData(UserSearchQuery);
        }
        catch (Exception ex)
        {
        }
        return dsResidentUserFlat;
    }

    public DataSet GetResident(String FlatNumber, String ResType)
    {        
        try
        {           
            String ResidentGenQuery = "";

            if (FlatNumber != "" && ResType != "")
            {
                ResidentGenQuery = "select * from " + ViewName + " where FlatNumber ='" + FlatNumber + "' and Type = '" + ResType + "'";
            }


            else if (FlatNumber != "" && ResType == "")
            {
                ResidentGenQuery = "  select * from " + ViewName + " where FlatNumber ='" + FlatNumber + "'";
            }

            else if (FlatNumber == "" && ResType != "")
            {
                ResidentGenQuery = "select * from " + ViewName + " where Type ='" + ResType + "'";
            }

            else if (FlatNumber == "" && ResType == "")
            {
                ResidentGenQuery = "select * from " + ViewName;
            }

            DataAccess dacess = new DataAccess();
            return dacess.GetData(ResidentGenQuery);
        }
        catch (Exception ex)
        {
            return null;
        }
       
    }

    
    public bool UpdateResidentDeactive(DateTime Date, string UserID)
    {
        DataAccess dacess = new DataAccess();
        String DeactiveUserQuery = "Update " + Table_Name + "  set DeActiveDate = '" + Date + "' where UserID = '" + UserID + "'";
        bool result = dacess.Update(DeactiveUserQuery);
        return result;
    }

    public bool InsertUserResident(string FirstName, string LastName, string MobileNo, string EmailId, string Gender, string Parentname, string UserLogin, string Address, string UserType, string SocietyID)
    {
        DataAccess dacess = new DataAccess();
        String UpdateQuery = "Insert into " + Table_TotalUser + " (FirstName, MiddleName,LastName,MobileNo,EmailId,Gender,Parentname,UserLogin,Address,UserType,SocietyID) Values('" + FirstName + "','','" + LastName + "','" + MobileNo + "','" + EmailId + "','" + Gender + "','" + Parentname + "','" + UserLogin + "','" + Address + "','" + UserType + "','" + SocietyID + "')";
        bool result = dacess.Update(UpdateQuery);
        return result;
    }

    public int GetUserAvailable(string UserName)
    {
        DataAccess dacess = new DataAccess();
        String UserAvailbleQuery = "Select  * from " + Table_TotalUser + " where UserLogin = '" + UserName + "'";
        int UserID = dacess.GetSingleUserValue(UserAvailbleQuery);
        return UserID;
    }

    public DataSet GetEmailAvailable(String EmailID)
    {
        DataAccess dacess = new DataAccess();
        String EmailAvailableQuery = "Select * from " + Table_TotalUser + " where EmailId = '" + EmailID + "' ";
        DataSet dUser = dacess.GetUserData(EmailAvailableQuery);
        return dUser;
    }

    public DataSet GetDeactiveCheck(String UserID)
    {
        DataAccess dacess = new DataAccess();
        String CheckDeactiveQuery = "Select DeActiveDate from " + Table_Name + " where UserID = '" + UserID + "' ";
        DataSet ds = dacess.GetData(CheckDeactiveQuery);
        return ds;
    }

    public bool UpdateUserResident(string FirstName, string MiddleName, string LastName, string EmailId, string ParentName, string MobileNo, string UserID)
    {
        DataAccess dacess = new DataAccess();
        String UpdateQuery = "Update " + Table_TotalUser + "  SET Firstname='" + FirstName + "', MiddleName='" + MiddleName + "', LastName='" + FirstName + "',Emailid='" + EmailId + "',Parentname= '" + ParentName + "' ,MobileNo='" + MobileNo + "' WHERE UserID ='" + UserID + "'";
        bool result = dacess.Update(UpdateQuery);
        return result;
    }

    public bool UpdateResident(string FirstName, string LastName, string EmailId, string MobileNo, string UserID)
    {
        DataAccess dacess = new DataAccess();
        String UpdatReseQuery = "Update " + Table_Name + " SET Firstname='" + FirstName + "', LastName='" + LastName + "',Emailid='" + EmailId + "',MobileNo='" + MobileNo + "' WHERE UserID ='" + UserID + "'";
        bool result = dacess.Update(UpdatReseQuery);
        return result;
    }

    public int GetEditMobileNoAvailable(String UserEditMobileNo)
    {
        DataAccess dacess = new DataAccess();
        String UserAvailbleQuery = "Select  UserID from " + Table_TotalUser + " where MobileNo = '" + UserEditMobileNo + "'";
        int UserID = dacess.GetSingleUserValue(UserAvailbleQuery);
        return UserID;
    }
    public int GetEditEmailIDAvailable(String UserEditEmailID)
    {
        DataAccess dacess = new DataAccess();
        String EmailAvailableQuery = "Select UserID from " + Table_TotalUser + " where EmailId = '" + UserEditEmailID + "'";
        int UserID = dacess.GetSingleUserValue(EmailAvailableQuery);
        return UserID;
    }

    public User GetEditResidentData(String UserId, String FlatNumber)
    {
        User editUSer = new User();
        DataAccess dacess = new DataAccess();
        SqlConnection con = dacess.ConnectUserDB();
        {
            DataTable dt = new DataTable();

            String EditUserQuery = "select * from " + Table_TotalUser + " where UserID ='" + UserId + "'";
            SqlCommand myCommand = new SqlCommand(EditUserQuery, con);
            SqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
               
                editUSer.FlatNumber = FlatNumber;
                editUSer.strFirstName = (myReader["FirstName"].ToString());
                editUSer.strMiddleName = (myReader["MiddleName"].ToString());
                editUSer.strLastName = (myReader["LastName"].ToString());
                editUSer.strEmailID = (myReader["Emailid"].ToString());
                editUSer.ParentName = (myReader["Parentname"].ToString().Trim());
                editUSer.strMobileNumber = (myReader["MobileNo"].ToString());
                editUSer.UserLogin = (myReader["UserLogin"].ToString());
                editUSer.strUserID = UserId;
                HttpContext.Current.Session.Add("EditUser", editUSer);
                HttpContext.Current.Session["UserID"] = UserId;           
            }
            con.Close();
        }
        return editUSer;
    }

    public List<string> GetFlatNumber(string FlatNumber)
    {
        List<string> Emp = new List<string>();
        //Changed View name by aarshi on 4 aug 2017, it was throwing exception for ViewOwnerResidents not exist
        //string query = string.Format("Select distinct FlatNumber from dbo.ViewOwnerResidents where FlatNumber like '" + FlatNumber + "%'");

        string query = string.Format("Select distinct FlatNumber from " + ViewName + " where FlatNumber like '" + FlatNumber + "%'");
        using (SqlConnection con = new SqlConnection(Utility.SocietyConnectionString))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Emp.Add(reader.GetString(0));

                }
            }
        }
        return Emp;
    }

    public bool InsertUserSetting(int ResID)
    {
        try
        {
            String strInsertQuery = "INSERT INTO ResidentNotification (BillingNotification, BillingMail, forumNotice, forumMail, ComplaintNotification, ComplaintMail, FlatMail,ResID) VALUES (1,1,1,1,1,1,1, " + ResID + ")";
            DataAccess da = new DataAccess();

            return da.Update(strInsertQuery);
        }
        catch (Exception ex)
        {

            return false;
        }
    
    }

    public bool UpdateUserSetting(int BillNotice, int BillMail, int forumNotice, int forumMail, int compNotice, int compMail, int ResID)
    {
        try
        {
            String strUpdateQuery = "UPDATE [dbo].[ResidentNotification] SET BillingNotification = " + BillNotice +
                                      ",[ComplaintNotification] = " + compNotice +
                                      ",[forumNotice] = " + forumNotice +
                                      ",[BillingMail] = " + BillMail +
                                      ",[ComplaintMail] = " + compMail +
                                      ",[forumMail] = " + forumMail + " WHERE ResID = " + ResID;

           

            DataAccess da = new DataAccess();

            return da.Update(strUpdateQuery);
        }
        catch (Exception ex)
        {

            return false;
        }
    }
}