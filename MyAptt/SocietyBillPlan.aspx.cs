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

public partial class SocietyBillPlan : System.Web.UI.Page
{

    static DataSet dsBillType;
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadSocietyBillPlanData();
    }


    #region BillPlan

    protected void linkSocietyplans_Click(object sender, EventArgs e)
    {
        try
        {
            
            ClientScript.RegisterStartupScript(this.GetType(), "alert('')", "SocietyplansClick()", true);
        }
        catch (Exception ex)
        {
            Utility.log("Society Bill Plan Link Error :" + ex.Message);
        }
    }

    public void LoadSocietyBillPlanData()
    {
        try
        {
            BillPlan bill = new BillPlan();
            DataSet dsBillPlan = bill.GetPlans();
            if (dsBillPlan != null)
            {
                if (dsBillPlan.Tables.Count > 0)
                {
                    BillPlanDataList.DataSource = dsBillPlan;
                    BillPlanDataList.DataBind();

                }
            }
            else
            {
                lblBillPlanStatus.Text = "Received Empty Data";
            }
        }
        catch (Exception ex)
        {
            Utility.log("Load Bill Plans Error " + ex.Message);
        }
    }

    protected void SocietyBillPlan_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (drv != null)
                {
                    string ID = drv.Row[0].ToString();
                }

                Label lbl = (Label)e.Item.FindControl("lblBillEntity");

                String lablstatus = lbl.Text;

                if (lablstatus == "1")
                {
                    lbl.Text = "All";
                }
                if (lablstatus == "0")
                {
                    lbl.Text = "Selected";
                }

                Label lblID = (Label)e.Item.FindControl("lblID");
                String lablID = lblID.Text;


                //Added by Aarshi on 17 Aug 2017 for code restructuring
                BillCycle billCycle = new BillCycle();
                int Count = billCycle.GetFlatIDCount(lablID);

                Label lblcounttext = (Label)e.Item.FindControl("lblcount");
                lblcounttext.Text = Count.ToString();

            }
        }
        catch (Exception ex)
        {
            Utility.log("Bill Plan Data Bound Error :" + ex.Message);
        }
    }

    protected void drpAddBillType_SelectedIndexChanged(object sender, EventArgs e)
    {

        //Added by Aarshi on 17 Aug 2017 for code restructuring
        BillPlan billPlan = new BillPlan();
        int BillID = billPlan.GetBillID(drpAddBillType.SelectedItem.Text);

        if (BillID == 0)
        {
            lblBillCheck.Text = "";
        }
        else
        {
            lblBillCheck.Text = "Bill Type already Exists";
        }
    }

    protected void btnPlanSubmit_Click(object sender, EventArgs e)
    {
        DataAccess dacess = new DataAccess();

        int ApplyTo = 1;
        try
        {
            if (radioall.Checked == true)
            {
                ApplyTo = 1;
            }

            if (radioselected.Checked == true)
            {
                ApplyTo = 0;
            }

            // DateTime date = System.DateTime.Now;
            DateTime date = Utility.GetCurrentDateTimeinUTC();

            String ChargeType = drpchargetype.SelectedItem.Text;
            String CycleType = drpbillcycle.SelectedItem.Text;


            BillPlan billPlan = new BillPlan();

            int BillID = billPlan.AddSocietyBillPlan(drpAddBillType.SelectedItem.Text, ChargeType, txtBillRate.Text, CycleType, ApplyTo);

            if (BillID > 0)
            {
                BillCycle billCycle = new BillCycle();
                bool result = billCycle.AddBillCycleToAllFlats(BillID, ApplyTo);
                if (result == true)
                {
                    //added by aarshi on 31-July 2017 to add zero bill
                    //Bill.InsertFirstZeroBill(,BillID,CycleType,)
                    lblBillstatus.Text = "Bill Plan Added Sucessfully, Bill Mapped to All Flats";
                    billCycle.GenerateInitialZeroBillFlat(BillID, ApplyTo, CycleType);
                }

                else
                {
                    lblBillstatus.Text = "Bill Plan Added Sucessfully, Bill mapping to Flat failed";
                }
                LoadSocietyBillPlanData();

                dsBillType = billPlan.GetActiveBillType();
            }
            else
            {
                lblBillstatus.Text = "Failed to Add Bill Plan, Please try later.";
            }



        }
        catch (Exception ex)
        {
            lblBillstatus.Text = ex.Message;
        }


    }

    protected void btnBillingEdit_Click(object sender, EventArgs e)
    {
        String billID = HiddenPlanBillID.Value;

        int ApplyTo = 0;

        if (radioall.Checked == true)
        {
            ApplyTo = 1;
        }

        if (radioselected.Checked == true)
        {

            ApplyTo = 0;
        }



        //Added by Aarshi on 17 Aug 2017 for code restructuring
        BillPlan billPlan = new BillPlan();
        bool result = billPlan.UpdateSocietyBillPlan(drpchargetype.SelectedItem.Text, txtBillRate.Text, drpbillcycle.SelectedItem.Text, ApplyTo, billID);

        if (result == true)
        {
            LoadSocietyBillPlanData();
            lblBillstatus.Text = "Bill Edited Sucessfully";
        }
        else
        {
            lblBillstatus.Text = "Bill Edited Failed";
        }


    }

    protected void btnsocietyPlanEdit_Click(object sender, EventArgs e)
    {
        //MultiView1.ActiveViewIndex = 0;
        String billID = HiddenPlanBillID.Value;
        String BillType = HiddenPlanBillType.Value;
        HiddenEditsocietyID.Value = billID;
        HiddenFormRequired.Value = "NewEditSocietyPlanForm";

        drpAddBillType.SelectedValue = BillType;
        drpbillcycle.SelectedValue = HiddenCycleType.Value;
        drpchargetype.SelectedValue = HiddenChargeType.Value;
        txtBillRate.Text = HiddenEditSocietyRate.Value;

        int ApplyTo = Convert.ToInt32(HiddenApplyTo.Value);

        if (ApplyTo == 1)
        {
            radioall.Checked = true;
        }

        else
        {

            radioselected.Checked = true;
        }

        //btnBillingEdit.Visible = true;
        //btnBillingsubmit.Visible = false;

        ClientScript.RegisterStartupScript(this.GetType(), "alert('')", "ShowEditPlanPopup()", true);
    }

    protected void btnSocietyPlanDeactive_Click(object sender, EventArgs e)
    {

        String billID = HiddenPlanBillID.Value;
        String BillType = HiddenPlanBillType.Value;

        //Added by Aarshi on 17 Aug 2017 for code restructuring
        BillPlan billPlan = new BillPlan();
        billPlan.DeactiveSocietyBillPlan(billID);
        LoadSocietyBillPlanData();
    }

    #endregion

}