<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="DemoMasterPage2_Configurations, App_Web_2o2bniex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <link href="../dist/css/cssforConfig.css" rel="stylesheet">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager runat="server">
    </asp:ScriptManager>

    <!-- Begin Page Content -->
    <div id="page-wrapper" class="container-fluid">

        <!-- Page Heading -->
        <%--<h1 class="h3 mb-2 text-gray-800">Process Management</h1>--%>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <!-- DataTables Example -->
                <div id="DivConfiguarationManagement" class="card shadow mb-4" runat="server" >
                    <div class="card-header py-3">
                        <h2 class="m-0 font-weight-bold poptxt">CONFIGURATION MANAGEMENT
                        </h2>
                    </div>

                    <div class="card-body TableHeaderFont">
                        <div>
                            <div class="form-group">
                                <asp:Label ID="Label4" runat="server" Text="Label" class=" font-weight-bold poptxt">
                        Parameter Name<span class="text-danger">*</span>
                                </asp:Label>

                                <asp:TextBox ID="txtParameterName" class="form-control input-sm" runat="server" Width="100%"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label5" runat="server" Text="Label" class=" font-weight-bold poptxt">
                        Access Level<span class="text-danger">*</span>
                                </asp:Label>
                                <asp:DropDownList ID="DrpProcesses" runat="server" class="form-control input-sm" AppendDataBoundItems="True"
                                    AutoPostBack="True" Width="100%">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Global</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-group">

                                <asp:Label ID="Label1" runat="server" Text="Label" class=" font-weight-bold poptxt">
                        Parameter Value<span class="text-danger">*</span>
                                </asp:Label>


                                <asp:TextBox ID="txtParameterValue" class="form-control input-sm" runat="server" Width="100%"></asp:TextBox>
                            </div>
                            <div class="form-group" align="center">
                                <asp:Button ID="btnSave" runat="server" class="btn btn-primary colorSidebar" Text="Save" OnClick="btnsave_Click" Width="49%" />

                                <asp:Button ID="Button2" runat="server" class="btn btn-danger" Text="Cancel" OnClick="BtnCancelConfiguration" Width="49%" />

                            </div>

                            <div class="form-group">
                            </div>
                        </div>
                        <br />

                    </div>
                </div>
                <%--</div>--%>
                <%-- </ContentTemplate>
                        </asp:UpdatePanel>
