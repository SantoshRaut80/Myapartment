<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SocietyBillPlan.aspx.cs" Inherits="SocietyBillPlan" %>

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

    <style>
        
        .BillCycle_table {
            background-color:#faf9f9;
            border-radius:10px 10px 10px 10px;
            border-collapse:collapse;
            margin:5px;
           /* border: 1px solid #c1c1c1;*/
            box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.2), 0 5px 10px 0 rgba(0, 0, 0, 0.19);
            width:225px;
            text-align:center;
            padding:4%;
        }

        .Bill_Plan_Table
        {
            width:100%;
            border:1px solid #f2f2f2;
            box-shadow:2px 2px 5px #bfbfbf;
            padding:2% 0 1% 1%;
            border-collapse:collapse;
            margin-top:1%;
            min-height:350px;
        }
    </style>
    <script>

        var varBillID;
        var varBillType;
        var varChargeType;
        var varCycletype;
        var varRate;
        var varApplyTo;


        $(document).ready(function () {


            $("#BillCancel_Button").click(function () {
               $('#NewEditSocietyPlanForm').hide();
            });


            $("#IDNewBill").click(function () {
                $('#NewEditSocietyPlanForm').show();
                $('#radioall').prop('checked', false);
                $('#radioselected').prop('checked', false);
                $('#drpAddBillType').prop('selectedIndex', 0);
                $('#drpchargetype').prop('selectedIndex', 0);
                $('#drpbillcycle').prop('selectedIndex', 0);

                lblNewEdit.innerHTML = "Add Plan";
                $("#btnBillingEdit").hide();
                $("#btnBillingsubmit").show();
                //$("#New_Plan").css("visibility", "visible");

            });


            $("html").click(function () {
                $("#BillPlans_dropdown").hide();
            });




            $("#btnEditPlan").click(function () {

                $('#NewEditSocietyPlanForm').show();
               
                if (varApplyTo == "1") {
                    $('#radioall').prop('checked', true);
                    $('#radioselected').prop('checked', false);
                }
                else {
                    $('#radioall').prop('checked', false);
                    $('#radioselected').prop('checked', true);
                }
                $('#radioselected').attr('disabled', true);
                $('#radioall').attr('disabled', true);

                $('#drpAddBillType').val(varBillType);
                $('#drpAddBillType').attr('disabled', true);
                $('#drpchargetype').val(varChargeType);
                $('#drpbillcycle').val(varCycletype);
                $('#drpbillcycle').attr('disabled', true);
                $('#txtBillRate').val(varRate);

                lblNewEdit.innerHTML = "Edit Plan";
                $("#btnBillingEdit").show();
                $("#btnBillingsubmit").hide();
                

            });


        });

        function CloseAddPlan()
        {
            $("#NewEditSocietyPlanForm").hide();

        }

        function EditPlansPopup(BillID, BillType, ChargeType, Rate, Cycletype, ApplyTo, Element) {
            //alert(Element);
            document.getElementById("HiddenPlanBillID").value = BillID;
            //document.getElementById("HiddenPlanBillType").value = BillType;
            //document.getElementById("HiddenChargeType").value = ChargeType;
            //document.getElementById("HiddenCycleType").value = Cycletype;
            //document.getElementById("HiddenEditSocietyRate").value = Rate;
            //document.getElementById("HiddenApplyTo").value = ApplyTo;

            varBillID = BillID;
            varBillType = BillType;
            varChargeType = ChargeType;
            varCycletype = Cycletype;
            varRate = Rate;
            varApplyTo = ApplyTo;

            var myArray = new Array();
            myArray[0] = BillType;
            myArray[1] = BillID;

            var Posx = 0;
            var Posy = 0;
            while (Element != null) {

                Posx += Element.offsetLeft;
                Posy += Element.offsetTop;
                Element = Element.offsetParent;
            }

         
                $("#BillPlans_dropdown").slideDown();
         
                document.getElementById("BillPlans_dropdown").style.left = Posx - 90 + 'px';
                document.getElementById("BillPlans_dropdown").style.top = Posy + 'px';
                event.stopPropagation();
        }
    </script>
