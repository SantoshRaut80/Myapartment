using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GenerateBill
/// </summary>
public class GenerateBill
{
    public int PayID;
    public String FlatNumber;
    public int BillID;
    public int CurrentBillAmount;
    public String CycleType;
    public DateTime PaymentDueDate;
    public DateTime BillMonth;
    public String BillType;
    public int AmountTobePaid;
    public int PreviousMonthBalance;
    public DateTime AmountPaidDate;
    public string SAmountPaidDate;
    public int AmountPaid;
    public String PaymentMode;
    public String TransactionID;
    public String InvoiceID;
    public int CurrentMonthBalance;
    public DateTime ModifiedAt;
    public String BillDescription;
    public DateTime BillStartDate;
    public DateTime BillEndDate;


    public DateTime CycleStartDate;
    public DateTime CycleEndDate;
    public String ChargeType;
    public Double Rate;
    public int FlatArea;
    public int Days;


    public GenerateBill()
    {
        //
        // TODO: Add constructor logic here
        //
    }



    public void AddBill(GenerateBill previousBill, String GenerateCycle)
    {
       // String GenerateCycle = "";
        DateTime newstartdate = previousBill.BillEndDate.AddDays(1);
        int day = previousBill.CycleEndDate.Day;

        if (GenerateCycle == "Manual")
        {
            this.BillEndDate = new DateTime(Utility.GetCurrentDateTimeinUTC().Year, Utility.GetCurrentDateTimeinUTC().Month, day);

        }

        else if (GenerateCycle == "Auto")
        {
            if (CycleType == "Yearly")
            {
                this.BillEndDate = this.BillStartDate.AddYears(1);
            }

            else if (CycleType == "Quaterly")
            {
                this.BillEndDate = this.BillStartDate.AddMonths(3);

            }

            else if (CycleType == "Monthly")
            {
                DateTime newEndDate = this.BillStartDate.AddMonths(1);

            }

        }

        int days = Utility.GetDifferenceinDays(BillStartDate, BillEndDate);
        DateTime DueDate = BillEndDate.AddDays(7);


        if (BillEndDate.Year != Utility.GetCurrentDateTimeinUTC().Year && BillEndDate.Month != Utility.GetCurrentDateTimeinUTC().Month)
        {
            days = 0;
        }


        if (days != 0)
        {

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


            ModifiedAt = Utility.GetCurrentDateTimeinUTC();

            PreviousMonthBalance = previousBill.CurrentMonthBalance;

            // insert Query
        }
        else
        {

        }
    }
}