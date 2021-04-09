<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogIn.aspx.cs" Inherits="DemoMasterPage2_LogIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta charset="utf-8" />
  <meta http-equiv="X-UA-Compatible" content="IE=edge" />
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
  <meta name="description" content="" />
  <meta name="author" content="" />

  <title>
      
      <asp:Literal Id = "LitE2EBOTSLogin" runat="server" Text="<%$Resources:content,LitE2EBOTSLogin%>">

      </asp:Literal>  </title>

   <%-- SweetAlert JavaScript--%>
   <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@8"></script>

  <!-- Custom fonts for this template-->
  <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
  <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet" />

  <!-- Custom styles for this template-->
  <link href="css/sb-admin-2.min.css" rel="stylesheet" />
    <script type="text/javascript">

        window.onload = function () {
          
            $('div[id$=cover-spin]').hide();
        };

      
        </script>
</head>
<body style="background-color:#0275d8" class="bg-gradient-primary">
   <%-- <form id="form1" runat="server">--%>
        <div>
      <div id="cover-spin"></div>      
  <div class="container">

    <!-- Outer Row -->
    <div class="row justify-content-center">

      <div class="col-xl-10 col-lg-12 col-md-9">

        <div class="card o-hidden border-0 shadow-lg my-5">
          <div class="card-body p-0">
            <!-- Nested Row within Card Body -->
            <div class="row">
              <div class="col-lg-6 d-none d-lg-block bg-login-image"></div>
              <div class="col-lg-6">
                  <div align="center">
                        <%--<img src="../Images/E2EFace.png" width="60%">--%><br/>
                        <img src="../Images/E2ELogo.png" width="40%" />
                    </div>
                <div class="p-3">
                  <div class="text-center">
                      <h1 class="h4 text-gray-900 mb-4 font-weight-bold">
                          <asp:Literal ID="LitWelcomeBack" runat="server" Text="<%$Resources:content,LitWelcomeBack%>"></asp:Literal>

                      </h1>
                  </div>
                  <form class="user" runat="server">
                    <div class="form-group">
                        <asp:TextBox ID="txtUserName" style="font-size:medium" type="text" runat="server" CssClass="form-control form-control-user font-weight-bold" AutoPostBack="true" OnTextChanged="txtPassWord_TextChanged" placeholder="<%$Resources:content,LitDomainNameUserName%>"></asp:TextBox>
                       </div>
                    <div class="form-group">
                       <asp:TextBox ID="txtPassWord" style="font-size:medium" type="password" runat="server" CssClass="form-control form-control-user font-weight-bold"  placeholder="<%$Resources:content,LitPassword%>"></asp:TextBox>
                    </div>

                  <div class="form-group">
                      <asp:TextBox ID="txtTenantName" style="font-size:medium" runat="server" CssClass="form-control form-control-user font-weight-bold" placeholder="<%$Resources:content,LitTenantName%>"></asp:TextBox>
                  </div>

                    <div class="form-group">
                        <asp:TextBox ID="txtGroupName" style="font-size:medium" runat="server" CssClass="form-control form-control-user font-weight-bold" placeholder="<%$Resources:content,LitGroupName%>"></asp:TextBox>
                        </div>

                    <div class="form-group">
                      <div class="custom-control custom-checkbox small">
                        <input type="checkbox" class="custom-control-input" id="customCheck" name="customCheck" value="IsChecked">
                        <label class="custom-control-label font-weight-bold " style="font-size:medium" for="customCheck">
                          <asp:Literal Id = "LitRememberMe" runat="server" Text="<%$Resources:content,LitRememberMe%>"></asp:Literal>    

                        </label>
                      </div>
                    </div>
                      <asp:Button ID="btnSubmit" class="btn btn-primary btn-user btn-block font-weight-bold"  runat="server" Text="<%$Resources:content,LitLogin%>" OnClick="btnLogin_Click" />
                  </form>
                  <hr>
                  <div class="text-center">
                    <a class="small" href="forgot-password.html">
                      <asp:Literal Id = "LitForgotPassword" runat="server" Text="<%$Resources:content,LitForgotPassword%>"></asp:Literal>  

                    </a>
                  </div>
                  <div class="text-center">
                    <a class="small"<%-- href="register.html"--%>  >
                        <asp:Literal Id = "LitCreateanAccount" runat="server" Text="<%$Resources:content,LitCreateanAccount%>"></asp:Literal> </a>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

      </div>

    </div>

  </div>


  <!-- Bootstrap core JavaScript-->
  <script src="vendor/jquery/jquery.min.js"></script>
  <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

  <!-- Core plugin JavaScript-->
  <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

  <!-- Custom scripts for all pages-->
  <script src="js/sb-admin-2.min.js"></script>
        </div>
   <%-- </form>--%>
</body>
</html>
