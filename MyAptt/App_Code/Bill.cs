using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;


/// <summary>
/// Summary description for Bill
/// </summary>
public class Bill
{

    private static String TableName = "dbo.GeneratedBill";
    private static String ViewName = "dbo.ViewLatestGeneratedBill";

    public Bill()
    {
        //
        // TODO: Add constructor logic here
    }

    private static DataSet ReadData(String FlatNumber)
    {

        DataAccess dacess = new DataAccess();
        String BillTypeQuery = "Select FlatID,FlatArea,BillID,BillType,ChargeType,CycleType,DueDay,Rate,CurrentBalance from " + ViewName + " where FlatNumber ='" +FlatNumber +"'";
        DataSet BillTypeData = dacess.ReadData(BillTypeQuery);
        return BillTypeData;
    }

    private static void AddFlat()
    {
    }

    


    private static void AddFlatBillCycle(int BillID, int ApplyTo)
    {
        DateTime CycleStart = new DateTime();
        DateTime CycleEnd = new DateTime();
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
        DataAccess dacess = new DataAccess();
        String FlatDataQuery = "Select * from dbo.Flats";
        DataSet Data = dacess.ReadData(FlatDataQuery);
        DataTable dataFlat = Data.Tables[0];
        DataTable tempBillCycle = new DataTable();

        tempBillCycle.Columns.Add("FlatNumber");

        tempBillCycle.Columns.Add("BillID", typeof(int));
        tempBillCycle.Columns.Add("CycleStart", typeof(DateTime));
        tempBillCycle.Columns.Add("CycleEnD", typeof(DateTime));

        foreach (DataRow FlatRow in dataFlat.Rows)
        {

            String Flat = FlatRow["FlatNumber"].ToString();
            int BillIDAdd = BillID;

            tempBillCycle.Rows.Add(Flat, BillIDAdd, CycleStart, CycleEnd);

        }

        String SqlSocietyString = String.Empty;
        SqlSocietyString = Utility.SocietyConnectionString; ;
        SqlConnection SocietyCon = new SqlConnection(SqlSocietyString);
        SocietyCon.Open();

        using (SqlBulkCopy FlatBillsData = new SqlBulkCopy(SocietyCon))
        {

            FlatBillsData.DestinationTableName = "aBillCycle";
            FlatBillsData.ColumnMappings.Add("FlatNumber", "FlatID");
            FlatBillsData.ColumnMappings.Add("BillID", "BillID");
            FlatBillsData.ColumnMappings.Add("CycleStart", "CycleStart");
            FlatBillsData.ColumnMappings.Add("CycleEnD", "CycleEnD");

            FlatBillsData.WriteToServer(tempBillCycle);
        }
        SocietyCon.Close();


    }

   
    public static bool AddSocietyBillPlan(String BillType, String ChargeType, String Rate, String CycleType, int Applyto)
    {
        DataAccess dacess = new DataAccess();
        String BillingQuery = "Insert into dbo.aSocietybillplans(BillType,ChargeType,Rate,CycleType,Applyto) Values('" + BillType + "','" + ChargeType + "','" + Rate + "','" + CycleType + "','" + Applyto + "')";
        bool result = dacess.Update(BillingQuery);

        if (result == true)
        {
            String GetBillIDQuery = "Select BillID  ,Applyto from dbo.aSocietybillplans  where BillType = '" + BillType + "' ";
            DataSet data = dacess.ReadData(GetBillIDQuery);
            DataTable dta = data.Tables[0];

            int BillId = Convert.ToInt32(dta.Rows[0]["BillID"]);
            int ApplyTO = Convert.ToInt32(dta.Rows[0]["Applyto"]);
            AddFlatBillCycle(BillId, ApplyTO);

        }

        return result;
    }

