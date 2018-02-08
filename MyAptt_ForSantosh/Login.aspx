<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MyAptt</title>
    <meta name="viewport" content="width=device-width, maximum-scale=1, initial-scale=1, user-scalable=0"/>
    <meta name="description" content="The MyAptt System is developed considering the day to day needs of society like complaints, Notification, social communication as well as managing the Residents, Vendors, and Employees Data of a society. Application caters the need of small to large societies and also provide the customization to meet your specific needs."/>
    <meta name="keywords" content="Society Management,Residential Society Management,Complaint Management,Society Expenses,Billing Software"/>
    <meta name="developer" content="Anvisys Technologies Pvt. Ltd."/>

            <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"/>
        <!-- CORE CSS -->

      <!-- Latest compiled and minified CSS -->
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"/>

            <!-- jQuery library -->
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

            <!-- Latest compiled JavaScript -->
            <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>


        <link href="Login/css/settings.css" rel="stylesheet" type="text/css"/>
        <!-- THEME CSS -->

        <link href="Login/css/essentials.css" rel="stylesheet" type="text/css"/>
        <link href="Login/css/layout.css" rel="stylesheet" type="text/css"/>
        <link href="Login/css/layout-responsive.css" rel="stylesheet" type="text/css"/>

        <link href="Styles/layout.css" rel="stylesheet" />
        <link href="Styles/Responsive.css" rel="stylesheet" />

    <link rel="stylesheet" href="CSS/ApttTheme.css" />
  <link rel="stylesheet" href="CSS/ApttLayout.css" />
   
    <script>


        $(function () {
            $("[id*=submitbutton]").bind("click", function () {
                var user = {};
                user.Username = $("[id*=TxtUserID]").val();
                user.Password = $("[id*=txtPwd]").val();
                $("#txtPwd").val("");
                $("#Forgotpass").hide();
                $.ajax({
                    type: "POST",
                    url: "Login.aspx/ValidateUser",
                    data: '{user: ' + JSON.stringify(user) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response.d == true) {
                            window.location = "MainPage.aspx";
                        }
                        if (response.d == false) {
                         
                            $('#lblerror').text("Login Failed");
                            $('#lblerror').show();
                            $('#lblPasswordRes').hide();
                            $("#Forgotpass").show();
                        }
                    }
                });
                return false;
            });
        });

        if (top.location != self.location) {
            top.location = self.location.href
        }

       <%-- function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblMessage.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };--%>

        $(document).ready(function () {
           
            $("#login").click(function () {           
                $("#login_div").show();
                $("#Forgot_div").hide();
            });
        });

        $(document).ready(function () {
            $(".close").click(function () {
                $("#login_div").hide();
                $("#Forgot_div").hide();
            });
        });


        $(document).ready(function () {
            $("#Forgotpass").click(function () {
                $("#login_div").hide();
                $("#Forgot_div").show();
               
            });

            $("#GoLogin").click(function () {
                $("#login_div").show();
                $("#Forgot_div").hide();

            });

            $("#btnForgotpass").click(function () {
                $("#Forgot_div").show();
                $("#login_div").hide();
            })

        });

        $(document).ready(function () {
            $("#close").click(function () {
                $("input:text").val("");
                $("#txtPwd").val("");
                $("#lblerror").html("");

            });
        });


        $(document).ready(function () {

           $("#txtPwd").on('input', function () {
                $("#lblerror").html("");
            });
                
            
        });

        $(document).keypress(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                $('#submitbutton').trigger('click');
            }
        });

        $(document).ajaxStart(function () {
            $('.dvLoading_first').show();
        }).ajaxStop(function () {
            $('.dvLoading_first').hide();
        });


        if (top.location != self.location) {
            top.location = self.location.href
        }

    </script>

    <style>
        img.close{
            position:absolute;
            right:-14px;
            top:-14px;
            cursor:pointer;
        }

        .bosre{padding: 8px 0 0 0;
    border: solid 2px #6eab91 ;
    border-radius: 50%;
    height: 78px;
    width: 78px;
    display: inherit;
    margin: 0 0 12px 0;}

        .Forgot_button{

        background-color:transparent;
        height:30px;      
        color:white;
        font-family:'Times New Roman', Times, serif;
        font-style:oblique;
        font-weight:bold;
         outline:0;
         display:none;
	}

        .forgot_pass{

            color:#6eab91;
            text-decoration:none;
            padding:1% 0 2% 0;
        }
        .forgottxt{
            padding:1% 2%
        }

        a{
            color:white;
        }

        .dvLoading_first
        {
            display:none;
           background: url(Images/Icon/ajax-loader.gif) no-repeat center center;
           opacity: 0.5;
           height: 50px;
           width: 50px;         
           z-index: 1000;                   
           border:0;
           outline:0;
           border-style:none;
           border-color:white;
           border-radius:5px;
           text-align:center;
        }
    </style>

