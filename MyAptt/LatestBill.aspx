<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LatestBill.aspx.cs" Inherits="LatestBill" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <meta name="viewport" content="width=device-width, initial-scale=1"/>

       <script src="Scripts/jquery-1.11.1.min.js"></script>
             <!-- Latest compiled and minified CSS -->
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"/>

            <!-- jQuery library -->
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

            <!-- Latest compiled JavaScript -->
            <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
   

         <link rel="stylesheet" href="http://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css"/>  

        <script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.8.0.js"></script>  
        <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.22/jquery-ui.js"></script>  
        <link rel="Stylesheet" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.10/themes/redmond/jquery-ui.css" /> 

     
      
              <link rel="stylesheet" href="CSS/ApttLayout.css"/>
              <link rel="stylesheet" href="CSS/ApttTheme.css" />

    <script>

        $(function () {
            $("#txtLatestFlatFilter").autocomplete({
                source: function (request, response) {
                    var param = {
                        FlatNumber: $('#txtLatestFlatFilter').val()
                    };

                    $.ajax({
                        url: "LatestBill.aspx/GetLatestFlatNumber",
                        data: JSON.stringify(param),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {

                            response($.map(data.d, function (item) {
                                return {
                                    value: item
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {


                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                            alert(err.Message)
                            console.log("Ajax Error!");
                        }
                    });
                },
                minLength: 2 //This is the Char length of inputTextBox  
            });
        });
        
        $(document).ready(function () {

            $("#btnImport").click(function () {
                document.getElementById("importModal").style.display = "block";
                //$("#importModal").show();

            });


            $("#btnClose").click(function () {
                document.getElementById("importModal").style.display = "none";
                //$("#importModal").show();

            });

            $("#btnPaycancel").click(function () {
                $("#PaymentForm").hide();
            });

            $(document).click(function () {
                $("#current_dropdown").hide();

            });

            $("#txtBillDate").datepicker();

            $("#BillDescpCancel").click(function () {
                $("#generatedbilldescriptnmodal").hide();
                $("#lblBillGeneraStatus").html('');
                $("#HiddenBillDescp").val("");
                $("input:text").val("");
            });

                        
        });

        function CloseBillImport()
        {
            $("#importModal").hide();
           
        }

        //Added by Aarshi on 26 Sept 2017
        $(document).ready(function () {

            $("#btnPayNow").click(function () {
                alert(1);
                var text = document.getElementById("HiddenBillData").value;
               // FlatNumber + "&" + BillStartDate + "&" + BillEndDate + "&" + CurrentBillAmount + "&" + CycleType + "&" + PreviousMonthBalance + "&" + BillID + "&" + PayID + "&" + CurrentMonthBalance;
                var res = text.split("&");
                flatNumber = res[0];
                alert(flatNumber);

                $("#lblFlat").html(res[0]);
                $("#lblBillTypee").html("");
                $("#lblDueDate").html(res[1]);
                $("#lblAmtPaid").html(res[3]);
                $("#lblAmountPaid").html(res[3]);
                $("#lblBalance").html(res[8]);
                $("#lblTotalPay").html("");
                $("#lblDueFor").html(res[2]);
                $("#lblBillGenDate").html(res[2]);
                $("#HiddenPayMannualID").val(res[7]);
                alert(res[7]);
              

                $("#newPaymentForm").show();


            });


            $("#btnBillPay").click(function () {
                //alert(1);
               // ShowPaymentForm();
            });

            $("#btnBillGencancel").click(function () {
                $("#GenerateDeActivateBillForm").hide();

            });
        });

        function ShowPaymentForm()
        {
            alert(flatNumber);
            $("#lblFlat").html(flatNumber);
            $("#lblBillTypee").html(billType);
            $("#lblDueDate").html(dueDate);
            $("#lblAmtPaid").html(amount);
            $("#lblAmountPaid").html(amountPaid);
            $("#lblBalance").html(amountBalance);
            $("#lblTotalPay").html(totalPayable);
            $("#lblDueFor").html(dueForDate);
            $("#lblBillGenDate").html(billGeneratedDate);
            $("#HiddenPayMannualID").val(flatNumber);
          


            if (amountPaid == totalPayable) {
                $('#<%=txtAmt.ClientID %>').val("0");

            }
            else {
                $('#<%=txtAmt.ClientID %>').val(currentMonthBalance);

            }

         
            $("#PaymentForm").show();

        }

        function LatestBillPopup(PayID, BillID, FlatNumber, BillType, DueDate, Amount, Balance, TotalPay,
                                    DueFor, BillGendate, AmountPaidDate, BillStartDate, BillEndDate, CurrentBillAmount, CycleType,
                                    PaymentDueDate, BillMonth, PreviousMonthBalance, ModifiedAt, AmountPaid, CurrentMonthBalance, theElement) {
            
                                    document.getElementById("hiddenSelectedPayID").value = PayID;
                                    document.getElementById("HiddenFlatNumberHistory").value = FlatNumber;

                                    document.getElementById("HiddenBillTypeHistory").value = BillType;

                                    //Added by Aarshi on 14-Sept-2017 for bug fix
                                    document.getElementById("HiddenGeneratedBillData").value = FlatNumber + "&" + BillStartDate + "&" + BillEndDate + "&" + CurrentBillAmount + "&" + CycleType + "&" + PreviousMonthBalance + "&" + BillID + "&" + PayID + "&" + CurrentMonthBalance;

                                    document.getElementById("HiddenAmountPaidDate").value = AmountPaidDate;
                                    var status = theElement.parentNode.parentNode.cells[7].innerHTML;

                                    //Added by Aarshi on 26 Sept 2017
                                    flatNumber = FlatNumber;
                                    billType = BillType;
                                    dueDate = DueDate;
                                    amount = Amount;
                                    amountBalance = Balance;
                                    totalPayable = TotalPay;
                                    dueForDate = DueFor;
                                    billGeneratedDate = BillGendate;
                                    payID = PayID;
                                    amountPaidDate = AmountPaidDate;
                                    amountPaid = AmountPaid;
                                    currentMonthBalance = CurrentMonthBalance;
                                    //Ends here

                                    var Posx = 0;
                                    var Posy = 0;

                                    while (theElement != null) {
                                        Posx += theElement.offsetLeft;
                                        Posy += theElement.offsetTop;
                                        theElement = theElement.offsetParent;

                                    }

                                    document.getElementById("current_dropdown").style.top = Posy + 'px';
                                    document.getElementById("current_dropdown").style.left = Posx + 'px';

                                    $("#current_dropdown").slideDown();
                                    event.stopPropagation();
        }

        function ShowGenerateDeActivateBillForm()
        {
            document.getElementById("GenerateDeActivateBillForm").style.display = "block";
        }

        function ShowGenerateBillPreview()
        {
            document.getElementById("generatedbilldescriptnmodal").style.display = "block";
        }

     

    </script>
</head>
<body>
      <div class="container-fluid">

          <div class="row">
        <div class="col-xs-12 col-sm-10">
               <form id="form1" runat="server">
                   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                   <asp:HiddenField ID="hiddenSelectedPayID" runat="server" />
                   
                    <asp:MultiView ID="MultiView1" runat="server">
              <asp:View ID="Latest_Bill_View" runat="server">
                       <div class="container-fluid">
                         <div class="row layout_header theme_third_bg font_size_2 vcenter" style="height:40px;">
                                     <div class="col-sm-3 hidden-xs" >
                                         <div><label class="pull-left ">Latest Bills :</label></div>
                                     </div>
                                    <div class="col-sm-6 col-xs-12" style="padding:0px;">
                                        <div class="layout_filter_box" style="width:270px;">
                                                <asp:TextBox ID="txtLatestFlatFilter" placeholder="Flat Number" runat="server" CssClass="layout_txtbox_filter"></asp:TextBox>
                                              <asp:DropDownList ID="drpCurrentBillType" runat="server" CssClass="layout_ddl_filter"></asp:DropDownList> 
                                              <asp:LinkButton runat="server" BackColor="Transparent" ForeColor="Black" OnClick="searchLatestGenBill_Click" ValidationGroup="Flat_Search"> <span class="glyphicon glyphicon-search"></span></asp:LinkButton>
                                         </div>
                                      </div>
                                     <div class="col-sm-3 hidden-xs" style="vertical-align:middle;">
                                        <div>
                                            <button type="button" id="btnImport" class="theme-btn-primary">Generate Bill</button>  
                                        </div>
                                     </div>
                                 </div>
                       </div>

                       
                         <table  style="width:99%; margin-top:1%;border:1px solid #f2f2f2;min-height:450px;">
                      <tr>
                          <td>
                              <asp:CheckBox ID="chckLatestBills" runat="server" Text="Show Plan Details"  AutoPostBack="true" OnCheckedChanged="chckLatestBills_CheckedChanged"/>
                  <asp:Label ID="lblLatestBillGrid" runat="server" ForeColor="Red"></asp:Label>
                          </td>
                      </tr>
                       
                      <tr>                                                                                                                  
                          <td style="text-align:center;width:100%;">      
                                                      
                              <asp:GridView ID="GridlatestBills" runat="server"
                               HeaderStyle-BackColor="#6eab91" 
                             HeaderStyle-BorderStyle="None"  
                               style="margin-bottom: 0px;" BackColor="#E8E8E8" AutoGenerateColumns="false" AllowPaging="True"                                  
                               HorizontalAlign="Center" PageSize="15" EmptyDataText="No Records Found" ShowHeaderWhenEmpty="True" 
                                  BorderColor="Silver"                                 
                               BorderStyle="Solid" BorderWidth="1px" Font-Names="Calibri" ForeColor="#666666" Width="90%" 
                                  OnPageIndexChanging="GridlatestBills_PageIndexChanging" OnRowDataBound="GridlatestBills_RowDataBound">                                          
                        <AlternatingRowStyle BackColor="#ffffff" />
                       <Columns>              
                       <asp:TemplateField HeaderStyle-Width="50px">
                      <ItemTemplate>                                           
                          <button id ="btnLatestPopup" onclick ="LatestBillPopup('<%# Eval("PayID") %>','<%# Eval("BillID") %>','<%# Eval("FlatNumber") %>','<%# Eval("BillType") %>','<%# Eval("PaymentDueDate") %>','<%# Eval("CurrentBillAmount") %>','<%# Eval("PreviousMonthBalance") %>','<%# Eval("AmountTobePaid") %>','<%# Eval("BillMonth") %>','<%# Eval("BillDate") %>','<%# Eval("AmountPaidDate") %>','<%# Eval("BillStartDate") %>','<%# Eval("BillEndDate") %>','<%# Eval("CurrentBillAmount") %>','<%# Eval("CycleType") %>','<%# Eval("PaymentDueDate") %>','<%# Eval("BillMonth") %>','<%# Eval("PreviousMonthBalance") %>','<%# Eval("ModifiedAt") %>','<%# Eval("AmountPaid") %>','<%# Eval("CurrentMonthBalance") %>',this)"  
                             type="button" style=" height: 20px; border:0;outline:0; background-color:transparent"> >></button>
                          
                      </ItemTemplate>
                     </asp:TemplateField>
                           <asp:BoundField DataField ="PayID" HeaderText ="PayID" HeaderStyle-Width ="0px" Visible ="false" />
                            <asp:BoundField DataField ="BillID" HeaderText ="BillID" HeaderStyle-Width ="0px" visible="false"/>
                             <asp:BoundField DataField="FlatNumber" HeaderText="Flat" HeaderStyle-Width="50px" /> 
                            <asp:BoundField DataField="BillType" HeaderText="BillType"  ItemStyle-CssClass="BillActiveGridLeftAlign" HeaderStyle-Width="60px" /> 
                           <asp:BoundField DataField="Rate" HeaderText="Rate" ItemStyle-Width="40px" HeaderStyle-Width="40px" ItemStyle-CssClass="BillActiveGrid"  Visible="false">                        
                             <HeaderStyle Width="40px" />
                              <ItemStyle Width="40px"></ItemStyle>
                             </asp:BoundField>                          
                             <asp:BoundField DataField="FlatArea" HeaderText="FlatArea"  Visible="false"/>  
                           <asp:BoundField DataField="CurrentBillAmount" HeaderText="CurrentBill" />  
                                <asp:BoundField DataField="CycleType" HeaderText="CycleType" Visible="false"/>  
                             
                            <asp:BoundField DataField="BillStartDate" HeaderText="From:" DataFormatString="{0:dd/MMM/yy}" ItemStyle-Font-Size="Small"  ItemStyle-Width="120px"/>  
                             <asp:BoundField DataField="BillEndDate" HeaderText="To:" DataFormatString="{0:dd/MMM/yy}" ItemStyle-Font-Size="Small"  ItemStyle-Width="120px"/>  
                           
                          <%--  <asp:BoundField DataField="BillMonth" HeaderText="BillMonth" ItemStyle-Font-Size="Small" ItemStyle-Width="100px" DataFormatString="{0:dd/MMM/yyyy}"/>  --%>
                            <asp:BoundField DataField="BillDate" HeaderText="BillDate" ItemStyle-Font-Size="Small" ItemStyle-Width="120px" DataFormatString="{0:dd/MMM/yy}"/>  
                             <asp:BoundField DataField="PreviousMonthBalance" HeaderText="Previous Balance" Visible="false"  ItemStyle-CssClass="BillActiveGrid"  HeaderStyle-Width="80px"/>    
                            <asp:BoundField DataField="CurrentBillAmount" HeaderText="Bill Amount" Visible="false"  ItemStyle-CssClass="BillActiveGrid"  HeaderStyle-Width="80px"/> 
                            <asp:BoundField DataField="AmountTobePaid" HeaderText="Total Payble"   ItemStyle-CssClass="BillActiveGrid" HeaderStyle-Width="80px"/>  
                            <asp:BoundField DataField="PaymentDueDate" HeaderText="Due Date" DataFormatString="{0:dd/MMM/yy}" ItemStyle-Font-Size="Small"  HeaderStyle-Width="120px">  
                            <ItemStyle Font-Size="Small" />
                            </asp:BoundField>
                                                         
                             <asp:BoundField DataField="AmountPaid" HeaderText="AmountPaid" HeaderStyle-Width="50px" />                          
                             <asp:BoundField DataField="AmountPaidDate" HeaderText="Paid On"  ItemStyle-Width="120px" DataFormatString="{0:dd/MMM/yy}" ItemStyle-Font-Size="Small"/>                            
                             <asp:BoundField DataField="CurrentMonthBalance" HeaderText="Balance" HeaderStyle-Width="50px" /> 
                             
                             <asp:BoundField DataField="PaymentMode" HeaderText="Pay Mode" ItemStyle-Width="30px"  Visible="false"/>  
                             <asp:BoundField DataField="TransactionID" HeaderText="Trans.ID"   ItemStyle-CssClass="BillActiveGrid"  HeaderStyle-Width="50px" Visible="false"/>  
                             <asp:BoundField DataField="InvoiceID" HeaderText="InvoiceID"   ItemStyle-CssClass="BillActiveGrid"  HeaderStyle-Width="50px" Visible="false"/>  
                           <%--   <asp:BoundField DataField="CycleEnD" HeaderText="CycleEnD"  DataFormatString="{0:dd/MMM/yy}"    HeaderStyle-Width="90px"/>   --%>
                             <asp:BoundField DataField="BillDescription" HeaderText="Comment"   ItemStyle-CssClass="BillActiveGrid" HeaderStyle-Width="100px"/>  
                         
                       <asp:TemplateField Visible="false">
                            <ItemTemplate>        
                            <asp:HiddenField  ID="hdnModifiedAt" Value='<%# Eval("ModifiedAt") %>' runat="server"></asp:HiddenField>  
                                                    
                            </ItemTemplate>
                        </asp:TemplateField>
                       
                       
                       
                       </Columns>
                          <EmptyDataRowStyle BackColor="#EEEEEE" />
                          
                        <PagerSettings Mode="NumericFirstLast" />
                        <PagerStyle BackColor="White" BorderColor="#F0F5F5" Font-Bold="False" HorizontalAlign="Center" ForeColor="#62BCFF" Font-Names="Berlin Sans FB" Font-Size="Medium" />
                       </asp:GridView>&nbsp;
                             
                              <%--Added by Aarshi on 27-Sept-2017 for bug fix--%>
                          <asp:HiddenField ID="HiddenFlatNumberHistory" runat="server" />
                          <asp:HiddenField ID="HiddenBillTypeHistory" runat="server" />
                              <asp:HiddenField ID="HiddenGeneratedBillData" runat="server" />
			                <asp:HiddenField ID="HiddenAmountPaidDate" runat="server" />
                      
                           <div id="current_dropdown" class="layout-dropdown-content theme-dropdown-content" style="width:8%;margin-left:0.5%;">           
                               <asp:Button ID="btnBillHistory" runat="server"  Text="Details" CssClass="layout_dropdown_Button" CausesValidation="false" OnClick="btnBillHistory_Click"   />
                               <%--Added by Aarshi on 26 Sept 2017--%>
                               <asp:Button ID="btnBillPay" runat="server" Text="Pay"  CssClass="layout_dropdown_Button" CausesValidation="false" OnClick="btnBillPay_Click" />
                               <%--Added by Aarshi on 27-Sept-2017--%>
                               <%--<button type="button" id="btnBillPay" class="layout_dropdown_Button">Pay</button> <br/>--%>
                               <asp:Button ID="btnLatestGenBill" runat="server" CssClass="layout_dropdown_Button" Text="Generate" CausesValidation="false"  OnClick="btnLatestGenBill_Click" />
                          </div>                                                                 
                             </td>
                      </tr>
                     <tr>
                          <td style="text-align:center;">
                              &nbsp;</td>
                      </tr>
                      <tr>
                          <td style="text-align:center;">
                              <asp:Label ID="Label27" runat="server" Text="Total Count:" ForeColor="#0099FF"></asp:Label>
                              <span style="margin-left:1%;"></span>
                              <asp:Label ID="lblLatestBillCount" runat="server" ForeColor="#0099FF"></asp:Label>
                          </td>
                      </tr>
                      <tr>
                          <td style="height:10px;"></td>
                      </tr>
                      </table>
                
         <%--------------------------------------------------------- Pay the  Amount Section starts from here  ---------------------------------------------------------------------------------- --%>
                         

                  </asp:View>

              <asp:View ID="Bill_Detail_View" runat="server">

                <div class="container-fluid">
                       <div class="row layout_header theme_third_bg font_size_2 vcenter" style="height:40px;">
                                 <div class="col-sm-2 hidden-xs" >
                                     <div><label class="pull-left ">Bill Details : </label></div>
                                 </div>
                                <div class="col-sm-8 col-xs-9" style="padding:0px;">
                                    <div class="layout_filter_box" style="width:500px;">
                                        <asp:TextBox ID="txtGenBillsFlatfilter" runat="server" CssClass="layout_txtbox_filter"></asp:TextBox>
                                        <asp:DropDownList ID="drpGeneratedBillType" runat="server" CssClass="layout_ddl_filter"></asp:DropDownList> <span style="margin-left:10px" ></span> 
                                        <asp:TextBox ID="txtStartDate" runat="server" Width="100px" CssClass="layout_txtbox_filter"></asp:TextBox>                                      
                                        <asp:Label ID="Label36" runat="server" ForeColor="#999999" Text="To"></asp:Label>                                                                   
                                        <asp:TextBox ID="txtEndDate" runat="server" Width="100px" CssClass="layout_txtbox_filter"></asp:TextBox>                                
                                        <asp:LinkButton runat="server" BackColor="Transparent" ForeColor="Black" CausesValidation="false" OnClick="searchGenerateBill_Click"> <span class="glyphicon glyphicon-search"></span></asp:LinkButton> 
                                    </div>           
                                 </div>
                                 <div class="col-sm-2" style="vertical-align:middle;">
                                    
                                        <asp:LinkButton runat="server" BackColor="Transparent" ForeColor="White" CausesValidation="false" OnClick="BillBack_Click"><span class="glyphicon glyphicon-backward"></span></asp:LinkButton> 
                                  
                                 </div>
                             </div>
                </div>
                
                 <table  style="width:99%; margin-top:1%;border:1px solid #f2f2f2;">   
                     
            <tr>
                <td colspan="1" style="width:15%; "> 
                     <asp:HiddenField ID="HiddenBillData" runat="server" />
                    
                </td>
                <td colspan="1" style="width:70%; text-align:left;"> 
                    <asp:CheckBox ID="ChckBillsGenerated" runat="server"  Text="Show Details" AutoPostBack="true" OnCheckedChanged="ChckBillsGenerated_CheckedChanged"/>
                </td>
                <td colspan="1" style="width:15%;text-align:center;"> 
                   
                </td>
            </tr>
            
            <tr>
                <td colspan="3" style="text-align:right;">
                    <asp:Label ID="lblPaymentCheck" runat="server"></asp:Label>
                </td>
          
            </tr>

             <tr>
                        
                    <td colspan="3" style="text-align:center;">
                        <asp:GridView ID="GeneratedBillsGrid" runat="server" AllowPaging="True" AutoGenerateColumns="false" BackColor="#E8E8E8" BorderColor="Silver" BorderStyle="Solid" 
                            BorderWidth="1px" EmptyDataText="No Records Found" Font-Names="Calibri" ForeColor="#666666" HorizontalAlign="Center" 
                            OnPageIndexChanging="GeneratedBillsGrid_PageIndexChanging" OnRowDataBound="GeneratedBillsGrid_RowDataBound" PageSize="15" ShowHeaderWhenEmpty="True" 
                            style="margin-bottom: 0px" Width="80%">
                            <AlternatingRowStyle BackColor="#ffffff" />
                            <Columns>
                                <%--<asp:BoundField DataField="FlatNumber" HeaderText="Flat" />--%>
                                <asp:TemplateField HeaderStyle-Width="50px">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblFlat" runat="server" Text="Flats"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkFlatNumber" runat="server" CssClass="BillActiveGrid"  Font-Bold="true" Font-Underline="true" ForeColor="#0066cc" Text='<%# Bind("FlatNumber") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BillType" HeaderStyle-Width="100px" HeaderText="BillType" ItemStyle-CssClass="BillActiveGridLeftAlign" />
                                <asp:BoundField DataField="Rate" HeaderStyle-Width="40px" HeaderText="Rate" ItemStyle-CssClass="BillActiveGrid" ItemStyle-Width="40px">
                                <HeaderStyle Width="40px" />
                                <ItemStyle Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FlatArea" HeaderText="FlatArea" />
                                <%--  <asp:BoundField DataField="BillStartDate" DataFormatString="{0:dd/MMM/yy}" HeaderText="From:" />
                                <asp:BoundField DataField="BillEndDate" DataFormatString="{0:dd/MMM/yy}" HeaderText="To:" />--%>
                                <asp:BoundField DataField="CurrentBillAmount" HeaderText="CurrentBillAmount" />
                                 <asp:BoundField DataField="ModifiedAt" DataFormatString="{0:dd/MMM/yy}" HeaderText="Bill Date" />
                                <asp:BoundField DataField="CycleType" HeaderText="CycleType" />
                                <asp:BoundField DataField="PaymentDueDate" DataFormatString="{0:dd/MMM/yyyy}" HeaderText="PaymentDueDate" ItemStyle-Font-Size="Small">
                                <ItemStyle Font-Size="Small" />
                                </asp:BoundField>
                               <%-- <asp:BoundField DataField="BillMonth" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Bill Gen Month" ItemStyle-Font-Size="Small" ItemStyle-Width="120px" />--%>
                                <asp:BoundField DataField="PreviousMonthBalance" HeaderStyle-Width="70px" HeaderText="PreviousMonthBalance" ItemStyle-CssClass="BillActiveGrid" />
                                <asp:BoundField DataField="CurrentBillAmount" HeaderStyle-Width="70px" HeaderText="Current Amount" ItemStyle-CssClass="BillActiveGrid" />
                                <asp:BoundField DataField="AmountTobePaid" HeaderStyle-Width="70px" HeaderText="AmountTobePaid" ItemStyle-CssClass="BillActiveGrid" />
                                <asp:BoundField DataField="AmountPaidDate" DataFormatString="{0:dd/MMM/yyyy}" HeaderStyle-Width="80px" HeaderText="AmountPaidDate" ItemStyle-Font-Size="Small" />
                                <asp:BoundField DataField="AmountPaid" HeaderStyle-Width="70px" HeaderText="AmountPaid" />
                                
                                <asp:BoundField DataField="BillDescription" HeaderText="BillDescription" ItemStyle-CssClass="BillActiveGrid">
                                <ItemStyle Width="300px" />
                                <HeaderStyle Width="300px" />
                                </asp:BoundField>
                           
                            </Columns>
                            <EmptyDataRowStyle BackColor="#EEEEEE" />
                            <HeaderStyle BackColor="#0065A8" Font-Bold="false" Font-Names="Modern" Font-Size="Small" ForeColor="White" Height="30px" />
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle BackColor="White" BorderColor="#F0F5F5" Font-Bold="False" Font-Names="Berlin Sans FB" Font-Size="Medium" ForeColor="#62BCFF" HorizontalAlign="Center" />
                        </asp:GridView>
                    </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align:center;">
                         <asp:Label ID="lblTotal" runat="server" ForeColor="#2facff" Text="Total Bills :"></asp:Label>    
                            <asp:Label ID="lblTotalBillscount" runat="server" ForeColor="#35aeff" Text=""></asp:Label><br />
                             <asp:Label ID="lblGeneratebill" runat="server" Font-Size="Small" ForeColor="#FF6262" Visible="False"></asp:Label>
                        </td>
                         </tr>
                    <tr>
                        <td colspan="1" style="text-align:center; width:15%;">
                             <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="theme-btn-primary" CausesValidation="false" OnClick="ExportToExcel" />

                        </td>
                                <td colspan="1" style="text-align:center; width:30%;">
                            

                        </td>
                                <td colspan="1" style="text-align:center; width:15%;">
                                  <!--  <button type="button" id="btnPayNow" class="theme-btn-primary">Pay</button>-->
                                     <asp:LinkButton ID="linkPay" runat="server" OnClick="PayLink_Click" CausesValidation="false" CssClass="theme-btn-primary"> Pay Now</asp:LinkButton>
                            <!-- <asp:Button ID="Button2" runat="server" Text="PayNow" CssClass="theme-btn-primary" CausesValidation="false" OnClick="PayNow_Click" />-->

                        </td>
                    </tr>
                </table>

                  <div id="newPaymentForm" class="layout_modal_Window">
                      <div class="container-fluid">
                          <div class="row layout_header theme_primary_bg" style="height:30px;">
                              <div class="col-xs-12">
                                    PayBill:
                                  </div>
                        </div>
                      </div>

                  </div>
               
            </asp:View>

              <asp:View ID="PaymentView" runat="server">

                               <table style="width:70%;margin-left:15%;">
                                   <tr>
                                       <td colspan="2" style="text-align:center;color:#ff6a00;font-family:Arial, Helvetica, sans-serif;font-size:large;height:10px;">
                                           Detail Bill:</td>
                                   </tr>
                                  <tr>
                                      <td colspan="2" style="height:10px;border-bottom:1px solid #d5d3d3;">

                                      </td>
                                  </tr>
                                   
                                    <tr>
                                       <td  style="width:50%;text-align:right;">
                                          Flat Number :
                                       </td>
                                          <td style="width:50%;padding-left:3%;">
                                              <asp:Label ID="lblFlat" runat="server"></asp:Label>
                                          </td>
                                       
                                   </tr>
                                      <tr>
                                       <td  style="width:50%;text-align:right;">
                                           Bill Type :
                                       </td>
                                          <td style="width:50%;padding-left:3%;">
                                              <asp:Label ID="lblBillType" runat="server"></asp:Label>
                                          </td>
                                   </tr>

                                    <tr>
                                       <td style="width:50%;text-align:right;">
                                           Due Date :
                                       </td>
                                          <td  style="width:50%;padding-left:3%;">
                                              <asp:Label ID="lblDueDate" runat="server"></asp:Label>
                                          </td>
                                   </tr>

                                      <tr>
                                        <td  style="width:50%;text-align:right;">
                                           Bill Amount :
                                  </td>
                                          <td  style="width:50%;padding-left:3%;">
                                              <asp:Label ID="lblBillAmount" runat="server"></asp:Label>
                                          </td>
                                   </tr>

                                    <tr>
                                        <td  style="width:50%;text-align:right;">
                                           Previous Balance :
                                  </td>
                                          <td  style="width:50%;padding-left:3%;">
                                              <asp:Label ID="lblBalance" runat="server"></asp:Label>
                                          </td>
                                   </tr>
                                   
                                    <tr>
                                        <td  style="width:50%;text-align:right;">
                                            Total Payble :
                                  </td>
                                          <td  style="width:50%;padding-left:3%;">
                                              <asp:Label ID="lblTotalPay" runat="server"></asp:Label>
                                          </td>
                                   </tr>
                                      <tr>
                                        <td  style="width:50%;text-align:right;">
                                            Due For :
                                  </td>
                                          <td  style="width:50%;padding-left:3%;">
                                              <asp:Label ID="lblDueFor" runat="server"></asp:Label>
                                          </td>
                                   </tr>

                                   <tr>
                                        <td  style="width:50%;text-align:right;">
                                           Cycle Type :
                                  </td>
                                          <td  style="width:50%;padding-left:3%;">
                                              <asp:Label ID="lblCycletype" runat="server"></asp:Label>
                                          </td>
                                   </tr>

                                      <tr>
                                        <td  style="width:50%;text-align:right;">
                                         Due Date :
                                  </td>
                                          <td style="width:50%;padding-left:3%;">
                                              &nbsp;<asp:Label ID="lblCurrentTime" runat="server"></asp:Label>
                                          </td>
                                   </tr>
                                   
                                   <tr >
                                       <td colspan="2" style="height:30px;background-color:#f2f2f2;color:#bfbfbf;padding-left:3%;font-size:x-large;">
                                           Payment Made :
                                       </td>
                                   </tr>
                                   <tr >

                                       <td  style="width:50%;text-align:right;">
                                           Paid Amount :
                                  </td>
                                          <td  style="width:50%;padding-left:3%;">
                                              <asp:Label ID="lblAmountPaid" runat="server"></asp:Label>
                                          </td>

                                   </tr>
                                   <tr >

                                       <td  style="width:50%;text-align:right;">
                                           Paid On :
                                  </td>
                                          <td  style="width:50%;padding-left:3%;">
                                              <asp:Label ID="lblPaidDate" runat="server"></asp:Label>
                                          </td>

                                   </tr>

                                   <tr>
                                       <td colspan="2" style="height:50px;background-color:#f2f2f2;color:#bfbfbf;padding-left:3%;font-size:x-large;">
                                           Payment Mode :
                                       </td>
                                   </tr>
                                   <tr style="text-align:center;">
                                       <td>
                                           <a href="#" class="mode_button" id="lnkPaypal">PayPal Entry</a>
                                       </td>
                                       <td>
                                      <a href="#" class="mode_button" id="lnkmannual">Mannual Entry</a>
                                       </td>
                                     
                                   </tr>
                               </table>
                            
                               <table id="paymannual" style="width:70%;margin-left:15%;text-align:center;">
                                                   <tr>
                                                       <td colspan="3" style="width:100%;">
                                                            <asp:Label ID="lblPayment" runat="server" ForeColor="Red"></asp:Label>
                                                       </td>
                                                   </tr>
                                                   <tr>
                                                       <td style="width:40%;text-align:right;">
                                                           Amount :
                                                       </td>
                                                         <td style="width:60%;">
                                                             <asp:TextBox ID="txtAmt" runat="server" CssClass="txtbox_style"></asp:TextBox>
                                                       </td>

                                                        <td>
                                                       </td>
                                                   </tr>
                                                     <tr>
                                                       <td style="text-align:right;">
                                                           Payment Mode:
                                                       </td>
                                                         <td>
                                                             <asp:DropDownList ID="drpPayMode" runat="server"  CssClass="ddl_style"  >
                                                                 <asp:ListItem>Cash</asp:ListItem>
                                                                 <asp:ListItem>Cheque</asp:ListItem>
                                                             </asp:DropDownList>
                                                       </td>
                                                          <td>
                                                       </td>
                                                   </tr>
                                                     <tr>
                                                       <td style="text-align:right;"> 
                                                           Transaction ID:
                                                       </td>
                                                         <td>
                                                             <asp:TextBox ID="txtTransaID" runat="server" CssClass="txtbox_style"></asp:TextBox>
                                                       </td>
                                                          <td>
                                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTransaID" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                             </td>
                                                   </tr>
                                                     <tr>
                                                       <td style="text-align:right;">
                                                           Invoice ID:
                                                       </td>
                                                         <td>
                                                             <asp:TextBox ID="txtInvID" runat="server" CssClass="txtbox_style"></asp:TextBox>
                                                       </td>
                                                          <td>
                                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtInvID" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                              </td>
                                                   </tr>
                                                   <tr>
                                                       <td colspan="3" style="text-align:center;">
                                                          
                                                       </td>
                                                   </tr>
                                                   <tr>
                                                       <td style="height: 50px;text-align:right;">
                                                           <asp:Button ID="btnPaySubmit" runat="server" Text="Submit" CssClass="send" OnClick="btnpaymannual_Click" />
                                                       </td>


                                                       <td colspan="2"  style="text-align:center;">
                                                             <asp:Button ID="btnPayCancel" runat="server" Text="Cancel" CssClass="send"  CausesValidation="False" OnClick="btnpayCancel_Click" />
                                                       </td>
                                                   </tr>
                                               </table>

               </asp:View>
        </asp:MultiView>

                  <%-- ------------------------------------------------- GenerateDeActivateBillForm For Single Flat ---------------------------------------------------  --%>
                 <%--   <div id="GenerateBillSingleFlat" class="modal">  --%>
                       <div id="GenerateDeActivateBillForm" class="modal container-fluid layout_shadow_box" style="background-color:#f2f2f2; width:400px; height:400px; position:absolute;left:50px; top:50px;">
                           <div class="row  theme_third_bg layout_header">
                        <div class="col-xs-11">
                          FlatBill Summary :
                        </div>
                        <div class="col-xs-1">
                          <a onclick="CloseAddFlat()" style="cursor:pointer;"> <span class="fa fa-close" style="color:white;"></span></a> 
                        </div>
                    </div>
                           
                          
                <table style="width:400px;border-collapse:collapse;text-align:left;">                                  
                      
                 <tr>
                     <td  class="lbltxt" style="width:100px;">Flat Number :</td>
                      <td style="width:100px;" > <asp:Label ID="lblFlatNuber" runat="server" Font-Size="Small"></asp:Label> </td>
                     <td style="width:5px;"> </td>
                 </tr>

                  <tr>
                     <td   class="lbltxt" style="width:100px;"> Bill Type :  </td>
                      <td style="width:100px;" > <asp:Label ID="lblNewBillType" runat="server" Font-Size="Small"></asp:Label> </td>
                       <td style="width:5px;"> </td>
                 </tr>

                  <tr>
                     <td  class="lbltxt"  style="width:100px;">Flat Area : </td>
                      <td style="width:100px;" > <asp:Label ID="lblFlatArea" runat="server" Font-Size="Small"></asp:Label> </td>
                       <td style="width:5px;"> </td>
                 </tr>

                 <tr>
                     <td  class="lbltxt"  style="width:100px;"> Charge Type :</td>
                      <td style="width:100px;" > <asp:Label ID="lblChargeType" runat="server" Font-Size="Small"></asp:Label> </td>
                      <td style="width:5px;"> </td>
                 </tr>

                     <tr>
                     <td  class="lbltxt"  style="width:100px;"> Rate :</td>
                      <td style="width:100px;" > <asp:Label ID="lblRate" runat="server" Font-Size="Small"></asp:Label> </td>
                          <td style="width:5px;"> </td>
                 </tr>

                  <tr>
                     <td  class="lbltxt"  style="width:100px;"> From Date :  </td>
                      <td style="width:100px;" > <asp:Label ID="lblFromDate" runat="server" Font-Size="Small"></asp:Label> </td>
                       <td style="width:5px;"> </td>
                 </tr>
                      <tr>
                     <td class="lbltxt" style="width:100px;"> Previous Balance : </td>
                      <td style="width:100px;" > <asp:Label ID="lblPreviousBalance" runat="server" Font-Size="Small"></asp:Label> </td>
                           <td style="width:5px;"> </td>
                 </tr>
                  <tr>
                     <td class="lbltxt" style="width:100px;">Till Date : </td>
                      <td style="width:100px;" >   
                           
                                               
                                   <asp:TextBox ID="txtBillDate" runat="server"></asp:TextBox>                     
                        <%-- >  <asp:Label ID="lblTillDate" runat="server" Font-Size="Small"></asp:Label>--%>
                                 
                      </td>
                      <td style="width:10px;">
                          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                              <ContentTemplate>
                                   <asp:ImageButton ID="btnCalculate" runat="server" ImageUrl="~/Images/Icon/caluclate_icon.png" Width="30" Height="30" CausesValidation="false" OnClick="btnCalculateOnEnddate_Click" />
                              </ContentTemplate>
                          </asp:UpdatePanel>
                          </td>
                    
                 </tr>

                   

                   <tr>
                     <td class="lbltxt" style="width:100px;"> Amount : </td>
                      <td style="width:100px;" >
                          <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                              <ContentTemplate>
                                    <asp:TextBox ID="txtFlatBillAmt" runat="server" CssClass="txtbox_style" Visible="False"></asp:TextBox>
                              </ContentTemplate>
                          </asp:UpdatePanel>                        
                       </td>
                       <td style="width:5px;"> 
                           
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtFlatBillAmt" ErrorMessage="*" ForeColor="#FF5050" ValidationGroup="Textbox" InitialValue="0"></asp:RequiredFieldValidator>
                           
                       </td>
                 </tr>
                
                    <tr>
                        <td class="lbltxt"> Description :</td>
                        <td>
                            <asp:TextBox ID="txtBillGenSingleFlatdesc" runat="server" onchange="ResizeBillGeTextbox();" CssClass="txtbox_style"></asp:TextBox>
                        </td>
                    </tr>
                 <tr>
                     <td colspan="2" style="height:15px;">   </td>
                 </tr>
                 <tr>
                     <td colspan="2" style="height:15px;text-align:center;">
                          <asp:Label ID="lblBillDuplicate" runat="server" Font-Size="Small" ForeColor="#55AAFF"></asp:Label>
                         
                         <span style="margin-left:5px;"></span> 
                         
                        

                     </td>
                 </tr>
                 <tr>
                     <td colspan="2" style="height:15px;">   </td>
                 </tr>
                 <tr style="text-align:center;">
                     <td> <asp:Button ID="btnSingleFlatGenerate" runat="server" Text="Generate Bill" CssClass="btn_style" OnClick="btnSingleFlatGenerate_Click" ValidationGroup="Textbox" /></td>
                       <td>
                            <button type="button" id="btnBillGencancel"  class="btn_style">Cancel</button> 

                           <%--<asp:Button ID="btnBillGencancel" runat="server" Text="Cancel"  CssClass="btn_style" OnClick="btnBillGencancel_Click"/>--%>

                       </td> 
                 </tr>
                    <tr>
                        <td style="height:15px;">  </td>
                    </tr>
                 </table>
                </div>

                    <%--   Import Bill Form  --%>
                     <div id="importModal" class="layout_modal_Window">
                                  <div class="container-fluid" style="background-color:#f6f6f6; width:500px; border-radius:5px;">
                                    <div class="row layout_header theme_third_bg" style="height:30px;">
                                        <div class="col-xs-10">
                                        Generate Bill:
                                            </div>
                                        <div class="col-xs-2">
                                            <a onclick="CloseBillImport()" style="cursor:pointer;"> <span class="fa fa-close" style="color:white;"></span></a> 
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12" style="height:10px;">
                                            <asp:Label ID="lblGenerateStatus"  runat="server" Font-Size="Small" Text="" ForeColor="Red"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4" style="text-align:left">
                                            Generate by Rate :
                                        </div>
                                        <div class="col-md-8">
                                            <asp:DropDownList ID="drpGenBillForOnLatest" runat="server" CssClass="layout_ddl_filter"></asp:DropDownList>
                                           <asp:Button ID="btnGenerateLatest" runat="server" OnClientClick="return ValidateDropdown();" OnClick="btnGenerateLatestBill_Click" Text="Generate Bill" CausesValidation="false"/>
                                        </div>
                                        <div class="col-xs-12">
                                            <hr/>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4" style="text-align:left">
                                           Import File :
                                        </div>
                                        <div class="col-md-8">
                                             <asp:FileUpload ID="BillsUpload" runat="server" />
                                           <span style="margin-left:5%;"></span>
                                          <asp:Button ID="BillsUploadSubmit" runat="server" CausesValidation="false" CssClass="send" OnClick="BillsUploadSubmit_Click" Text="Submit" />
                                        </div>
                             
                                    </div>
                                     <hr/>
                                    <div class="row">
                                        <div class="col-md-4" style="text-align:left">
                                            Export Grid Data :
                                        </div>
                                        <div class="col-md-8">
                                            <asp:Button ID="btnExport2" runat="server" Text="Export" CausesValidation="false" OnClick="ExportLatestToExcel" />
                                        </div>
                             
                                    </div>
                                     <hr/>
                                    <div class="row">
                                        <div class="col-xs-12" style="text-align:center;">
                                                <button type="button" id="btnClose" class="Add_Button" >Close</button> 
                                        </div>
                                    </div>
                                    </div>
                             </div>

                      <%--   Generated Bill Form  --%>
                   <div id="generatedbilldescriptnmodal" class="modal"> 
                 <table style="width:40%;margin-left:30%;background-color:#bfbfbf;border-radius:5px;"> 
                     <tr style="background-color:#5ca6de;">
                         <td colspan="2"  style="color:white;font-size:large;padding:1% 0 1% 3%;"> Bill Description :</td>
                         </tr>
                               <tr>
                       <td colspan="2" style="height:10px;"> </td>
                   </tr>
                <tr>
                    <td class="lbltxt" style="width:50%;"> Bill Type :</td>
                    <td style="width:50%;">
                        <asp:Label ID="lblBilltypeDes" runat="server" Text=""></asp:Label> </td>              
                </tr>
                <tr>
                    <td class="lbltxt"  style="width:50%;"> Chargetype : </td>
                    <td style="width:50%;">
                        <asp:Label ID="lblchargetypeDes" runat="server" Text=""></asp:Label> </td>
                </tr>
                   <tr>
                       <td class="lbltxt" > Rate :</td>
                        <td>
                            <asp:Label ID="lblrateDes" runat="server" Text=""></asp:Label> </td>
                   </tr>
                   <tr>
                        <td class="lbltxt"  style="width:50%;"> Rows Effect : </td> 
                       <td>
                           <asp:Label ID="lblRowsEffectDes" runat="server" Text=""></asp:Label> </td>
                   </tr>
                     <tr> <td colspan="2" style="height:15px;"> </td>

                     </tr>

                        <tr> 
                            <td colspan="2" style="text-align:center;"> 
                               <asp:Label ID="lblBillGeneraStatus" runat="server" ForeColor="White" Text="status"></asp:Label>
                             </td>
                        </tr>
                     <tr>
                         <td colspan="2" style="text-align:center;padding:1% 0 1% 0;">                                                                    
                          <asp:FileUpload ID="uploadBill" runat ="server" Visible="false"/>
                         </td>
                     </tr>
                     <tr>                        
                         <td colspan="2" style="text-align:center;border-bottom:1px solid #969393;"> 
                             <asp:Button ID="btnbillCaluclate" runat="server" CssClass="btn_style" OnClick="btnbillCaluclate_Click" Text="Generate Bill" ValidationGroup="BillGentype"  Visible="false"/>  
                         </td>
                     </tr>
                                     
                     <tr>
                         <td colspan="2" style="height:10px;text-align:right;">
                             <button type="button" id="BillDescpCancel" style="background-color:#bfbfbf;font-weight:bold;color:#f2f2f2;border-style:none;outline:0;cursor:pointer;">Close</button>
                         </td>
                     </tr>
               </table>
           </div>
       </form>
             </div>
          </div>
          <div class="row">

        <div class="col-xs-12 col-sm-2"></div>
              </div>
  </div>
</body>
</html>