<asp:UpdatePanel runat="server">
   <ContentTemplate>--%>
                <br />

                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h2 class="m-0 font-weight-bold poptxt">CONFIGURATION DETAILS
                   <asp:ImageButton ID="ImageButton1" runat="server" Class="rotate refreshbtn" ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="ImageButton3_Command" />

                        </h2>
                    </div>

                    <div class="card-body">
                        <div>

                            <%--<div class="panel-body">--%>
                            
                            <asp:Repeater ID="GVConfigParameters" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-striped table-bordered table-hover" id="dataTables-botqueuemapping" width="100%">
                                        <thead class="colorSidebar">
                                            <tr class="poptxt TableHeaderFont">
                                                <th scope="col" style="font-weight: normal; text-align: center;" class=" font-weight-bold ">Parameter Name
                                                </th>
                                                <th scope="col" style="font-weight: normal; text-align: center;" class=" font-weight-bold ">Parameter Value
                                                </th>
                                                <th scope="col" style="font-weight: normal; text-align: center;" class=" font-weight-bold ">Access Level
                                                </th>
                                                <th scope="col" style="font-weight: normal; text-align: center;" class=" font-weight-bold text-danger">Delete
                                                </th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="poptxtdata">
                                        <td class="odd gradeX " style="text-align: center;">
                                            <asp:Label ID="lblParameterName" name="PatameterName" Text='<%# Eval("parametername") %>' runat="server" Height="12px" />
                                        </td>
                                        <td class="odd gradeX " style="text-align: center;">
                                            <asp:Label ID="lblParameterValue" Text='<%# Eval("parametervalue") %>' runat="server" />
                                        </td>
                                        <td class="odd gradeX " style="text-align: center;">
                                            <asp:Label ID="lblAccessLevel" runat="server" Text='<%# Eval("processname") %>' />
                                        </td>
                                        <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">

                                            <asp:ImageButton ID="btnDeleteConfiguration" runat="server" ImageUrl="~/Images/Delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("parameterid")+","+ Eval("parametername")+","+ Eval("parametervalue")%>' OnCommand="btnDelete_Click" />
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
                            <%--</div>--%>
                        </div>
                    </div>
                </div>


            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

    <%-- Delete configuration Modal Start--%>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="modal fade " id="modalDelete" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content ">
                        <div class="modal-header">
                            <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">Delete Configuration</h5>
                            <asp:Button class="close" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" data-dismiss="modal"></asp:Button>
                        </div>

                        <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                            Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?
                                                <br />
                            <br />
                            <div class="form-group">

                                <div class="container">
                                    <div class="row">
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                            Parameter Name :
                                        </div>
                                        <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2" align="left">
                                            <asp:Label ID="lblParameterName" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                            <asp:Label ID="lblParameterId" Class="text-success" runat="server" hidden> </asp:Label>
                                        </div>
                                        <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                            Parameter Value :
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="left">
                                            <asp:Label ID="lblParameterValue" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
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
    <%--End Delete configuration--%>


    <%-- Delete configuration Modal Start  Second PopUp--%>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="modal fade " id="modalDeleteSecondPopUp" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelDeleteSecondPopUp" aria-hidden="true">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content ">
                        <div class="modal-header">
                            <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">Delete Configuration Permanently</h5>
                            <asp:Button class="close" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop"></asp:Button>
                        </div>

                        <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                            You are about to delete the configuration permanently.Deleted configuration will not be recovered.<br />
                            Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?

                                                <br />
                            <br />
                            <div class="form-group">

                                <div class="container">
                                    <div class="row">
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                            Parameter Name :
                                        </div>
                                        <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2" align="left">
                                            <asp:Label ID="lblParameterNameSecondPopUp" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                            <asp:Label ID="lblParameterIdSecondPopUp" Class="text-success" runat="server" hidden> </asp:Label>
                                        </div>
                                        <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                            Parameter Value :
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="left">
                                            <asp:Label ID="lblParameterValueSecondPopUp" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
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
                                        <asp:Button ID="Button1" runat="server" class="btn btn-danger btn-block" Text="Delete" OnClick="ModalPopUpBtnDelete_ClickSecondPopUp" />
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
    <%--End Delete configuration  Second PopUp--%>









    <!-- /.panel-body -->
    </div>
                <!-- /.panel -->
    </div>
            <!-- /.col-lg-12 -->
    </div>
    </div>


    <!-- /#wrapper -->

    <!-- jQuery -->
    <script src="../vendor/jquery/jquery.min.js"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="../vendor/bootstrap/js/bootstrap.min.js"></script>

    <!-- Metis Menu Plugin JavaScript -->
    <script src="../vendor/metisMenu/metisMenu.min.js"></script>

    <!-- DataTables JavaScript -->
    <script src="../vendor/datatables/js/jquery.dataTables.min.js"></script>
    <script src="../vendor/datatables-plugins/dataTables.bootstrap.min.js"></script>
    <script src="../vendor/datatables-responsive/dataTables.responsive.js"></script>

    <!-- Custom Theme JavaScript -->
    <script src="../dist/js/sb-admin-2.js"></script>

    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script>
        $(document).ready(function () {
            $("#page-wrapper").delay(500).fadeIn(500);
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
        function openModalHideDelete() {
            $('body').removeClass().removeAttr('style'); $('.modal-backdrop').remove();
        };

    </script>

</asp:Content>
