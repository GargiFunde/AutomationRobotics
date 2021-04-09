<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="DemoMasterPage2_AddTenant, App_Web_2o2bniex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    

  
   <script type="text/javascript">

       function openModal() {

           $('#AddNewTenant').modal('show');

       }
       function closeModal() {
           location.reload();
       }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


      <asp:ScriptManager runat="server">
                    </asp:ScriptManager>
     
    <!-- Begin Page Content -->
    <div id="page-wrapper" class="container-fluid">
        
                  
        <!-- Page Heading -->
        <%--<h1 class="h3 mb-2 text-gray-800">Add Tenant</h1>--%>
        
        <!-- DataTables Example -->
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

           
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h2 class="m-0 font-weight-bold poptxt">ADD TENANTS
                </h2>
            </div>
            <div class="card-body">
                <div>

                   
                            <div class="form-group">
                                <asp:Label ID="lblTenantName" runat="server" class="TableHeaderFont font-weight-bold poptxt">Tenant Name<span class="text-danger">*</span>&nbsp;&nbsp;</asp:Label>
                             <asp:TextBox ID="txtTenantName" class="form-control" runat="server"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lblOwner" class="TableHeaderFont font-weight-bold poptxt" runat="server">Owner<span class="text-danger">*</span>&nbsp;&nbsp;</asp:Label>
                             <asp:TextBox ID="txtOwner" class="form-control input-sm" runat="server"></asp:TextBox>
                            </div>

                            <div class="form-group" align="center">
                                <asp:Button ID="btnShow" runat="server" class="btn btn-primary colorSidebar" Text="Save" OnClick="Save_Click" Width="49%" />
                                <asp:Button ID="btnCancel" runat="server" class="btn btn-danger" Text="Clear" Width="49%" OnClick="btnCancel_Click"  />
                            </div>

                            <div class="form-group">
                            </div>
                            </div>
                </div>
            </div>
                  </br>
                 <div class="card shadow mb-4">
                    <div class="card-header py-3">
               <h2 class="m-0 font-weight-bold poptxt">TENANTS
                   <asp:ImageButton ID="ImageButton2" runat="server" Class="rotate refreshbtn " ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="ImageButton3_Command" />

                </h2>
            </div>
            <div class="card-body">
                <div>
                            <div class="panel-body table-responsive">
                                <asp:Repeater ID="GVRoles" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-bordered table-hover " id="dataTables-users" width="100%">
                                            <thead class="colorSidebar" align="center">
                                                <tr class="TableHeaderFont">
                                                    <th scope="col" class=" font-weight-bold poptxt">Tenant Id
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold poptxt">Tenant   Name
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold poptxt">Owner
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold poptxt">Created By
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold poptxt">Create Date
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold text-danger">Delete
                                                    </th>

                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="poptxtdata">
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="lblRolesID" name="RolesID" runat="server" Text='<%# Eval("tenantid") %>'  />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="lblRolesName" runat="server" Text='<%# Eval("tenantname") %>' />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("owner") %>' />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("createdby") %>' />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("createddate") %>' />
                                            </td>

                                            <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                                <asp:ImageButton ID="Delete" Text="Delete" runat="server" ImageUrl="~/Images/delete3.png" ImageAlign="Middle" CommandName="Delete" CommandArgument='<%#Eval("tenantid")+","+Eval("tenantname")%>' OnCommand="btnDelete_Click" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <div id="output1">
                                    <textarea id="output" cols="6" rows="10" hidden="hidden"></textarea>
                                </div>
                                <!-- /.table-responsive -->

                            </div>
                    </div>
                            <br />
   
                </div>
                <!-- /.panel -->



             </div>


                <div class="modal fade" id="AddNewTenant" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false" aria-labelledby="exampleModalLabel2" aria-hidden="true">
                                        <div class="modal-dialog modal-lg" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="m-0 font-weight-bold poptxt" id="exampleModalLabel2">Add New Tenant</h5>
                                                     <asp:Button runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close"   OnClick="btnXclosePopup"  ><%--data-dismiss="modal" aria-label="Close"--%> <%--OnClick="btnXclosePopup"--%>
                                                    </asp:Button>
                                                    <%--<button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">×</span>
                                                    </button>--%>
                                                </div>

                                                <div class="modal-body poptxt" style="align-content: center" runat="server" >
                                                    <div class="container">
                                                        <table width="100%" align="center">
                                                            <tr>
                                                                <td>
                                                                    <br />
                                                                </td>
                                                            </tr>

                                                            <tr style="height: 25px" >
                                                                <td align="left" style="text-align: right">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label8" runat="server"><b>User Name*&nbsp;&nbsp;</b></asp:Label>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtUser" class="form-control input-sm" runat="server"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                
                                                                <td align="right" style="text-align: right">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label9" runat="server"><b>Password*&nbsp;&nbsp;</b></asp:Label>
                                                                    </div>

                                                                </td>
                                                                <td>
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtPwd" class="form-control input-sm" runat="server" TextMode="Password"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="auto-style16"></td>

                                                            </tr>
                                                            <tr style="height: 25px">
                                                                <td align="left" class="auto-style14" style="text-align: right">

                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label10" runat="server"><b>Domain Name*&nbsp;&nbsp;&nbsp;</b></asp:Label>
                                                                    </div>

                                                                </td>
                                                                <td class="auto-style14">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtDomain" class="form-control input-sm" runat="server"  ReadOnly></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td align="left" class="auto-style14" style="text-align: right">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label11" runat="server"><b>RoleType*&nbsp;&nbsp;&nbsp;</b></asp:Label>
                                                                    </div>


                                                                </td>
                                                                <td class="auto-style14">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="DrpRoleType" runat="server"  class="form-control input-sm"  Text="Admin" ReadOnly> <%--AppendDataBoundItems="True"--%> 

                                                                            <%--<asp:ListItem Value="Admin">Admin</asp:ListItem>--%>
                                                                            <%--<asp:ListItem Value="Developer">Developer</asp:ListItem>
                                                    <asp:ListItem Value="Controller">Controller</asp:ListItem>
                                                    <asp:ListItem Value="Controller">Tester</asp:ListItem>--%>
                                                                        </asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="auto-style16"></td>
                                                            </tr>

                                                            <tr>
                                                                <td></td>
                                                                <td class="auto-style2" colspan="3">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="15%"></td>
                                                                            <td width="35%">
                                                                                <div class="form-group">
                                                                                    <asp:Button ID="Button1" runat="server" class="btn btn-primary colorSidebar" Text="Save Admin Details" OnClick="CreateNewUser" Width="160px" />
                                                                                    <%--<asp:Button ID="Button3" runat="server" class="btn btn-primary colorSidebar"  Text="Save" OnClick="Button1_Click" width="150px"/>--%>
                                                                                </div>
                                                                            </td>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                            <td width="35%">
                                                                                <div class="form-group">
                                                                                    <asp:Button ID="Button3" runat="server" class="btn btn-danger" Text="Clear" Width="150px"  OnClick="cancelPopup" /><%--data-dismiss="modal"--%>
                                                                               
                                                                                    </div>
                                                                            </td>
                                                                            <td width="15%"></td>
                                                                        </tr>
                                                                    </table>


                                                                </td>
                                                                <td></td>
                                                            </tr>


                                                        </table>




                                                    </div>

                                                    <div class="modal-footer">
                                                        <%-- <asp:Button ID="Button2" runat="server" class="btn btn-primary" Text="Save ZIP" align="center" Width="150px" OnClick="Button1_Click" />--%>

                                                        <%-- <button class="btn btn-danger" type="button" data-dismiss="modal">Cancel</button>--%>





                                                        <%--  <div class="panel-body" style="text-align:center">
                       
                                                          </div>--%>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>



                <div class="modal fade " id="modalDelete" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false" aria-labelledby="exampleModalLabel1" aria-hidden="true" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog modal-lg" role="document" style="width: 600px; height: 140px;">
                                <div class="modal-content ">
                                    <div class="modal-header">
                                        <h5 class="modal-title h4 font-weight-bold text-danger" id="exampleModalLabel1">Delete Tenant</h5>
                                       
                                         <asp:Button runat="server" data-dismiss="modal" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" ><%--data-dismiss="modal" aria-label="Close"--%> <%--OnClick="btnXclosePopup"--%>
                                                    </asp:Button>
                                        
                                    </div>

                                    <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                        Are You Sure You Want to <span class=" font-weight-bold text-danger">Delete ?</span>
                                        <br />
                                        <div class="form-group">

                                            <div class="container">
                                                <div class="row">
                                                    <br />
                                                </div>

                                                <div class="row">

                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                        Tenant Id:
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanIdDelete" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                    </div>
                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                        Tenant Name:
                                                    </div>
                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanNameDelete" runat="server"> </asp:Label>
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="row">
                                                <br />
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                </div>
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                    <asp:Button ID="btnDelete" runat="server" class="btn btn-danger btn-block" Text="Delete" OnClick="CommandBtnDelete_Click" />
                                                </div>
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                    <button class="btn btn-primary btn-block" type="button" data-dismiss="modal">Close</button>
                                                </div>
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                   


                
                        <div class="modal fade " id="modalDeleteSecondPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelSecondPopUp" aria-hidden="true" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog modal-lg" role="document" style="width: 900px; height: 140px;" >
                                <div class="modal-content ">
                                    <div class="modal-header">
                                        <h5 class="modal-title h4 font-weight-bold text-danger" id="exampleModalLabelSecondPopUp">Delete Tenant</h5>
                                      
                                         <asp:Button ID="Button6" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                                        <%-- <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">×</span>
                                        </button>--%>
                                    </div>

                                    <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                          You are about to delete the tenant permanently.Deleted tenant will not be recovered.<br />
                                                Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?

                                                <br />
                                        <br />


                                        <div class="form-group">

                                            <div class="container">
                                                <div class="row">
                                                    <br />
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                        Tenant Id:
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanIdDeleteSecondPopUp" runat="server"> </asp:Label>
                                                    </div>
                                                   <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                    </div>
                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                        Tenant Name:
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanNameDeleteSecondPopUp" runat="server"> </asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <br />
                                                </div>
                                                <div class="row">
                                                    <br />
                                                </div>
                                                <div class="modal-body h6  text-primary" style="text-align: center" runat="server">
                                                    <span class="h5 font-weight-bold text-danger">Counts</span>
                                                    <br />
                                                </div>


                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        Groups:
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblGroups" runat="server"> </asp:Label>
                                                    </div>

                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        Users:
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblUsers" runat="server"> </asp:Label>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        Bots :
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblBot" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        Processes :
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblProcess" runat="server"> </asp:Label>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        Process Uploads :
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblProcessUpload" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        RQ Details :
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblRQDetails" runat="server"> </asp:Label>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        Schedule Details :
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblScheduleDeatils" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        User Bot Mapping :
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblUserBotMapping" runat="server"> </asp:Label>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        Bot Default Queue :
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblBotdefaultqueue" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        Configuration Parameters :
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblconfigurationparameters" runat="server"> </asp:Label>
                                                    </div>

                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        Messaging Details :
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblMessagingdetails" runat="server"> </asp:Label>
                                                    </div>

                                                </div>

                                            </div>

                                        </div>

                                        <div class="row">
                                            <br />
                                        </div>


                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2">
                                                <asp:Button ID="Button2" runat="server" class="btn btn-danger btn-block" Text="Delete" OnClick="CommandBtnDelete_ClickSecondPopUp" />
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2">
                                                <asp:Button ID="btnEnableDisable" runat="server" class="btn btn-warning"  OnClick="CommandBtnDisable_Click"  />
                                                <%--<asp:Button ID="btnEnableDisable" runat="server" OnClick="CommandBtnDisable_Click" />--%>
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2">
                                              <asp:Button ID="Button5" runat="server" class="btn btn-primary btn-block" Text="Close" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                                            </div>
                                            <divclass="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                       
                           

                
                        <%-- Delete tentant 3  Second PopUp Modal Start--%>
                     
                        <div class="modal fade " id="modalDeleteThirdPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel3" aria-hidden="true">
                            <div class="modal-dialog modal-lg" role="document" style="width: 600px; height: 140px;">
                                <div class="modal-content ">
                                    <div class="modal-header">
                                        <h5 class="modal-title h4 font-weight-bold text-danger" id="exampleModalLabel3">Delete Tenant</h5>
                                        <asp:Button ID="Button7" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                                    </div>

                                    <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                        You are about to delete the tenant permanently.Deleted tenant will not be recovered.<br />
                                                Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?

                                                <br />
                                        <div class="form-group">

                                            <div class="container">
                                                <div class="row">
                                                    <br />
                                                </div>

                                                <div class="row">

                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                        Tenant Id:
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanIdDeleteThird" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                    </div>
                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                        Tenant Name:
                                                    </div>
                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanNameDeleteThird" runat="server"> </asp:Label>
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="row">
                                                <br />
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                </div>
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                    <asp:Button ID="Button4" runat="server" class="btn btn-danger btn-block" Text="Delete" OnClick="ModalPopUpBtnDelete_ClickThirdPopUp" />
                                                </div>
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                            <asp:Button ID="btnCloseSecondPopUp" runat="server" class="btn btn-primary btn-block" Text="Close" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                                                </div>
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                   
                        <%--End Delete  3 PopUp user--%>

                 </ContentTemplate>
        </asp:UpdatePanel>            
                        </div>

    
               
                        






            </div>
            <!-- /.col-lg-12 -->
        
    <!-- /#wrapper -->


    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script>
        $(document).ready(function () {
            $("#page-wrapper").delay(500).fadeIn(500);
            $('#dataTables-users').DataTable({
                responsive: true
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $(".rotate").click(function () {
                        $(this).toggleClass("down");
                    })
                    $("#page-wrapper").show();
                    $('#dataTables-users').DataTable({
                        responsive: true,
                        destroy: true
                    });

                }
            });
        };

        //$(".rotate").click(function () {
        //    $(this).addClass("down");
        //})


        function openModalDelete() {
            $('#modalDelete').modal('show');

        };





        function openModalDeleteSecondPopUp() {
            $('#modalDeleteSecondPopUp').modal('show');

        };
        function openModalDeleteThirdPopUp() {
            $('#modalDeleteThirdPopUp').modal('show');

        };
        function openModalHideDelete() {
            $('body').removeClass().removeAttr('style'); $('.modal-backdrop').remove();
            //$("#modalDeleteSecondPopUp").modal('hide');

        }

    </script>
</asp:Content>

