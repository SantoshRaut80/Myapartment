<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActiveBillPlan.aspx.cs" Inherits="ActiveBillPlan" %>

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
            $("#txtActBillsFlats").autocomplete({
                  source: function (request, response) {
                    var param = {
                        FlatNumber: $('#txtActBillsFlats').val()
                    };
                    $.ajax({
                        url: "ActiveBillPlan.aspx/GetFlatNumber",
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
            $(document).click(function () {
                $("#ActiveBillDropdown").hide();

            });

            $("#btnActivateBill").click(function () {
                $("#ActivateBillForm").show();
            });

            $("#btnBillGencancel").click(function () {

                $("#GenerateDeActivateBillForm").hide();
            });

            $("#txtBillDate").datepicker();

        });

        function ActiveBillPopup(BillID, FlatID, FlatArea, BillType, Rate, ChargeType, CycleType, cyclestart, cycleEnd, Element) {
           

            var status = Element.parentNode.parentNode.cells[7].innerHTML;

            document.getElementById("HiddenField1").value = FlatID;
            document.getElementById("HiddenField2").value = BillType;
            txtFlatID.value = FlatID;
            document.getElementById("txtRate").value = Rate;
            document.getElementById("txtchargeType").value = ChargeType;
            document.getElementById("txtBillType").value = BillType;


            var Posx = 0;
            var Posy = 0;
            while (Element != null) {

                Posx += Element.offsetLeft;
                Posy += Element.offsetTop;
                Element = Element.offsetParent;
            }


         

            document.getElementById("ActiveBillDropdown").style.top = Posy + 'px';
            document.getElementById("ActiveBillDropdown").style.left = Posx + 30 + 'px';


            document.getElementById("HiddenBillID").value = BillID;
            document.getElementById("HiddenFieldFlatArea").value = FlatArea;
            document.getElementById("HiddenFieldRate").value = Rate;
            document.getElementById("HiddenFieldCycleType").value = CycleType;
            document.getElementById("HiddenFieldChargeType").value = ChargeType;
            document.getElementById("HiddenFieldCycleStart").value = cyclestart;


            var Currentdate = new Date();
            var CurrentDateFormat = Currentdate.toLocaleDateString();
            var cyclestartdatenew = new Date(cyclestart);
            var cycleenddatenew = new Date(cycleEnd);
            var cyclestartdate = cyclestartdatenew.toLocaleDateString();
            var cycleenddate = cycleenddatenew.toLocaleDateString();

            if (status == "InActive") {

                document.getElementById("btnActivateBill").style.display = "block";
                document.getElementById("btnDeactivate").style.display = "none";


            }

            if (status == "Active") {

                document.getElementById("btnActivateBill").style.display = "none";
                document.getElementById("btnDeactivate").style.display = "block";
            }


            if (status == "DeActive") {

                document.getElementById("btnActivateBill").style.display = "block";
                document.getElementById("btnDeactivate").style.display = "none";

            }

            $("#ActiveBillDropdown").slideDown();
            event.stopPropagation();

        }

        function ShowGenerateDeActivateBillForm() {

            document.getElementById("GenerateDeActivateBillForm").style.display = "block";

        }
    </script>
</head>
<body>
      <div class="container-fluid">
        <div class="col-xs-12 col-sm-10">
               <form id="form1" runat="server">
                       <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
              <div class="container-fluid">


                  <div class="row layout_header theme_third_bg font_size_2 vcenter" style="height:40px;">
                 <div class="col-sm-3 hidden-xs" >
                     <div><label class="pull-left ">Active Bill : </label></div>
                 </div>
                <div class="col-sm-9 col-xs-12" style="padding:0px;">
                                          <div class="layout_filter_box" style="width:400px;">
                                              <asp:DropDownList ID="drpBillStatusype" runat="server" CssClass="layout_ddl_filter" >
                                                   <asp:ListItem>Show All</asp:ListItem>
                                                   <asp:ListItem>Active</asp:ListItem>
                                                   <asp:ListItem>DeActive</asp:ListItem>
                                                   <asp:ListItem>InActive</asp:ListItem>
                                               </asp:DropDownList>
                                              <asp:DropDownList ID="drpActivatedBillType" runat="server" CssClass="layout_ddl_filter"/>                                 
                                               <asp:TextBox ID="txtActBillsFlats" runat="server" ToolTip="Enter Flat" placeholder="Flat Number"   CssClass="layout_txtbox_filter"></asp:TextBox>   
                                               <asp:LinkButton runat="server" BackColor="Transparent" ForeColor="Black" OnClick="searchActivatedBills_Click" ValidationGroup="Flat_Search"> <span class="glyphicon glyphicon-search"></span></asp:LinkButton>
                                              
                                              </div>
                                      </div>
              
             </div>


              </div>

                  <table id="tblFlatBills" runat="server" style="margin-top:1%;width:100%;border:1px solid #f2f2f2; box-shadow:2px 2px 5px #bfbfbf; border-collapse:collapse;">    
                                                         
                   
                   <tr>
                       <td colspan="4" style="height:15px;"> </td>
                   </tr>
                  
                   <tr>
                       <td colspan="4" style="text-align:center;">
                           Activated :
                           <asp:Label ID="lblActivateCount" runat="server"  Text=""></asp:Label>
                           Deactivated :
                            <asp:Label ID="lblDeactivateCount" runat="server" Text=""></asp:Label>
                           Not Activated :
                           <asp:Label ID="lblNotActivateCount" runat="server" Text=""></asp:Label>

                       </td>

                        
                     </tr>
                   <tr>
                       <td colspan="4" style="text-align:center;height:10px;"> 

                       </td>
                   </tr>
                
                     <tr style="height:3vh;text-align:center">
                       
                         <td colspan="4" style="text-align:center;">
                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                         <ContentTemplate>--%>
                             <asp:GridView ID="FlatsBillsGrid" runat="server" AllowPaging="True" 
                                 HeaderStyle-BackColor="#6eab91" 
                             HeaderStyle-BorderStyle="None"  
                                 AutoGenerateColumns="false"  BackColor="#E8E8E8" BorderColor="Silver" BorderStyle="Solid"
                                  BorderWidth="1px" EmptyDataText="No Records Found" Font-Names="Calibri" ForeColor="#666666" 
                                 HorizontalAlign="Center" PageSize="15" 
                                 ShowHeaderWhenEmpty="True" style="margin-bottom: 0px" 
                                 OnPageIndexChanging="FlatsBillsGrid_PageIndexChanging" OnRowDataBound="FlatsBillsGrid_RowDataBound">
                                
                                   <AlternatingRowStyle BackColor="#ffffff" />
                                 <Columns>
                                        <asp:BoundField DataField="BillID" HeaderText="BillID" ItemStyle-CssClass="BillActiveGrid" HeaderStyle-Width="30px"/>
                                     <asp:BoundField DataField="FlatID" HeaderText="FlatNumber" ItemStyle-CssClass="BillActiveGrid" HeaderStyle-Width="60px"/>
                                     <asp:BoundField DataField="FlatArea" HeaderText="FlatArea" ItemStyle-CssClass="BillActiveGrid"  HeaderStyle-Width="60px"/>
                                     <asp:BoundField DataField="BillType" HeaderText="BillType" ItemStyle-CssClass="BillActiveGrid" HeaderStyle-Width="80px"/>
                                     <asp:BoundField DataField="Rate" HeaderStyle-Width="70px" HeaderText="Rate" ItemStyle-Width="70px" ItemStyle-CssClass="BillActiveGrid">
                                     <HeaderStyle Width="70px" />
                                     <ItemStyle Width="70px" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="ChargeType" HeaderText="ChargeType"  ItemStyle-CssClass="BillActiveGrid" HeaderStyle-Width="60px"/>
                                     <asp:BoundField DataField="CycleType" HeaderText="CycleType"  ItemStyle-CssClass="BillActiveGrid" HeaderStyle-Width="80px"/>
                                     <asp:BoundField DataField="CycleStart" HeaderText="CycleStart" DataFormatString="{0:dd/MMM/yyyy}" ItemStyle-Font-Size="Small"  HeaderStyle-Width="80px"/>
                                       <asp:BoundField DataField="CycleEnD" HeaderText="CycleEnD" DataFormatString="{0:dd/MMM/yyyy}" ItemStyle-Font-Size="Small"  HeaderStyle-Width="80px"/>
                                       <asp:BoundField  HeaderText="Status"  ItemStyle-CssClass="BillActiveGrid"  ItemStyle-Wrap="false" ItemStyle-Width="50px" HeaderStyle-Width="60px"/>
                                     <asp:TemplateField  HeaderStyle-Width="20px">
                                  <ItemTemplate>
                             <button id="button" onclick="ActiveBillPopup('<%# Eval("BillID") %>' ,'<%# Eval("FlatID") %>' , '<%# Eval("FlatArea") %>' ,'<%# Eval("BillType") %>','<%# Eval("Rate") %>','<%# Eval("ChargeType") %>','<%# Eval("CycleType") %>','<%# Eval("CycleStart") %>','<%# Eval("CycleEnD") %>',this)" type="button" style=" width:20px;background-color:transparent;border:none;outline:0; height:20px;">
                              <i class="fa fa-angle-double-right" id="left_icon" style="color:gray;font-size:20px"></i>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                 </Columns>
                                 <EmptyDataRowStyle BackColor="Silver" />
                                 
                                 <PagerSettings Mode="NumericFirstLast" />
                                 <PagerStyle BackColor="White" BorderColor="#F0F5F5" Font-Bold="False" Font-Names="Berlin Sans FB" Font-Size="Medium" ForeColor="#62BCFF" HorizontalAlign="Center" />
                             </asp:GridView>                            
                         </td>

                            <td style="">  
                                 <asp:HiddenField ID="HiddenBillID" runat="server" />
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <asp:HiddenField ID="HiddenField2" runat="server" />
                               
                                <asp:HiddenField ID="HiddenBillActvFlat" runat="server" />
                                <asp:HiddenField ID="HiddenActDeact" runat="server" />
                                <asp:HiddenField ID="HiddenbillType" runat="server" />
                               
                                
                                <asp:HiddenField ID="HiddenFieldRate" runat="server" />
                                <asp:HiddenField ID="HiddenFieldFlatArea" runat="server" />
                                <asp:HiddenField ID="HiddenFieldCycleType" runat="server" />
                                <asp:HiddenField ID="HiddenFieldChargeType" runat="server" />
                                 <asp:HiddenField ID="HiddenFieldCycleStart" runat="server" />


                                <div id="ActiveBillDropdown" class="layout-dropdown-content theme-dropdown-content">                                   
                                             <asp:Button ID="btnFlatbillGen" runat="server" CssClass="layout_dropdown_Button"  CausesValidation="false" Text="Generate Bill "  OnClick="btnFlatbillGen_Click"/>
                                            <button type="button" id="btnActivateBill"  class="layout_dropdown_Button">Activate</button> 
                                    <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate" CssClass="layout_dropdown_Button"  CausesValidation="false" OnClick="btnDeactivatebill_Click" />
                                </div>
                         </td>
                     </tr>
                      <tr>
                          <td colspan="4" style="height:10px;"> </td>
                      </tr>
                  
                    <tr>
                          <td colspan="4" style="text-align:center;"> <asp:Label ID="lblBillGenStatus" runat="server" ForeColor="#4FA7FF"></asp:Label></td>
                      </tr>
                      <tr>
                          <td colspan="4" style="text-align:center;"> <asp:Label ID="Label26" runat="server" Text="Total Count :" ForeColor="#0099FF"></asp:Label> 
                              <span style="margin-left:1%;"></span>
                              <asp:Label ID="lblActvBillsCount" runat="server" ForeColor="#0099FF"></asp:Label>
                          </td>
                      </tr>                    
                  <tr>                    
                      <td colspan="4" style="height:10px;"> </td>
                     </tr>                                                       
                 </table>
               <asp:Button ID="btnActiveExport" runat="server" Text="Export" CausesValidation="false" OnClick="ExportToExcelActive"/><%--Added by Aarshi on 14 - Sept - 2017 for bug fix--%>

                        <%----------------------------------------------   Bill description ---------------------------------------------  --%>
              


               <%----------------------------------------------------- Activate a Bill for flat----------------------------------------------------%>
               <%--  <div id="Mymodalactivatenewplan" class="modal"> --%>
                 <div id="ActivateBillForm" class="modal">
                  <table style="width:40%;margin-left:30%;margin-top:3%;background-color:#e0dada;">                 
                    <tr style="background-color:#5ca6de;color:#579ed4;padding:2% 0 2% 0;">
                        <td colspan="3" style="text-align:left;color:white;font-weight:bold;font-size:large;padding:1% 0 1% 3%;">
                          New Bill :
                        </td>
                    </tr>
                       <tr>
                      <td style="height:15px;"> </td>
                  </tr>
                          <tr>
                               <td class="lbltxt" style="width:50%;">
                                  Flat Number :
                                  </td>
                                   <td style="width:50%;">
                                       <asp:TextBox ID="txtFlatID" runat="server" CssClass="txtbox_style"></asp:TextBox>
                                        </td>
                                  <td style="width:1%;">

                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtFlatID" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                  </td>
                              </tr>
                              <tr>
                                  <td class="lbltxt" style="width:50%;">
                                      BillType :
                                  </td>
                                   <td style="width:50%;">
                                       <asp:TextBox ID="txtBillType" runat="server" CssClass="txtbox_style"></asp:TextBox>
                                                                   
                                       
                                         </td>
                                  <td style="width:1%;">

                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBillType" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                  </td>
                              </tr>
                             <tr>
                                  <td class="lbltxt" style="width:50%;">
                                      Charge Type:
                                  </td>
                                   <td style="width:50%;">
                                       <asp:TextBox ID="txtchargeType" runat="server" CssClass="txtbox_style"></asp:TextBox>
                                  </td>
                              </tr>
                            <tr>
                                  <td class="lbltxt" style="width:50%;">
                                      Rate:
                                  </td>
                                   <td style="width:50%;">
                                       <asp:TextBox ID="txtRate" runat="server" CssClass="txtbox_style"></asp:TextBox>
                                  </td>
                              </tr>
                           <tr>
                                  <td class="lbltxt" style="width:50%;">
                                      CycleType:
                                  </td>
                                   <td style="width:50%;">
                                       <asp:DropDownList ID="drpCycletype" runat="server" CssClass="ddl_style" Enabled="false">
                                           <asp:ListItem>Monthly</asp:ListItem>
                                           <asp:ListItem>Quarterly</asp:ListItem>
                                           <asp:ListItem>Yearly</asp:ListItem>
                                       </asp:DropDownList>
                                  </td>
                              </tr>
                            <tr>
                                  <td class="lbltxt" style="width:10%;">
                                      Cyclestart :
                                  </td>
                                  <td style="width:10%;">   
                                      <asp:TextBox ID="txtCyclestart" runat="server" CssClass="txtbox_style"    ForeColor="#808080" ></asp:TextBox>
                                  </td>
                                  <td style="width:1%;">
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCyclestart" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                  </td>
                              </tr>
                               <tr>
                                  <td class="lbltxt">
                                      CycleEnd :
                                  </td>
                                  <td>
                                      <asp:TextBox ID="txtCycleend" runat="server" CssClass="txtbox_style"  ForeColor="#808080" ></asp:TextBox> 
                                  </td>
                                   <td style="width:1%;">
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCycleend" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                   </td>
                              </tr>
                              <tr>
                                  <td colspan="2" style="height:30px;text-align:center;">
                                      <asp:Label ID="lblbilltypeexist" runat="server" Font-Size="Small" ForeColor="#FF6262"></asp:Label>
                                  </td>
                              </tr>
                              <tr>   
                                  <td  style="text-align:center;">
                                      <asp:Button ID="btnBillcycleSubmit" runat="server"  Text="Activate" CssClass="btn_style" OnClick="btnBillcycleSubmit_Click" />
                                  </td>

                                  <td style="text-align:center;">
                                   <button type="button" id="btnBillactvationcancel"  class="btn_style">Cancel</button>
                                  </td>
                              </tr>
                      <tr>
                          <td style="height:15px;"> </td>
                      </tr>
                          </table>

                    </div>
       

               <%------------------------------------------------------- end of the bills activation page----------------------------------%>
               


             <%--   ---------------------------    Activated Bills View Section Ends Here     -------------  -------- --------------%>
                                <%-- ------------------------------------------------- GenerateDeActivateBillForm For Single Flat ---------------------------------------------------  --%>
                      
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
                      <td style="width:100px;" > <asp:Label ID="lblBillType" runat="server" Font-Size="Small"></asp:Label> </td>
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


         </form>
             </div>
        <div class="col-xs-12 col-sm-2"></div>
  </div>
</body>
</html>
