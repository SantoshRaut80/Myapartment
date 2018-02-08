using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
public partial class Dashboard : System.Web.UI.Page
{

    User mUser;
    protected void Page_Load(object sender, EventArgs e)
    {
        mUser = SessionVariables.User;
        if (mUser == null)
        {
            Response.Redirect("Login.aspx");
        }

        SetSummary();

        SetBillData();
        SetComplaintData();
        SetForumData();
        ComplaintChart();
    }


    private void SetSummary()
    {
        int TotalFlat = 0, Resident = 0, Tenant = 0;
        String Offer1 = "", Offer2 = "", Offer3 = "", Notice1 = "", Notice2 = "", Notice3 = "", Poll1 = "", Poll2 = "", Poll3 = "";
        String sumQuery = "Select * from dbo.NewSummary";
        DataAccess da = new DataAccess();
        DataSet ds = da.GetData(sumQuery);
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                TotalFlat = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalFlat"]);
                Resident = Convert.ToInt32(ds.Tables[0].Rows[0]["Resident"]);
                Tenant = Convert.ToInt32(ds.Tables[0].Rows[0]["Tenant"]);
                Offer1 = ds.Tables[0].Rows[0]["Offer1"].ToString();
                Offer2 = ds.Tables[0].Rows[0]["Offer2"].ToString();
                Offer3 = ds.Tables[0].Rows[0]["Offer3"].ToString();
                Notice1 = ds.Tables[0].Rows[0]["Notice1"].ToString();
                Notice2 = ds.Tables[0].Rows[0]["Notice2"].ToString();
                Notice3 = ds.Tables[0].Rows[0]["Notice3"].ToString();
                Poll1 = ds.Tables[0].Rows[0]["Poll1"].ToString();
                Poll2 = ds.Tables[0].Rows[0]["Poll2"].ToString();
                Poll3 = ds.Tables[0].Rows[0]["Poll3"].ToString();

            }
        }

        lblFlatInfo.Text = "Number of Flats: " + TotalFlat + "</br>" + "Number of Owners: " + Resident + "</br>" + "Number of Tenants: " + Tenant;

        lblOffer.Text = Offer1 + "</br>" + Offer2 + "</br>" + Offer3;

        lblTopNotice.Text = Notice1 + "</br>" + Notice2 + "</br>" + Notice3;

        lblTopPoll.Text = Poll1 + "</br>" + Poll2 + "</br>" + Poll3;
    }

    private void SetBillData()
    {
        try
        {

            String billQuery = "  Select Count(PayID) from [dbo].[ViewLatestGeneratedBill] where FlatNumber = '" + mUser.FlatNumber + "' and CurrentMonthBalance >0";
            DataAccess da = new DataAccess();
            int pendingBillCount = da.GetSingleValue(billQuery);

            lblpending.Text = pendingBillCount + " Bills Pending";

        }
        catch (Exception ex)
        {
            int a = 1;
        }

    }

    private void SetComplaintData()
    {
        try
        {
            String query = "  Select Count(LastStatus) as number, LastStatus   from [dbo].[ViewComplaintSummary] where month(LastAt) = month(CURRENT_TIMESTAMP) and ResidentID = " + mUser.ResiID + " Group by LastStatus";

            DataAccess da = new DataAccess();
            DataSet ds = da.GetData(query);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    String yrQuery = "  Select Count(LastStatus) as number, LastStatus   from [dbo].[ViewComplaintSummary] where year(LastAt) = year(CURRENT_TIMESTAMP) and ResidentID = " + mUser.ResiID + " Group by LastStatus";
                    DataSet dsy = da.GetData(query);
                    if (dsy == null)
                    {
                        lblComplaintInfo.Text = "Unable to retrieve data try again";
                        return;
                    }
                    dt = dsy.Tables[0];
                }
                int[] x = new int[3];
                int initiated = 0, resolved = 0, assigned = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    x[i] = Convert.ToInt32(dt.Rows[i][0]);
                    if ((dt.Rows[i][1]).ToString() == "Initiated")
                    {
                        initiated = x[i];
                    }
                    else if ((dt.Rows[i][1]).ToString() == "Resolved")
                    {
                        resolved = x[i];
                    }
                    else if ((dt.Rows[i][1]).ToString() == "Assigned")
                    {
                        assigned = x[i];
                    }
                }
                lblComplaintInfo.Text = "Initiated = " + initiated + "<br/>" + "Assigned = " + assigned + "<br/>" + "Resolved = " + resolved;
            }
        }
        catch (Exception ex)
        {
            lblComplaintInfo.Text = "Unable to retrieve data try again";
        }

    }

    private void SetForumData()
    {
        String forumQuery = "Select Top 3 * from dbo.ViewThreadSummaryNoImageCount order by UpdatedAt desc";

        DataAccess da = new DataAccess();
        DataSet ds = da.GetData(forumQuery);

        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                String forumString = "";// "<div class=\"long_text\">";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var forText = ds.Tables[0].Rows[i]["FirstThread"].ToString();
                    if (forText.Length > 50)
                    {
                        forText = forText.Substring(0, 50);
                    }

                    forumString += "<div class=\"long_text\">" + forText + "</div>";

                }
                //forumString += "</div>";
                lblTopForum.Text = forumString; //forumString.Substring(0,forumString.Length-5) ;
            }

        }
    }

    private void ComplaintChart()
    {
        try
        {
            DataAccess dacess = new DataAccess();

            String BarChartQuery = "Select Count(*) as 'Number_Of_Complaints' , Age from dbo.ViewComplaintSummary where LastStatusID=4 Group By Age";

            String query = " select t.range as [Age_range], count(*) as [Number_Of_Complaints]"
                          + "from (  select case  "
                               + " when Age between 0 and 1 then ' 0- 1'"
                               + " when Age between 2 and 5 then '2-5'"
                               + " else '6-10' end as range"
                                + " from dbo.ViewComplaintSummary) t group by t.range";

            DataSet databarchart = dacess.ReadData(query);
            if (databarchart == null)
            {
                lblEmptyDataText.Text = "Age of Complaints Data is Empty";
                BarChart.Width = 0;
            }

            else
            {
                DataTable dtable = databarchart.Tables[0];
                BarChart.DataSource = dtable;
                BarChart.Series["Series1"].XValueMember = "Age_range";
                BarChart.Series["Series1"].YValueMembers = "Number_Of_Complaints";
                BarChart.ChartAreas[0].AxisX.Title = "Duration in Days";
                BarChart.ChartAreas[0].AxisY.Title = "number of complaints";
                BarChart.DataBind();
            }

            String PieChartQuery = "Select Count(*) as Number_Of_Complaints , CompType from dbo.ViewComplaintSummary Group By CompType";

            //  ReportPieChart.DataSource = dacess.ReadData(PieChartQuery).Tables[0];

            DataSet ds = dacess.ReadData(PieChartQuery);


            if (ds == null)
            {
                lblpiechart.Text = "Percentage of Complaint Category Data is Empty";
            }
            else
            {
                DataTable dt = ds.Tables[0];
                string[] x = new string[dt.Rows.Count];
                int[] y = new int[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    x[i] = dt.Rows[i][1].ToString();
                    y[i] = Convert.ToInt32(dt.Rows[i][0]);

                }

                Piechart.Series["Series1"].Points.DataBindXY(x, y);
                Piechart.Series["Series1"].Label = "#PERCENT{P2}";
                this.Piechart.Series[0]["PieLabelStyle"] = "Inside";
                //Title Title = new Title("No Of Complaints", Docking.Top, new Font("Verdana", 12), Color.Black);
                Piechart.Titles.Add(Title);
                Piechart.DataBind();
            }
        }
        catch (Exception ex)
        {
            Piechart.Visible = false;
            //Barchart.Visible = false;
            //lblbarchart.Text = "Complaints data is empty";
            lblpiechart.Text = "Complaint Category data is empty";
        }
    }
}