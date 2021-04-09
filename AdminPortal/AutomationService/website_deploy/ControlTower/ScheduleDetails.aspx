<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="DemoMasterPage2_ScheduleDetails, App_Web_2o2bniex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <asp:ScriptManager runat="server">
                        </asp:ScriptManager>
     
    <!-- Begin Page Content -->
    <div class="container-fluid">
        <div id="page-wrapper">
            <asp:UpdatePanel runat="server">
                            <ContentTemplate>
        <!-- DataTales Example -->
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h2 class="m-0 font-weight-bold poptxt">Schedule Details Logs
                   <asp:ImageButton ID="ImageButton3" runat="server" Class="rotate refreshbtn" ImageUrl="~/Images/refresh3.png" ImageAlign="Right"  OnCommand="refreshCommand" />
                 </h2>
            </div>
            <div class="card-body">
                <div class="table">

                    <div class="panel-body colorFont">
                                <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick"></asp:Timer>
                                <div class="row">
                                </div>

                                <div class="row" align="center">
                                    <div class="col-sm-12">
                                        <asp:Button ID="btnPrevious" runat="server" CssClass="btn btn-dark buttoncss" Text="Previously Executed Processes" OnClick="btnPrevious_Click"  />&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnProcessing" runat="server" CssClass="btn btn-success buttoncss" Text="Currently Executing Processes" OnClick="btnProcessing_Click" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnFailed" runat="server" CssClass="btn btn-dark buttoncss" Text="Failed Executing Processes" Onclick="btnFailed_Click" />&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnScheduled" runat="server" CssClass="btn btn-dark buttoncss" Text="Scheduled Executable Processes" OnClick="btnScheduled_Click"  />
                                         <br /><br />
                                    </div>
                                </div>
                                <div class="row" align="center">
                                    <div class="col-sm-12 modal-title TableHeaderFont font-weight-bold poptxt">
                                        <asp:Label ID="lblPageName" runat="server" Text=""></asp:Label>
                                        <br />
                                    </div>
                                </div>

                                <div class="panel-body tableBody">
                                    <asp:Repeater ID="GrvSchedules" runat="server">
                                        <HeaderTemplate>
                                            <table class=" table-striped table-bordered table-hover dataTable no-footer dtr-inline tablecolor" id="dataTables-schedules" width="100%">
                                                <thead class="mastercolor colorSidebar" align="center">
                                                    <tr class="TableHeaderFont">

                                                        <th scope="col" class=" font-weight-bold poptxt">Bot Name
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold poptxt">Queue Name
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold poptxt">Cron Expression
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold poptxt">Status
                                                        </th>
                                                        <th scope="col" class="h5 font-weight-bold poptxt">Logs
                                                        </th>
                                                    </tr>
                                                </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" id="mainData " class="repeaterrow poptxtdata ">
                                                <td class="odd gradeX " style="text-align: center;" >
                                                    <asp:Label ID="lblBotName" name="tofindbot" runat="server" Text='<%# Eval("botname") %>' Height="12px" />
                                                </td>
                                                <td class="odd gradeX" style="text-align: center;">
                                                    <asp:Label ID="lblQueueName" runat="server" Text='<%# Eval("queuename") %>' />
                                                </td>
                                                <td class="odd gradeX" style="text-align: center;">
                                                    <asp:Label ID="lblCronExpr" runat="server" Text='<%# Eval("ChronExpression")  %>' />
                                                </td>
                                                <td class="odd gradeX" style="text-align: center;" onclick="Open('<%# Eval("botname") %>')">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Status") %>' />
                                                </td>
                                                <td style="text-align: center; align-content: center; align-items: center;">
                                                    <asp:ImageButton ID="ImageButton5" runat="server" CssClass="info" ImageUrl="~/Images/info3.png" ImageAlign="Middle" CommandArgument='<%#Eval("botname") + ","+Eval("machinename")+ ","+Eval("StartTime")+ ","+Eval("EndTime") %>' OnCommand="LoadBotLogs" />
                                                 </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <div id="output1">
                                        <textarea id="output" cols="20" rows="10" hidden="hidden"></textarea>
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                        <asp:HiddenField ID="HiddenField3" runat="server" />
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
                                 </ContentTemplate>
                        </asp:UpdatePanel>
    </div>
        </div>
                               
    <!-- /.container-fluid -->
    <!-- End of Main Content -->
    <!-- BotLog Modal-->

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="modal fade bd-example-modal-xl" id="botlogModal" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-xl" role="document" >
                    <div class="modal-content ">
                        <div class="modal-header">
                            <h5 class="modal-title h4 font-weight-bold text-primary" id="exampleModalLabel">Bot Log</h5>
                            <button id="ClosePop" class="close" type="button" aria-label="Close" data-dismiss="modal">
                                <span aria-hidden="true">×</span>
                            </button>
                        </div>

                        <div class="modal-body " runat="server">
                            <asp:Repeater ID="Repeater2" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-striped table-bordered table-hover"  id="dataTables-BotDashboard">
                                        <thead class="mastercolor colorSidebar" align="center">
                                            <tr>
                                                <th scope="col" class="h5 font-weight-bold text-primary">UserName</th>
                                                <th scope="col" class="h5 font-weight-bold text-primary">Machine Name
                                                </th>
                                                <th scope="col" class="h5 font-weight-bold text-primary">Bot Name
                                                </th>
                                                <th scope="col" class="h5 font-weight-bold text-primary">Process Name
                                                </th>
                                                <th scope="col" class="h5 font-weight-bold text-primary">Time Stamp

                                                </th>
                                                <th scope="col" class="h5 font-weight-bold text-primary">Log Level
                                                </th>
                                                <th scope="col" class="h5 font-weight-bold text-primary">Message Value
                                                </th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" id="TableData" class="repeaterrow pre-scrollable text-muted" >

                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="lblBotName" name="tofindbot" Style="" runat="server" Text='<%# Eval("UserName") %>' Height="12px" />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="lblMachineName" runat="server" Style="" Text='<%# Eval("Machine") %>' />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center">
                                            <asp:Label ID="lblTotalRequestServed" Style="" runat="server" Text='<%# Eval("BotName") %>' />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="Label3" Style="" runat="server" Text='<%# Eval("ProcessName") %>' />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="lblProcessName" Style="" runat="server" Text='<%# Eval("TimeStampValue") %>' />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="lblLogLevel" Style="" runat="server" Text='<%# Eval("LogLevel") %>' />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="lblMessageValue" Style="" runat="server" Text='<%# Eval("MessageValue") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="modal-footer">
                            <button id="ClosePop2" class="btn btn-danger" type="button" data-dismiss="modal">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!--End of Modal Pop Up for Bot Logs-->

    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script type="text/javascript">

        $(document).ready(function () {
            //$("#dataTables-schedules").delay(5000).fadeIn(5000);
            $('#dataTables-schedules').DataTable({
                responsive: true,
            });

            $("#page-wrapper").delay(5000).fadeIn(5000);
            //$("#BotDashBoardPage").delay(800).fadeIn(400);
            $('#TableData');

            $("#page-wrapper").show();

        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $(".rotate").click(function () {
                        $(this).toggleClass("down");
                    })
                    $("#page-wrapper").show();
                    $('#dataTables-schedules').DataTable({
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
    <script type="text/javascript">
                                                          function Open(BotName) {
                                                              //Window.location.replace("BotLogs.aspx");
                                                              window.location.replace("BotLogs.aspx?BotName=" + BotName + "");
                                                          }

                                                          function showModal() {
                                                              $('#botlogModal').modal('show');
                                                          }


        window.onload = function () {



            if (window.location.hash === "#Previous") {
                document.getElementById('<%= btnPrevious.ClientID %>').click();
                // window.location.hash = "";
                history.replaceState(null, null, ' ');

            }
            else if (window.location.hash === "#Failed") {
                document.getElementById('<%= btnFailed.ClientID %>').click();
                history.replaceState(null, null, ' ');
            }
            else if (window.location.hash === "#Scheduled") {
                document.getElementById('<%= btnScheduled.ClientID %>').click();
                history.replaceState(null, null, ' ');
            }



        }

    </script>

</asp:Content>

