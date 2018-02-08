using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class ActiveBillPlan : System.Web.UI.Page
{
    static GenerateBill newBill = null;
    static GenerateBill previousBill = null;
    static DataSet dsBillType;
    Bill bill = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        drpBillStatusype.SelectedIndex = 1;

        LoadBillTypeDropdown(drpActivatedBillType);//Added by Aarshi on 3 aug 2017 for bill management structuring
        LoadActivatedBill();
    }

    #region ActivatedBill

    protected void linkActivatedBill_Click(object sender, EventArgs e)
    {
      //  MultiView1.ActiveViewIndex = 1;
        
       
        ClientScript.RegisterStartupScript(this.GetType(), "alert('')", "ActivatedBillClick()", true);

    }

    protected void btnGenerateBill_Click(object sender, EventArgs e)
    {
        //String BilltypeActive = drpToGenerateBillTypes.SelectedItem.Text;
        //GenerateBillPreview(BilltypeActive);
    }

    protected void searchActivatedBills_Click(object sender, EventArgs e)
    {
        LoadActivatedBill();
    }

    private void LoadActivatedBill()
    {
        try
        {
            String BillStatus = drpBillStatusype.SelectedItem.Text;
            String BillType = drpActivatedBillType.SelectedItem.Text;
            String FlatNumber = txtActBillsFlats.Text;
            BillCycle billCycle = new BillCycle();
            DataSet dsActivatedBill = billCycle.GetActivatedBill(BillStatus, BillType, FlatNumber);
            if (dsActivatedBill != null)
            {
                if (dsActivatedBill.Tables.Count > 0)
                {
                    FlatsBillsGrid.DataSource = dsActivatedBill;
                    FlatsBillsGrid.DataBind();
                    // lblTotalBillscount.Text = dsActivatedBill.Tables[0].Rows.Count.ToString();
                    lblActvBillsCount.Text = dsActivatedBill.Tables[0].Rows.Count.ToString();
                    FlatsBillsGrid.Columns[6].Visible = false;
                    FlatsBillsGrid.Columns[0].Visible = false;
                    DataTable model = dsActivatedBill.Tables[0];

                    //Added by Aarshi on 14 - Sept - 2017 for bug fix
                    if (dsActivatedBill.Tables[1].Rows[0][0].ToString() != string.Empty)
                        lblActivateCount.Text = dsActivatedBill.Tables[1].Rows[0][0].ToString();
                    if (dsActivatedBill.Tables[2].Rows[0][0].ToString() != string.Empty)
                        lblDeactivateCount.Text = dsActivatedBill.Tables[2].Rows[0][0].ToString();
                    if (dsActivatedBill.Tables[3].Rows[0][0].ToString() != string.Empty)
                        lblNotActivateCount.Text = dsActivatedBill.Tables[3].Rows[0][0].ToString();
                    //Ends here

                }
                else
                {

                }
            }
        }
        catch(Exception ex)
        {

        }

    }

    protected void FlatsBillsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        FlatsBillsGrid.PageIndex = e.NewPageIndex;
        String FlatNumber = txtActBillsFlats.Text;
        String ActBillType = drpBillStatusype.SelectedItem.Text;

        String BillType = drpActivatedBillType.SelectedItem.Text;

        LoadActivatedBill();

        if (FlatNumber == "Select" || BillType == "Select")
        {
            LoadActivatedBill();
        }
    }

    protected void FlatsBillsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            if (drv != null)
            {
                DateTime Currentdate = Utility.GetCurrentDateTimeinUTC();
                string id = drv.Row[0].ToString();
                String strStartDate = drv.Row["CycleStart"].ToString();
                String strEndDate = drv.Row["CycleEnd"].ToString();
                DateTime StartDate, EndDate;

                if (strStartDate != null && strStartDate != "" && strStartDate != "null")
                {
                    StartDate = Convert.ToDateTime(strStartDate);
                    EndDate = Convert.ToDateTime(strEndDate);
                }
                else
                {
                    StartDate = EndDate = DateTime.Now.AddDays(-1);
                }
                TableCell statusCell = e.Row.Cells[9];

                if (StartDate == EndDate)
                {
                    statusCell.Text = "InActive";
                }

                else if ((Currentdate > StartDate || Currentdate == StartDate) && Currentdate < EndDate)
                {
                    statusCell.Text = "Active";
                }

                else if (Currentdate < EndDate && Currentdate > StartDate)
                {
                    statusCell.Text = "DeActive";
                }

                else if (Currentdate < EndDate && Currentdate < StartDate)
                {
                    statusCell.Text = "SubScribed";
                }

                else if (Currentdate > StartDate && EndDate == Currentdate || EndDate < Currentdate)
                {
                    statusCell.Text = "DeActive";
                }
            }
        }
    }

    [System.Web.Services.WebMethod]
    public static List<string> GetFlatNumber(string FlatNumber)
    {
        //Added by Aarshi on 17 Aug 2017 for code restructuring
        List<string> Emp = new List<string>();
        BillCycle billCycle = new BillCycle();
        Emp = billCycle.GetFlatNumber(FlatNumber);
        return Emp;
    }

    protected void btnBillcycleSubmit_Click(object sender, EventArgs e)//Activated Bill
    {
        DataAccess dacess = new DataAccess();

        Double Amount = 0;
        String Description = "";
        Double PrevBalance = 0;

        String billType = HiddenField2.Value;

        //Added by Aarshi on 17 Aug 2017 for code restructuring
        BillPlan billPlan = new BillPlan();
        int billid = billPlan.GetBillID(billType);

        String CycleStart = txtCyclestart.Text;
        String CycleEnD = txtCycleend.Text;
        String ChargeType = HiddenFieldChargeType.Value;
        DateTime Date = Utility.GetCurrentDateTimeinUTC();
        String CycleType = drpCycletype.SelectedItem.Text;
        String Rate = txtRate.Text;
        String FlatNumber = txtFlatID.Text;



        //Added by Aarshi on 17 Aug 2017 for code restructuring
        BillCycle billCycle = new BillCycle();
        int ID = billCycle.GetFlatID(FlatNumber);

        int count = 0;
        if (ID != 0)
        {
            //BillCycle billCycle = new BillCycle();
            // Bill.ActivateBillForFlat(Convert.ToInt32(billid), FlatNumber, Convert.ToDateTime(CycleStart), Convert.ToDateTime(CycleEnD));
            bool result = billCycle.AddSingleFlatBillCycle(Convert.ToInt32(billid), FlatNumber, Convert.ToDateTime(CycleStart), Convert.ToDateTime(CycleEnD), "Activate Bill");

            if (result)
            {
                count = Bill.InsertFirstZeroBill(FlatNumber, billid, CycleType, CycleStart);
            }
        }

        else
        {
            lblbilltypeexist.Text = FlatNumber + " is Not Exist with this Society";
        }


        if (count == 1)
        {
            lblbilltypeexist.Text = "Bill Generated Sucessfully";
        }

        else
        {
            lblbilltypeexist.Text = "Bill Generated Failed";
        }


        LoadActivatedBill();
    }



    protected void ExportToExcelActive(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            FlatsBillsGrid.AllowPaging = false;
            this.LoadActivatedBill();

            FlatsBillsGrid.HeaderRow.BackColor = System.Drawing.Color.White;
            foreach (TableCell cell in FlatsBillsGrid.HeaderRow.Cells)
            {
                cell.BackColor = FlatsBillsGrid.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in FlatsBillsGrid.Rows)
            {
                row.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = FlatsBillsGrid.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = FlatsBillsGrid.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            FlatsBillsGrid.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }







    #endregion


    private void GenerateBillPreview(String BillType)
    {
        try
        {
            String ChargeType = "";
            DataAccess dacess = new DataAccess();
            if (BillType == "Select")
            {
               // uploadBill.Visible = false;
               // btnbillCaluclate.Visible = false;
               

            }
            else
            {
                //btnbillCaluclate.Visible = true;
                //lblBilltypeDes.Text = BillType;
                BillPlan billPlan = new BillPlan();
                bool result = billPlan.SetPlanDetails(BillType);
                if (result == true)
                {
                    //lblchargetypeDes.Text = billPlan.ChargeType;
                    //lblrateDes.Text = billPlan.Rate;
                    //HiddenBillDescp.Value = billPlan.BillID.ToString();

                    if (billPlan.ChargeType == "Manual")
                    {
                        //uploadBill.Visible = true;
                        //lblBillGeneraStatus.Text = "Manual Charge need to be imported from Excel";
                        //btnbillCaluclate.Text = "Import";

                    }
                    else
                    {
                        //uploadBill.Visible = false;
                        
                        //Added by Aarshi on 15 Aug 2017 for code restructuring
                        BillCycle billCycle = new BillCycle();
                        int count = billCycle.GetSingleValueBillCycle(billPlan.BillID);

                        //lblRowsEffectDes.Text = count.ToString();
                        //btnbillCaluclate.Text = "Generate Bill";
                        //lblBillGeneraStatus.Text = "";

                    }
                }
                else
                {
                   // lblBillGeneraStatus.Text = "Bil Plan do not exist, or connection problem";
                }

            }
        }
        catch (Exception ex)
        { }

        ClientScript.RegisterStartupScript(this.GetType(), "alert('')", "ShowGenerateBillPreview()", true);
    }

    public void LoadBillTypeDropdown(params DropDownList[] controls)
    {
        if (dsBillType == null)
        {
            BillPlan billPlan = new BillPlan();
            dsBillType = billPlan.GetActiveBillType();
        }
        if (dsBillType != null)
        {
            foreach (ListControl ctrl in controls)
            {
                ctrl.Items.Clear();
                foreach (DataRow dtRow in dsBillType.Tables[0].Rows)
                {
                    ctrl.Items.Add(new ListItem(dtRow["BillType"].ToString(), dtRow["BillID"].ToString()));
                }

                if (ctrl.ID == "drpCurrentBillType" || ctrl.ID == "drpActivatedBillType" || ctrl.ID == "drpGeneratedBillType")
                    ctrl.Items.Insert(0, new ListItem("Show All", "NA"));
                else
                    ctrl.Items.Insert(0, new ListItem("Select", "NA"));
            }
        }
    }

    protected void btnFlatbillGen_Click(object sender, EventArgs e)
    {
        DateTime CurrentBillEndDate = Utility.GetCurrentDateTimeinUTC();
        String GenerateCycle = "Manual";
        btnSingleFlatGenerate.Text = "Generate Bill";
        lblFlatNuber.Text = HiddenField1.Value;
        lblBillType.Text = HiddenField2.Value;



        previousBill = Bill.GetLastGeneratedBill(lblFlatNuber.Text, lblBillType.Text);

        newBill = Bill.CalculateNewBill(previousBill, GenerateCycle, CurrentBillEndDate);

        if (newBill.Days <= 0)
        {
            lblBillDuplicate.Text = "Bill is Already Generated";
        }

        else
        {
            lblBillDuplicate.Text = "";
        }

        lblFlatArea.Text = newBill.FlatArea.ToString();
        lblRate.Text = newBill.Rate.ToString();
        lblChargeType.Text = newBill.ChargeType;
        lblFromDate.Text = newBill.BillStartDate.ToShortDateString();
        txtFlatBillAmt.Visible = true;
        txtFlatBillAmt.Text = newBill.CurrentBillAmount.ToString();
        txtBillDate.Text = newBill.BillEndDate.ToShortDateString();
        lblPreviousBalance.Text = newBill.PreviousMonthBalance.ToString();

        ClientScript.RegisterStartupScript(this.GetType(), "alert('')", "ShowGenerateDeActivateBillForm()", true);
    }

    protected void btnDeactivatebill_Click(object sender, EventArgs e)
    {
        DateTime CurrentBillEndDate = Utility.GetCurrentDateTimeinUTC();
        btnSingleFlatGenerate.Text = "De Activate";

        String GenerateCycle = "Manual";
        lblFlatNuber.Text = HiddenField1.Value;
        lblBillType.Text = HiddenField2.Value;

        previousBill = Bill.GetLastGeneratedBill(lblFlatNuber.Text, lblBillType.Text);

        newBill = Bill.CalculateNewBill(previousBill, GenerateCycle, CurrentBillEndDate);

        if (newBill.Days <= 0)
        {
            lblBillDuplicate.Text = "Bill is Already Generated";
        }

        lblFlatArea.Text = newBill.FlatArea.ToString();
        lblRate.Text = newBill.Rate.ToString();
        lblChargeType.Text = newBill.ChargeType;
        lblFromDate.Text = newBill.BillStartDate.ToShortDateString();
        txtFlatBillAmt.Visible = true;
        txtFlatBillAmt.Text = newBill.CurrentBillAmount.ToString();
        txtBillDate.Text = newBill.BillEndDate.ToShortDateString();
        lblPreviousBalance.Text = newBill.PreviousMonthBalance.ToString();



        ClientScript.RegisterStartupScript(this.GetType(), "alert('')", "ShowGenerateDeActivateBillForm()", true);
    }

    //////Activated Bills  GenerateDeActivateBillForm     Caluclationbill Based on End date  click    //////
    protected void btnCalculateOnEnddate_Click(object sender, EventArgs e)
    {
        try
        {
            String GenerateCycle = "Manual";
            DateTime BillEnddate = Convert.ToDateTime(txtBillDate.Text);

            if (previousBill != null)
            {
                newBill = Bill.CalculateNewBill(previousBill, GenerateCycle, BillEnddate);
            }

            txtFlatBillAmt.Text = newBill.CurrentBillAmount.ToString();

            ClientScript.RegisterStartupScript(this.GetType(), "alert('')", "ShowAddBillPopup()", true);
        }
        catch( Exception ex)
        {}
    }

    //////Activated Bills GenerateDeActivateBillForm  Generate button click method //////
    protected void btnSingleFlatGenerate_Click(object sender, EventArgs e)
    {
      

        if (bill != null)
        {
            return;
        }
        else
        {
            bill = new Bill();
        }

        int BillID = newBill.BillID;
        DateTime BillEndDate = Convert.ToDateTime(txtBillDate.Text);
        String FlatNumber = lblFlatNuber.Text;
        Double PrevBalance = Convert.ToDouble(lblPreviousBalance.Text);
        Double CurrentAmount = Convert.ToDouble(txtFlatBillAmt.Text);
        String CycleType = HiddenFieldCycleType.Value;
        DateTime BillStartDate = Convert.ToDateTime(lblFromDate.Text);
        String BillType = HiddenField2.Value;
        String ChargeType = HiddenFieldChargeType.Value;
        String BillDescription = txtBillGenSingleFlatdesc.Text;

        if (newBill != null)
        {

            if (btnSingleFlatGenerate.Text == "De Activate")
            {
                bool result2 = bill.InsertNewBill(newBill);

                if (result2)
                {
                    bool result1 = DeActivateBill(newBill.BillID, newBill.FlatNumber, newBill.BillEndDate);

                    //Added by aarshi on 11-Sept-2017 for bug fix
                    if (result1)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "SetFocusScript", "<Script>self.close();</Script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertmessage", "javascript:alert('Bill Deactivated Successfully')", true);
                    }

                }
            }

            else if (btnSingleFlatGenerate.Text == "Generate Bill")
            {

                newBill.CurrentBillAmount = Convert.ToInt32(txtFlatBillAmt.Text);
                newBill.BillDescription = txtBillGenSingleFlatdesc.Text;
                bool result = bill.InsertNewBill(newBill);
                //bool result = true;
                if (result)
                {
                    //Added by aarshi on 8-Sept-2017 for bug fix
                    //lblBillDuplicate.Text = "Bill Generated Sucessfully";

                    //Added by Aarshi on 14 - Sept - 2017 for bug fix                    
                    string PaymentDueDate = newBill.PaymentDueDate.ToString("dd-MM-yyyy");
                    //Added by Aarshi on 21 - Sept - 2017 for bug fix

                    SendNotification(lblBillType.Text, PaymentDueDate);
                    SendMail();


                    ClientScript.RegisterStartupScript(GetType(), "SetFocusScript", "<Script>self.close();</Script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertmessage", "javascript:alert('Bill Generates Successfully')", true);

                }

                else
                {
                    lblBillDuplicate.Text = "Bill Generated Failed";
                }
            }

        }
        bill = null;


        //ClientScript.RegisterStartupScript(this.GetType(), "alert('')", "ShowGenerateDeActivateBillForm()", true);
    }


    private bool DeActivateBill(int BillID, String FlatNumber, DateTime DeactivationDate)
    {
        try
        {
            //Added by Aarshi on 16 Aug 2017 for Placing Sql Code in Bill Cycle Class
            //DataAccess dacess = new DataAccess();
            //String deactivatePlan = "update dbo.aBillCycle set CycleEnd = '" + DeactivationDate + "'  where BillID = '" + billid + "' and FlatID = '" + FlatNumber + "'";
            //bool result = dacess.Update(deactivatePlan);

            //Added by Aarshi on 16 Aug 2017 for Placing Sql Code in Bill Cycle Class
            BillCycle billCycle = new BillCycle();
            bool result = billCycle.UpdateDeactiveBill(DeactivationDate, BillID, FlatNumber);

            if (result == true)
            {
                lblbilltypeexist.Text = "plan deactivated Sucessfully";
            }

            else
            {
                lblbilltypeexist.Text = "plan deactivated failed";
            }

            return true;
        }

        catch (Exception ex)
        {
            return false;
        }



    }


    //Added by Aarshi on 14-Sept-2017 for bug fix
    public void SendNotification(string BillType, string PaymentDueDate)
    {
        Notification notification = new Notification();
        String GCMRegIDs = GetRegIDS();
        if (GCMRegIDs != "")
        {
            string message = "Bill for " + BillType + " is created.Due by " + PaymentDueDate;
            notification.SendNotification(GCMRegIDs, message);
        }
    }

    //Added by Aarshi on 14-Sept-2017 for bug fix
    public void SendMail()
    {
        string EmailID = string.Empty;
        string EmailSubject = string.Empty;
        string EmailBody = string.Empty;
        string FirstName = string.Empty;
        try
        {

            using (SqlConnection con1 = new SqlConnection(Utility.SocietyConnectionString))
            {
                int i = 0;

                DataTable dt = new DataTable();
                con1.Open();
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("Select Resident.EmailId,Resident.FirstName from dbo.Resident inner join ResidentNotification on Resident.ResID = ResidentNotification.ResID WHERE ResidentNotification.BillingMail = 1 AND (DeActiveDate IS NULL OR DeActiveDate > GETDATE()) AND Resident.FlatID = '" + HiddenField1.Value + "'", con1);

                myReader = myCommand.ExecuteReader();

                StringBuilder sb = new StringBuilder();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        EmailID = myReader["EmailId"].ToString();
                        FirstName = myReader["FirstName"].ToString();

                        EmailSubject = "Bill for " + lblBillType.Text + " is created.";

                        StringBuilder result = new StringBuilder();
                        result.Append("Hi " + FirstName + ",");
                        result.AppendLine();
                        result.AppendLine();
                        result.Append("For flat " + HiddenField1.Value + ",bill for " + lblBillType.Text + " is created.");
                        result.AppendLine();
                        result.Append("Do the payment by " + newBill.PaymentDueDate.ToString("dd-MM-yyyy") + ".");
                        result.AppendLine();
                        result.AppendLine();
                        result.Append("This is auto generated mail please do not reply.");

                        EmailBody = Convert.ToString(result);

                        Notification notification = new Notification();
                        notification.SendMail(EmailID, EmailSubject, EmailBody);
                    }
                }
            }
        }
        catch (Exception ex) { }
    }

    //Added by Aarshi on 14-Sept-2017 for bug fix
    private String GetRegIDS()
    {
        String RegIDS = "";
        try
        {

            using (SqlConnection con1 = new SqlConnection(Utility.SocietyConnectionString))
            {
                int i = 0;

                DataTable dt = new DataTable();
                con1.Open();
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("Select distinct RegID from dbo.GCMList GList INNER JOIN Resident Res ON Res.MobileNo = GList.MobileNo inner join ResidentNotification on Res.ResID = ResidentNotification.ResID WHERE ResidentNotification.BillingNotification = 1 AND Res.FlatID = '" + HiddenField1.Value + "'", con1);

                myReader = myCommand.ExecuteReader();

                StringBuilder sb = new StringBuilder();

                while (myReader.Read())
                {
                    sb.Append(myReader["RegID"].ToString().Trim());
                    sb.Append("\",\"");
                }
                if (sb.Length > 10)
                {
                    sb.Remove(sb.Length - 3, 3);

                    RegIDS = sb.ToString();
                }

            }

            return RegIDS;
        }
        catch (Exception ex)
        {
            return RegIDS;
        }

    }


    //Ends here
}