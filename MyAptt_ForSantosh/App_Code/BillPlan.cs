using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for BillPlan
/// </summary>
public class BillPlan
{
    public int BillID;
    public String BillType;
    public String ChargeType;
    public String Rate;
    public String CycleType;
    public int Applyto;

    private static String Table_Name = "dbo.Societybillplans";
	public BillPlan()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public DataSet GetPlans()
    {
        try
        {
            DataAccess dacess = new DataAccess();
            //String VendorQuery = "Select * from dbo.aSocietybillplans";
            String VendorQuery = "Select * from "+ Table_Name;
            DataSet ds = dacess.GetData(VendorQuery);
            return ds;
        }
        catch(Exception ex)
        {
            return null;
        }
    }

    public bool SetPlanDetails(String BillType)
    {
        DataAccess dacess = new DataAccess();
        try {
            String BillTypeDescp = "Select * from "+Table_Name+" where Billtype ='" + BillType + "'";
            SqlConnection dbConnect = dacess.ConnectSocietyDB();
            SqlCommand cmd = new SqlCommand(BillTypeDescp, dbConnect);
            SqlDataReader rdr = cmd.ExecuteReader();

            if (!rdr.HasRows)
            {
            }

            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    ChargeType = rdr["ChargeType"].ToString();
                    Rate = rdr["Rate"].ToString();
                    BillID = Convert.ToInt32(rdr["BillId"].ToString());
                }
            }
            return true;
        }
        catch (Exception ex)
        {

            return false;
        }
    }

    public DataSet GetActiveBillType()
    {
        try
        {
            DataAccess dacess = new DataAccess();
            String FillBillType = "Select distinct BillType,BillID from " + Table_Name;
            return dacess.ReadData(FillBillType);
        }
        catch (Exception ex)
        {

            return null;
        }

    }

    public int AddSocietyBillPlan(String BillType, String ChargeType, String Rate, String CycleType, int Applyto)
    {
        int ID=0;
        DataAccess dacess = new DataAccess();
        String BillingQuery = "Insert into "+ Table_Name+" (BillType,ChargeType,Rate,CycleType,Applyto) Values('" + BillType + "','" + ChargeType + "','" + Rate + "','" + CycleType + "','" + Applyto + "')";
        bool result = dacess.Update(BillingQuery);
        if (result == true)
        {
            String IDQuery = "Select Max(BillID) from "+Table_Name;
            ID = dacess.GetSingleValue(IDQuery);
        }


        return ID;
    }

    public int GetBillID(string BillType)
    {
        DataAccess dacess = new DataAccess();
        String GetBillIdQuery = "Select BillID from " + Table_Name + " where BillType ='" + BillType + "'";
        int billID = dacess.GetSingleValue(GetBillIdQuery);
        return billID;
    }

    public bool UpdateSocietyBillPlan(string ChargeType, string BillRate, string CycleType, int ApplyTo, string billID)
    {
        DataAccess dacess = new DataAccess();
        String DelSocietyPlanQuery = "Update " + Table_Name + " set ChargeType ='" + ChargeType + "', Rate ='" + BillRate + "', CycleType ='" + CycleType + "', ApplyTo ='" + ApplyTo + "' where BillID = '" + billID + " '";
        bool result = dacess.Update(DelSocietyPlanQuery);
        return result;
    }

    public bool DeactiveSocietyBillPlan(string billID)
    {
        DataAccess dacess = new DataAccess();
        String DelSocietyPlanQuery = "Delete from  dbo.SocietyBillPlans where BillID = '" + billID + "' ";
        bool result = dacess.Update(DelSocietyPlanQuery);
        return result;
    }
}