    public DataSet GetLatestBills(String FlatNumber, String BillType,string StartDate, string EndDate)//Added by Aarshi on 13-Sept-2017 for bug fix
    {
        try
        {
            //Added by Aarshi on 13-Sept-2017 for bug fix
            String LatestBillGenQuery = "";

            String FlatCond, BillTypeCond, DateCond = "";

            if (FlatNumber != "")
            {
                FlatCond = "FlatNumber = '" + FlatNumber + "'";
            }

            else
            {
                FlatCond = "FlatNumber is not null";
            }

            if (BillType != "Show All")
            {
                BillTypeCond = "BillType = '" + BillType + "'";
            }
            else
            {
                BillTypeCond = "BillType is not null";
            }

            if (StartDate != string.Empty && EndDate != string.Empty)
            {
                DateCond = "CAST(BillStartDate AS DATE) >= '" + StartDate + "' and CAST(BillEndDate AS DATE) <= '" + EndDate + "'";
            }
            else
            {
                DateCond = "BillStartDate is not null and BillEndDate is not null";
            }

            //Added by Aarshi on 21-Sept-2017 for bug fix
            LatestBillGenQuery = "select * from " + ViewName + " where " + FlatCond + " and " + BillTypeCond + " and " + DateCond;
            //Ends here

           
            //if (FlatNumber != "" && BillType != "Show All")
            //{
            //    LatestBillGenQuery = "select * from " + ViewName + " where FlatNumber ='" + FlatNumber + "' and BillType = '" + BillType + "'";
            //}

            //else if (FlatNumber != "" && BillType == "Show All")
            //{
            //    LatestBillGenQuery = "  select * from " + ViewName + " where FlatNumber ='" + FlatNumber + "'";
            //}

            //else if (FlatNumber == "" && BillType != "Show All")
            //{
            //    LatestBillGenQuery = "  select * from " + ViewName + " where BillType = '" + BillType + "'";
            //}

            ////Added by Aarshi on 11-Sept-2017 for bug fix
            //else if (FlatNumber == "" && BillType == "Show All")
            //{
            //    if (GenerateClickID == "GenerateBill")
            //        LatestBillGenQuery = "  select top 10  * from " + ViewName;

            //    else
            //        LatestBillGenQuery = "  select * from " + ViewName;
            //}
            ////Ends here

            DataAccess dacess = new DataAccess();
            return dacess.GetData(LatestBillGenQuery);
            
        }       

        catch (Exception ex)
        {
            return null;
        }
    }

    public static BillPlan GetSocietyBillPlan(String Query, String BillType)
    {
        DataAccess dacess = new DataAccess();
        BillPlan plan = new BillPlan();
        DataSet ds = dacess.ReadData(Query);
        DataTable dt = ds.Tables[0];
        plan.ChargeType = dt.Rows[0]["CycleType"].ToString();
        plan.Rate = dt.Rows[0]["Rate"].ToString();
        plan.BillID = Convert.ToInt32(dt.Rows[0]["BillID"]);
        plan.ChargeType = dt.Rows[0]["ChargeType"].ToString();
        plan.Applyto = Convert.ToInt32(dt.Rows[0]["Applyto"]);
        return plan;

    }

  

    public static String CalculateBillingDays(DateTime PreviousBillEndDate, DateTime CycleEndDate, String CycleType)
    {
        String GenerateCycle = "";
        if (GenerateCycle == "Manual")
        {

            DateTime newstartdate = PreviousBillEndDate.AddDays(1);
            int day = CycleEndDate.Day;
            DateTime newEndDate = new DateTime(Utility.GetCurrentDateTimeinUTC().Year, Utility.GetCurrentDateTimeinUTC().Month, day);
            int days = Utility.GetDifferenceinDays(newstartdate, newEndDate);


        }
        else if (GenerateCycle == "Auto")
        {
            if (CycleType == "Yearly")
            {
                DateTime newstartdate = PreviousBillEndDate.AddDays(1);
                int day = CycleEndDate.Day;
                DateTime newEndDate = newstartdate.AddYears(1);
                int days = Utility.GetDifferenceinDays(newstartdate, newEndDate);
                DateTime DueDate = newEndDate.AddDays(7);

                if (newEndDate.Year != Utility.GetCurrentDateTimeinUTC().Year && newEndDate.Month != Utility.GetCurrentDateTimeinUTC().Month)
                {
                    days = 0;
                }

            }

            else if (CycleType == "Quaterly")
            {
                DateTime newstartdate = PreviousBillEndDate.AddDays(1);
                int day = CycleEndDate.Day;

                DateTime newEndDate = newstartdate.AddMonths(3);
                int days = Utility.GetDifferenceinDays(newstartdate, newEndDate);
                DateTime DueDate = newEndDate.AddDays(7);

                if (newEndDate.Year != Utility.GetCurrentDateTimeinUTC().Year && newEndDate.Month != Utility.GetCurrentDateTimeinUTC().Month)
                {
                    days = 0;
                }

            }

            else if (CycleType == "Monthly")
            {
                DateTime newstartdate = PreviousBillEndDate.AddDays(1);
                int day = CycleEndDate.Day;
                DateTime newEndDate = new DateTime(Utility.GetCurrentDateTimeinUTC().Year, Utility.GetCurrentDateTimeinUTC().Month, day);
                int days = Utility.GetDifferenceinDays(newstartdate, newEndDate);
                DateTime DueDate = newEndDate.AddDays(7);
           }

        }

       return "";
    }

