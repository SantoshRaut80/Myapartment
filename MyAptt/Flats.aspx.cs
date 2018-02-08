using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Text;

public partial class Flats : System.Web.UI.Page
{

    User muser;
    static User newUser;
    static bool ExistingUser = false;
    static int ExistingUserID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Added by Aarshi on 18 auh 2017 for session storage
        //muser = (User)Session["User"];
        muser = SessionVariables.User;
        if (muser == null)
        {
            Response.Redirect("Login.aspx");
        }

        if (!IsPostBack)
        {
            FillFlatdata();
            btnFltsShwall.Visible = false;
        }

        ClientScript.RegisterStartupScript(this.GetType(), "alert('')", "PopupFlats()", true);
    }

    public void FillFlatdata()
    {
        String querystring = "Select * from dbo.Flats";
        DataAccess dacess = new DataAccess();
        DataSet ds = dacess.GetData(querystring);
        if (ds != null)
            if (ds.Tables.Count > 0)
            {
                

                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblFlatGridEmptyText.Text = "Hello! Welcome to  the Society Flats, Add a Flat Here.";
                }

                else
                {
                    FlatGrid.DataSource = ds;
                    FlatGrid.DataBind();
                    btnFltsShwall.Visible = false;
                }
            }
            else
            {
                lblFlatGridEmptyText.Text = "Unable to retrieve data.";
            }
        else {
            lblFlatGridEmptyText.Text = "Unable to retrieve data.";
        }
        string TotalCountQuery = "select count(FlatNumber) from dbo.Flats";
        lblTotalFlats.Text = (dacess.GetSingleValue(TotalCountQuery)).ToString();
    }

    protected void FlatGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        FlatGrid.PageIndex = e.NewPageIndex;

        FillFlatdata();
    }
    protected void btnFlatnumbrSrch_Click(object sender, EventArgs e)
    {
        try
        {
            String FlatNumber = txtFltsFlatNmbr.Text;
            String OwnerName = txtFlltsOwnernme.Text;
            DataAccess dacess = new DataAccess();

            String FlatNumberQuery = "";

            if (OwnerName == "" & FlatNumber != "")
            {
                FlatNumberQuery = "Select * from dbo.Flats where FlatNumber like  '" + FlatNumber + "%'";
            }

            if (OwnerName != "" & FlatNumber == "")
            {
                FlatNumberQuery = "Select * from dbo.Flats where OwnerName like  '%" + OwnerName + "%'";
            }

            if (OwnerName != "" & FlatNumber != "")
            {
                FlatNumberQuery = "Select * from dbo.Flats where FlatNumber like  '" + FlatNumber + "%'  and OwnerName like  '%" + OwnerName + "%'";
            }

            DataSet ds = dacess.GetData(FlatNumberQuery);

            if (ds.Tables.Count > 0)
            {
                FlatGrid.DataSource = ds;
                FlatGrid.DataBind();
                btnFltsShwall.Visible = true;
            }
        }


        catch (Exception ex)
        {
            //Utility.log("Flats :btnFlatnumbrSrch_Click :" +ex.Message);
        }
    }

    protected void btnFltsShwall_Click(object sender, EventArgs e)
    {
        FillFlatdata();
        var txt = txtFltsFlatNmbr.Text;
        txtFltsFlatNmbr.Text = "";

        var txtOwnerName = txtFlltsOwnernme.Text;
        txtFlltsOwnernme.Text = "";
    }

    protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (txtFlltsOwnernme.Text == "" & txtFltsFlatNmbr.Text == "")
        {
            args.IsValid = false;
        }
    }

    //=---------------------------------------------------------Import Flat Data Section starts from here =------------------------------------------------------------------------------------------//
    protected void FlatsDataUploadSubmit_Click(object sender, EventArgs e)
    {
        if (FlatsDataUpload.HasFile)
        {
            string strFileName = string.Empty;
            string strdate = string.Empty;
            string strMonth = string.Empty;
            string strYear = string.Empty;
            string strTime = string.Empty;
            string strfullDate = string.Empty;

            //  strMonth = System.DateTime.Now.ToLongDateString();
            strMonth = Utility.GetCurrentDateTimeinUTC().ToLongDateString();
            string[] strMonthName = strMonth.ToString().Split(',');

            strMonth = strMonthName[1].ToString();
            string[] strMonthValue = strMonth.ToString().Split(' ');
            strMonth = strMonthValue[1].ToString();
            strdate = strMonthValue[2].ToString();
            strYear = strMonthName[2].ToString();

            // strTime = System.DateTime.Now.ToLongTimeString();
            strTime = Utility.GetCurrentDateTimeinUTC().ToLongTimeString();

            strTime = strTime.ToString().Replace(':', '-');
            strfullDate = strdate + "-" + strMonth + "-" + strYear + "-" + strTime;

            string path = strfullDate + "-" + FlatsDataUpload.PostedFile.FileName;

            String Extension = Path.GetExtension(FlatsDataUpload.PostedFile.FileName);
            path = Server.MapPath(Path.Combine("~/Data/", path));

            FlatsDataUpload.SaveAs(path);
            ImportDataFromExcel(path, Extension);

        }
        else
        {

            lblFileUploadstatus.Text = "File Not found";
            ClientScript.RegisterStartupScript(this.GetType(), "alert()", "alert('Select a file to upload')", true);

        }
    }


    public void ImportDataFromExcel(String path, String Extension)
    {
        OleDbConnection Connection = null;
        try
        {
            String Connectionstring = String.Empty;

            switch (Extension)
            {
                case ".xls":

                    Connectionstring = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;

                    break;

                case ".xlsx":

                    Connectionstring = ConfigurationManager.ConnectionStrings["Excel07conString"].ConnectionString;

                    break;
            }

            Connectionstring = String.Format(Connectionstring, path);
            //Session["Path"] = path;
            //Added by Aarshi on 18 auh 2017 for session storage
            SessionVariables.Path = path;
            Connection = new OleDbConnection(Connectionstring);

            Connection.Open();

            String insertQuery = "select * from [Sheet1$]";

            OleDbDataAdapter adp = new OleDbDataAdapter(insertQuery, Connection);

            DataTable DataExcel = new DataTable();

            adp.Fill(DataExcel);
            //Session["ExcelData"] = DataExcel;
            //Added by Aarshi on 18 auh 2017 for session storage
            SessionVariables.ExcelData = DataExcel;
            DataAccess dacess = new DataAccess();
            String PickDbData = "select * from dbo.Flats";

            DataSet dtFlatData = dacess.ReadData(PickDbData);
            DataTable DBFlatData = new DataTable();
            if (dtFlatData == null)
            {

            }
            else
            {
                DBFlatData = dtFlatData.Tables[0];

                //Session["ServerData"] = DBFlatData;
                //Added by Aarshi on 18 auh 2017 for session storage
                SessionVariables.ServerData = DBFlatData;
            }


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
            //-----------------------------------------------------------------Already Exist Values Duplicate-------------------------------------------------------------------//
            DataTable dtcompare = new DataTable();
            DataTable Duplicatedata = new DataTable();

            var compare2 = DataExcel.AsEnumerable().Select(r => r.Field<String>("FlatNumber")).Intersect(DBFlatData.AsEnumerable().Select(r => r.Field<String>("FlatNumber")));

            if (compare2.Any())
            {
                Duplicatedata = (from row in DataExcel.AsEnumerable() join FlatNumber in compare2 on row.Field<String>("FlatNumber") equals FlatNumber select row).CopyToDataTable();

                ImportduplicateGrid.DataSource = Duplicatedata;
                //Session["Duplicate"] = Duplicatedata;
                //Added by Aarshi on 18 auh 2017 for session storage
                SessionVariables.Duplicate = Duplicatedata;
                btnselsectdata.Text = "Update";
                ImportduplicateGrid.DataBind();
                //  ClientScript.RegisterStartupScript(this.GetType(), "alert('working')", "PopupFlats()", true);
                String url = "ImportData.aspx";
                ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
                HiddenImportPopup.Value = "2";
                FlatGrid.Visible = false;
                FlatGrid.Height = 0;
                lblDuplicate.Visible = true;
                //   lblDuplicate.Style["height"] = "200px";
                chkFlats.Visible = true;
                lblTotalFlats.Visible = false;
                totalFlats.Visible = false;

            }

            else
            {
                lblDuplicate.Visible = false;
                chkFlats.Visible = false;
                DuplicateScroll_Div.Visible = false;

            }
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
            //---------------------------------------------------------------------------New Entry Values------------------------------------------------------------------------------//

            DataTable Newdata = new DataTable();

            var intersect = DataExcel.AsEnumerable().Select(r => r.Field<String>("FlatNumber")).Except(DBFlatData.AsEnumerable().Select(r => r.Field<String>("FlatNumber")));

            if (intersect.Count() > 0 && intersect.Any())
            {
                Newdata = (from row in DataExcel.AsEnumerable() join FlatNumber in intersect on row.Field<String>("FlatNumber") equals FlatNumber select row).CopyToDataTable();

                String BillsApplyToQuery = "select * from dbo.Societybillplans where  Applyto =1";
                DataSet SocietyBillsDataset = dacess.ReadData(BillsApplyToQuery);
                btnselsectdata.Text = "Import";
                if (SocietyBillsDataset == null)
                {

                }
                else
                {
                    DataTable BillsApplyToAll = SocietyBillsDataset.Tables[0];
                    DataTable tempBillCycle = new DataTable();
                    DataRow dr = null;
                    tempBillCycle.Columns.Add("FlatNumber");
                    tempBillCycle.Columns.Add("BillID", typeof(int));
                    tempBillCycle.Columns.Add("CycleStart", typeof(DateTime));
                    tempBillCycle.Columns.Add("CycleEnD", typeof(DateTime));

                    foreach (DataRow ExcelRow in Newdata.Rows)
                    {
                        foreach (DataRow drBills in BillsApplyToAll.Rows)
                        {
                            String Flat = ExcelRow["FlatNumber"].ToString();
                            int BillID = (int)drBills["BillID"];
                            DateTime Cuurrdate = Utility.GetCurrentDateTimeinUTC();
                            DateTime Nexmonth = Cuurrdate.AddMonths(+1);
                            int month = Nexmonth.Month;
                            int year = Nexmonth.Year;
                            DateTime CycleEnD = new DateTime(year, month, 1);
                            tempBillCycle.Rows.Add(Flat, BillID, Cuurrdate, CycleEnD);
                        }
                    }

                    //Session["SocietyBillPlans"] = tempBillCycle;
                    //Added by Aarshi on 18 auh 2017 for session storage
                    SessionVariables.SocietyBillPlans = tempBillCycle;
                }

                ImportNewRecordGrid.DataSource = Newdata;
                ImportNewRecordGrid.DataBind();

                //Session["Newvalues"] = Newdata;
                //Added by Aarshi on 18 auh 2017 for session storage
                SessionVariables.Newvalues = Newdata;

                String url = "ImportData.aspx";
                ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
                HiddenImportPopup.Value = "3";
                FlatGrid.Visible = false;
                lblTotalFlats.Visible = false;
                totalFlats.Visible = false;


                if (Newdata.Rows.Count == 0)
                {
                    lblNewvalues.Visible = false;
                    NewValueScroll_Div.Visible = false;
                }
            }

            else
            {
                lblNewvalues.Visible = false;
                NewValueScroll_Div.Visible = false;
            }

        }
        catch (Exception ex)
        {
            lblNewvalues.Visible = false;


            NewValueScroll_Div.Visible = false;

            String message = ex.Message;

            lblFileUploadstatus.Text = ex.Message;
        }

        finally
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }

            Connection = null;
        }
    }

    public void FillServerData()
    {
        DataAccess dacess = new DataAccess();

        String SqlUserString = String.Empty;
        SqlUserString = Utility.MasterDBString;

        DataTable databulk = new DataTable();

        //databulk = (DataTable)Session["Newvalues"];
        //Added by Aarshi on 18 auh 2017 for session storage
        databulk = SessionVariables.Newvalues;
        try
        {
            databulk.Columns.Add("SocietyID");

            foreach (DataRow dr in databulk.Rows)
            {
                dr["SocietyID"] = muser.SocietyID;

            }

            SqlConnection UserCon = new SqlConnection(SqlUserString);
            UserCon.Open();
            using (SqlBulkCopy Usersdata = new SqlBulkCopy(UserCon))
            {
                Usersdata.DestinationTableName = "TotalUsers";
                Usersdata.ColumnMappings.Add("UserLogin", "UserLogin");
                Usersdata.ColumnMappings.Add("RandomPWD", "Password");
                Usersdata.ColumnMappings.Add("FirstName", "FirstName");
                Usersdata.ColumnMappings.Add("LastName", "LastName");
                Usersdata.ColumnMappings.Add("MobileNo", "MobileNo");
                Usersdata.ColumnMappings.Add("EmailId", "EmailId");
                Usersdata.ColumnMappings.Add("Gender", "Gender");
                Usersdata.ColumnMappings.Add("ParentName", "Parentname");
                Usersdata.ColumnMappings.Add("Address", "Address");
                Usersdata.ColumnMappings.Add("SocietyID", "SocietyID");
                Usersdata.ColumnMappings.Add("UserType", "UserType");
                Usersdata.WriteToServer(databulk);

            }
            UserCon.Close();


            String UserDataQuery = "Select UserID,UserLogin,MobileNo from dbo.TotalUsers where SocietyID ='" + muser.SocietyID + "'";
            DataSet UserData = dacess.ReadUserData(UserDataQuery);
            DataTable TempUserData = UserData.Tables[0];

            String SqlSocietyString = String.Empty;
            SqlSocietyString = Utility.SocietyConnectionString;
            SqlConnection SocietyCon = new SqlConnection(SqlSocietyString);
            SocietyCon.Open();

            using (SqlBulkCopy TempUsersdata = new SqlBulkCopy(SocietyCon))
            {
                TempUsersdata.DestinationTableName = "TempUsers";
                TempUsersdata.ColumnMappings.Add("UserID", "UserID");
                TempUsersdata.ColumnMappings.Add("UserLogin", "UserLogin");
                TempUsersdata.ColumnMappings.Add("MobileNo", "mobileNo");
                TempUsersdata.WriteToServer(TempUserData);
            }


            using (SqlBulkCopy TempResData = new SqlBulkCopy(SocietyCon))
            {
                TempResData.DestinationTableName = "TempResident";
                TempResData.ColumnMappings.Add("FlatNumber", "FlatID");
                TempResData.ColumnMappings.Add("FlatArea", "FlatArea");
                TempResData.ColumnMappings.Add("Type", "Type");
                TempResData.ColumnMappings.Add("Floor", "Floor");
                TempResData.ColumnMappings.Add("Block", "Block");
                TempResData.ColumnMappings.Add("IntercomNumber", "IntercomNumber");
                TempResData.ColumnMappings.Add("OwnerName", "OwnerName");
                TempResData.ColumnMappings.Add("BHK", "BHK");
                TempResData.ColumnMappings.Add("EmailId", "EmailId");
                TempResData.ColumnMappings.Add("MobileNo", "MobileNo");
                TempResData.ColumnMappings.Add("Address", "Address");
                TempResData.ColumnMappings.Add("FirstName", "FirstName");
                TempResData.ColumnMappings.Add("LastName", "LastName");
                TempResData.ColumnMappings.Add("ActiveDate", "ActiveDate");
                TempResData.ColumnMappings.Add("UserLogin", "UserLogin");
                TempResData.WriteToServer(databulk);
            }


            String TempDataViewQuery = "Select * from dbo.TempResidentView";
            DataSet tempData = dacess.ReadData(TempDataViewQuery);
            DataTable ResFlatData = tempData.Tables[0];

            using (SqlBulkCopy ResData = new SqlBulkCopy(SocietyCon))
            {
                ResData.DestinationTableName = "Resident";

                ResData.ColumnMappings.Add("UserID", "UserID");
                ResData.ColumnMappings.Add("FlatID", "FlatID");
                ResData.ColumnMappings.Add("Type", "Type");
                ResData.ColumnMappings.Add("FirstName", "FirstName");
                ResData.ColumnMappings.Add("LastName", "LastName");
                ResData.ColumnMappings.Add("MobileNo", "MobileNo");
                ResData.ColumnMappings.Add("EmailId", "EmailId");
                ResData.ColumnMappings.Add("ActiveDate", "ActiveDate");
                ResData.WriteToServer(ResFlatData);
            }

            using (SqlBulkCopy FlatsData = new SqlBulkCopy(SocietyCon))
            {

                FlatsData.DestinationTableName = "Flats";

                FlatsData.ColumnMappings.Add("FlatID", "FlatNumber");
                FlatsData.ColumnMappings.Add("FlatArea", "FlatArea");
                FlatsData.ColumnMappings.Add("Floor", "Floor");
                FlatsData.ColumnMappings.Add("Block", "Block");
                FlatsData.ColumnMappings.Add("IntercomNumber", "IntercomNumber");
                FlatsData.ColumnMappings.Add("OwnerName", "OwnerName");
                FlatsData.ColumnMappings.Add("BHK", "BHK");
                FlatsData.ColumnMappings.Add("Address", "Address");
                FlatsData.ColumnMappings.Add("EmailId", "EmailId");
                FlatsData.ColumnMappings.Add("MobileNo", "MobileNo");
                FlatsData.ColumnMappings.Add("UserID", "UserID");
                FlatsData.ColumnMappings.Add("Type", "Type");
                FlatsData.ColumnMappings.Add("FirstName", "FirstName");
                FlatsData.ColumnMappings.Add("LastName", "LastName");
                FlatsData.ColumnMappings.Add("UserLogin", "UserLogin");
                FlatsData.WriteToServer(ResFlatData);

            }


            //DataTable SocietyBills = (DataTable)Session["SocietyBillPlans"];
            //Added by Aarshi on 18 auh 2017 for session storage
            DataTable SocietyBills = SessionVariables.SocietyBillPlans;
            if (SocietyBills == null)
            {

            }
            else
            {
                using (SqlBulkCopy FlatBillsData = new SqlBulkCopy(SocietyCon))
                {

                    FlatBillsData.DestinationTableName = "BillCycle";
                    FlatBillsData.ColumnMappings.Add("FlatNumber", "FlatID");
                    FlatBillsData.ColumnMappings.Add("BillID", "BillID");
                    FlatBillsData.ColumnMappings.Add("CycleStart", "CycleStart");
                    FlatBillsData.ColumnMappings.Add("CycleEnD", "CycleEnD");
                    FlatBillsData.WriteToServer(SocietyBills);
                }
            }

            SocietyCon.Close();

            String TruncateQuery = "Truncate table dbo.TempResident";
            dacess.Update(TruncateQuery);

            String TruncateTempQuery = "Truncate table dbo.TempUsers";
            dacess.Update(TruncateTempQuery);

            PickUserData();
            FlatGrid.Visible = true;
            FillFlatdata();

            lblFlatEditStatus.Text = "Imported Sucessfully";
        }


        catch (Exception ex)
        {

            lblFlatEditStatus.Text = ex.Message;
        }

        finally
        {
            DeleteExcelFile();
        }

    }


    public void DeleteExcelFile()
    {
        //if (Session["Path"] != null)
        //Added by Aarshi on 18 auh 2017 for session storage
        if (SessionVariables.Path != string.Empty)
        {

            String path = Server.MapPath("~/Data/");

            if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    File.Delete(file);
                }
            }
        }
    }

    public void PickUserData()
    {
        // DataTable dtNewValues = (DataTable)Session["Newvalues"];
        //Added by Aarshi on 18 auh 2017 for session storage
        DataTable dtNewValues = SessionVariables.Newvalues;

        for (int j = 0; j < dtNewValues.Rows.Count; j++)
        {
            DataRow dtuplicate = dtNewValues.Rows[j];


            String UserLogin = dtuplicate["UserLogin"].ToString();

            String Password = dtuplicate["RandomPWD"].ToString();

            String FirstName = dtuplicate["FirstName"].ToString();

            String EmailId = dtuplicate["EmailId"].ToString();

            // muser.SendMailToUsers(UserLogin, Password, EmailId, FirstName);
            SendMail(UserLogin, Password, EmailId, FirstName);
            UpdateUsersEncrypt(UserLogin, Password);


        }
    }


    public void UpdateUsersEncrypt(String UserLogin, String Password)
    {
        DataAccess dacess = new DataAccess();
        String NewPassword = muser.EncryptPassword(UserLogin.ToLower(), Password);
        String UpdateQuery = "Update dbo.TotalUsers Set Password = '" + NewPassword + "' where UserLogin ='" + UserLogin + "'";
        dacess.UpdateUser(UpdateQuery);
    }

    protected void btnselsectdata_Click(object sender, EventArgs e)
    {
        DataAccess dacess = new DataAccess();
        // if (Session["Newvalues"] != null)
        //Added by Aarshi on 18 auh 2017 for session storage
        if (SessionVariables.Newvalues != null)
        {
            FillServerData();
        }

        // if (Session["Duplicate"] != null)
        //Added by Aarshi on 18 auh 2017 for session storage
        if (SessionVariables.Duplicate != null)
        {
            if (chkFlats.Checked)
            {
                try
                {
                    // DataTable dtDuplicate = (DataTable)Session["Duplicate"];
                    //Added by Aarshi on 18 auh 2017 for session storage
                    DataTable dtDuplicate = SessionVariables.Duplicate;
                    for (int i = 0; i < dtDuplicate.Rows.Count; i++)
                    {
                        DataRow r = dtDuplicate.Rows[i];

                        String FlatNumber = r["FlatNumber"].ToString();
                        Double Floor = (Double)r["Floor"];
                        Double NewFloor = Math.Truncate(Floor);

                        String Block = r["Block"].ToString();

                        String IntercomNumber = r["IntercomNumber"].ToString();
                        String OwnerName = r["OwnerName"].ToString();

                        Double BHK = (Double)r["BHK"];
                        Double NewBHK = Math.Truncate(BHK);

                        String Address = r["Address"].ToString();

                        String EmailId = r["EmailId"].ToString();

                        Double MobileNo = (Double)r["MobileNo"];
                        Double NewMobileNo = Math.Truncate(MobileNo);

                        String Type = r["Type"].ToString();
                        String FirstName = r["FirstName"].ToString();
                        String LastName = r["LastName"].ToString();

                        // Double MobileNoRes = (Double)r["MobileNo"];
                        //Double NewMobileRes = Math.Truncate(MobileNoRes);

                        String ParentName = r["ParentName"].ToString();
                        String Gender = r["Gender"].ToString();

                        String EmailIdRes = r["EmailId"].ToString();
                        String UserLogin = r["UserLogin"].ToString();
                        String RandomPWD = r["RandomPWD"].ToString();
                        String ActiveDate = r["ActiveDate"].ToString();
                        String UserType = r["UserType"].ToString();
                        String UpdateQuery = "Update dbo.Flats set FlatNumber= '" + FlatNumber + "', Floor = '" + Floor + "',Block ='" + Block + "', IntercomNumber = '" + IntercomNumber + "',OwnerName = '" + OwnerName + "',BHK = '" + NewBHK + "',Address = '" + Address + "',EmailId = '" + EmailId + "',MobileNo = '" + NewMobileNo + "',Type = '" + Type + "',FirstName = '" + FirstName + "',LastName = '" + LastName + "',UserLogin ='" + UserLogin + "'  where FlatNumber = '" + FlatNumber + "'";
                        dacess.Update(UpdateQuery);

                        //  lblFlatsEditStatus.Text = "Flats Data Edited Sucessfully";

                        String UpdateFlatQuery = "Update dbo.Resident set  Type = '" + Type + "',FirstName = '" + FirstName + "',LastName = '" + LastName + "',MobileNo ='" + NewMobileNo + "',EmailId = '" + EmailId + "',ActiveDate = '" + ActiveDate + "' where FlatID = '" + FlatNumber + "'";
                        dacess.Update(UpdateFlatQuery);
                        //lblResEditStatus.Text = "Resident Data Edited Sucessfully";

                        String UpdateUserQuery = "Update dbo.TotalUsers  set  FirstName = '" + FirstName + "',LastName = '" + LastName + "',MobileNo ='" + NewMobileNo + "',EmailId = '" + EmailId + "',Gender = '" + Gender + "',Parentname = '" + ParentName + "',Address = '" + Address + "', UserType = '" + UserType + "' where UserLogin = '" + UserLogin + "'";
                        dacess.UpdateUser(UpdateUserQuery);
                        FlatGrid.Visible = true;
                        FillFlatdata();
                    }
                }

                catch (Exception ex)
                {
                    String Message = ex.Message;

                    //lblUserEditStatus.Text = Message;
                }

                finally
                {

                }

            }
        }


        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert('')", "alert('Data is not updated')", true);
            FlatGrid.Visible = true;
            FillFlatdata();
        }


    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        FlatGrid.Visible = true;
        FillFlatdata();
    }


    //=---------------------------------------------------------Add Flat Section starts from here =------------------------------------------------------------------------------------------//

    protected void txtFltAdd_TextChanged(object sender, EventArgs e)
    {
        try
        {
            String FlatCheckQuery = "select  ID from Flats where FlatNumber = '" + txtFltAdd.Text + "'";
            DataAccess dacess = new DataAccess();
            int ID = dacess.GetSingleValue(FlatCheckQuery);

            if (ID == 0)
            {
                flatMsg.ImageUrl = "~/Images/Icon/green-tick.jpg";
            }
            else
            {
                //lblAddfltAvailble.ForeColor = Color.Red;
                flatMsg.ImageUrl = "~/Images/Icon/Cancelled.png";
            }
        }
        catch (Exception ex)
        {
            lblAddfltAvailble.Text = ex.Message;
        }
    }

    protected bool IfFlatNumberExist(String FlatNumber)
    {
        try
        {
            String FlatCheckQuery = "select  ID from Flats where FlatNumber = '" + txtFltAdd.Text + "'";
            DataAccess dacess = new DataAccess();
            int ID = dacess.GetSingleValue(FlatCheckQuery);

            if (ID == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            return true;
        }

    }

    protected void btnAddflatSubmit_Click(object sender, EventArgs e)
    {
        DataAccess daAccess = new DataAccess();

        String FirstName = txtAddflatFirstname.Text;
        String LastName = txtAddflatLastName.Text;
        String Block = txtAddBlock.Text;
        String Floor = txtAddfltFlr.Text;
        String Intercomnumber = txtAddflatIntrc.Text;
        //String Resident = txtAddflatResident.Text;
        String FlatNo = txtFltAdd.Text;
        String FlatArea = txtFlatArea.Text;
        String BHK = drpAddflatBHK.SelectedItem.Text;
        String Address = txtAddfltAddrs.Text;
        String MobileNo = txtAddfltMobile.Text;
        String EmailId = txtAddfltEmail.Text;
        String Gender = drpFlatGender.SelectedItem.Text;
        String Parentname = txtAddfltParentName.Text;
        String UserLogin = txtAddflatUserLogin.Text.ToLower();
        String Password = "Password@123";
        String UserType = "Owner";
        if (FirstName == "" || LastName == "" || Block == "" || Floor == "" || FlatNo == "" || FlatArea == "" || Address == "" || MobileNo == "" || EmailId == "")
        {
            lblAddflatStatus.ForeColor = System.Drawing.Color.Red;
            lblAddflatStatus.Text = "Empty Field not allowed";
            return;
        }
        if (IfMobileExist(MobileNo))
        {
            lblAddflatStatus.ForeColor = System.Drawing.Color.Red;
            lblAddflatStatus.Text = "Mobile Number Already in use";
            return;
        }
        if (IfFlatNumberExist(FlatNo))
        {
            lblAddflatStatus.ForeColor = System.Drawing.Color.Red;
            lblAddflatStatus.Text = "Flat Number Already in use";
            return;
        }
        if (IfUserLoginExist(UserLogin))
        {
            lblAddflatStatus.ForeColor = System.Drawing.Color.Red;
            lblAddflatStatus.Text = "User Login Already in use";
            return;
        }

        //DateTime ActiveDate = System.DateTime.Now;
        DateTime ActiveDate = Utility.GetCurrentDateTimeinUTC();
        try
        {
            if (ExistingUser == false)
            {
                if (UserLogin == "")
                {
                    lblAddflatStatus.ForeColor = System.Drawing.Color.Red;
                    lblAddflatStatus.Text = "Empty Field not allowed";
                    return;
                }
                String UpdateUserQuery = "Insert Into dbo.TotalUsers  (FirstName, MiddleName,LastName,MobileNo,EmailId,Gender,Parentname,UserLogin,Address,UserType,SocietyID) Values('" + FirstName + "','','" + LastName + "','" + MobileNo + "','" + EmailId + "','" + Gender + "','" + Parentname + "','" + UserLogin + "','" + Address + "','" + UserType + "','" + muser.SocietyID + "')";
                bool result = daAccess.UpdateUser(UpdateUserQuery);
                if (result == true)
                {
                    User newUser = new User();
                    newUser.UpdatePassword(UserLogin, Password);

                    String EmpUserIDQuery = "select * from dbo.TotalUsers  where UserLogin ='" + UserLogin + "' and UserType ='" + UserType + "' and SocietyID ='" + muser.SocietyID + "'";

                    int UserID = daAccess.GetSingleUserValue(EmpUserIDQuery);

                    String FlatUpdateQuery = "Insert into dbo.Flats(OwnerName,FlatNumber,FlatArea,Floor,Block,BHK,IntercomNumber,Address,MobileNo,EmailId,UserLogin,UserID,Type,FirstName,LastName) values ('" + FirstName + "' ,'" + FlatNo + "' ,'" + FlatArea + "','" + Floor + "'  ,'" + Block + "' ,'" + BHK + "' ,'" + Intercomnumber + "','" + Address + "'  ,'" + MobileNo + "' ,'" + EmailId + "','" + UserLogin + "','" + UserID + "','" + UserType + "','" + FirstName + "','" + LastName + "' )";

                    bool result2 = daAccess.Update(FlatUpdateQuery);

                    if (result2 == true)
                    {
                        String ResidentQuery = "Insert into dbo.Resident(UserID,FlatID,Type,FirstName,LastName,MobileNo,EmailId,Activedate,Addres) values ('" + UserID + "' ,'" + FlatNo + "' ,'" + UserType + "','" + FirstName + "'  ,'" + LastName + "' ,'" + MobileNo + "' ,'" + EmailId + "','" + ActiveDate + "','" + Address + "' )";
                        bool result3 = daAccess.Update(ResidentQuery);
                        lblAddflatStatus.Text = "Flat Added Sucessfully";
                        //Added by Aarshi on 14 - Sept - 2017 for bug fix
                        //muser.SendMailToUsers(UserLogin, Password, EmailId, FirstName);
                        SendMail(UserLogin, Password, EmailId, FirstName); 
                        AddBillToFlat(FlatNo, FlatArea);
                        GenerateInitialZeroBill(FlatNo, FlatArea);
                        FillFlatdata();
                    }

                    else
                    {
                        lblAddflatStatus.ForeColor = System.Drawing.Color.Red;
                        lblAddflatStatus.Text = "Could not Submitt  Flat try later or Contact Admin";
                    }
                }

                else
                {
                    lblAddflatStatus.ForeColor = System.Drawing.Color.Red;
                    lblAddflatStatus.Text = " Valid MobileNo,UserLogin,EmailID.";
                }
            }
            else if (ExistingUser == true)
            {

                String FlatUpdateQuery = "Insert into dbo.Flats(OwnerName,FlatNumber,FlatArea,Floor,Block,BHK,IntercomNumber,Address,MobileNo,EmailId,UserLogin,UserID,Type,FirstName,LastName) values ('" + FirstName + "' ,'" + FlatNo + "' ,'" + FlatArea + "','" + Floor + "'  ,'" + Block + "' ,'" + BHK + "' ,'" + Intercomnumber + "','" + Address + "'  ,'" + MobileNo + "' ,'" + EmailId + "','" + UserLogin + "','" + ExistingUserID + "','" + UserType + "','" + FirstName + "','" + LastName + "' )";

                bool result2 = daAccess.Update(FlatUpdateQuery);

                if (result2 == true)
                {
                    String ResidentQuery = "Insert into dbo.Resident(UserID,FlatID,Type,FirstName,LastName,MobileNo,EmailId,Activedate,Addres) values ('" + ExistingUserID + "' ,'" + FlatNo + "' ,'" + UserType + "','" + FirstName + "'  ,'" + LastName + "' ,'" + MobileNo + "' ,'" + EmailId + "','" + ActiveDate + "','" + Address + "' )";
                    bool result3 = daAccess.Update(ResidentQuery);
                    lblAddflatStatus.Text = "Flat Added Sucessfully";
                    //Added by Aarshi on 14 - Sept - 2017 for bug fix
                    //muser.SendMailToUsers(UserLogin, Password, EmailId, FirstName);
                    SendMail(UserLogin, Password, EmailId, FirstName); 
                    AddBillToFlat(FlatNo, FlatArea);
                    //Bill.InsertFirstZeroBill added by aarshi to add zero bill
                    GenerateInitialZeroBill(FlatNo, FlatArea);
                    FillFlatdata();
                }

                else
                {
                    lblAddflatStatus.ForeColor = System.Drawing.Color.Red;
                    lblAddflatStatus.Text = "Could not Submitt  Flat try later or Contact Admin";
                }

            }
        }
        catch (Exception ex)
        {
            Utility.log("Addflat:btnAddflatSubmit_Click Exception" + ex.Message);
            lblAddflatStatus.Text = ex.Message;
        }
    }

    private void GenerateInitialZeroBill(String FlatNumber, String FlatArea)
    {
        Bill bill = new Bill();
        int ApplyTo = 0;
        DateTime CycleStart = new DateTime();
        DateTime CycleEnd = new DateTime();

        DataAccess dacess = new DataAccess();
        try
        {
            //Changed by aarshi on 7 July 2017 for Generate zero bill
            //String GetBillIDQuery = "Select BillID,Applyto from dbo.aSocietybillplans";
            String GetBillIDQuery = "Select BillID,Applyto,CycleType from dbo.Societybillplans";
            DataSet data = dacess.ReadData(GetBillIDQuery);

            if (data != null)
            {
                DataTable dta = data.Tables[0];

                for (int i = 0; i < dta.Rows.Count; i++)
                {
                    int BillId = Convert.ToInt32(dta.Rows[i]["BillID"]);
                    ApplyTo = Convert.ToInt32(dta.Rows[i]["Applyto"]);
                    String CycleType = Convert.ToString(dta.Rows[i]["CycleType"]);


                    //if statement added by aarshi on on 7 July 2017 for Generate zero bill for selected plans
                    if (ApplyTo == 1)
                    {
                        CycleStart = Utility.GetCurrentDateTimeinUTC();
                        int year = CycleStart.Year;
                        CycleEnd = new DateTime(year + 5, CycleStart.Month, CycleStart.Day);

                        GenerateBill emptyBill = new GenerateBill();
                        emptyBill.FlatNumber = FlatNumber;
                        emptyBill.BillID = BillId;
                        emptyBill.CurrentBillAmount = 0;
                        emptyBill.CycleType = CycleType;
                        emptyBill.PaymentDueDate = Convert.ToDateTime(CycleStart).AddDays(7);
                        emptyBill.BillMonth = Convert.ToDateTime(CycleStart);
                        emptyBill.PreviousMonthBalance = 0;
                        emptyBill.ModifiedAt = Utility.GetCurrentDateTimeinUTC();
                        emptyBill.BillDescription = "First Empty Bill";
                        emptyBill.BillStartDate = Convert.ToDateTime(CycleStart).AddDays(-1);
                        emptyBill.BillEndDate = Convert.ToDateTime(CycleStart).AddDays(-1);
                        bool result = bill.InsertNewBill(emptyBill);

                    }
                }
            }
        }

        catch (Exception ex)
        {
            lblDefalutBillText.Text = ex.Message;
        }

    }

    public void AddBillToFlat(String FlatNumber, String FlatArea)
    {
        int ApplyTo = 0;
        DateTime CycleStart = new DateTime();
        DateTime CycleEnd = new DateTime();

        DataAccess dacess = new DataAccess();
        try
        {
            String GetBillIDQuery = "Select BillID,Applyto from dbo.Societybillplans";
            DataSet data = dacess.ReadData(GetBillIDQuery);

            if (data != null)
            {
                DataTable dta = data.Tables[0];

                for (int i = 0; i < dta.Rows.Count; i++)
                {
                    int BillId = Convert.ToInt32(dta.Rows[i]["BillID"]);
                    ApplyTo = Convert.ToInt32(dta.Rows[i]["Applyto"]);

                    if (ApplyTo == 0)
                    {
                        CycleStart = new DateTime(2000, 1, 1);
                        CycleEnd = new DateTime(2000, 1, 1);
                    }
                    else
                    {
                        CycleStart = Utility.GetCurrentDateTimeinUTC();
                        int year = CycleStart.Year;
                        CycleEnd = new DateTime(year + 5, CycleStart.Month, CycleStart.Day);
                    }


                    String BillCycleInsertQuery = "Insert into BillCycle(FlatID,BillID,CycleStart,CycleEnD) values('" + FlatNumber + "','" + BillId + "','" + CycleStart + "','" + CycleEnd + "')";
                    bool result = dacess.Update(BillCycleInsertQuery);
                    if (result == true)
                    {
                        lblDefalutBillText.Text = "Bills Added Sucessfully";
                    }
                }
            }
        }

        catch (Exception ex)
        {
            lblDefalutBillText.Text = ex.Message;
        }
    }

    protected void txtAddflatUserLogin_TextChanged(object sender, EventArgs e)
    {
        DataAccess dacess = new DataAccess();
        try
        {
            String UserLoginCheckQuery = "  select UserID  from dbo.TotalUsers where UserLogin = '" + txtAddflatUserLogin.Text + "'";
            int UserId = dacess.GetSingleUserValue(UserLoginCheckQuery);
            if (UserId == 0)
            {
                loginMsg.ImageUrl = "~/Images/green-tick.jpg";
            }
            else
            {
                loginMsg.ImageUrl = "~/Images/Cancelled.png";
            }
        }

        catch (Exception ex)
        {
            loginMsg.ImageUrl = "~/Images/Cancelled.png";
        }
    }

    private bool IfUserLoginExist(String UserLogin)
    {
        DataAccess dacess = new DataAccess();
        try
        {
            String UserLoginCheckQuery = "  select UserID  from dbo.TotalUsers where UserLogin = '" + txtAddflatUserLogin.Text + "'";
            int UserId = dacess.GetSingleUserValue(UserLoginCheckQuery);
            if (UserId == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        catch (Exception ex)
        {
            return true;
        }
    }
    protected void txtAddfltMobile_TextChanged(object sender, EventArgs e)
    {
        DataAccess dacess = new DataAccess();
        try
        {
            String MobileNoCheckQuery = "select *  from dbo.TotalUsers where MobileNo= '" + txtAddfltMobile.Text + "'";
            DataSet dUser = dacess.GetUserData(MobileNoCheckQuery);

            if (dUser == null || dUser.Tables[0].Rows.Count == 0)
            {
                lblMobileCheck.Text = "MobileNo is available";
                ExistingUser = false;
                //RegularExpressionValidator13.ForeColor = System.Drawing.Color.AliceBlue;
                //RegularExpressionValidator13.ErrorMessage = "√";
                mobileMsg.ForeColor = System.Drawing.Color.AliceBlue;
                mobileMsg.ImageUrl = "~/Images/green-tick.jpg";
            }
            else
            {
                newUser = new User();
                DataTable usertable = dUser.Tables[0];
                newUser.ID = Convert.ToInt32(usertable.Rows[0]["UserID"]);
                newUser.strFirstName = usertable.Rows[0]["FirstName"].ToString();
                newUser.strLastName = usertable.Rows[0]["LastName"].ToString();
                newUser.ParentName = usertable.Rows[0]["ParentName"].ToString().Trim();
                newUser.Address = usertable.Rows[0]["Address"].ToString();
                newUser.Gender = usertable.Rows[0]["Gender"].ToString();
                newUser.strEmailID = usertable.Rows[0]["EmailId"].ToString();

                mobileMsg.ForeColor = System.Drawing.Color.Red;
                mobileMsg.ImageUrl = "~/Images/Cancelled.png";
                lblUserExist.Text = "User already Exist. Enter Email for Exising User or use another Mobile";
                ExistingUser = true;
            }
        }

        catch (Exception ex)
        {
            lblMobileCheck.Text = ex.Message;
        }
    }

    private Boolean IfMobileExist(String MobileNo)
    {
        DataAccess dacess = new DataAccess();
        try
        {
            String MobileNoCheckQuery = "select *  from dbo.TotalUsers where MobileNo= '" + MobileNo + "'";
            DataSet dUser = dacess.GetUserData(MobileNoCheckQuery);

            if (dUser == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return true;
        }

    }
    protected void oldtxtAddfltEmail_TextChanged(object sender, EventArgs e)
    {
        DataAccess dacess = new DataAccess();
        try
        {
            if (ExistingUser == false)
            {
                String EmailIDCheckQuery = "  select UserID  from dbo.TotalUsers where EmailId = '" + txtAddfltEmail.Text + "'";
                int UserId = dacess.GetSingleUserValue(EmailIDCheckQuery);

                if (UserId == 0)
                {

                    emailMsg.ImageUrl = "~/Images/green-tick.jpg";
                }
                else
                {

                    emailMsg.ImageUrl = "~/Images/Cancelled.png";
                }
            }

            else if (ExistingUser == true)
            {
                if (txtAddfltEmail.Text == newUser.strEmailID)
                {
                    ExistingUserID = newUser.ID;
                    txtAddflatFirstname.Text = newUser.strFirstName;
                    txtAddflatLastName.Text = newUser.strLastName;
                    txtAddfltParentName.Text = newUser.ParentName;
                    txtAddfltAddrs.Text = newUser.Address;
                    drpFlatGender.SelectedItem.Text = newUser.Gender;
                    drpFlatGender.Items.Insert(1, new ListItem(newUser.Gender));
                    checkFlatsOnly.Visible = true;

                    mobileMsg.ImageUrl = "~/Images/Icon/green-tick.jpg";
                    emailMsg.ImageUrl = "~/Images/Icon/green-tick.jpg";
                }
                else
                {
                    //lblEmailCheck.Text = "Wrong Email ID";
                    txtAddfltEmail.Text = "";
                    emailMsg.ImageUrl = "~/Images/Icon/Cancelled.png";
                }
            }
        }

        catch (Exception ex)
        {
            //lblEmailCheck.Text = ex.Message;
        }
    }


    protected void txtAddfltEmail_TextChanged(object sender, EventArgs e)
    {
        DataAccess dacess = new DataAccess();
        try
        {
            if (ExistingUser == false)
            {
                String EmailIDCheckQuery = "  select UserID  from dbo.TotalUsers where EmailId = '" + txtAddfltEmail.Text + "'";
                int UserId = dacess.GetSingleUserValue(EmailIDCheckQuery);

                if (UserId == 0)
                {

                    emailMsg.ImageUrl = "~/Images/Icon/green-tick.jpg";
                }
                else
                {

                    emailMsg.ImageUrl = "~/Images/Icon/Cancelled.png";
                }
            }

            else if (ExistingUser == true)
            {
                if (txtAddfltEmail.Text.ToLower() == newUser.strEmailID.ToLower())
                {
                    ExistingUserID = newUser.ID;
                    txtAddflatFirstname.Text = newUser.strFirstName;
                    txtAddflatLastName.Text = newUser.strLastName;
                    txtAddfltParentName.Text = newUser.ParentName;
                    txtAddfltAddrs.Text = newUser.Address;
                    drpFlatGender.SelectedItem.Text = newUser.Gender;
                    drpFlatGender.Items.Insert(1, new ListItem(newUser.Gender));
                    checkFlatsOnly.Visible = true;
                    //Added by Aarshi on 21-July-2017
                    String ViewFlat = "select * from dbo.ViewFlats where Ownermobile = '" + txtAddfltMobile.Text + "' and OwnerEmail = '" + txtAddfltEmail.Text + "'";
                    DataSet dsFlatsView = dacess.GetData(ViewFlat);
                    if (dsFlatsView != null && dsFlatsView.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtFlatsView = dsFlatsView.Tables[0];
                        txtFltAdd.Text = dtFlatsView.Rows[0]["FlatNumber"].ToString();
                        //txtFlatArea.Text = dtFlatsView.Rows[0]["FlatNumber"].ToString();
                        txtAddBlock.Text = dtFlatsView.Rows[0]["Block"].ToString();
                        txtAddfltFlr.Text = dtFlatsView.Rows[0]["Floor"].ToString();
                        drpAddflatBHK.SelectedItem.Text = dtFlatsView.Rows[0]["BHK"].ToString();
                        txtAddflatIntrc.Text = dtFlatsView.Rows[0]["IntercomNumber"].ToString();
                    }

                    mobileMsg.ImageUrl = "~/Images/Icon/green-tick.jpg";
                    emailMsg.ImageUrl = "~/Images/Icon/green-tick.jpg";
                }
                else
                {
                    //lblEmailCheck.Text = "Wrong Email ID";
                    txtAddfltEmail.Text = "";
                    emailMsg.ImageUrl = "~/Images/Icon/Cancelled.png";
                }
            }
        }

        catch (Exception ex)
        {
            //lblEmailCheck.Text = ex.Message;
        }
    }

    //---------------------------------------------------------------------------------------Flats Edit section  -----------------------------------------------------------------//

    protected void btnFlatsEdit_Click(object sender, EventArgs e)
    {
        DataAccess daAccess = new DataAccess();
        try
        {
            String UserLogin = HiddenField1.Value;
            String FlatNumber = HiddenField2.Value;

            //Session["FlatNumber"] = FlatNumber;
            //Added by Aarshi on 18 auh 2017 for session storage
            SessionVariables.FlatNumber = FlatNumber;

            Editdata(FlatNumber);

            // Response.Redirect("EditFlats.aspx");

        }

        catch (Exception ex)
        {
            //Utility.log("Flats :btnFlatsEdit_Click Exception " + ex.Message);
        }

    }

    public void Editdata(String FlatNumber)
    {
        try
        {
            HiddenEditID.Value = "";

            DataAccess dacess = new DataAccess();
            SqlConnection ConnectDB = dacess.ConnectSocietyDB();
            SqlCommand myCommand = new SqlCommand("select * from dbo.Flats where FlatNumber ='" + FlatNumber + "'", ConnectDB);
            SqlDataReader myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                int ID = Convert.ToInt32(myReader["ID"]);
                txtEditFlatNumber.Text = (myReader["FlatNumber"].ToString().Trim());
                txtEditFlatBlock.Text = (myReader["Block"].ToString().Trim());
                txtFlatOwnerName.Text = (myReader["OwnerName"].ToString().Trim());
                txtEditFlatFloor.Text = (myReader["Floor"].ToString());
                txtFltInterNum.Text = (myReader["IntercomNumber"].ToString());
                txtFltBhk.Text = (myReader["BHK"].ToString());
                txtFlatEmail.Text = (myReader["EmailId"].ToString());
                txtFlatMobile.Text = (myReader["MobileNo"].ToString());
                txtFlatAddress.Text = (myReader["Address"].ToString().Trim());

                lblfltEditstatus.Text = "";
                HiddenEditID.Value = ID.ToString();
            }

        }
        catch (Exception ex)
        {
            //Utility.log("Error found at EditFlats.EditData at " + System.DateTime.Now.ToString() + ex.Message);
        }

    }

    protected void btneditFlat_Click(object sender, EventArgs e)
    {
        String FlatNumber = txtEditFlatNumber.Text;
        String Block = txtEditFlatBlock.Text;
        String OwnerName = txtFlatOwnerName.Text;
        String Floor = txtEditFlatFloor.Text;
        String BHK = txtFltBhk.Text;
        String Email = txtFlatEmail.Text;
        String Mobile = txtFlatMobile.Text;
        String Address = txtFlatAddress.Text;
        String Intercom = txtFltInterNum.Text;
        String UpdateQuery = "Update dbo.Flats  Set Block='" + Block + "', OwnerName='" + OwnerName + "', Floor='" + Floor + "', BHK='" + BHK + "', EmailId='" + Email + "', MobileNo='" + Mobile + "', Address='" + Address + "', IntercomNumber='" + Intercom + "'  WHERE FlatNumber ='" + txtEditFlatNumber.Text + "'";
        DataAccess dacess = new DataAccess();
        bool result = dacess.Update(UpdateQuery);
        if (result == true)
        {
            lblfltEditstatus.Text = "Edited Sucessfully";
            FillFlatdata();
            ClientScript.RegisterStartupScript(this.GetType(), "alert('')", "HideLabel()", true);
        }
        else
        {
            lblfltEditstatus.Text = "Edited Failed, try later";
        }

    }
    protected void txtAddflatUserLogin_TextChanged1(object sender, EventArgs e)
    {
    }








    //Add Tenenet section
    protected void btnAddTenant_Click(object sender, EventArgs e)
    {

        try
        {
            String UserLogin = HiddenField1.Value;

            String FlatNumber = HiddenField2.Value;

            // Session["FlatID"] = FlatNumber;
            //Response.Redirect("AddTenant.aspx");

            CheckTenant(FlatNumber);

        }


        catch (Exception ex)
        {
            //Utility.log("Flats :btnFlatsEdit_Click Exception " + ex.Message);
        }

    }

    public void CheckTenant(String FlatNumber)
    {
        String FirstName = String.Empty;
        String LastName = String.Empty;
        String Mobile = String.Empty;
        String Email = String.Empty;
        String UserID = String.Empty;
        String Address = String.Empty;
        String DeactiveDate = String.Empty;

        DataAccess dacsess = new DataAccess();
        String TenantCheckQuery = "Select * from dbo.Resident where FlatID ='" + FlatNumber + "' ";
        String Type = "Tenant";
        // DateTime Date = System.DateTime.Now;
        DateTime Date = Utility.GetCurrentDateTimeinUTC();
        DataSet ds = dacsess.ReadData(TenantCheckQuery);
        String ResType = "";
        if (ds != null)
        {
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ResType = dr["Type"].ToString();
                HiddenResType.Value = ResType;
                FirstName = dr["FirstName"].ToString();
                LastName = dr["LastName"].ToString();
                Mobile = dr["MobileNo"].ToString();
                Email = dr["EmailId"].ToString();
                UserID = dr["UserID"].ToString();
                //Session["UserID"] = UserID;
                //Added by Aarshi on 18 auh 2017 for session storage
                SessionVariables.UserID = UserID;
                Address = dr["Addres"].ToString();
                DeactiveDate = dr["DeActiveDate"].ToString();
                txtAddTFlatNo.Text = FlatNumber;
                HiddenAddTID.Value = FirstName;

            }

        }
        lblResStatus.Text = "";
        DateTime CuurentDate = Utility.GetCurrentDateTimeinUTC();

        if (ResType == "Tenant")
        {
            DateTime Deactive = Convert.ToDateTime(DeactiveDate);

            if (Deactive > CuurentDate)
            {
                lblHeadText.Text = "Deactivate  This  tenant  to  add a new tenant";

                txtAddTFirstName.Text = FirstName;
                txtAddTLastName.Text = LastName;
                txtAddTMobile.Text = Mobile;
                txtAddTEmailID.Text = Email;
                txtAddTAddress.Text = Address;
                txtAddTDeactiveDate.Text = Deactive.ToString("dd/MM/yyyy");
                btnDeactivate.Visible = true;
                lblHeadText.Visible = true;
                HideTenantFields();
            }
        }

        else
        {
            lblHeadText.Visible = false;
            ShowTenantFields();
            btnDeactivate.Visible = false;
        }
    }



    public void HideTenantFields()
    {
        btnSubmit.Visible = false;
        lbluserlogin.Visible = false;
        txtAddTUserLogin.Visible = false;
        lblAddTenantUserCheck.Visible = false;
        lblParent.Visible = false;
        lblGender.Visible = false;
        drpAddTGender.Visible = false;
        RequireGender.Visible = false;
        regUserLoginexp.Visible = false;
        txtAddTParentName.Visible = false;
        RequireParentName.Visible = false;
        txtAddTPassword.Visible = false;
        regexpPassword.Visible = false;
        lblPassword.Visible = false;
        RequireAddTEmail.Visible = false;
        lblAddTEmailChck.Visible = false;
        regAddTEmailCheck.Visible = false;

        lblConfirmPassword.Visible = false;
        txtAddTConfirmPassword.Visible = false;
        requireconfirmPassword.Visible = false;
        requireUserLogin.Visible = false;
        requirepassword.Visible = false;

    }


    public void ShowTenantFields()
    {
        btnSubmit.Visible = true;
        lbluserlogin.Visible = true;
        txtAddTUserLogin.Visible = true;
        lblAddTenantUserCheck.Visible = true;
        lblParent.Visible = true;
        lblGender.Visible = true;
        drpAddTGender.Visible = true;
        RequireGender.Visible = true;
        regUserLoginexp.Visible = true;
        txtAddTParentName.Visible = true;

        txtAddTPassword.Visible = true;
        regexpPassword.Visible = true;
        lblPassword.Visible = true;
        lblAddTEmailChck.Visible = true;
        lblConfirmPassword.Visible = true;
        txtAddTConfirmPassword.Visible = true;
        requireconfirmPassword.Visible = true;
        regAddTEmailCheck.Visible = true;
        requireUserLogin.Visible = true;
        requirepassword.Visible = true;
        RequireParentName.Visible = true;
        RequireAddTEmail.Visible = true;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        // DateTime Presentdate = System.DateTime.Now;
        DateTime Presentdate = Utility.GetCurrentDateTimeinUTC();
        DateTime Deactivedate = Convert.ToDateTime(txtAddTDeactiveDate.Text);

        if (Deactivedate > Presentdate)
        {
            DataAccess dacess = new DataAccess();
            String UserType = "Resident";
            String Tenant = "Tenant";
            // DateTime Date = System.DateTime.Now;
            DateTime Date = Utility.GetCurrentDateTimeinUTC();
            String UserID = "";
            String Firstname = txtAddTFirstName.Text;
            String Lastname = txtAddTLastName.Text;
            String UserLogin = txtAddTUserLogin.Text;
            String Password = txtAddTPassword.Text;
            String Email = txtAddTEmailID.Text;
            String Address = txtAddTAddress.Text;

            String AddTenantUserQuery = "Insert Into dbo.TotalUsers  (FirstName, MiddleName,LastName,MobileNo,EmailId,Gender,Parentname,UserLogin,Address,UserType,SocietyID) Values('" + Firstname + "','','" + Lastname + "','" + txtAddTMobile.Text + "','" + txtAddTEmailID.Text + "','" + drpAddTGender.SelectedItem.Text + "','" + txtAddTParentName.Text + "','" + UserLogin + "','" + Address + "','" + UserType + "','" + muser.SocietyID + "')";
            bool Userresult = dacess.UpdateUser(AddTenantUserQuery);

            //String Message = "Hi\r\n"  .$username. " \r\n \r\n Your Society Management Account is  activated  Just now. \r\n UserId   :".$username. "\r Password :" .$password. "\r Regards, Society Management System;"

            if (Userresult == true)
            {
               
                //Added by Aarshi on 14 - Sept - 2017 for bug fix
                //muser.SendMailToUsers(UserLogin, Password, EmailId, FirstName);
                SendMail(UserLogin, Password, Email, Firstname); 
                bool EncryptPass = muser.UpdatePassword(UserLogin, Password);

                if (EncryptPass == true)
                {
                    String UserIDQuery = "select max(UserID) as UserID from dbo.TotalUsers  where UserLogin = '" + txtAddTUserLogin.Text + "' and SocietyID = '" + muser.SocietyID + "'";

                    DataSet ds = dacess.ReadUserData(UserIDQuery);
                    DataTable dt = ds.Tables[0];
                    UserID = dt.Rows[0][0].ToString();

                    String AddTenantQuery = "Insert into dbo.Resident(UserID,FlatID,Type,FirstName,LastName,MobileNo,EmailId,Addres,ActiveDate,DeactiveDate) values('" + UserID + "','" + txtAddTFlatNo.Text + "','" + Tenant + "','" + txtAddTFirstName.Text + "','" + txtAddTLastName.Text + "','" + txtAddTMobile.Text + "','" + txtAddTEmailID.Text + "','" + txtAddTAddress.Text + "','" + Date + "','" + Deactivedate + "')";
                    bool Resresult = dacess.Update(AddTenantQuery);

                    if (Resresult == true)
                    {
                        lblResStatus.Text = "Tenant Added Sucessfully for  the  Flat " + "" + txtAddTFlatNo.Text;
                    }
                    else
                    {
                        lblResStatus.Text = "Tenant Added Failed for  the  Flat " + "" + txtAddTFlatNo.Text;
                    }
                }
            }
        }

        else
        {
            lblResStatus.Text = "Deactve Date shuild be greater than Current Date";
        }
    }

    protected void btnDeactivate_Click(object sender, EventArgs e)
    {
        DataAccess dacess = new DataAccess();

        //String UpdateTenant = "Update dbo.Resident Set DeActiveDate = '" + txtAddTDeactiveDate.Text + "' where UserID = '" + Session["UserID"] + "'";
        //Added by Aarshi on 18 auh 2017 for session storage
        String UpdateTenant = "Update dbo.Resident Set DeActiveDate = '" + txtAddTDeactiveDate.Text + "' where UserID = '" + SessionVariables.UserID + "'";
        bool result = dacess.Update(UpdateTenant);
        if (result == true)
        {
            lblResStatus.Text = "Tenant Deactivated Sucessfully";
        }
    }
    protected void txtUserLogin_TextChanged(object sender, EventArgs e)
    {
        DataAccess dacess = new DataAccess();
        String FlatTenantLoginID = txtAddTUserLogin.Text;
        if (FlatTenantLoginID != "")
        {
            String UserAvailbleQuery = "Select  UserID from dbo.TotalUsers where UserLogin = '" + FlatTenantLoginID + "'";
            int UserID = dacess.GetSingleUserValue(UserAvailbleQuery);

            if (UserID == 0)
            {
                lblAddTenantUserCheck.Text = "";
            }
            else
            {
                lblAddTenantUserCheck.Text = "UserLogin is already exists";
            }
        }
    }

    protected void txtEmailID_TextChanged(object sender, EventArgs e)
    {
        DataAccess dacess = new DataAccess();
        String FlatTenantEmailID = txtAddTEmailID.Text;
        if (FlatTenantEmailID != "")
        {
            String EmailAvailbleQuery = "Select  max(UserID) from dbo.TotalUsers where EmailId = '" + FlatTenantEmailID + "'";
            int UserID = dacess.GetSingleUserValue(EmailAvailbleQuery);
            if (UserID == 0)
            {
                lblAddTEmailChck.Text = "";
            }
            else
            {
                lblAddTEmailChck.Text = "EmailID is already exists";
            }
        }
    }
    protected void txtMobile_TextChanged(object sender, EventArgs e)
    {
        DataAccess dacess = new DataAccess();
        String FlatTenantMobileNo = txtAddTMobile.Text;
        if (FlatTenantMobileNo != "")
        {
            String UserAvailbleQuery = "Select  UserID from dbo.TotalUsers where MobileNo = '" + FlatTenantMobileNo + "'";
            int UserID = dacess.GetSingleUserValue(UserAvailbleQuery);

            if (UserID == 0)
            {
                lblAddTMobilechck.Text = "";
            }
            else
            {
                lblAddTMobilechck.Text = "MobileNo is not available";
            }
        }
    }
    protected void btnAddBill_Click(object sender, EventArgs e)
    {
        String UserLogin = HiddenField1.Value;
        String FlatNumber = HiddenField2.Value;

        // Response.Redirect("BillingManagement.aspx?FlatNumber=" + FlatNumber);
    }


    [System.Web.Services.WebMethod]
    public static List<string> GetOwnerName(string empName)
    {
        List<string> Emp = new List<string>();
        string query = string.Format("SELECT OwnerName FROM Flats WHERE OwnerName LIKE '" + empName + "%'");
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
    [System.Web.Services.WebMethod]
    public static List<string> GetFlatNumber(string FlatNumber)
    {
        List<string> Emp = new List<string>();
        string query = string.Format("Select FlatNumber from dbo.Flats where FlatNumber like '" + FlatNumber + "%'");
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





    protected void checkFlatsOnly_CheckedChanged(object sender, EventArgs e)
    {

        if (checkFlatsOnly.Checked == true)
        {
            txtAddflatUserLogin.Enabled = false;
            //txtFlatusrPasswrd.Enabled = false;
            //txtFlatusrConfirmPasswrd.Enabled = false;

            //// RequireAddfltUserlogin.Enabled = false;
            //RequiredAddfltPassword.Enabled = false;
            //RequiredAddfltConfirmPass.Enabled = false;

        }
        else
        {

            txtAddflatUserLogin.Enabled = true;
            //txtFlatusrPasswrd.Enabled = true;
            //txtFlatusrConfirmPasswrd.Enabled = true;

            //// RequireAddfltUserlogin.Enabled = true;
            //RequiredAddfltPassword.Enabled = true;
            //RequiredAddfltConfirmPass.Enabled = true;
        }
    }


    //Added by Aarshi on 14-Sept-2017 for bug fix
    public void SendMail(string UserLogin, string Password, string EmailId, string FirstName)
    {
        string EmailBody = string.Empty;

        string EmailSubject = "New password from Anvisys";


        StringBuilder result = new StringBuilder();
        result.Append("Hi " + FirstName + ",");
        result.AppendLine();
        result.AppendLine();
        result.Append("Your Society Management Account is  activated  just now.");
        result.AppendLine();
        result.AppendLine();
        result.Append("UserId: " + UserLogin);
        result.AppendLine();
        result.Append("Password: " + Password);
        result.AppendLine();
        result.AppendLine();
        result.Append("Visit the website at www.MyAptt.com");
        result.AppendLine();
        result.AppendLine();
        result.Append("Regards,");
        result.AppendLine();
        result.Append("Society Management System");
        EmailBody = Convert.ToString(result);

        Notification notification = new Notification();
        notification.SendMail(EmailId, EmailSubject, EmailBody);
    }
}