</head>
<body>
   <div class="container-fluid">

        <div class="col-xs-12 col-sm-10">
            <form id="form1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                             
                <div class="container-fluid">
                                <div class="row layout_header theme_third_bg font_size_2 vcenter" style="height:40px;">
                                 <div class="col-sm-3 hidden-xs" >
                                     <div><label class="pull-left ">Bill Plans : </label></div>
                                 </div>
                                <div class="col-sm-6 col-xs-12" style="padding:0px;">
                                                         
                                                      </div>
                                 <div class="col-sm-3 hidden-xs" style="vertical-align:middle;">
                                    <div>
                                        <button type="button" id="IDNewBill" class="theme-btn-primary">Add Plan</button>  
                                    </div>
                                 </div>
                             </div>
                    </div>

                    <table class="Bill_Plan_Table">
                       
                              
                  <tr>
                    <td colspan="2" style="padding-bottom:1%;padding-top:0%;">

                         <div id="BillPlans_dropdown" class="layout-dropdown-content theme-dropdown-content" style="width:7%;"> 
                             <button type="button" id="btnEditPlan" class="layout_dropdown_Button">Edit</button>                                                                                    
                     <asp:Button ID="btnsocietyPlanEdit" Visible="false" runat="server" CommandArgument="Hello" CausesValidation="false"  CssClass="layout_dropdown_Button"  Text="Edit"   OnClick="btnsocietyPlanEdit_Click"/>
                     <asp:Button ID="btnSocietyPlanDeactive" runat="server" Text="Deactivate" CausesValidation="false" CssClass="layout_dropdown_Button"  OnClick="btnSocietyPlanDeactive_Click"/>                                                                                                      
                 </div>    
                
                                      <asp:DataList ID="BillPlanDataList" runat="server" HorizontalAlign="Center" RepeatColumns="3" RepeatDirection="Horizontal" Width="80%"  CellSpacing="5" 
                                           OnItemDataBound="SocietyBillPlan_ItemDataBound"> 
                                       
                                        <ItemStyle />
                                        <ItemTemplate>
                                    <table class="BillCycle_table" >
                                        <tr class="layout_header theme_third_bg" style="height:35px; ">
                                            <td colspan="2" style="text-align:center;">
                                                <asp:Label ID="Label3" runat="server" ForeColor="White" Text='<%# Eval("BillType") %>'> </asp:Label>
                                                
                                            </td>
                                        </tr>
                                       <tr style="text-align:left;">
                                      
                                           <td style="color:#969393;padding-left:8%;"> ChargeType :</td>
                                           <td>
                                                <asp:Label ID="Label4" runat="server" ForeColor="#999999" Text='<%# Eval("ChargeType") %>'></asp:Label>
                                           </td>
                                       </tr>
                                           <tr style="text-align:left;">
                                               <td style="color:#969393;padding-left:8%;"> Rate : </td>
                                           <td>
                                                <asp:Label ID="Label5" runat="server" ForeColor="#808080" Text='<%# Eval("Rate") %>'></asp:Label>
                                           </td>
                                       </tr>
                                        <tr style="text-align:left;">
                                            <td style="color:#969393;padding-left:8%;"> CycleType:</td>
                                           <td>
                                                <asp:Label ID="Label6" runat="server" ForeColor="#808080" Text='<%# Eval("CycleType") %>'></asp:Label>
                                             
                                           </td>
                                       </tr>

                                        <tr style="text-align:left;">
                                            <td style="color:#969393;padding-left:8%;"> ApplyTo:
                                           </td>  
                                             <td>  <asp:Label ID="lblBillEntity" runat="server" ForeColor="#808080" Text='<%# Eval("Applyto") %>'></asp:Label></td>
                                        </tr>

                                        <tr style="text-align:left;">
                                            <td style="color:#969393;padding-left:8%;"> Total Flats : </td>
                                              <td>
                                                  <asp:Label ID="lblcount" runat="server" Text=""></asp:Label> </td>
                                        </tr>                                                                     
                                         <tr style="text-align:right;">
                          <td colspan="2">
                          <asp:Label ID="lblID" runat="server" ForeColor="White" Text='<%# Eval("BillID") %>'> </asp:Label>

                              <button type ="button" id="PlansEdit" style="border:none;border-radius:5px; background-color:#faf9f9;" onclick="EditPlansPopup('<%# Eval("BillID") %>','<%# Eval("BillType") %>','<%# Eval("chargeType") %>','<%# Eval("Rate") %>','<%# Eval("CycleType") %>','<%# Eval("Applyto") %>',this)">
                                  <img src="Images/Icon/icon-edit-1.png" style="width:20px; height:20px; border-radius:5px;" />

                              </button>

                      <%--   <asp:LinkButton ID="LinkButton1" runat="server" CssClass="Edit_icon" CausesValidation="false"><img src="Images/icon-edit-1.png" style="width:30px; height:30px;" /></asp:LinkButton>
                          --%> 
                          </td>
                 </tr>
                                          
                                    </table>
                                </ItemTemplate>
                                <AlternatingItemStyle />

                                
                            </asp:DataList>

                </td>
                    </tr>
                       <tr>
                           <td style="text-align:center;">
                            <asp:Label ID="lblBillPlanStatus" runat="server" ></asp:Label>
                              <asp:HiddenField ID="HiddenPlanBillID" runat="server" />
                              <asp:HiddenField ID="HiddenPlanBillType" runat="server" />  
                              <asp:HiddenField ID="HiddenEditSocietyRate" runat="server" />
                               <asp:HiddenField ID="HiddenCycleType" runat="server" />
                               <asp:HiddenField ID="HiddenChargeType" runat="server" />
                                <asp:HiddenField ID="HiddenApplyTo" runat="server" />
                               <asp:HiddenField ID="HiddenEditsocietyID" runat="server" />
                             <asp:HiddenField ID="HiddenFormRequired" runat="server" />

                           </td>

                       </tr>
                </table>                          
                 <%------------------------------------------------------------------------Society Bill Plan View End ----------------------------------------------------------%>



                  <%---------------------------------------------------------- Add New Plan starts from here  -----------------------------------------------------------------%>

             <%--  <div id="myModalNewBillPopup" class="modal">--%>
                  <div id="NewEditSocietyPlanForm" class="modal layout_shadow_box" style="width:300px; height:300px;position:absolute; top:80px; left:200px; padding-left:5px;">
                      <table id="New_Plan" style="background-color:#f1f1f1; text-align:left; line-height:30px;">
       
                <tr class="theme_third_bg" style="height:40px;border-radius:5%;border-collapse:collapse;">
                <td colspan="2" style="text-align:left;padding-left:5px; width:200px;">
                    <asp:Label ID="lblNewEdit" runat="server" Text="New Plan " ForeColor="White" Font-Size="Large"></asp:Label>
                </td>   
                    <td colspan="2" style="width:20px;">
                        <a onclick="CloseAddPlan()" style="cursor:pointer;"> <span class="fa fa-close" style="color:white;"></span></a> 
                    </td>         
           </tr>
                <tr>
                    <td colspan="4" style="height:15px;">  </td>
                </tr>
            <tr>
               <td class="lbltxt"  style="width:100px;">
                  Bill Type :
                </td>             
                <td style="width:100px;">                    
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="drpAddBillType" runat="server" CssClass="ddl_style" AutoPostBack="true" OnSelectedIndexChanged="drpAddBillType_SelectedIndexChanged">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem>Electricity</asp:ListItem>
                                <asp:ListItem>Maintenance</asp:ListItem>
                                <asp:ListItem>Club</asp:ListItem>
                                <asp:ListItem>Gas</asp:ListItem>
                                <asp:ListItem>Water</asp:ListItem>
                                <asp:ListItem>Security</asp:ListItem>
                                <asp:ListItem>CulturalActivities</asp:ListItem>
                            </asp:DropDownList> <br />
                            <asp:Label ID="lblBillCheck" runat="server" Font-Size="Small" ForeColor="#FF6666"></asp:Label>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                </td>
                <td style="width:10px;">
                 
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="drpAddBillType" ErrorMessage="*" ForeColor="#FF5050" InitialValue="0"></asp:RequiredFieldValidator>
                 
                </td>
                    <td style="width:10px;">
                        
                     
                </td>
              </tr>
         <tr>
             <td class="lbltxt"> 
                Charge Type :
             </td>
              <td>
                  <asp:DropDownList ID="drpchargetype" runat="server" CssClass="ddl_style">
                      <asp:ListItem>Manual</asp:ListItem>
                      <asp:ListItem>Rate</asp:ListItem>
                      <asp:ListItem>Fixed</asp:ListItem>
                  </asp:DropDownList>
             </td>
              <td>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="drpchargetype"></asp:RequiredFieldValidator>
             </td>
              <td>
                  &nbsp;</td>
         </tr>
          <tr>
             <td class="lbltxt">
                 Rate :
              </td>
              <td>
                  <asp:TextBox ID="txtBillRate" runat="server" CssClass="txtbox_style"></asp:TextBox>
             </td>
              <td>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtBillRate"></asp:RequiredFieldValidator>
             </td>
              <td>
                </td>
         </tr>
            <tr>
             <td class="lbltxt">
                 <asp:Label ID="Label22" runat="server" Text="Cycle :"></asp:Label>
                </td>
              <td>
                  <asp:DropDownList ID="drpbillcycle" runat="server" CssClass="ddl_style">
                        <asp:ListItem>Daily</asp:ListItem>
                      <asp:ListItem>Monthly</asp:ListItem>
                      <asp:ListItem>Quarterly</asp:ListItem>
                  </asp:DropDownList>
             </td>
              <td>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="drpbillcycle"></asp:RequiredFieldValidator>
             </td>
              <td>
                  &nbsp;</td>
         </tr>
          <tr style="height:50px;">
             <td class="lbltxt" style="width:100px;">
                 Apply To :
              </td>
              <td style="width:120px;">
                  <asp:RadioButton ID="radioall" runat="server"  GroupName="Radiobill"/>
                                    <label>All</label><br />
                                    <asp:RadioButton ID="radioselected" runat="server"  GroupName="Radiobill"/>
                                    <label>Selected</label>
             </td>
             
         </tr>
         <tr>
             <td colspan="4" style="text-align:center;"> 
                 <asp:Label ID="lblBillstatus" runat="server" ForeColor="#48A4FF" Font-Size="Small"></asp:Label>
             </td>
         </tr>
                
          <tr style="text-align:center;">
             <td >

                 <asp:Button ID="btnBillingsubmit" runat="server" CssClass="btn_style" Text="Submit" OnClick="btnPlanSubmit_Click" />
                 <asp:Button ID="btnBillingEdit" runat="server" Text="Update"  CssClass="btn_style" OnClick="btnBillingEdit_Click"/>
             </td>
               <td colspan="3">
                   <button type="button" id="BillCancel_Button"  class="btn_style">Cancel</button> 
               

             </td>
            
         </tr>
         </table>
                  </div>
                
             <%---------------------------------------------------------------------Add New Plan  Ends Here ----------------------------------------------------------%>

            </form>
        </div>
        <div class="col-xs-12 col-sm-2"></div>
  </div>
</body>
</html>
