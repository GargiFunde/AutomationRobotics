<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="DemoMasterPage2_AddRobot, App_Web_2o2bniex" %>

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
                <div id="DivAddRobot" class="card shadow mb-4" runat="server">
                    <div class="card-header py-3">
                        <h2 class="m-0 font-weight-bold poptxt">ADD ROBOT
                        </h2>
                    </div>

                    <div class="card-body">
                        <div>
                            <div class="panel panel-default colorFont">
                                <div class="form-group">
                                    <asp:Label ID="Label3" runat="server" class="TableHeaderFont poptxt font-weight-bold ">Bot Name<span class="text-danger">*</span></asp:Label>
                                    <asp:TextBox ID="txtBotName" class="form-control input-sm animated--grow-in" runat="server"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" class=" font-weight-bold TableHeaderFont poptxt">Machine Name<span class="text-danger">*</span></asp:Label>
                                    <asp:TextBox ID="txtMachineName" class="form-control input-sm" runat="server"></asp:TextBox>
                                    <br />
                                    <div class="form-group">
                                        <asp:Label ID="Label4" runat="server" class=" font-weight-bold TableHeaderFont poptxt">Bot User Id<span class="text-danger">*</span></asp:Label>
                                        <asp:TextBox ID="txtBotId" class="form-control input-sm" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="Label5" runat="server" class=" font-weight-bold TableHeaderFont poptxt">Bot Password<span class="text-danger">*</span></asp:Label>
                                        <asp:TextBox ID="txtPwd" class="form-control input-sm" runat="server" TextMode="Password"></asp:TextBox>
                                    </div>

                                    <br />

                                    <div class="form-group" align="center">
                                        <asp:Button ID="btnsave" runat="server" class="btn btn-primary colorSidebar" Text="Save" OnClick="btnsave_Click" Width="49%" />
                                        <asp:Button ID="Button2" runat="server" class="btn btn-danger" Text="Clear" OnClick="BtnCancelRobot" Width="49%" />
                                    </div>
                                    <br />


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
                        <h2 class="m-0 font-weight-bold text-primary">ADD ROBOT
                   <asp:ImageButton ID="ImageButton2" runat="server" Class="rotate refreshbtn" ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="refreshRobots" />

                        </h2>
                    </div>

                    <div class="card-body">
                        <div>
                            <!-- /.panel-heading -->
                            <div class="panel-body">
                                <asp:Repeater ID="GrvBots" runat="server" >
                                    <HeaderTemplate>
                                        <table class="table table-striped table-bordered table-hover" id="dataTables-users" width="100%">
                                            <thead class="colorSidebar" align="center">
                                                <tr class="poptxt TableHeaderFont">
                                                    <th scope="col" class=" font-weight-bold ">Status
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">Bot Name
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">Bot Key
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">Machine Name
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">Created By
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">Create Date
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold text-danger">Del
                                                    </th>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="text-muted">
                                            <td style="text-align: center; align-content: center; align-items: center; align-self: center; padding-left: 10px;" align="center">
                                                <span class="d-inline-block" tabindex="0" data-toggle="tooltip" title='<%# Eval("connectionStatusMessage") %>'>
                                                <asp:Button class="btn btn-circle btn-default" id="connectionButtonStatus" name="divStatus" Style='<%# Eval("styleClass") %>'  readonly="true" runat="server" />
                                                    </span>
                                                <%-- <button class="btn btn-circle btn-default" id="piyush" name="divStatus" style="background-color: #999999; width: 17px; height: 25px"
                                                        type="button" >--%>
                                                    </button>
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="lblBotName" name="BotName" runat="server" Text='<%# Eval("botname") %>' Height="12px" />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="lblBotKey" runat="server" Text='<%# Eval("botkey") %>' />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("machinename") %>' />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("createdby") %>' />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("createddate" , "{0:dd/MMM/yyyy HH:mm:ss}") %>' />
                                            </td>
                                            <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                                <asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/Images/delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("id")+","+Eval("botname")+","+ Eval("machinename")%>' CommandName="Delete" OnCommand="btnDelete_Click" />
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
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--DELETE ROBOT MODAL--%>
        <asp:UpdatePanel runat="server">

            <ContentTemplate>
                <div class="modal fade " id="modalDelete" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">Delete Robot</h5>
                                <asp:Button class="close" runat="server" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close"></asp:Button>
                            </div>

                            <div class="modal-body  TableHeaderFont  poptxt " style="text-align: center" runat="server">
                                Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?
                                                                          
                                         <br />
                                <br />
                                <div class="form-group">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                Robot Name :
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2" align="left">
                                                <asp:Label ID="lblRobotName" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                                <asp:Label ID="lblId" Class="text-success" runat="server" hidden> </asp:Label>
                                            </div>
                                            <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                Machine Name :
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="left">
                                                <asp:Label ID="lblMachineName" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <br />
                                    <br />
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


        <%--DELETE ROBOT MODAL  SECOND POP UP--%>
        <asp:UpdatePanel runat="server">

            <ContentTemplate>
                <div class="modal fade " id="modalDeleteSecondPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelDeleteSecondPopUp" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">Delete Robot Permanently</h5>
                                <asp:Button class="close" runat="server" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop"></asp:Button>
                            </div>

                            <div class="modal-body   TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                You are about to delete the robot permanently.Deleted robot will not be recovered.<br />
                                Are you sure you want to <span class=" font-weight-bold text-danger">Delete ?</span>

                                <br />
                                <br />
                                <div class="form-group">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                Robot Name :
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2" align="left">
                                                <asp:Label ID="lblRobotNameSecondPopUp" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                                <asp:Label ID="lblIdSecondPopUp" Class="text-success" runat="server" hidden> </asp:Label>
                                            </div>
                                            <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                Machine Name :
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="left">
                                                <asp:Label ID="lblMachineNameSecondPopUp" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <br />
                                        </div>
                                        <div class="row">
                                            <br />
                                        </div>
                                        <div class="modal-body TableHeaderFont  poptxt  " style="text-align: center" runat="server">
                                            <span class=" font-weight-bold text-danger">Note:</span>
                                            </br>
                                                            <br />
                                            While deleting this robot,you will be also deleting all the mappings to this robot. </br>
                                                            Are you sure you want to delete it 
                                                            from  <span class="h6 font-weight-bold text-danger">Bots,</span><span class="h6 font-weight-bold text-danger">Schedule Details, </span><span class="h6 font-weight-bold text-danger">User Bot Mapping, </span>
                                            <span class="h6 font-weight-bold text-danger">Bot Default Queue, </span><span class="h6 font-weight-bold text-danger">Qrtz Triggers, </span><span class="h6 font-weight-bold text-danger">Qrtz Job Details and </span>
                                            <span class="h6 font-weight-bold text-danger">Qrtz Cron Triggers</span>?
                                        </div>



                                        <div class="modal-body h6  text-primary" style="text-align: center" runat="server">
                                            <span class="h5 font-weight-bold text-danger">Counts</span>
                                            <br />
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold">
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                Schedule :
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                <asp:Label ID="lblScheduleDeatils" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <br />
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Button ID="btnDeleteSecondPopUp" class="btn btn-danger btn-block" Text="Delete" runat="server" OnClick="ModalPopUpBtnDelete_ClickSecondPopUp" />
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
    </div>
    <!-- /.col-lg-12 -->
    <!-- /#wrapper -->




    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script>
        $(document).ready(function () {
            $("#page-wrapper").delay(300).fadeIn(200);
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
        function openModalDeleteSecondPopUp() {
            $('#modalDeleteSecondPopUp').modal('show');
        };
        function openModalHideDelete() {
            $('body').removeClass().removeAttr('style'); $('.modal-backdrop').remove();
        };

        function turnRed() {
            $('.piyush:nth-child(0)').addClass('btn-danger');
            // $('.piyush').css("background-color", "yellow");
        }


    </script>

</asp:Content>
