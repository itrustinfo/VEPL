<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProjectManagementTool.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>Login</title>
     <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <link href="_assets/images/favicon.png" rel="icon" />

    <link rel="stylesheet" href="_assets/styles/style.css" />

</head>
<body>
                <div class="container-scroller">
    <div class="container-fluid page-body-wrapper full-page-wrapper auth-page">
      <div class="content-wrapper d-flex align-items-center auth auth-bg-1 theme-one">
        <div class="row w-100">
            
          <div class="col-lg-8 mx-auto">
              
            <div class="auto-form-wrapper">
                <div style="width:100%">
                <div style="text-align:left;float:left;width:30%;padding-top:15px" id="VEPL" runat="server">
                    <img src="_assets/images/viswaraj _logo.jpg" height="50" width="150" alt="VEPL" /><br />
                    <span style="vertical-align:middle; color:#0521AC; font-size:large;">M/s Vishvaraj Environment Pvt Ltd</span>
                </div>
                <div style="text-align:center;float:left;width:40%;padding-left:20px" runat="server">
                    <asp:Image ID="sLogo" runat="server" />
                    <span class="font-weight-bold" style="vertical-align:middle; color:#0521AC; font-size:xx-large;"><asp:Label ID="LblTitle" runat="server"></asp:Label></span>
                    <h5 style="text-align:center; margin-top:10px;"><asp:Label ID="LblDescription" runat="server"></asp:Label></h5>
                
                </div>
                    <div style="text-align:left;float:left;width:10%;margin-top:-20px">&nbsp;</div>
                 <div style="text-align:center;float:left;width:20%;vertical-align:top" id="Div1" runat="server">
                    <img src="_assets/images/Picture5.png" height="120" width="130" alt="BWSSB" />
                   <span style="vertical-align:middle; color:#0521AC; font-size:large;">BWSSB</span>
                </div>
                <div style="float:none;clear:both;width:100%">&nbsp;</div>
                </div>
                
               
              <form id="form2" runat="server">
                  <div style="background-color:#ccf5ff;border-radius:10px;padding:35px;margin-bottom:35px">
                   <h3 style="margin-top:0; color:#0e31ce;font-size:xx-large" >Login here</h3>
                <hr />
                <div class="form-group">
                  <label style="font-family:Segoe UI, Tahoma, Geneva, Verdana, 'sans-serif';font-size:larger">Username</label>
                  <div class="input-group">
                    <input type="text" class="form-control" id="txtusername" autocomplete="off" runat="server" required placeholder="Username" style="font-family:Segoe UI, Tahoma, Geneva, Verdana, 'sans-serif';font-size:larger"/>
                    <div class="input-group-append">
                      <span class="input-group-text">
                        <i class="mdi mdi-check-circle-outline"></i>
                      </span>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                  <label style="font-family:Segoe UI, Tahoma, Geneva, Verdana, 'sans-serif';font-size:larger" >Password</label>
                  <div class="input-group">
                    <input type="password"  class="form-control" id="txtpassword" runat="server" required placeholder="password" style="font-family:Segoe UI, Tahoma, Geneva, Verdana, 'sans-serif';font-size:larger"/>
                    <div class="input-group-append">
                      <span class="input-group-text">
                        <i class="mdi mdi-check-circle-outline"></i>
                      </span>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" BackColor="#0521AC" CssClass="btn btn-primary submit-btn btn-block" OnClick="btnLogin_Click" />
                    <%--<button class="btn btn-primary submit-btn btn-block">Login</button>--%>
                </div>
                <div class="form-group d-flex justify-content-between">
                  <div class="form-check form-check-flat mt-0">
                    <asp:Label ID="Label1" runat="server" ForeColor="blue" Text=""></asp:Label>
                      <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    
                  </div>
                  <a href="ForgotPassword.aspx" style="color:#0e31ce;" class="text-black" >Forgot Password</a>
                </div>
                      </div>
                <div class="form-group d-flex justify-content-between">
                  <div class="form-check form-check-flat mt-0" style="text-align:center">
                    <asp:Label ID="Label2" runat="server" ForeColor="blue" Font-Bold="true" Text="Site Design and Concept by NJSEI MIS Team"></asp:Label>
                     
                    
                  </div>
                   <asp:Label ID="Label3" runat="server" ForeColor="blue" Font-Bold="true" Text="Powered by "><asp:Image runat="server" ImageUrl="~/_assets/images/itrust_ logo.jpg" Height="40" Width="120"></asp:Image></asp:Label>
                     
                </div>
              </form>
            </div>
        
            <%--<p class="footer-text text-center">copyright © 2018 Bootstrapdash. All rights reserved.</p>--%>
          </div>
        </div>
      </div>
      <!-- content-wrapper ends -->
    </div>
    <!-- page-body-wrapper ends -->
  </div>
</body>
</html>