</head>
<body>
        
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>    
        <%--Login Popup --%>
      
        <div class="login_div" id="login_div" style="padding:0% 0 3% 0;">
        <img id="close" class="close" src="Images/Icon/close.png" width="25" height="25"  style="background-size:contain;"/> <br />     
            <div style="padding:0 1% 3% 1%;color:#6eab91; font-family:'Baskerville Old Face';font-size:x-large;">
                <img src="Images/Icon/login_icon.png"  width="25" height="25"/>
                <asp:Label ID="lbltext" runat="server" Text="Account Login "></asp:Label>              
              <h4 style="margin-bottom:0;"> 
                  <asp:Label ID="lblPasswordRes" runat="server" Text="" Font-Size="small" ForeColor="#ff8635"></asp:Label></h4> 

            </div>
           
       <asp:TextBox ID="TxtUserID" runat="server" CssClass="login_txtbox" placeholder="Username/MobileNumber"   BorderStyle="None"  TabIndex="1" ></asp:TextBox><br /><br />
         <asp:TextBox ID="txtPwd" runat="server" CssClass="login_txtbox"  placeholder="Password"  TextMode="Password" BorderStyle="None"  TabIndex="2"></asp:TextBox><br /><br />
               <div id="Login_background" style="padding:0.5% 0% 0.5% 0%;background-color:#6eab91;">
                    <button  type="button" id="submitbutton" style="width:200px;height:30px;color:white;" tabindex="3"> Submit </button>                
               </div>
               <div>
                   <img  class="dvLoading_first" src="Images/Icon/ajax-loader.gif" style="border:0;"/>  
               </div>

              <asp:Label ID="lblerror" runat="server" Text="" CssClass="lblerror"></asp:Label><br />
              <a href="#" id="Forgotpass" class="forgot_pass" tabindex="4">Forgot Password</a><br />
        </div>     
          <%--Login Popup --%>

          <%--Forgot Popup --%>

        <div class="login_div" id="Forgot_div" style="display:none;">
        <img id="closed" class="close" src="Images/Icon/close.png" width="25" height="25"  style="background-size:contain;"/> <br />
       
            <div style="padding:0 1% 3% 1%;color:#ff6a00; font-family:'Baskerville Old Face';font-size:x-large;">
                <img src="img/login_icon.png"  width="25" height="25"/>
                <asp:Label ID="Label1" runat="server" Text="Forgot Password "></asp:Label><br />

                <asp:Label ID="Label2" runat="server" ForeColor="#666666" Font-Size="Small"  CssClass="forgottxt" Text="We will send  your New password to  your Email  which  is  associated with this UserId."></asp:Label>
            </div>
           
       <asp:TextBox ID="txtForgotText" runat="server" CssClass="login_txtbox" onfocus="if (this.value == 'Username or Email') this.value = '';" onblur="if (this.value == '') this.value = 'Username or Email';" value="Username or Email" BorderStyle="None"></asp:TextBox><br /><br />
              <div id="Login_backgroundd" style="padding:2% 1% 2% 1%;background-color:#f19e64;">                 
                   <asp:Button ID="btnForgotpass" runat="server" Text="Reset Password" CssClass="login_Submit"  OnClick="btnForgotpass_Click"   BorderStyle="None"/><br />                                            
               </div>

              <asp:Label ID="lblEmailerror" runat="server" Text="" CssClass="lblerror"></asp:Label>
              <a href="#" id="GoLogin" class="forgot_pass" >Go to Login</a><br />
    <asp:Label ID="lblres" runat="server"></asp:Label>


        </div>

          <%--Forgot  Popup Ends --%>

 </form>

   <header id="topNav" class="layout_header " style="height: 81px; background-color:#3f3f3f; color:white;">

  <div class="container" > 
    
    <!-- Mobile Menu Button -->
    <button class="btn btn-mobile" data-toggle="collapse" data-target=".nav-main-collapse" style="margin-top: 8px;"> <i class="fa fa-bars"></i> </button>
    
    <!-- Logo text or image --> 
    <a class="logo" href="#" style="margin-top: 0px; line-height: 30px; float:left;"> <img src="Images/Icon/logo_1.gif" width="50" height="50" alt=""/> </a> 
      <div class="title" style="color:white;padding-top:1%;left:0;right:0;float:left;font-size:x-large;padding-left:5%;"> Society Management System</div>
    <!-- Top Nav -->
    <div class="navbar-collapse nav-main-collapse collapse pull-right" style="margin-top: 16px;color:white;">

  
      <nav class="nav-main mega-menu">
        <ul class="nav nav-pills nav-main scroll-menu" id="topMain">
          <li class=" active"><a style="color:white;" href="#">Home</a></li>
          <li class=" "><a style="color:white;"  href="#">About Us</a></li>
       
          <li class=" "><a  style="color:white;" href="#">Contact </a></li>
          <li class=" "><a  style="color:white;" href="#" id="login">Login</a></li>
          <!-- GLOBAL SEARCH -->
          <li class="search dropdown" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="true"  style="display:none;"> 
            
            <!-- search form -->
            <ul class="dropdown-menu">
              <li>
                <form method="get" action="#" class="input-group pull-right" style="">
                  <input type="text" class="form-control" name="k" id="k" value="" placeholder="Search"/>
                  <span class="input-group-btn">
                  <button class="btn btn-primary notransition" style="margin-top: 8px;"><i class="fa fa-search"></i></button>
                  </span>
                </form>
              </li>
            </ul>
            
            <!-- /search form -->
            
        </ul>
      </nav>
      
    </div>
    <!-- /Top Nav --> 
    
  </div>
