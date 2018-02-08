using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;


public partial class BillPayments : System.Web.UI.Page
{

    /*  This Total  code  is  licenced
    Developed By Anvisys Technologies Pvt Ltd.
    
    Copyright © 2016 Anvisys Technologies Pvt. Ltd.All rights reserved.
    */

    User muser;
    private static String TableName = "dbo.GeneratedBill";
    private static String ViewName = "dbo.ViewLatestGeneratedBill";

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
            
            DueData();
        }
    }

    public void DueData()
     {
        DataAccess daceess = new DataAccess();
        DateTime CureentDate = Utility.GetCurrentDateTimeinUTC(); 
   
        DateTime CurrentDate = CureentDate.AddMonths(+1);

        int month = CurrentDate.Month;
        int year = CurrentDate.Year;
   
        DateTime startDate = new DateTime(year, month, 1);
        String CurrentMonth = String.Format("{0:y}", startDate);
        String PrevMonth = String.Format("{0:y}", CureentDate);
        Bill bill = new Bill();

        DataSet ds = bill.GetLatestBills(muser.FlatNumber, "Show All","","");
     
        if (ds == null)
        {
                lblResBillsEmptyText.Text = "There are no bills  for this Resident";
        }
        else
        {
            DataTable dt = ds.Tables[0];
            if (ds.Tables.Count > 0)
            {
                DataUserBills.DataSource = dt;
                DataUserBills.DataBind();
                MultiView1.ActiveViewIndex = 0;
            }   
        }
    }

    protected void lnkAmount_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
    }
    protected void DataClub_ItemCommand(object source, DataListCommandEventArgs e)
    {
        LinkButton lnkid = (LinkButton)e.Item.FindControl("LinkID");
        //Session["PayID"] = lnkid.Text;
        //Added by Aarshi on 18 auh 2017 for session storage
        SessionVariables.PayID = lnkid.Text;
        LinkButton BillType = (LinkButton)e.Item.FindControl("lnkBilltype");
       // DateTime Date = System.DateTime.Now;
        DateTime Date = Utility.GetCurrentDateTimeinUTC();  
        DataAccess dacess = new DataAccess();
        String BillPaidCheck = "select AmountPaidDate,TransactionID,InvoiceID From dbo.GeneratedBill where PayID = '" + lnkid.Text + "'";
        DataSet dscheck = dacess.ReadData(BillPaidCheck);
          String PaidDateFormat ="";
        DataTable dtcheck = dscheck.Tables[0];
        

        String TransactionID = dtcheck.Rows[0][1].ToString();
        String InvoiceID = dtcheck.Rows[0][2].ToString();

        if (TransactionID == "" || InvoiceID == "")
        {
            String BillsQuery = " select BillType,FlatNumber,CurrentBillAmount,PreviousMonthBalance,BillMonth,PaymentDueDate,CycleType,AmountTobePaid,Rate,ChargeType,FlatArea from dbo.ViewLatestGeneratedBill where PayID ='" + lnkid.Text + "' ";
            DataSet ds = dacess.ReadData(BillsQuery);
            DataTable dt = ds.Tables[0];
            lblBillType.Text = dt.Rows[0][0].ToString();
            lblFlat.Text = dt.Rows[0][1].ToString();
            lblAmtPaid.Text = dt.Rows[0][2].ToString();
            lblBalance.Text = dt.Rows[0][3].ToString();
            lblDueFor.Text = dt.Rows[0][4].ToString();
            lblDueDate.Text = dt.Rows[0][5].ToString();
            lblCycletype.Text = dt.Rows[0][6].ToString();
            lblTotalPay.Text = dt.Rows[0][7].ToString();

           //Added by Aarshi on 18 auh 2017 for session storage
            SessionVariables.Rate = dt.Rows[0][8].ToString();
            SessionVariables.ChargeType = dt.Rows[0][9].ToString();
            SessionVariables.FlatArea = dt.Rows[0][10].ToString();

            lblCurrentTime.Text = Date.ToString();
            txtAmt.Text = lblAmtPaid.Text;
            MultiView1.ActiveViewIndex = 1;
        }

        else
        {
            lblPaymentCheck.Text =  BillType.Text  +"  " + "Bill  is Already Paid. " + PaidDateFormat;
        }
    }

    protected void btnpaymannual_Click(object sender, EventArgs e)
    {
        GenerateBill payBill = new GenerateBill();
        //payBill.PayID = Convert.ToInt32(Session["PayID"]);
        //Added by Aarshi on 18 auh 2017 for session storage
        payBill.PayID = Convert.ToInt32(SessionVariables.PayID);

        payBill.AmountPaid = Convert.ToInt32(txtAmt.Text);
        payBill.PaymentMode = drpPayMode.SelectedItem.Text;
        payBill.AmountPaidDate = Utility.GetCurrentDateTimeinUTC();
        payBill.TransactionID = txtTransaID.Text;
        payBill.InvoiceID = txtInvID.Text;

        Bill bill = new Bill();

        bool result = bill.UpdatePayment(payBill);
        if (result == true)
        {
            lblBillPaidstatus.Text = "Submitted Sucessfully";
            lblBillPaidstatus.ForeColor = System.Drawing.Color.Green;
           
        }
        else {
            lblBillPaidstatus.Text = "Could not Update Bill";
        }
    }
    

    protected void PayLink_Click(object sender, CommandEventArgs e)
    {
       int PayID = Convert.ToInt32(e.CommandArgument.ToString());
       ShowPaymentForm(PayID);
        //Label1.Text = e.CommandArgument.ToString();
      
    }

    protected void linkDetails_Click(object sender, CommandEventArgs e)
    {
        int PayID = Convert.ToInt32(e.CommandArgument.ToString());
        SessionVariables.PayID = PayID.ToString();

        String detailsQuery = "Select Top 2  * from [dbo].[GeneratedBill]   inner join " +
                               "( Select FlatNumber, BillID from [dbo].[GeneratedBill] where PayID=" + PayID +
                               ") FlatBill on GeneratedBill.FlatNumber = FlatBill.FlatNumber and GeneratedBill.BillID = FlatBill.BillID  order by ModifiedAt desc";
        //Label1.Text = e.CommandArgument.ToString();
        DataAccess da = new DataAccess();
        DataSet ds = da.GetData(detailsQuery);
        String row1="", row2="";
         if(ds != null)
         {
             if(ds.Tables[0].Rows.Count >= 2)
             {
                 row1 = "<tr style='height:20px;'><td colspan='8'></td></tr><tr>" +
                         "<td colspan='1' style='background-color:#cc0000; border-radius:3px;color:white;'>Previous Month</td>" +
                         "<td colspan='7'></td>"+
                     "</tr>"+
                     "<tr>"+
                         "<td> Start Date :</td><td>"+
                             ds.Tables[0].Rows[1]["BillStartDate"]
                                 +"</td><td style='width:20px'></td>"+
                         "<td> End Date: </td><td>"+
                          ds.Tables[0].Rows[1]["BillEndDate"]
                            +  "</td><td colspan='3'></td>"+
                     "</tr>"+
                     "<tr>"+
                         "<td> Bill Amount:</td><td>"+
                          ds.Tables[0].Rows[1]["CurrentBillAmount"]
                         +"</td><td style='width:20px'></td>"+
                         "<td> Previous Balance:</td><td>"+
                         ds.Tables[0].Rows[1]["PreviousMonthBalance"]
                          +"</td><td style='width:20px'></td>"+
                         "<td> Amount Paid:</td><td>"+
                          ds.Tables[0].Rows[1]["AmountPaid"]
                     + "</td></tr>"+
                    " <tr ><td> Payment Date:</td><td>" +
                   ds.Tables[0].Rows[1]["AmountPaidDate"]
                        +"</td><td style='width:20px'></td>"+
                          "<td> Payment Mode:</td><td>"+
                          ds.Tables[0].Rows[1]["PaymentMode"]
                        +"</td><td style='width:20px'></td>"+
                        "<td> Transaction ID:</td><td>"+
                        ds.Tables[0].Rows[1]["TransactionID"] 
                        +"</td></tr>"
                        + " <tr style='height:20px;' class='last_row'><td colspan='8'></td></tr>";  
             }

             if (ds.Tables[0].Rows[0] != null)
             {
                 row2 = "<tr style='height:20px;'><td colspan='8'></td></tr><tr>" +
                        "<td colspan='1' style='background-color:#cc0000; border-radius:3px;color:white;'>Current Month</td>" +
                        "<td colspan='7'></td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td > Start Date :</td><td>" +
                            ds.Tables[0].Rows[0]["BillStartDate"]
                                + "</td><td style='width:20px'></td>" +
                        "<td> End Date: </td><td>" +
                         ds.Tables[0].Rows[0]["BillEndDate"]
                           + "</td><td colspan='3'></td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td> Bill Amount:</td><td>" +
                         ds.Tables[0].Rows[0]["CurrentBillAmount"]
                        + "</td><td style='width:20px'></td>" +
                        "<td> Previous Balance:</td><td>" +
                        ds.Tables[0].Rows[0]["PreviousMonthBalance"]
                         + "</td><td style='width:20px'></td>" +
                        "<td> Amount Paid:</td><td>" +
                         ds.Tables[0].Rows[0]["AmountPaid"]
                    + "</td></tr>" +
                   " <tr ><td> Payment Date:</td><td>" +
                  ds.Tables[0].Rows[0]["AmountPaidDate"]
                       + "</td><td style='width:20px'></td>" +
                         "<td> Payment Mode:</td><td>" +
                         ds.Tables[0].Rows[0]["PaymentMode"]
                       + "</td><td style='width:20px'></td>" +
                       "<td> Transaction ID:</td><td>" +
                       ds.Tables[0].Rows[0]["TransactionID"]
                       + "</td></tr>"
                       + " <tr style='height:20px;' class='last_row'><td colspan='8'></td></tr>"; 

             }

             lblBillDetails.Text = "<table>" + row1 + row2+ "</table>";
         }
         else
         {
         lblBillDetails.Text = "No Details Available";
         }
        MultiView1.ActiveViewIndex = 2;
    }

    protected void btnPay_Click(object sender, EventArgs e)
    {
        int PayID = Convert.ToInt32(SessionVariables.PayID);
        ShowPaymentForm(PayID);
       // MultiView1.ActiveViewIndex = 1;
    }

    private void ShowPaymentForm( int PayID)
    {
    DateTime Date = Utility.GetCurrentDateTimeinUTC();  
        DataAccess dacess = new DataAccess();
        String BillPaidCheck = "select AmountPaidDate,TransactionID,InvoiceID From dbo.GeneratedBill where PayID = '" + PayID + "'";
        DataSet dscheck = dacess.ReadData(BillPaidCheck);
          String PaidDateFormat ="";
        DataTable dtcheck = dscheck.Tables[0];
        

        String TransactionID = dtcheck.Rows[0][1].ToString();
        String InvoiceID = dtcheck.Rows[0][2].ToString();

        if (TransactionID == "" || InvoiceID == "")
        {
            String BillsQuery = " select BillType,FlatNumber,CurrentBillAmount,PreviousMonthBalance,BillMonth,PaymentDueDate,CycleType,AmountTobePaid,Rate,ChargeType,FlatArea from dbo.ViewLatestGeneratedBill where PayID ='" + PayID + "' ";
            DataSet ds = dacess.ReadData(BillsQuery);
            DataTable dt = ds.Tables[0];
            lblBillType.Text = dt.Rows[0][0].ToString();
            lblFlat.Text = dt.Rows[0][1].ToString();
            lblAmtPaid.Text = dt.Rows[0][2].ToString();
            lblBalance.Text = dt.Rows[0][3].ToString();
            lblDueFor.Text = dt.Rows[0][4].ToString();
            lblDueDate.Text = dt.Rows[0][5].ToString();
            lblCycletype.Text = dt.Rows[0][6].ToString();
            lblTotalPay.Text = dt.Rows[0][7].ToString();
            
           
            //Added by Aarshi on 18 auh 2017 for session storage
            SessionVariables.Rate = dt.Rows[0][8].ToString();
            SessionVariables.ChargeType = dt.Rows[0][9].ToString();
            SessionVariables.FlatArea = dt.Rows[0][10].ToString();

            lblCurrentTime.Text = Date.ToString();
            txtAmt.Text = lblAmtPaid.Text;
            MultiView1.ActiveViewIndex = 1;
        }

        else
        {
            lblPaymentCheck.Text = lblBillType.Text + "  " + "Bill  is Already Paid. " + PaidDateFormat;
        }
    }

   
    protected void btnpayCancel_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;

        DueData();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;

        DueData();
    
    }

    protected void DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;

                int currentBalance = Convert.ToInt32(drv.Row["CurrentMonthBalance"].ToString());
                DateTime BillDate = Convert.ToDateTime(drv.Row["PaymentDueDate"]);

                if ((DateTime.Compare(Utility.GetCurrentDateTimeinUTC(), BillDate) > 0) && currentBalance > 0)
                {
                    e.Item.ForeColor = System.Drawing.Color.IndianRed;
                    Label item1 = (Label)e.Item.FindControl("Label4");
                    Label item2 = (Label)e.Item.FindControl("Label12");
                    item1.ForeColor = System.Drawing.Color.IndianRed;
                    item2.ForeColor = System.Drawing.Color.IndianRed;
                }

            }

        }
        catch (Exception ex)
        { }
    }
}