    public static GenerateBill CalculateNewBill(int FlatArea, String BillType, String ChargeType, String CycleType, Double Rate, int days)
    {
        GenerateBill bill = new GenerateBill();
        Double Amount = 1;


        if (ChargeType == "Fixed")
        {
            Amount = Convert.ToDouble(Rate);

            if (CycleType == "Monthly")
            {
                Amount = Convert.ToDouble(Rate) / 30 * days;
            }
            else if (CycleType == "Yearly")
            {
                Amount = Convert.ToDouble(Rate) / 365 * days;

            }
        }

        if (ChargeType == "Rate")
        {
            if (CycleType == "Monthly")
            {
                Amount = Amount = Convert.ToDouble(Rate) * Convert.ToDouble(FlatArea) / 30 * days;
            }
            else if (CycleType == "Yearly")
            {
                Amount = Amount = Convert.ToDouble(Rate) * Convert.ToDouble(FlatArea) / 365 * days;

            }
        }
        if (ChargeType == "Mannual")
        {
            Amount = 0;
        }


        Amount = Math.Round(Amount, 2);


        bill.ModifiedAt = Utility.GetCurrentDateTimeinUTC();

        String CurrentMonth = String.Format("{0:y}", DateTime.Now);
        DateTime Date = Convert.ToDateTime(CurrentMonth);

        DateTime EarlierMonth = Date.AddMonths(-1);

        string PrevMonth = String.Format("{0:y}", EarlierMonth);


        bill.PreviousMonthBalance = 0;
        bill.ModifiedAt = Utility.GetCurrentDateTimeinUTC();
        bill.PaymentDueDate = Utility.GetCurrentDateTimeinUTC().AddDays(15);
        bill.BillMonth = Utility.GetCurrentDateTimeinUTC().AddDays(-15);
        //bill.CurrentAmount = (int)(Amount + 05d);
        return bill;

    }




    public bool InsertNewBill(GenerateBill newBill)
    {
        try
        {
            DataAccess dacess = new DataAccess();
            String Generatebill = "Insert into " + TableName + "(FlatNumber,BillID,BillStartDate,BillEndDate,CurrentBillAmount,CycleType,PaymentDueDate,BillMonth,PreviousMonthBalance,ModifiedAt,BillDescription) Values('" + newBill.FlatNumber + "','" + newBill.BillID + "','" + newBill.BillStartDate + "','" + newBill.BillEndDate + "','" + newBill.CurrentBillAmount + "','" + newBill.CycleType + "','" + newBill.PaymentDueDate + "','" + newBill.BillMonth + "','" + newBill.PreviousMonthBalance + "','" + newBill.ModifiedAt + "','" + newBill.BillDescription + "')";
            return dacess.Update(Generatebill);
        }
        catch
        {
            return false;
        }
        
    }


