<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" enableeventvalidation="false" autoeventwireup="true" inherits="DemoMasterPage2_AddGroup, App_Web_shozqevm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
    <!-- Begin Page Content -->
    <div id="page-wrapper" class="container-fluid">

        <!-- Page Heading -->
        <%--<h1 class="h3 mb-2 text-gray-800">Process Management</h1>--%>

        <asp:UpdatePanel runat="server" ID="queueUpdatepanel">
            <ContentTemplate>
                <!-- DataTables Example -->

                <div class="card shadow mb-4">

                    <div class="card-header py-3">
                        <h2 class="m-0 font-weight-bold poptxt">Queues
                     <asp:ImageButton ID="ImageButton1" runat="server" Class="rotate refreshbtn " ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="ImageButton1_Command" />
                        </h2>
                    </div>
                    <div class="card-body">
                        <div>
                            <div class="card-header py-3" id="DivAddQueue" runat="server">
                                <asp:Button ID="Button2" data-toggle="modal" runat="server" align="center" class="btn  btn-block btn-primary btn-lg" Text="Add Queue" OnClick="openAddPopUp" />
                            </div>


                            <asp:Repeater ID="Repeater1" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-striped table-bordered table-hover" width="100%" id="dataTables-logs">
                                        <thead class="colorSidebar" align="center">
                                            <tr class="poptxt TableHeaderFont">

                                                <th scope="col" class=" font-weight-bold ">Queue Name
                                                </th>
                                                <th scope="col" class=" font-weight-bold ">Exchange Name
                                                </th>
                                                <th scope="col" class=" font-weight-bold text-danger">Delete Queue
                                                </th>
                                                <th scope="col" class=" font-weight-bold ">Purge Queue
                                                </th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="poptxtdata">

                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="lblQueueName" runat="server" Text='<%# Eval("queuename")  %>'></asp:Label>
                                        </td>
                                        <td class="odd gradeX " style="text-align: center;">
                                            <asp:Label ID="lblExchangeName" runat="server" Text='<%# Eval("exchangename") %>'></asp:Label>
                                        </td>
                                        <td class="odd gradeX " style="text-align: center;">
                                            <asp:ImageButton ID="btnDeleteQueue" ImageUrl="~/Images/Delete3.png" CommandName="Delete" CommandArgument='<%#Eval("queuename")%>' Text="Delete Queue" OnCommand="btnQueueAction" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                            <%--<asp:Button ID="btnPurgeQueue" runat="server" class="btn btn-warning  btn-sm" CommandName="Purge" CommandArgument='<%#Eval("queuename")%>' Text="Purge Queue"  OnCommand="btnQueueAction"/>--%>
                                            <asp:ImageButton ID="btnPurgeQueue" ImageUrl="~/Images/line.png" CommandName="Purge" CommandArgument='<%#Eval("queuename")%>' Text="Purge Queue" OnCommand="btnQueueAction" runat="server" />
                                        </td>

                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>


                        </div>
                    </div>
                </div>
                </div>

         
                     <div class="modal fade " id="modalDelete" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false" aria-labelledby="exampleModalLabel1" aria-hidden="true" data-backdrop="static" data-keyboard="false">
                         <div class="modal-dialog modal-lg" role="document" style="width: 600px; height: 140px;">
                             <div class="modal-content ">
                                 <div class="modal-header">
                                     <h5 class="modal-title h4 font-weight-bold text-danger" id="exampleModalLabel1">Delete Queue</h5>

                                     <asp:Button runat="server" data-dismiss="modal" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close"><%--data-dismiss="modal" aria-label="Close"--%> <%--OnClick="btnXclosePopup"--%>
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

                                                 <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 " align="right">
                                                     Queue Name:
                                                 </div>
                                                 <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold  text-danger" align="left">
                                                     <asp:Label ID="lblQueueNameDelete" runat="server"> </asp:Label>
                                                 </div>
                                                 <%-- <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 ">
                                                    </div>--%>
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


                <div class="modal fade" id="modalDeleteSecondPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelDeleteSecondPopUp" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">Delete Queue Permanently</h5>
                                <asp:Button ID="btnXdelete" runat="server" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop" />
                            </div>
                            <div class="modal-body Delete Process Permanently poptxt" style="text-align: center" runat="server">
                                You are about to delete the Queue permanently.Deleted Queue will not be recovered.<br />
                                Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?
                                <br />
                                <br />
                                <div class="form-group">

                                    <div class="container">

                                        <div class="row">
                                            <br />
                                        </div>

                                        <div class="row">

                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 ">
                                                Queue Name:
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold  text-danger">
                                                <asp:Label ID="lblQueueNameDeleteSec" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 ">
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
                                            <asp:Button ID="btnDeleteSecondpopup" runat="server" class="btn btn-danger btn-block" Text="Delete" OnClick="ModalPopUpBtnDelete_Click" />
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                            <asp:Button ID="btnCloseSecondPopUp" runat="server" class="btn btn-primary btn-block" Text="Close" OnClick="btnXdelete_clickHideBgPop" />
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="modal fade " id="modalPurge" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false" aria-labelledby="exampleModalLabel1" aria-hidden="true" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-lg" role="document" style="width: 600px; height: 140px;">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h4 font-weight-bold text-danger" id="exampleModalLabel1">Purge Queue</h5>

                                <asp:Button runat="server" data-dismiss="modal" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close"><%--data-dismiss="modal" aria-label="Close"--%> <%--OnClick="btnXclosePopup"--%>
                                </asp:Button>

                            </div>

                            <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                Are You Sure You Want to <span class=" font-weight-bold text-danger">Purge ?</span>
                                <br />
                                <div class="form-group">

                                    <div class="container">
                                        <div class="row">
                                            <br />
                                        </div>

                                        <div class="row">

                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 ">
                                                Queue Name:
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold  text-danger">
                                                <asp:Label ID="QueueName" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 ">
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
                                            <asp:Button ID="Button3" runat="server" class="btn btn-danger btn-block" Text="Purge" OnClick="CommandBtnPurge_Click" />
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

                <div class="modal fade" id="modalPurgeSecondPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelDeleteSecondPopUp" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">Purge Queue Permanently</h5>
                                <asp:Button ID="Button4" runat="server" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop" />
                            </div>
                            <div class="modal-body Delete Process Permanently poptxt" style="text-align: center" runat="server">
                                You are about to Purge the Queue permanently.Purge Queue will not be recovered.<br />
                                Are you sure you want to <span class=" font-weight-bold text-danger">Purge </span>?
                                <br />
                                <br />
                                <div class="form-group">

                                    <div class="container">

                                        <div class="row">
                                            <br />
                                        </div>

                                        <div class="row">

                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 ">
                                                Queue Name:
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold  text-danger">
                                                <asp:Label ID="QueueNameper" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 ">
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
                                            <asp:Button ID="Button5" runat="server" class="btn btn-danger btn-block" Text="Purge" OnClick="ModalPopUpBtnPurge_Click" />
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                            <asp:Button ID="Button6" runat="server" class="btn btn-primary btn-block" Text="Close" OnClick="btnXdelete_clickHideBgPop" />
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

        <asp:UpdatePanel runat="server">
            <ContentTemplate>


                <div class="modal fade" id="exampleModalCenter" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h4 font-weight-bold poptxt" id="exampleModalLabel">Processes</h5>
                                <asp:Button runat="server" class="close btn-danger" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop">
                                    <%-- <span aria-hidden="true">×</span>--%>
                                </asp:Button>
                            </div>
                            <div class="modal-body TableHeaderFont poptxt" style="align-content: center" runat="server">

                                <div class="form-group">
                                    <label for="txtQueueName" class="font-weight-bold">Queue Name</label>
                                    <asp:TextBox ID="txtQueueName" class="form-control " runat="server"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txtExchangeName" class="font-weight-bold">Binding Name</label>
                                    <asp:TextBox ID="txtExchangeName" class="form-control " Text="robot.x.automation" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>


                            <div class="modal-footer">
                                <asp:Button ID="button" runat="server" class="btn btn-danger" Text="Close" OnClick="btnXdelete_clickHideBgPop"></asp:Button>
                                <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Save Changes" OnClick="btnSaveQueue" />
                            </div>
                        </div>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>




    </div>

    <!-- Page-Level Demo Scripts - Tables - Use for reference -->

    <script>
        $(document).ready(function () {
            $("#page-wrapper").delay(100).fadeIn(300);
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

        function openModalAddpop() {
            $('#exampleModalCenter').modal('show');
        };

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
        }
        function openModalpurge() {
            $('#modalPurge').modal('show');
        }
        function openModalPurgeSecondPopUp() {
            $('#modalPurgeSecondPopUp').modal('show');
        }


    </script>
</asp:Content>
