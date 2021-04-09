<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="DemoMasterPage2_BotLogs, App_Web_xvedjsjb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <asp:ScriptManager runat="server">
                    </asp:ScriptManager>
                    <asp:UpdatePanel runat="server" style="width: 100%">
                        <ContentTemplate>
    <!-- Begin Page Content -->
    <div id="page-wrapper" class="container-fluid" >

        <!-- DataTables Example -->
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h2 class="m-0 font-weight-bold poptxt">BOT LOGS
                   <asp:ImageButton ID="ImageButton1" runat="server" Class="rotate refreshbtn" ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="ImageButton3_Command" />

                </h2>
            </div>
           
            <div class="card-body tabe-responsive">
                <div>
                <div class=" panel panel-default ">
                    <!-- /.panel-heading -->
                   
                                        <div class="panel-body ">
                                            <asp:Repeater ID="GVLog" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-striped table-bordered table-hover" width="100%" id="dataTables-logs">
                                                        <thead class="colorSidebar" align="center">
                                                            <tr class="poptxt TableHeaderFont">

                                                                <th scope="col" class=" font-weight-bold ">User Name
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold ">TimeStamp
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold">Message
                                                                </th>
                                                                  <th scope="col" class=" font-weight-bold ">Machine Name
                                                                </th>
                                                                  <th scope="col" class=" font-weight-bold ">Bot Name
                                                                </th>
                                                                  <th scope="col" class=" font-weight-bold ">Process Name
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr class="poptxtdata">

                                                        <td class="odd gradeX" style="text-align: center;">
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("UserName") %>' />
                                                        </td>
                                                        <td class="odd gradeX " style="text-align: center;">
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("TimeStampValue", "{0:dd/MMM/yyyy HH:mm:ss}") %>' />
                                                        </td>
                                                        <td class="odd gradeX " style="text-align: center;">
                                                            <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("MessageValue") %>' />
                                                        </td>
                                                              <td class="odd gradeX " style="text-align: center;">
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("Machine") %>' />
                                                        </td>
                                                        <td class="odd gradeX " style="text-align: center;">
                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("BotName") %>' />
                                                        </td>
                                                        <td class="odd gradeX " style="text-align: center;">
                                                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("ProcessName") %>' />
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
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <!-- /.panel-body -->
            </div>
            <!-- /.panel -->
        </div>
        <!-- /.col-lg-12 -->
    </div>

    <!-- /#wrapper -->

    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script>
        $(document).ready(function () {
            $("#page-wrapper").delay(300).fadeIn(300);
            $('#dataTables-logs').DataTable({
                responsive: true
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    
                    $("#page-wrapper").show();
                    $('#dataTables-logs').DataTable({
                        responsive: true,
                        destroy:true
                    });
                }
            });
        };


        //$(document).ready(function () {
        //    var uri = window.location;
        //    var url = new URL(uri);
        //    var param = url.searchParams.get("BotName");
            
        //    $("#page-wrapper").delay(500).fadeIn(500);
        // var table= $('#dataTables-logs').DataTable({
        //        responsive: true
        // });
        //    if (param !== null) {
        //        table.search(param).draw();
        //    }
        //});


        //var prm = Sys.WebForms.PageRequestManager.getInstance();
        //if (prm != null) {
        //    prm.add_endRequest(function (sender, e) {
        //        if (sender._postBackSettings.panelsToUpdate != null) {
        //            $(".rotate").click(function () {
        //                $(this).toggleClass("down");
        //            })
        //            $("#page-wrapper").show();
        //            $('#dataTables-logs').DataTable({
        //                responsive: true
        //            });

        //        }
        //    });
        //};

        $(".rotate").click(function () {
            $(this).addClass("down");
        })

    </script>

</asp:Content>
