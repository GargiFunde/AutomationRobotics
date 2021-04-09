<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="DemoMasterPage2_AddUsers, App_Web_2o2bniex" enabletheming="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <!-- Begin Page Content -->
    <div class="container-fluid">
        <div id="page-wrapper">
            <%--<asp:ScriptManager runat="server"  >
                    </asp:ScriptManager>--%>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="DIVAddUsers" class="card shadow mb-4" runat="server">
                        <div class="card-header py-3">
                            <h2 class="m-0 font-weight-bold poptxt">ADD USERS
                            </h2>
                        </div>
                        

                        <div class="card-body">
                            <div>
                                <div class="panel panel-default colorFont TableHeaderFont">

                                     <div class="form-group">
                                        <asp:Label ID="lblDomainName" class=" font-weight-bold poptxt" runat="server">Domain Name<span class="text-danger">*</span></asp:Label>
                                        <asp:TextBox ID="txtDomain" class="form-control input-sm" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                <asp:Label ID="Label4" runat="server" class=" font-weight-normal poptxt"><b>Group Name<span class="text-danger">*&nbsp;&nbsp;</span></b></asp:Label>
                                 <asp:DropDownList ID="DrpGroupName" runat="server" class="form-control animated--grow-in" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" AppendDataBoundItems="True" AutoPostBack="true" OnSelectedIndexChanged="getRoletypesForGroup">
                                            <asp:ListItem>--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                 </div>

                                    <div class="form-group">
                                        <asp:Label ID="lblUserName" class=" font-weight-bold poptxt" runat="server">User Name<span class="text-danger">*</span></asp:Label>
                                        <asp:TextBox ID="txtUser" class="form-control input-sm" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label ID="lblPassWord" class=" font-weight-bold poptxt" runat="server">Password<span class="text-danger">*</span></asp:Label>
                                        <asp:TextBox ID="txtPwd" class="form-control input-sm " runat="server" TextMode="Password"></asp:TextBox>
                                    </div>

                                   
                                    
                                    <div class="form-group  font-weight-bold poptxt">
                                        <asp:Label ID="lblRoleTypes" runat="server">RoleType<span class="text-danger">*</span></asp:Label>
                                        <asp:DropDownList ID="DrpRoleType" runat="server" class="form-control animated--grow-in" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" AppendDataBoundItems="True" >
                                            <asp:ListItem>--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    </td>
                            <div class="form-group" align="center">
                                <asp:Button ID="Button1" runat="server" class="btn btn-primary colorSidebar" Text="Save" OnClick="btnSave" Width="49%" />
                                <asp:Button ID="Button2" runat="server" class="btn btn-danger" Text="Clear All" OnClick="BtnCancelUser" Width="49%" />
                            </div>
                                </div>
                            </div>
                        </div>
                        
                    </div>
                </ContentTemplate>
                 <Triggers>
                  <asp:AsyncPostbackTrigger ControlID="DrpGroupName" EventName="SelectedIndexChanged" />
                 </Triggers>
            </asp:UpdatePanel>
            </br>
              <asp:UpdatePanel runat="server">
                  <ContentTemplate>
                      <div class="card shadow mb-4">
                          <div class="card-header py-3">
                              <h2 class="m-0 font-weight-bold poptxt">USERS
                   <asp:ImageButton ID="ImageButton2" runat="server" Class="rotate refreshbtn" ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="refreshUsers" />

                              </h2>
                          </div>

                          <div class="card-body">
                              <div>

                                  <div class="panel-body">
                                      <asp:Repeater ID="GVRoles" runat="server">
                                          <HeaderTemplate>
                                              <table class="table table-striped table-bordered table-hover table-responsive-lg" id="dataTables-users" width="100%">
                                                  <thead class="colorSidebar" align="center">
                                                      <tr class="TableHeaderFont">
                                                          <th scope="col" class=" font-weight-bold poptxt">User Id
                                                          </th>
                                                          <th scope="col" class=" font-weight-bold poptxt">Domain Name
                                                          </th>
                                                          <th scope="col" class=" font-weight-bold poptxt">Role
                                                          </th>
                                                          <th scope="col" class=" font-weight-bold poptxt">Created By
                                                          </th>
                                                          <th scope="col" class=" font-weight-bold poptxt">Create Date
                                                          </th>
                                                          <%--<th scope="col" class=" font-weight-bold poptxt">User Access 
                                                          </th>--%>
                                                         <%-- <th scope="col" class=" font-weight-bold poptxt">Change Role
                                                          </th>--%>
                                                          <th scope="col" class=" font-weight-bold poptxt">Delete
                                                          </th>
                                                      </tr>
                                                  </thead>
                                          </HeaderTemplate>
                                          <ItemTemplate>
                                              <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="poptxtdata">
                                                  <td class="odd gradeX" style="text-align: center;">
                                                      <asp:Label ID="lblRolesID" name="RolesID" runat="server" Text='<%# Eval("userid") %>' Height="12px" />
                                                  </td>
                                                  <td class="odd gradeX" style="text-align: center;">
                                                      <asp:Label ID="lblRolesName" runat="server" Text='<%# Eval("domainname") %>' />
                                                  </td>
                                                  <td class="odd gradeX" style="text-align: center;">
                                                      <asp:Label ID="Label1" runat="server" Text='<%# Eval("roletype") %>' />
                                                  </td>
                                                  <td class="odd gradeX" style="text-align: center;">
                                                      <asp:Label ID="Label2" runat="server" Text='<%# Eval("createdby") %>' />
                                                  </td>
                                                  <td class="odd gradeX" style="text-align: center;">
                                                      <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("createddate" , "{0:dd/MMM/yyyy HH:mm:ss}") %>' />
                                                  </td>
                                                 <%-- <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                                      <asp:ImageButton ID="btnUserAccess" runat="server" ImageUrl="~/Images/access.png" ImageAlign="Middle" Width="16%" CommandArgument='<%# Eval("groupid")+","+ Eval("tenantid")+","+Eval("userid")  %>' OnCommand="ImageButton1_Command" />
                                                  </td>--%>
                                                 <%-- <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                                      <asp:ImageButton ID="imgChangeGroup" runat="server" ImageUrl="~/Images/version3.png" ImageAlign="Middle" AlternateText="Change Group" CommandArgument='<%#Eval("userid") +","+ Eval("roletype") +","+ Eval("domainname") %>' OnCommand="imgChangeGroup_Command" />
                                                  </td>--%>
                                                  <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">

                                                      <asp:ImageButton ID="btnDeleteUser" runat="server" ImageUrl="~/Images/Delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("id")+","+ Eval("userid")%>' OnCommand="btnDelete_Click" />

                                                  </td>

                                                  <%--   <% } %>--%>
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

                                  <!-- /.panel-body -->
                              </div>

                              <%-- Delete user Modal Start--%>
                              <asp:UpdatePanel runat="server">
                                  <ContentTemplate>
                                      <div class="modal fade " id="modalDelete" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
                                          <div class="modal-dialog modal-lg" role="document">
                                              <div class="modal-content ">
                                                  <div class="modal-header">
                                                      <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">Delete User</h5>
                                                      <asp:Button class="close" runat="server" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close" data-dismiss="modal"></asp:Button>
                                                  </div>
                                                  <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                      Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?
                                                <br />
                                                      <div class="form-group">

                                                          <div class="container">

                                                              <div class="row">
                                                                  <br />
                                                              </div>

                                                              <div class="row">
                                                                  <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6" align="right">
                                                                      User Id:
                                                                  </div>
                                                                  <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6 font-weight-bold  text-danger" align="left">
                                                                      <asp:Label ID="lblId" runat="server" hidden> </asp:Label>
                                                                      <asp:Label ID="lblUserId" runat="server"> </asp:Label>
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
                                                                  <asp:Button ID="btnDelete" runat="server" class="btn btn-danger btn-block" Text="Delete" OnClick="ModalPopUpBtnDelete_Click" />
                                                              </div>
                                                              <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                                  <button class="btn btn-primary btn-block" type="button" data-dismiss="modal">Close</button>
                                                              </div>
                                                              <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                              </div>
                                                          </div>
                                                      </div>

                                                  </div>

                                                  <%-- &nbsp;&nbsp;&nbsp;&nbsp;--%>
                                              </div>
                                          </div>
                                      </div>
                                      </div>
                                  </ContentTemplate>
                              </asp:UpdatePanel>
                              <%--End Delete user--%>





                              <%-- Delete user  Second PopUp Modal Start--%>
                              <asp:UpdatePanel runat="server">
                                  <ContentTemplate>
                                      <div class="modal fade " id="modalDeleteSecondPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelDeleteSecondPopUp" aria-hidden="true">
                                          <div class="modal-dialog modal-lg" role="document">
                                              <div class="modal-content ">
                                                  <div class="modal-header">
                                                      <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">Delete User Permanently</h5>
                                                      <asp:Button CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" BackColor="White" BorderStyle="None" runat="server" OnClick="btnXdelete_clickHideBgPop"></asp:Button>

                                                      <%-- <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true">×</span>
                                                </button>--%>
                                                  </div>

                                                  <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                      You are about to delete the user permanently.Deleted user will not be recovered.<br />
                                                      Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?

                                                <br />
                                                      <div class="form-group">

                                                          <div class="container">

                                                              <div class="row">
                                                                  <br />
                                                              </div>

                                                              <div class="row">
                                                                  <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6" align="right">
                                                                      User Id:
                                                                  </div>
                                                                  <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6 font-weight-bold  text-danger" align="left">
                                                                      <asp:Label ID="lblIdSecondPopUp" runat="server" hidden> </asp:Label>
                                                                      <asp:Label ID="lblUserIdSecondPopUp" runat="server"> </asp:Label>
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
                                                                  <asp:Button ID="Button3" runat="server" class="btn btn-danger btn-block" Text="Delete" OnClick="ModalPopUpBtnDelete_ClickSecondPopUp" />
                                                              </div>
                                                              <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                                  <asp:Button ID="btnCloseSecondPopUp" runat="server" class="btn btn-primary btn-block" Text="Close" OnClick="ModalPopUpBtnClose_ClickSecondPopUp" />

                                                              </div>
                                                              <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                              </div>
                                                          </div>
                                                      </div>

                                                  </div>

                                                  <%-- &nbsp;&nbsp;&nbsp;&nbsp;--%>
                                              </div>
                                          </div>
                                      </div>
                                      </div>
                                  </ContentTemplate>
                              </asp:UpdatePanel>
                              <%--End Delete  Second PopUp user--%>


                              <asp:UpdatePanel runat="server">
                                  <ContentTemplate>
                                      <div class="modal fade" id="changeGroup123" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel34" aria-hidden="true">
                                          <div class="modal-dialog " role="document">
                                              <div class="modal-content">
                                                  <div class="modal-header">
                                                      <h5 class="modal-title h5 font-weight-bold text-primary" id="exampleModalLabel34">Change Group</h5>
                                                      <asp:Button CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" BackColor="White" BorderStyle="None" runat="server" OnClick="btnXdelete_clickHideBgPop" />
                                                  </div>

                                                  <div class="modal-body poptxt" runat="server">
                                                      <div class="container">
                                                          <table width="100%" align="center">

                                                              <tr style="height: 25px">
                                                                  <td align="left" style="text-align: right">
                                                                      <div class="form-group">
                                                                          <asp:Label ID="lblUserIDpop" runat="server"><b>User ID*&nbsp;&nbsp;</b></asp:Label>
                                                                      </div>
                                                                  </td>
                                                                  <td>
                                                                      <div class="form-group">
                                                                          <asp:TextBox ID="txtCGUserID" class="form-control input-sm" runat="server"></asp:TextBox>
                                                                      </div>
                                                                  </td>


                                                                  <td class="auto-style16"></td>

                                                              </tr>

                                                              <tr style="height: 25px">
                                                                  <td align="left" style="text-align: right">
                                                                      <div class="form-group">

                                                                          <asp:Label ID="lblCurrentRole" runat="server"><b>Current Role*&nbsp;&nbsp;</b></asp:Label>
                                                                      </div>

                                                                  </td>
                                                                  <td>
                                                                      <div class="form-group">
                                                                          <asp:TextBox ID="txtCGCurrentRole" class="form-control input-sm" runat="server"></asp:TextBox>
                                                                      </div>
                                                                  </td>


                                                                  <td class="auto-style16"></td>

                                                              </tr>

                                                              <tr style="height: 25px">
                                                                  <td align="left" style="text-align: right">
                                                                      <div class="form-group">

                                                                          <asp:Label ID="Label3" runat="server"><b>Domain Name*&nbsp;&nbsp;</b></asp:Label>
                                                                      </div>

                                                                  </td>
                                                                  <td>
                                                                      <div class="form-group">
                                                                          <asp:TextBox ID="txtDomainname" class="form-control input-sm" runat="server"></asp:TextBox>
                                                                      </div>
                                                                  </td>


                                                                  <td class="auto-style16"></td>

                                                              </tr>


                                                              <tr style="height: 25px">
                                                                  <td align="left" style="text-align: right">
                                                                      <div class="form-group">

                                                                          <asp:Label ID="lblAssignRole" runat="server"><b>Assign Role*&nbsp;&nbsp;</b></asp:Label>
                                                                      </div>

                                                                  </td>
                                                                  <td>
                                                                      <div class="form-group">

                                                                          <asp:DropDownList ID="ddlAssignRole" runat="server" class="form-control animated--grow-in" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" AppendDataBoundItems="True">
                                                                              <asp:ListItem>--Select--</asp:ListItem>

                                                                          </asp:DropDownList>
                                                                      </div>
                                                                  </td>


                                                                  <td class="auto-style16"></td>

                                                              </tr>

                                                          </table>
                                                          </td>
                                                                <td></td>
                                                          </tr>
                                                        </table>
                                                      </div>

                                                      <div class="modal-footer">
                                                          <table width="100%">
                                                              <tr>
                                                                  <td width="15%"></td>
                                                                  <td width="35%">
                                                                      <div class="form-group">
                                                                          <asp:Button ID="btnSaveGroupChanges" CssClass="btn btn-primary" runat="server" Text="Update Group" OnCommand="btnSaveGroupChanges_Command" />
                                                                          <%--<asp:Button ID="Button3" runat="server" class="btn btn-primary colorSidebar"  Text="Save" OnClick="Button1_Click" width="150px"/>--%>
                                                                      </div>
                                                                  </td>
                                                                  <td>&nbsp;</td>
                                                                  <td>&nbsp;</td>
                                                                  <td width="35%">
                                                                      <div class="form-group">
                                                                          <asp:Button ID="Button6" runat="server" class="btn btn-danger" Text="Reset" Width="150px" OnClick="btnClear_click" /><%--data-dismiss="modal"--%>
                                                                      </div>
                                                                  </td>
                                                                  <td width="15%"></td>
                                                              </tr>
                                                          </table>
                                                      </div>
                                                  </div>
                                              </div>
                                          </div>
                                      </div>
                                  </ContentTemplate>
                              </asp:UpdatePanel>



                              <asp:UpdatePanel runat="server" ID="popup">
                                  <ContentTemplate>
                                      <div class="modal fade" id="UpdateVersion" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="exampleModalLabel2" aria-hidden="true">
                                          <div class="modal-dialog " role="document">
                                              <div class="modal-content">
                                                  <div class="modal-header">
                                                      <h5 class="modal-title h5 font-weight-bold poptxt" id="exampleModalLabel2">Update Process File</h5>
                                                      <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                          <span aria-hidden="true">×</span>
                                                      </button>
                                                  </div>

                                                  <div class="modal-body" style="align-content: center" runat="server">
                                                      <asp:HiddenField runat="server" ID="_HGroupid" />
                                                      <asp:HiddenField runat="server" ID="_HTenantID" />
                                                      <asp:HiddenField runat="server" ID="_HUserName" />

                                                      <%--card begins--%>
                                                      <div class="card shadow">
                                                          <div class="container">

                                                              <asp:Table ID="Table1" CssClass="table table-striped table-bordered table-hover poptxt" runat="server">
                                                                  <asp:TableHeaderRow align="center" CssClass="TableHeaderFont">
                                                                      <asp:TableHeaderCell><span class="text-dark">Page Access</span><asp:CheckBox ID="chkHeader" name="sample" onclick="javascript:CheckUncheckall(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableHeaderCell><span class="text-success" >Read</span></asp:TableHeaderCell>
                                                                      <asp:TableHeaderCell><span class="text-muted" >Create</span></asp:TableHeaderCell>
                                                                      <asp:TableHeaderCell><span class="text-warning" ">Edit</span></asp:TableHeaderCell>
                                                                      <asp:TableHeaderCell><span class="text-danger" ">Delete</span></asp:TableHeaderCell>
                                                                  </asp:TableHeaderRow>



                                                                  <asp:TableRow ID="DashBoardRow" align="center">
                                                                      <asp:TableHeaderCell>
                                                                          <span class="poptxt font-weight-bold">BotDashboard</span>&nbsp;&nbsp;
                                                                    <asp:CheckBox ID="CheckBox1" name="sample" onclick="javascript:CheckUncheckDashBoard(this);" class="selectall" runat="server" />
                                                                      </asp:TableHeaderCell>
                                                                      <asp:TableCell>

                                                                          <asp:CheckBox ID="chkBotDashboardR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotDashboardC"  Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotDashboardE" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotDashboardD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="ProcessManagemenRow" align="center">
                                                                      <asp:TableHeaderCell><span>Process Management</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox13" name="sample" onclick="javascript:CheckUncheckProcessManagemen(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkProcessManagementR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkProcessManagementC" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkProcessManagementE" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkProcessManagementD" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="QueueDetailsRow" align="center">
                                                                      <asp:TableHeaderCell><span >Queue Details</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox2" name="sample" onclick="javascript:CheckUncheckQueueDetail(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueD" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>


                                                                  <asp:TableRow ID="AddSchedulesRow" align="center">
                                                                      <asp:TableHeaderCell><span >Add Schedules</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox3" name="sample" onclick="javascript:CheckUncheckAddSchedules(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddScheduleR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="ChkAddScheduleC" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddScheduleE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddScheduleD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="ScheduleDetailsRow" align="center">
                                                                      <asp:TableHeaderCell><span >Schedule Details</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox11" name="sample" onclick="javascript:CheckUncheckScheduleDetails(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkScheduleDetailsR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="ChkScheduleDetailsC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="ChkScheduleDetailsE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="ChkScheduleDetailsD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>



                                                                  <asp:TableRow ID="AddUserRow" align="center">
                                                                      <asp:TableHeaderCell><span >Add User</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox4" name="sample" onclick="javascript:CheckUncheckAddUser(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddUserR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddUserC" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddUserE" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddUserD" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="AddRobotRow" align="center">
                                                                      <asp:TableHeaderCell><span >Add Robot</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox5" name="sample" onclick="javascript:CheckUncheckAddRobot(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddRobotR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddRobotC" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddRobotE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddRobotD" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="AssignQueueToBotRow" align="center">
                                                                      <asp:TableHeaderCell><span >Assign Queue To Bot</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox7" name="sample" onclick="javascript:CheckUncheckAssignQueueToBot(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAssignQueueBotR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAssignQueueBotC" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAssignQueueBotE" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAssignQueueBotD" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="AssignBotToUserRow" align="center">
                                                                      <asp:TableHeaderCell><span >Assign Bot To User</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox8" name="sample" onclick="javascript:CheckUncheckAssignBotToUser(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAssignBotUserR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAssignBotUserC" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAssignBotUserE" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAssignBotUserD" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="QueueManagementRow" align="center">
                                                                      <asp:TableHeaderCell><span >Queue Management</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox6" name="sample" onclick="javascript:CheckUncheckQueueManagement(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueManagementR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueManagementC" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueManagementE" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueManagementD" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="figurationManagementRow" align="center">
                                                                      <asp:TableHeaderCell><span >Configuration Management</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox12" name="sample" onclick="javascript:CheckUncheckfigurationManagement(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkConfigurationR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkConfigurationC" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkConfigurationE" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkConfigurationD" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="PromoteDemoteAutomationRow" align="center">
                                                                      <asp:TableHeaderCell><span >Promote Demote Automation</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox15" name="sample" onclick="javascript:CheckUncheckPromoteDemoteAutomation(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkPromoteDemoteR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkPromoteDemoteC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkPromoteDemoteE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkPromoteDemoteD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>


                                                                   <asp:TableRow id="BotLogRow" align="center">
                                                                      <asp:TableHeaderCell><span >Bot Log</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox9" name="sample" onclick="javascript:CheckUncheckBotLog(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotLogR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotLogC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotLogE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotLogD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  
                                                                  <asp:TableRow ID="AuditTrailRow" align="center">
                                                                      <asp:TableHeaderCell><span >Audit Trail</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox10" name="sample" onclick="javascript:CheckUncheckAuditTrail(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAuditTrailR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAuditTrailC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAuditTrailE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAuditTrailD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="DetaillLogRow" align="center">
                                                                      <asp:TableHeaderCell><span>Detaill Log</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox14" name="sample" onclick="javascript:CheckUncheckDetaillLog(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkDetailLogR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkDetailLogC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkDetailLogE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkDetailLogD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="ReportsRow" align="center">
                                                                      <asp:TableHeaderCell><span class="text-primary">Reports</span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox16" name="sample" onclick="javascript:CheckUncheckReports(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkReportsR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkReportsC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkReportsE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkReportsD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>
                                                              </asp:Table>
                                                          </div>
                                                      </div>
                                                  </div>
                                                  <div class="modal-footer">
                                                      <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                                                      <asp:Button ID="btnSaveAccess" CssClass="btn btn-primary" runat="server" Text="Save All" OnClientClick="javascript:po();" OnCommand="btnSaveAccess_Command" />

                                                  </div>
                                              </div>
                                          </div>
                                      </div>
                                  </ContentTemplate>
                              </asp:UpdatePanel>

                              <!-- /.panel -->
                          </div>
                          <!-- /.col-lg-12 -->
                      </div>
                  </ContentTemplate>
              </asp:UpdatePanel>
        </div>

    </div>
    <!-- /#wrapper -->
    </div>
    <%--Container Fluid End--%>
    <script>
        function po() {
            $('#UpdateVersion').modal('hide');
        }
    </script>
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

        $(".rotate").click(function () {
            $(this).addClass("down");
        })

        function openModalDelete() {
            $('#modalDelete').modal('show');
        };

        function openModalHideDelete() {
            $('body').removeClass().removeAttr('style'); $('.modal-backdrop').remove();
        };


        function openModalDeleteSecondPopUp() {
            $('#modalDeleteSecondPopUp').modal('show');
        };

        function openModalGroupChange() {
            $('#changeGroup123').modal('show');
        }
    </script>
    <script>
        function openModal() {
            $('#UpdateVersion').modal('show');
        }
    </script>

    <script language="javascript" type="text/javascript">
        function CheckUncheckall(chk) {
            var chks = document.getElementById("<% = Table1.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
               
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }

        function CheckUncheckDashBoard(chk) {
            var chks = document.getElementById("<% = DashBoardRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                 
                if (chks[i].type == "checkbox" && !chks[i].disabled) 
                     chks[i].checked = chk.checked;
                
               
            }
        }
        function CheckUncheckQueueDetail(chk) {
            var chks = document.getElementById("<% = QueueDetailsRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAddSchedules(chk) {
            var chks = document.getElementById("<% = AddSchedulesRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAddUser(chk) {
            var chks = document.getElementById("<% = AddUserRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAddRobot(chk) {
            var chks = document.getElementById("<% = AddRobotRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckQueueManagement(chk) {
            var chks = document.getElementById("<% = QueueManagementRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAssignQueueToBot(chk) {
            var chks = document.getElementById("<% =  AssignQueueToBotRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAssignBotToUser(chk) {
            var chks = document.getElementById("<% =  AssignBotToUserRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckBotLog(chk) {
            var chks = document.getElementById("<% =  BotLogRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAuditTrail(chk) {
            var chks = document.getElementById("<% = AuditTrailRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckScheduleDetails(chk) {
            var chks = document.getElementById("<% = ScheduleDetailsRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }

        function CheckUncheckfigurationManagement(chk) {
            var chks = document.getElementById("<% = figurationManagementRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }

        function CheckUncheckProcessManagemen(chk) {
            var chks = document.getElementById("<% = ProcessManagemenRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckDetaillLog(chk) {
            var chks = document.getElementById("<% = DetaillLogRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckPromoteDemoteAutomation(chk) {
            var chks = document.getElementById("<% = PromoteDemoteAutomationRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
           function  CheckUncheckReports(chk) {
            var chks = document.getElementById("<% = ReportsRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
       

    </script>


</asp:Content>


