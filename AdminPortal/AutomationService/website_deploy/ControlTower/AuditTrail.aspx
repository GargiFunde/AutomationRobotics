<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="AuditTrail, App_Web_2o2bniex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../dist/css/cssforAuditTrail.css" rel="stylesheet">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>


    <!-- Begin Page Content -->
    <div id="page-wrapper" class="container-fluid">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <!-- Page Heading -->
                <%-- <h1 class="h3 mb-2 text-gray-800">Process Management</h1>--%>

                <!-- DataTables Example -->
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h2 class="m-0 font-weight-bold poptxt">AUDIT TRAIL
                   <asp:ImageButton ID="ImageButton1" runat="server" Class="rotate refreshbtn" ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="ImageButton3_Command" />

                        </h2>
                    </div>

                    <div class="card-body">
                        <div>
                            <div class="panel panel-default colorFont">
                                <div>
                                    <div class="auto-style17">
                                        <div class="panel panel-default">
                                            <div class="panel-body">
                                                <asp:Repeater ID="GVLog" runat="server">
                                                    <HeaderTemplate>
                                                        <table class="table table-striped table-bordered table-hover" id="dataTables-logs" width="100%">
                                                            <thead class="colorSidebar" align="center">
                                                                <tr class="poptxt TableHeaderFont">

                                                                    <th scope="col" class=" font-weight-bold ">User Name
                                                                    </th>
                                                                    <th scope="col" class=" font-weight-bold">Date
                                                                    </th>
                                                                    <th scope="col" class=" font-weight-bold ">Details
                                                                    </th>

                                                                </tr>
                                                            </thead>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="poptxtdata">

                                                            <td class="odd gradeX " style="text-align: center">
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("userid") %>' />
                                                            </td>
                                                            <td class="odd gradeX " style="text-align: center">
                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("date", "{0:dd/MMM/yyyy HH:mm:ss}") %>' />
                                                            </td>
                                                            <td class="odd gradeX " style="text-align: center">
                                                                <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("details") %>' />
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
                                            <!-- /.panel-body -->
                                        </div>
                                        <!-- /.panel -->
                                    </div>
                                    <!-- /.col-lg-12 -->
                                </div>



                            </div>
                        </div>

                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function () {
            $("#page-wrapper").delay(500).fadeIn(500);
            $('#dataTables-logs').DataTable({
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
                    $('#dataTables-logs').DataTable({
                        responsive: true,
                        destroy: true
                    });

                }
            });
        };

        $(".rotate").click(function () {


            $(this).addClass("down");

        })

    </script>

</asp:Content>
