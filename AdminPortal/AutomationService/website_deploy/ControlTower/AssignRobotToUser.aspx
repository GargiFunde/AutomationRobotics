    <%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="DemoMasterPage2_AssignRobotToUser, App_Web_2o2bniex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ScriptManager runat="server">
</asp:ScriptManager>

    <!-- Begin Page Content -->
    <div id="page-wrapper" class="container-fluid">

        <!-- Page Heading -->
        <%--   <h1 class="h3 mb-2 text-gray-800">Process Management</h1>--%>
         <asp:UpdatePanel runat="server">

                        <ContentTemplate>
        <!-- DataTables Example -->
        <div id="DivRobotUserMapping" class="card shadow mb-4" runat="server">
            <div class="card-header py-3">
                <h2 class="m-0 font-weight-bold poptxt">ASSIGN ROBOT TO USER
                </h2>
            </div>

            <div class="card-body">
                <div class="panel panel-default colorFont">
                            <div class="card-body TableHeaderFont">
                                <div class="form-group">
                                    <asp:Label ID="Label4" runat="server" Text="Label" class=" font-weight-bold poptxt">
                                         User Name<span class="text-danger">*</span>
                                    </asp:Label>
                                    <asp:DropDownList ID="DrpUsers" runat="server" class="form-control input-sm" AppendDataBoundItems="True" AutoPostBack="False" >
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="form-group">
                                    <asp:Label ID="Label5" runat="server" Text="Label" class=" font-weight-bold poptxt">
                                            Bot Name<span class="text-danger">*</span>
                                    </asp:Label>
                                    <asp:DropDownList ID="DrpBots" runat="server" class="form-control input-sm" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <br />
                                <div class="form-group" align="center">
                                    <asp:Button ID="btnSave" runat="server" class="btn btn-primary " Text="Save" OnClick="Save_Click" Width="49%" />
                                    <asp:Button ID="btnCancel" runat="server" class="btn btn-danger" Text="Cancel" OnClick="BtnCancelUser" Width="49%" />
                                </div>

                            </div>
                            </div>
                                </div>
                       </div>      
     </ContentTemplate>
             </asp:UpdatePanel>
   <br />
      
     <asp:UpdatePanel runat="server">

                        <ContentTemplate>
            <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h2 class="m-0 font-weight-bold poptxt">USERS
                   <asp:ImageButton ID="ImageButton2" runat="server" Class="rotate refreshbtn" ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="ImageButton3_Command" />

                </h2>
            </div>

            <div class="card-body">
                <div class="panel panel-default colorFont">
        

                        <%--    <% } %>--%>
                            <div class="panel-body">
                                <asp:Repeater ID="GVUserToBotMapping" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-bordered table-hover" id="dataTables-botqueuemapping" width="100%">
                                            <thead class="colorSidebar" align="center">
                                                <tr class="poptxt TableHeaderFont">
                                                    <th scope="col" class=" font-weight-bold ">User Id
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">Bot Name
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">Created By
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold">Create Date
                                                    </th>

                                                  <%--  <% if ("SuperAdmin" == Session["Role"].ToString() || "Admin" == Session["Role"].ToString())
                                                        { %>--%>
                                                    <th scope="col" class=" font-weight-bold text-danger">Delete
                                                    </th>
                                                  <%--  <% } %>--%>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;text-align:center;" class="poptxtdata">
                                            <td class="odd gradeX ">
                                                <asp:Label ID="lblBotID" name="BotID" runat="server" Text='<%# Eval("userid") %>'  />
                                            </td>
                                            <td class="odd gradeX " style="text-align: center">
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("botname") %>' />
                                            </td>
                                            <td class="odd gradeX " style="text-align: center">
                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("createdby") %>' />
                                            </td>
                                            <td class="odd gradeX " style="text-align: center">
                                                <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("createddate" , "{0:dd/MMM/yyyy HH:mm:ss}") %>' />
                                            </td>

                                            <%--<% if ("SuperAdmin" == Session["Role"].ToString() || "Admin" == Session["Role"].ToString())
                                                { %>--%>
                                            <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                                <asp:ImageButton ID="btnDeleteRobotUserMapping" runat="server" ImageUrl="~/Images/delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("id")+","+Eval("botname")+","+ Eval("userid")%>' CommandName="Delete" OnCommand="btnDelete_Click" />

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
                        

                </div>
    </div>
             </div>
            </ContentTemplate>
                    </asp:UpdatePanel>
                <!-- /.panel-body -->
                   <%--DELETE ROBOT MODAL--%>
                     <asp:UpdatePanel runat="server">

                        <ContentTemplate>
                            <div class="modal fade " id="modalDelete" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
                                <div class="modal-dialog modal-lg" role="document" >
                                    <div class="modal-content ">
                                        <div class="modal-header">
                                            <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">Delete Robot To User</h5>
                                            <asp:Button class="close" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" data-dismiss="modal" >
                                              
                                            </asp:Button>
                                        </div>

                                        <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                            Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?
                                                                          
                                         <br /><br />
                                            <div class="form-group">
                                                <div class="container">
                                                      <div class="row">
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                         User Name :
                                                        </div>
                                                        <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2" align="left" >
                                                         <asp:Label ID="lblUserName" Class="font-weight-bold text-danger"  runat="server"> </asp:Label>
                                                            <asp:Label ID="lblId" Class="text-success" runat="server" hidden> </asp:Label>
                                                        </div>
                                                          <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                         
                                                        </div>
                                                           <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                         Bot Name :
                                                        </div>
                                                          <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="left">
                                                          <asp:Label ID="lblBotName" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                                        </div>
                                                      </div>
                                                    </div>

                                                

                                                <br /><br />
                                                <div class="container">
                                                    <div class="row">
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                        </div>
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                             <asp:Button ID="btnDelete" runat="server" class="btn btn-danger btn-block" Text="Delete" align="center" OnClick="ModalPopUpBtnDelete_Click" />
                                                        </div>
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
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
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--COMPLETE DELETE MODAL--%>

                                   <%--DELETE ROBOT MODAL SECOND POPUP--%>
                     <asp:UpdatePanel runat="server">

                        <ContentTemplate>
                            <div class="modal fade " id="modalDeleteSecondPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelDeleteSecondPopUp" aria-hidden="true">
                                <div class="modal-dialog modal-lg" role="document" >
                                    <div class="modal-content ">
                                        <div class="modal-header">
                                            <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">Delete Robot To User Permanently</h5>
                                            <asp:Button class="close" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop">
                                              
                                            </asp:Button>
                                                
                                        </div>

                                        <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                         You are about to delete the robot to user permanently.Deleted  robot to user will not be recovered.<br />
                                         Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?
                                                                          
                                         <br /><br />
                                            <div class="form-group">
                                                <div class="container">
                                                      <div class="row">
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                         User Name :
                                                        </div>
                                                        <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2" align="left" >
                                                         <asp:Label ID="lblUserNameSecondPopUp" Class="font-weight-bold text-danger"  runat="server"> </asp:Label>
                                                            <asp:Label ID="lblIdSecondPopUp" Class="text-success" runat="server" hidden> </asp:Label>
                                                        </div>
                                                          <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                         
                                                        </div>
                                                           <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                         Bot Name :
                                                        </div>
                                                          <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="left">
                                                          <asp:Label ID="lblBotNameSecondPopUp" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                                        </div>
                                                      </div>
                                                    </div>

                                                

                                                <br /><br />
                                                <div class="container">
                                                    <div class="row">
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                        </div>
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                             <asp:Button ID="Button1" runat="server" class="btn btn-danger btn-block" Text="Delete" align="center" OnClick="ModalPopUpBtnDelete_ClickSecondPopUp" />
                                                        </div>
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                            <asp:Button ID="btnCloseSecondPopUp" runat="server" class="btn btn-primary btn-block" Text="Close" OnClick="ModalPopUpBtnClose_ClickSecondPopUp" />
                                                        </div>
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                        </div>
                                                    </div>
                                                </div>
                                         
                                            </div>
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--COMPLETE DELETE MODAL SECOND POPUP--%>
            </div>
            <!-- /.panel -->
        </div>
        <!-- /.col-lg-12 -->
    </div>



    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script>
        $(document).ready(function () {
            $("#page-wrapper").delay(100).fadeIn(300);
            $('#dataTables-botqueuemapping').DataTable({
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
                    $('#dataTables-botqueuemapping').DataTable({
                        responsive: true,
                        destroy:true
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

        function openModalDeleteSecondPopUp() {
            $('#modalDeleteSecondPopUp').modal('show');
        };
        function openModalHideDelete() {
            $('body').removeClass().removeAttr('style'); $('.modal-backdrop').remove();
        };
    </script>

</asp:Content>