</header>

<!-- WRAPPER -->
    <span  itemtype="http://schema.org/SoftwareApplication"/>
<div id="wrapper" style="padding-top: 10px; margin-top: 0px;background-color:white;"> 
  
  <!-- REVOLUTION SLIDER -->
  
  <div class="container" style="padding:50px 0;background-color:white;">
      <div id="myCarousel" class="carousel slide" data-ride="carousel">
          <!-- Indicators -->
          <ol class="carousel-indicators">
              <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
              <li data-target="#myCarousel" data-slide-to="1"></li>
              <li data-target="#myCarousel" data-slide-to="2"></li>
          </ol>

          <!-- Wrapper for slides -->
          <div class="carousel-inner" role="listbox">
              <div class="item active">
                  <img src="Images/Static/slider1.jpg" alt=""/>
                  <div class="carousel-caption"> Better Connectivity</div>
              </div>
              <div class="item">
                  <img src="Images/Static/slider2.jpg" alt=""/>
                  <div class="carousel-caption"> Coordinated Activities </div>
              </div>
              <div class="item">
                  <img src="Images/Static/slider3.png" alt=""/>
                  <div class="carousel-caption"> Managed Societies </div>
              </div>
          </div>

          <!-- Controls -->
          <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
              <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span> <span class="sr-only">Previous</span>
          </a>
          <a class="right carousel-control" href="#myCarousel" role="button" data-slide="next">
              <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span> <span class="sr-only">Next</span>
          </a>
      </div>
  </div>
  <!-- /REVOLUTION SLIDER -->
  
  <section class="goletfullwidth text-center">
    <div class="container">
      <h1 class="text-center"> <span class="">Meets all your Society Management needs</span> </h1>
      <div class="item-box box"> <span class="bosre"> <span class="fa fa-user fontsize theme_third_main" aria-hidden="true"></span></span>
        <h5>Owners/ Residents records</h5>
      </div>
      <div class="item-box box"> <span class="bosre"> <span class="fa fa-cubes fontsize theme_third_main" aria-hidden="true"></span></span>
        <h5>Complaints / Ticket Management</h5>
      </div>
      <div class="item-box box"> <span class="bosre"> <span class="fa fa-external-link-square fontsize theme_third_main" aria-hidden="true"></span></span>
        <h5>Internal Forum Discussion</h5>
      </div>
      <div class="item-box box"> <span class="bosre"> <span class="fa fa-bell-o fontsize theme_third_main" aria-hidden="true"></span></span>
        <h5>Notifications</h5>
      </div>
    </div>
  </section>
  <div class="container">
    <hr class="half-margins">
    <!-- hr line --> 
  </div>
  
  <!-- PARALLAX -->
  <section class="parallax delayed margin-footer parallax-init" data-stellar-background-ratio="0.7"> <span class="overlay"></span>
    <div class="container">
      <div class="row"> 
        <!-- left content -->
          <div class="col-md-7 animation_fade_in">
              <h2>My Apartment</h2>
              <p class="lead"> My Apartment is developed by Anvisys Technologies  for the Apartment Societies. </p>
              <p> The MyAptt System is developed considering the day to day needs of society like complaints, Notification, social communication as well as managing the Residents, Vendors, and Employees Data of a society. Application caters the need of small to large societies and also provide the customization to meet your specific needs. </p>
              <p> <small><em> We provide you access to the common application deployed at cloud, can host a specific instance on you chosen web hosting environment and also give you the application as product with your complete control.</em></small> </p>

              <p> <small><em>   The system supports Web environment for Administrator (RWA etc.) and for employee and residents as well as android application for easy accessibility by residents. So why wait please contact us to join the numerous societies using this interface or try the trial version………….</em></small> </p>
              <a class="btn theme-btn-primary btn-lg" href="#"> More</a>
          </div>
               
        <!-- right image -->
        <div class="col-md-5 animation_fade_in"> <img class="visible-md visible-lg img-responsive pull-right" src="Images/Static/desktop2.png" alt=""/> </div>
      </div>
    </div>
  </section>
  <!-- PARALLAX -->
  <div class="clearfix" style="margin-top:100px;"></div>
  <hr class="half-margins">
  <!-- hr line -->
  <section class="container">
    <div class="row">
      <div class="col-md-3">
        <div class="featured-box nobg border-only" style="text-align:center;"> <span class="bosre"> <span class="fa fa-users fontsize" aria-hidden="true" style="color:#6eab91;"></span></span>
        <h4>For Everyone</h4>
          <p>The System has access for Residents, Employee, RWA and Administrator</p>
      </div>
        </div>
     
      <div class="col-md-3">
        <div class="featured-box nobg border-only left-separator" style="text-align:center;"><span class="bosre" style="align-self:center;"> <span class="fa fa-book fontsize theme_third_main" aria-hidden="true"></span></span> 
          <h4>Well Documented</h4>
          <p>Keep the data well organized and well recorded</p>
        </div>
      </div>
      <div class="col-md-3">
        <div class="featured-box nobg border-only left-separator"><span class="bosre"><span class="fa fa-trophy fontsize theme_third_main" aria-hidden="true"></span></span>
          <h4>For Your Business</h4>
          <p>Maintian interation among residents, Maintainence activities, Maintain Billing</p>
        </div>
      </div>
      <div class="col-md-3">
        <div class="featured-box nobg border-only left-separator"><span class="bosre"><span class="fa fa-cogs fontsize theme_third_main" aria-hidden="true"></span></span>
          <h4>Highly Customizable</h4>
          <p>We belive in evalution and continuous improvement, hence open for customization</p>
        </div>
      </div>
   </div>
  </section>
  <div class="clearfix"></div>
  <section class="parallax delayed margin-footer parallax-init" data-stellar-background-ratio="0.7" style="background:#fff"> <span class="overlay"></span>
    <div class="container">
      <div class="row"> 
        <!-- left content -->
        <div class="col-md-5 animation_fade_in"> <img class="visible-md visible-lg img-responsive pull-right" src="img/desktop_slider_2.png" alt=""> </div>
        <div class="col-md-7 animation_fade_in">
          <h2 itemprop="applicationCategory">Society Management System</h2>
          <p class="lead"> Society Management system is developed by 
              <span itemprop="author" itemtype="http://schema.org/Person">
                  <span itemprop="name">Anvisys Technologies </span> </span></p>
          <p> Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas metus nulla, commodo a sodales sed, dignissim pretium nunc. Nam et lacus neque. Ut enim massa, sodales tempor convallis et, iaculis ac massa. </p>
          <p> <small><em> Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas metus nulla, commodo a sodales sed, dignissim pretium nunc.</em></small> </p>
          <a class="btn btn-lg theme-btn-primary" href="#"> More</a> </div>
        
        <!-- right image --> 
        
      </div>
    </div>
  </section>
