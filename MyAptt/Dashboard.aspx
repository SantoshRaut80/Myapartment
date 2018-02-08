<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <meta name="viewport" content="width=device-width, initial-scale=1"/>
            <!-- Latest compiled and minified CSS -->
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"/>

            <!-- jQuery library -->
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

            <!-- Latest compiled JavaScript -->
            <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
           
           <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css"/>
       
           <script src="Scripts/jquery-1.11.1.min.js"></script>

     
        <link rel="stylesheet" href="CSS/ApttTheme.css" />
    <link rel="stylesheet" href="CSS/ApttLayout.css" />
   
    <style>
         .dashboard_box{
       margin-top:1%;
       top:70px; 
      padding-left:5px;
       cursor:pointer;
       border-radius:5px;
       vertical-align:central;
       overflow:hidden;
            
     }
         .dashboard_box:hover{
             box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.2), 0 5px 10px 0 rgba(0, 0, 0, 0.19);
         }

         .dashboard_box p{
           text-align:center;
         }

        .dashboard_box label{
            align-self:initial;
            margin-left:5px;
            padding-left:5px;
            text-overflow: ellipsis;
        }
    </style>
    <script>

       
    
    </script>

</head>
<body>
      <div class="container-fluid">
        <div class="row">
            <div class="col-xs-12 col-sm-10">
    <form id="form1" runat="server">
            
    <div class="container-fluid"  style="margin-top:10px; margin-left:10px;">
        <div class="row" style="margin-left:0px;margin-right:0px;">
            <div class="col-md-4 hidden-xs" style="margin-top:5px;">
                
                     <div class="dashboard_box" onclick="location.href='MyFlat.aspx'" style="background-color:#00bfff;height:150px;">
                     <p class="heading_dark  font_size_2" style="">Society in Brief</p>
                       <asp:Label ID="lblFlatInfo" runat="server"></asp:Label>
                
               </div>  
            </div>
            <div class="col-md-4 col-xs-6" style="margin-top:10px;">
                   <div class="dashboard_box" onclick="location.href='BillPayments.aspx'" style="background-color:#d9534f;height:150px; color:white;">
                        <p class="heading_light font_size_2" >My Bills</p>
                       <asp:Label ID="lblpending" runat="server" ></asp:Label>
                        </div>
            </div>
            <div class="col-md-4 col-xs-6" style="margin-top:10px;">
                <div class="dashboard_box" onclick="location.href='ViewComplaints.aspx'" style="background-color:#5cb85c;height:150px; color:white;">
                   <p class="heading_light  font_size_2">Complaints in Current Month</p> 
                    <table style="width:100%;">
                        <tr>
                            <td style="width:60%;">
                            <asp:Label ID="lblComplaintInfo" runat="server"></asp:Label>
                            </td>
                            <td style="width:40%;">
                                <img src="Images/Icon/Complaint_1.png"/>
                            </td>
                        </tr>
                    </table>
                
                    </div>
            </div>
    
               <div class="col-md-4 col-xs-6" style="margin-top:10px;">
                   <div class="dashboard_box" onclick="location.href='Discussions.aspx'" style="background-color:#6eab91;height:150px; ">
                       <p class="heading_dark  font_size_2" >Top Trending</p> 
                       <asp:Label ID="lblTopForum"  runat="server"></asp:Label> 
              
                       </div>
               </div>
               <div class="col-md-4 col-xs-6" style="margin-top:10px;">
                   <div class="dashboard_box" onclick="location.href='Notifications.aspx'" style="background-color:#f0ad4e;height:150px; color:white;">
                       <p class="heading_light font_size_2">Notice</p> 
                       <asp:Label ID="lblTopNotice"  runat="server"></asp:Label>
                   
                           </div>
               </div>
               <div class="col-md-4 hidden-xs" style="margin-top:10px;">
                                     <div class="dashboard_box" onclick="location.href='ViewComplaints.aspx'" style="background-color:violet; height:150px;">
                                                <asp:Chart ID="BarChart" runat="server" Height="130px" Width="200px" BackColor="Violet">
                                                       <Titles><asp:Title Font="15" Text="Complaints Age"></asp:Title></Titles>
                                                  <Series>
                                                      <asp:Series Name="Series1" ChartType="Bar"></asp:Series>
                                                  </Series>
                                                  <ChartAreas>
                                                      <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                                                  </ChartAreas>
                                              </asp:Chart>
                   <asp:Label ID="lblEmptyDataText" runat="server" ForeColor="#999999" ></asp:Label>
                   </div>               
               </div>
          
               <div class="col-md-4 col-xs-6" style="margin-top:10px;">
                   <div class="dashboard_box" onclick="location.href='Poll.aspx'" style="background-color:cadetblue; height:150px; color:white;margin-top:10px;">
                    <p class="heading_dark font_size_2">Latest Polls</p>
                   <asp:Label ID="lblTopPoll" runat="server"></asp:Label>
               </div>
                    </div>
               <div class="col-md-4 col-xs-6" style="margin-top:10px;">
                   <div class="dashboard_box" onclick="location.href='Vendors.aspx'" style="background-color:ButtonFace; height:150px; color:black;margin-top:10px;">
                       <p class="heading_dark font_size_2">Latest From Vendor </p>
                       <asp:Label ID="lblOffer" runat="server"></asp:Label>
                    
               </div>
                   </div>
               <div class="col-md-4 hidden-xs" style="margin-top:10px;">
                  
                   <div class="dashboard_box" onclick="location.href='ViewComplaints.aspx'" style="background-color:#a07e7e; height:160px; vertical-align:middle; margin-top:10px;">
                                            <asp:Chart ID="Piechart" runat="server" Height="150px" Width="200px" BackColor="YellowGreen" >
                                                <Titles><asp:Title Font="large" Text="Complaint Distribution"></asp:Title></Titles>
                                                              <Series>
                                                               <asp:Series Name="Series1" ChartType="Pie"  LegendText="#VALX" ChartArea="ChartArea1" Legend="Legend1" CustomProperties="PieDrawingStyle=SoftEdge" MarkerSize="0" MarkerStyle="Circle">
                                                               </asp:Series>
                                                             </Series>
                                                            <Legends>  
                                            <asp:Legend Alignment="Center" Docking="Left" IsTextAutoFit="False" Name="Legend1"  BackColor="YellowGreen" 
                                                LegendStyle="Column" />  
                                        </Legends>

                                                           <ChartAreas>
                                            
                                                              <asp:ChartArea Name="ChartArea1" BackColor="YellowGreen">
                                                              </asp:ChartArea>   
                                                           </ChartAreas>
                                            </asp:Chart>
                        <asp:Label ID="lblpiechart" runat="server" CssClass="lblpiechartText" Text=""></asp:Label>
               </div>
                </div>
           </div>
    
    </div>
  
    </form>
                </div>
            <div class="col-xs-12 col-sm-2">
                  <div ></div>
                </div>
            </div>
   </div>
</body>
</html>