    public static int InsertFirstZeroBill(String FlatNumber, int BillID, String CycleType, String CycleStart)
    {
        GenerateBill emptyBill = new GenerateBill();
        emptyBill.FlatNumber = FlatNumber;
        emptyBill.BillID = BillID;
        emptyBill.CurrentBillAmount = 0;
        emptyBill.CycleType = CycleType;
        emptyBill.PaymentDueDate = Convert.ToDateTime(CycleStart).AddDays(7);
        emptyBill.BillMonth = Convert.ToDateTime(CycleStart);
        emptyBill.PreviousMonthBalance = 0;
        emptyBill.ModifiedAt = Utility.GetCurrentDateTimeinUTC();
        emptyBill.BillDescription = "First Empty Bill";
        emptyBill.BillStartDate = Convert.ToDateTime(CycleStart).AddDays(-1);
        emptyBill.BillEndDate = Convert.ToDateTime(CycleStart).AddDays(-1);
        int count = 0;
        DataAccess dacess = new DataAccess();

        String Generatebill = "Insert into dbo.GeneratedBill(FlatNumber,BillID,BillStartDate,BillEndDate,CurrentBillAmount,CycleType,PaymentDueDate,BillMonth,PreviousMonthBalance,ModifiedAt,BillDescription) Values('" + emptyBill.FlatNumber + "','" + emptyBill.BillID + "','" + emptyBill.BillStartDate + "','" + emptyBill.BillEndDate + "','" + emptyBill.CurrentBillAmount + "','" + emptyBill.CycleType + "','" + emptyBill.PaymentDueDate + "','" + emptyBill.BillMonth + "','" + emptyBill.PreviousMonthBalance + "','" + emptyBill.ModifiedAt + "','" + emptyBill.BillDescription + "')";
        bool result = dacess.Update(Generatebill);

        if (result == true)
        {
            count = 1;
        }

        return count;
    }



    public static DataSet GetGenerateBillType()
    {
        try
        {
            DataAccess dacess = new DataAccess();
            String FillBillType = "Select distinct BillType,BillID from SocietyBillplans where ChargeType != 'Manual'";
            return dacess.ReadData(FillBillType);
        }
#pragma warning disable 0168
        catch (Exception ex)
        {

            return null;
        }
#pragma warning restore 0168

    }

    public bool UpdatePayment(GenerateBill Bill)
    {
        try
        {
            DataAccess dacess = new DataAccess();

            String PayManuual = "Update " + TableName + " set AmountPaid =  '" + Bill.AmountPaid + "', AmountPaidDate ='" + Bill.AmountPaidDate + "' , PaymentMode ='" + Bill.PaymentMode + "' ,TransactionID ='" + Bill.TransactionID + "' ,InvoiceID = '" + Bill.InvoiceID + "'  where PayID = '" + Bill.PayID + "'";
            
            return dacess.Update(PayManuual);
        }
#pragma warning disable 0168
        catch (Exception ex)
        {

            return false;
        }
#pragma warning restore 0168


        
    }


    public bool InsertNewBillPay(GenerateBill newBill)
    {
        try
        {
            DataAccess dacess = new DataAccess();
            String Generatebill = "Insert into " + TableName + "(FlatNumber,BillID,BillStartDate,BillEndDate,CurrentBillAmount,CycleType,PaymentDueDate,BillMonth,PreviousMonthBalance,AmountPaidDate,AmountPaid,PaymentMode,TransactionID,InvoiceID,ModifiedAt,BillDescription) Values('" + newBill.FlatNumber + "','" + newBill.BillID + "','" + newBill.BillStartDate + "','" + newBill.BillEndDate + "','" + newBill.CurrentBillAmount + "','" + newBill.CycleType + "','" + newBill.PaymentDueDate + "','" + newBill.BillMonth + "','" + newBill.PreviousMonthBalance + "','" + newBill.AmountPaidDate + "','" + newBill.AmountPaid+ "','" + newBill.PaymentMode+ "','" + newBill.TransactionID + "','" + newBill.InvoiceID + "','" + newBill.ModifiedAt + "','" + newBill.BillDescription + "')";
            return dacess.Update(Generatebill);
        }
        catch
        {
            return false;
        }

    }

    private void CalculateBalance()
    {

    }

    private void GetBillForFlat(String FlatID)
    {

    }