</div>
<!-- /WRAPPER --> 

<!-- FOOTER -->
<footer> 
  
  <!-- copyright , scrollTo Top -->
  <div class="footer-bar">
    <div class="container"> 
    <span class="copyright">Copyright © <span itemprop="publisher" itemtype="http://schema.org/Organization">
        <span itemprop="name">Anvisys Technologies</span></span>,  All Rights Reserved.</span> <a class="toTop" href="#">BACK TO TOP <i class="fa fa-arrow-circle-up"></i></a> </div>
  </div>
  <!-- copyright , scrollTo Top --> 
  
  <!-- footer content -->
  <div class="footer-content">
    <div class="container">
      <div class="row"> 
        
        <!-- FOOTER CONTACT INFO -->
        <div class="column col-md-4">
          <h3>CONTACT</h3>
          <p class="contact-desc"> Company you will be able to create an awesome website in a very simple way. </p>
          <address class="font-opensans">
          <ul>
            <li class="footer-sprite address"> PO Box 201301<br>
             G-272,Sector-63<br>
              Noida, Uttar pradesh<br>
            </li>
            <li class="footer-sprite phone"> Phone: 8588000868  </li>
            <li class="footer-sprite email"> <a href="#">enquiry@anvisys.net</a> </li>
          </ul>
          </address>
        </div>
        <!-- /FOOTER CONTACT INFO --> 
        
        <!-- FOOTER LOGO -->
        <div class="column logo col-md-4 text-center">
          <div class="logo-content" style="display:none;"> 
              <img class="animate_fade_in" src="img/logo_footer.png" width="200" alt="" style="opacity: 1; right: 0px;">
            <h4>Company TEMPLATE</h4>

          </div>
        </div>
        <!-- /FOOTER LOGO --> 
        
        <!-- FOOTER LATEST POSTS -->
        <div class="column col-md-4 text-right">
          <h3>RECENT POSTS</h3>
          <div class="post-item"> <small>JANUARY 2, 2014 BY ADMIN</small>
            <h3><a href="#">Lorem ipsum dolor sit amet, consectetur adipiscing elit</a></h3>
          </div>
          <div class="post-item"> <small>JANUARY 2, 2014 BY ADMIN</small>
            <h3><a href="#">Lorem ipsum dolor sit amet, consectetur adipiscing elit</a></h3>
          </div>
          <div class="post-item"> <small>JANUARY 2, 2014 BY ADMIN</small>
            <h3><a href="#">Lorem ipsum dolor sit amet, consectetur adipiscing elit</a></h3>
          </div>
          <a href="#" class="view-more pull-right">View Blog <i class="fa fa-arrow-right"></i></a> </div>
        <!-- /FOOTER LATEST POSTS --> 
        
      </div>
    </div>
  </div>
  <!-- footer content --> 
  
</footer>
  
</body>
</html>