    public static GenerateBill CalculateNewBill(GenerateBill previousBill, String GenerateCycle, DateTime BillingDate)
    {
        GenerateBill newBill = new GenerateBill();
        int days = 0;
        double BillAmount=0;

        newBill.BillStartDate = previousBill.BillEndDate.AddDays(1);


        if (GenerateCycle == "Manual")
        {
            newBill.BillDescription = "Manually Generated";
            if (BillingDate != null)
            {
                newBill.BillEndDate = BillingDate;
                days = Utility.GetDifferenceinDays(newBill.BillStartDate, newBill.BillEndDate);
            }

        }

        else if (GenerateCycle == "Auto")
        {
            newBill.BillDescription = "Auto Generated";
            if (previousBill.CycleType == "Yearly")
            {
                newBill.BillEndDate = newBill.BillStartDate.AddYears(1);


            }

            else if (previousBill.CycleType == "Quaterly")
            {
                newBill.BillEndDate = newBill.BillStartDate.AddMonths(3);

            }

            else if (previousBill.CycleType == "Monthly")
            {
                newBill.BillEndDate = newBill.BillStartDate.AddMonths(1);

            }


            //Why we have this logic, it gives 0 days if I could not create Bill in December and doing that in Jan next year. Amit 8/02/2018
            // Hence commenting this as of now.
            //if (newBill.BillEndDate.Year != Utility.GetCurrentDateTimeinUTC().Year || newBill.BillEndDate.Month != Utility.GetCurrentDateTimeinUTC().Month)
            //{
            //    days = 0;
            //}
            //else
            //{
            //    days = Utility.GetDifferenceinDays(newBill.BillStartDate, newBill.BillEndDate);
            //}

            days = Utility.GetDifferenceinDays(newBill.BillStartDate, newBill.BillEndDate);
        }
        // End Date should not be greater than Cycle End Date code to be added



        if (days > 0)
        {

            newBill.PaymentDueDate = newBill.BillEndDate.AddDays(7);

            if (previousBill.ChargeType == "Fixed")
            {

                if (previousBill.CycleType == "Monthly")
                {
                    BillAmount = Convert.ToDouble(previousBill.Rate) / 30 * days + 0.5d;
                }
                else if (previousBill.CycleType == "Quaterly")
                {
                    BillAmount = Convert.ToDouble(previousBill.Rate) / (30 * 3) * days;
                }
                else if (previousBill.CycleType == "Yearly")
                {
                    BillAmount = Convert.ToDouble(previousBill.Rate) / 365 * days;

                }
            }

            if (previousBill.ChargeType == "Rate")
            {
                if (previousBill.CycleType == "Monthly")
                {
                    BillAmount = Convert.ToDouble(previousBill.Rate) * Convert.ToDouble(previousBill.FlatArea) / 30 * days;
                }

                else if (previousBill.CycleType == "Quaterly")
                {
                    BillAmount = Convert.ToDouble(previousBill.Rate) * Convert.ToDouble(previousBill.Rate) / (30 * 3) * days;
                }

                else if (previousBill.CycleType == "Yearly")
                {
                    BillAmount = Convert.ToDouble(previousBill.Rate) * Convert.ToDouble(previousBill.FlatArea) / 365 * days;

                }
            }
            if (previousBill.ChargeType == "Manual")
            {
                BillAmount = 0;
            }

        }

        newBill.Days = days;
        newBill.BillID = previousBill.BillID;
        newBill.FlatArea = previousBill.FlatArea;
        newBill.ChargeType = previousBill.ChargeType;
        newBill.CurrentBillAmount = (int)(BillAmount + 0.5d);
        newBill.ModifiedAt = Utility.GetCurrentDateTimeinUTC();
        newBill.PreviousMonthBalance = previousBill.CurrentMonthBalance;
        newBill.FlatNumber = previousBill.FlatNumber;
        newBill.CycleType = previousBill.CycleType;
        newBill.BillMonth = newBill.BillEndDate;
        newBill.Rate = previousBill.Rate;
        return newBill;
    }


    public static GenerateBill GetLastGeneratedBill(String FlatNumber, String BillType)
    {
        try
        {
            DataAccess dacess = new DataAccess();
            GenerateBill LastBill = new GenerateBill();

            String FlatsForBillType = "Select * from "+ ViewName +" where BillTYpe = '" + BillType + "' and FlatNumber = '" + FlatNumber + "'";

            DataSet FlatsData = dacess.ReadData(FlatsForBillType);

            if (FlatsData != null)
            {
                DataTable dataBills = FlatsData.Tables[0];

                for (int i = 0; i < dataBills.Rows.Count; i++)
                {
                    GenerateBill previousBill = new GenerateBill();
                    LastBill.FlatNumber = dataBills.Rows[i]["FlatNumber"].ToString();
                    LastBill.FlatArea = Convert.ToInt32(dataBills.Rows[i]["FlatArea"]);
                    LastBill.ChargeType = dataBills.Rows[i]["ChargeType"].ToString();
                    LastBill.CycleType = dataBills.Rows[i]["CycleType"].ToString();
                    LastBill.Rate = Convert.ToDouble(dataBills.Rows[i]["Rate"]);
                    LastBill.BillStartDate = Convert.ToDateTime(dataBills.Rows[i]["BillStartDate"]);
                    LastBill.BillEndDate = Convert.ToDateTime(dataBills.Rows[i]["BillEndDate"]);
                    LastBill.BillID = Convert.ToInt32(dataBills.Rows[i]["BillID"]);
                    LastBill.CurrentMonthBalance = Convert.ToInt32(dataBills.Rows[i]["CurrentMonthBalance"]);
                    // LastBill.CycleEndDate = Convert.ToDateTime(dataBills.Rows[i]["CycleEnD"]);
                }

            }

            return LastBill;
        }
#pragma warning disable 0168
        catch (Exception ex)
        {

            return null;
        }
#pragma warning restore 0168


    }

    public static GenerateBill GetLastGeneratedBill(String FlatNumber, int BillID)
    {
        try
        {
            DataAccess dacess = new DataAccess();
            GenerateBill LastBill = new GenerateBill();

            String FlatsForBillType = "Select * from "+ ViewName +" where BillID = '" + BillID + "' and FlatNumber = '" + FlatNumber + "'";

            DataSet FlatsData = dacess.ReadData(FlatsForBillType);

            if (FlatsData != null)
            {
                DataTable dataBills = FlatsData.Tables[0];

                for (int i = 0; i < dataBills.Rows.Count; i++)
                {
                    GenerateBill previousBill = new GenerateBill();
                    LastBill.FlatNumber = dataBills.Rows[i]["FlatNumber"].ToString();
                    LastBill.FlatArea = Convert.ToInt32(dataBills.Rows[i]["FlatArea"]);
                    LastBill.ChargeType = dataBills.Rows[i]["ChargeType"].ToString();
                    LastBill.CycleType = dataBills.Rows[i]["CycleType"].ToString();
                    LastBill.Rate = Convert.ToDouble(dataBills.Rows[i]["Rate"]);
                    LastBill.BillStartDate = Convert.ToDateTime(dataBills.Rows[i]["BillStartDate"]);
                    LastBill.BillEndDate = Convert.ToDateTime(dataBills.Rows[i]["BillEndDate"]);
                    LastBill.BillID = Convert.ToInt32(dataBills.Rows[i]["BillID"]);
                    LastBill.CurrentMonthBalance = Convert.ToInt32(dataBills.Rows[i]["CurrentMonthBalance"]);
                    // LastBill.CycleEndDate = Convert.ToDateTime(dataBills.Rows[i]["CycleEnD"]);
                }

            }

            return LastBill;
        }
       
#pragma warning disable 0168
        catch (Exception ex)
        {

            return null;
        }
#pragma warning restore 0168
    }

    public List<string> GetLatestFlatNumber(string FlatNumber)
    {
        List<string> Emp = new List<string>();
        string query = string.Format("select distinct FlatNumber from " + ViewName + " where FlatNumber like '" + FlatNumber + "%' order by Flatnumber asc");
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
}
