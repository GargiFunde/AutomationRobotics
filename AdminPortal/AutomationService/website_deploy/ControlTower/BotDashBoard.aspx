<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="DemoMasterPage2_BotDashBoard, App_Web_2o2bniex" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <%-- <script src="../Scripts/sockjs.js" type="text/javascript"></script>
    <script src="../Scripts/stomp.js" type="text/javascript"></script>--%>
    <%--<script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
     <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>  
    <script src="cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js" />--%>

    
    
    <%--<script src="js/jquery.min.js"></script>--%>
   



  <%--  <script type="text/javascript">
        var ws;
        var client;
        var output;
        window.onload = function () {
            //var input1 = document.getElementById("MainContent_HiddenField1").value;
            //var input2 = document.getElementById("MainContent_HiddenField2").value;
            //var input3 = document.getElementById("MainContent_HiddenField3").value;
            //document.getElementById("MainContent_HiddenField1").value = "";
            //document.getElementById("MainContent_HiddenField2").value = "";
            //document.getElementById("MainContent_HiddenField3").value = "";


            //var input1 = document.getElementById("HiddenField1").value;
            //var input2 = document.getElementById("HiddenField2").value;
            //var input3 = document.getElementById("HiddenField3").value;
            //document.getElementById("HiddenField1").value = "";
            //document.getElementById("HiddenField2").value = "";
            //document.getElementById("HiddenField3").value = "";

            var input1 = "<%=HiddenField1.Value%>";
            var input2 = "<%=HiddenField2.Value%>";
            var input3 = "<%=HiddenField3.Value%>";

            // ws = new SockJS('http://' + window.location.hostname + input3);
            //client = Stomp.over(ws);
            client = Stomp.client('ws://' + window.location.hostname + ':15674/ws');
            //client = Stomp.client('ws://' + '192.168.50.81' + ':15674/ws');
            //  ws = new SockJS('http://192.168.50.82' + input3);

            output = document.getElementById("output");

            client.connect(input1, input2, on_connect, on_error, '/');
            //client.connect("se", "se", on_connect, on_error, '/');
           
            //client.connect('<%=HiddenField1.Value%>',' <%=HiddenField2.Value%>', on_connect, on_error, '/');

        }

        function on_connect(x) {

            output.value += 'Connected to rabbitMQ web-Stomp<br />';
            client.subscribe("/topic/mymessage", on_receive); //standard topic and routingkey

        };

        function on_error() {
            output.value += 'Connection Failed<br />';

        };

        function on_receive(x) {

            output.value += x.body + '<br />'

            var obj = JSON.parse(x.body);
            if (obj.roboExecutionStatus == 1) //launching - dark grey
            {
                obj.RoboColor = '#FFCC00';
            }
            else if (obj.roboExecutionStatus == 2) {//launch failed - dark red
                obj.RoboColor = '#990000';
            }
            else if (obj.roboExecutionStatus == 3) { //ready & idle - Yellow
                //obj.RoboColor = '#FFCC00';
                obj.RoboColor = '#008000';
            }
            else if (obj.roboExecutionStatus == 4) {//processing Automation - Green
                obj.RoboColor = '#008000';
            }
            else if (obj.roboExecutionStatus == 5) {//Automation Completed - Yellow
                obj.RoboColor = '#FFCC00';
            }
            else if (obj.roboExecutionStatus == 6) {//RobotAutomationFailed - brown red 
                obj.RoboColor = '#800000';
            }
            else if (obj.roboExecutionStatus == 6) {//RobotStopping -  red 
                obj.RoboColor = '#FF0000';
            }

            var spans = document.getElementsByName('tofindbot');
            var l = spans.length;

            //Iteration of SPAN in DOM  
            for (var i = 0; i < l; i++) {
                var strText = spans[i].innerText;
                var robotName = obj.RobotName;

                if (strText == robotName) {
                    var strid = spans[i].id;
                    var divs = document.getElementsByName('divStatus');
                    divs[i].style.backgroundColor = obj.RoboColor;

                    /*Code for toggling play and Stop Button */
                    var dcx = document.getElementsByClassName('btnStart');
                    var dcy = document.getElementsByClassName('btnStop');
                    //dcx[i].style.backgroundColor = obj.RoboColor;
                    dcx[i].style.display = "none";
                    if (obj.RoboColor == "#FF0000") {
                        divs[i].style.backgroundColor = "#FFCC00";
                        dcx.style.display = "inline";
                    }
                    if (obj.RoboColor == "#008000") {
                        dcy[i].style.display = "inline"
                        
                    }
                    /*Code change Complete */

                    var reslblMachine = strid.replace("lblBotName", "lblMachineName");
                    var lblMachine = document.getElementById(reslblMachine);
                    if (lblMachine != null) {
                        lblMachine.innerText = obj.MachineName;
                    }

                    var reslblProcess = strid.replace("lblBotName", "lblProcessName");
                    var lblProcess = document.getElementById(reslblProcess);
                    if (lblProcess != null) {
                        lblProcess.innerText = obj.ProcessName;
                    }

                    var reslblTotalRequest = strid.replace("lblBotName", "lblTotalRequestServed");
                    var lblTotalRequest = document.getElementById(reslblTotalRequest);
                    if (lblTotalRequest != null) {
                        lblTotalRequest.innerText = obj.TotalRequestServed;
                    }

                    break;
                }

            }
        };

        window.setTimeout(function () {
            $(".alert1").fadeTo(200, 0).slideUp(500, function () {
                $(this).remove();
            });
        }, 4000);

      

    </script>--%>

  <%-- <script type="text/javascript">
       window.onload = function (e) { 
           
           var yourVariable = '<%= Session["TenantId"] %>';
           alert(yourVariable);
}  
       </script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% try
        { %>
    <!-- Begin Page Content -->
    <div id="DashContainerId" class="container-fluid" width="100%"  >
        
        <div class="col-md-12 col-lg-12 col-sm-12 col-xm-12" id="alertSuccessLogin" runat="server">
        </div>

        <div class="alert alert-warning" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            Please Press <strong><i class="fas fa-play"></i></strong> Button to Start Process and <i class="fas fa-info-circle"></i> button to check the Logs.
        </div>


        <!-- Content Row -->
        <%--<div class="row">--%>

            <!-- Total Processes Card Example -->
          <%--  <div id="cardProcess" class="col-xl-3 col-md-6 mb-4" runat="server" data-toggle="tooltip" data-placement="bottom" title="Check Running Processes">
                <div class="card border-left-danger shadow h-100 py-2 card bg-primary text-white shadow">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold  text-uppercase mb-1">Total Processes</div>
                                <div class="h5 mb-0 font-weight-bold ">
                                    <asp:Label ID="lblTotalProcesses" runat="server" Text="Label"></asp:Label>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-robot fa-2x text-gray-300 "></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>

            <!-- Successful Processes Card Example -->
           <%-- <div class="col-xl-3 col-md-6 mb-4" runat="server" data-toggle="tooltip" data-placement="bottom" title="Check Running Processes">
                <div class="card border-left-primary shadow h-100 py-2 card bg-success text-white shadow">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold  text-uppercase mb-1">Successful Processes</div>
                                <div class="h5 mb-0 font-weight-bold ">
                                   
                                </div>

                                <div class="row no-gutters align-items-center">
                                    <div class="col-auto">
                                        <div class="h5 mb-0 mr-3 font-weight-bold ">
                                            <asp:Label ID="lblSuccessfulProcess" runat="server" Text="Label"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="progress progress-sm mr-2">
                                            <div runat="server" id="successprogressbar" class="progress-bar bg-primary" role="progressbar" style="width: 0%" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                    </div>
                                    <div class="col-xs-1 center-block" align="center">
                                        <asp:Label ID="lblSuccessPercent" runat="server" Text="Label"></asp:Label>
                                    </div>
                                </div>

                            </div>
                            <div class="col-auto">
                                <i class="fas fa-rocket fa-2x text-gray-300 "></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>

            <!-- Processes Failed Card Example -->
           <%-- <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-left-success shadow h-100 py-2 card bg-danger text-white shadow">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-uppercase mb-1">Processes Failed</div>
                                <div class="row no-gutters align-items-center">
                                    <div class="col-auto">
                                        <div class="h5 mb-0 mr-3 font-weight-bold text-red-500">
                                            <asp:Label ID="lblFailedPercentage" runat="server" Text="Label"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="progress progress-sm mr-2">
                                            <div id="failedProgressBar" runat="server" class="progress-bar bg-success" role="progressbar" style="width: 50%" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-clipboard-list fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>

            <!-- Schedules Card Example -->
          <%--  <div id="cardSchedule" class="col-xl-3 col-md-6 mb-4" data-toggle="tooltip" data-placement="bottom" title="Check Scheduled Processes">
                <div class="card border-left-warning shadow h-100 py-2 card bg-secondary text-white shadow">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold  text-uppercase mb-1">Schedules</div>
                                <div class="h5 mb-0 font-weight-bold ">
                                    <asp:Label ID="lblScheduleCount" runat="server" Text="Label"></asp:Label>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-clock fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
        <%--</div>--%>

        <div class="row">
            <!-- Total Processes Card Example -->
            <div id="cardProcess" class="col-xl-3 col-md-6 mb-4" runat="server" data-toggle="tooltip" data-placement="bottom" title="Check Running Processes">
                <div class="card border-left-danger shadow h-100 py-2 card bg-primary text-white shadow">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold  text-uppercase mb-1">Total Executed Processes</div>
                                <div class="h5 mb-0 font-weight-bold ">
                                    <asp:Label ID="lblTotalProcesses" runat="server" Text="Label"></asp:Label>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-robot fa-2x text-gray-300 "></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Successful Processes Card Example -->
            <div id="cardSuccess" class="col-xl-3 col-md-6 mb-4" runat="server" data-toggle="tooltip" data-placement="bottom" title="Check Running Processes">
                <div class="card border-left-primary shadow h-100 py-2 card bg-success text-white shadow">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold  text-uppercase mb-1">Successful Processes</div>
                                <div class="h5 mb-0 font-weight-bold ">
                                    <%-- <asp:Label ID="lblSuccessfulProcess" runat="server" Text="Label"></asp:Label>--%>
                                </div>

                                <div class="row no-gutters align-items-center">
                                    <div class="col-auto">
                                        <div class="h5 mb-0 mr-3 font-weight-bold ">
                                            <asp:Label ID="lblSuccessfulProcess" runat="server" Text="Label"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="progress progress-sm mr-2">
                                            <div runat="server" id="successprogressbar" class="progress-bar bg-primary" role="progressbar" style="width: 0%" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                    </div>
                                    <div class="col-xs-1 center-block" align="center">
                                        <asp:Label ID="lblSuccessPercent" runat="server" Text="Label"></asp:Label>
                                    </div>
                                </div>

                            </div>
                            <div class="col-auto">
                                <i class="fas fa-rocket fa-2x text-gray-300 "></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Processes Failed Card Example -->
            <div id="cardFailed" class="col-xl-3 col-md-6 mb-4" runat="server">
                <div class="card border-left-success shadow h-100 py-2 card bg-danger text-white shadow">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-uppercase mb-1">Processes Failed</div>
                                <div class="row no-gutters align-items-center">
                                    <div class="col-auto">
                                        <div class="h5 mb-0 mr-3 font-weight-bold text-red-500">
                                            <asp:Label ID="lblFailedPercentage" runat="server" Text="Label"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="progress progress-sm mr-2">
                                            <div id="failedProgressBar" runat="server" class="progress-bar bg-success" role="progressbar" style="width: 50%" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-clipboard-list fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Schedules Card Example -->
            <div id="cardSchedule" class="col-xl-3 col-md-6 mb-4" data-toggle="tooltip" data-placement="bottom" title="Check Scheduled Processes" runat="server">
                <div class="card border-left-warning shadow h-100 py-2 card bg-secondary text-white shadow">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold  text-uppercase mb-1">Schedules</div>
                                <div class="h5 mb-0 font-weight-bold ">
                                    <asp:Label ID="lblScheduleCount" runat="server" Text="Label"></asp:Label>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-clock fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Content Row -->
        <asp:ScriptManager runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <!-- DataTales Example -->
        <div id="page-wrapper"  class="container-fluid" >
            <div id="BotDashBoardPage" class="card shadow row" >
                <div class="card-header py-3">
                    <h2 class="m-0 font-weight-bold poptxt">BOTS
                   <asp:ImageButton ID="ImageButton3" runat="server" Class="rotate " ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="refreshBotDashboard" />
                    </h2>
                </div>
                <div class="card-body table-responsive"  >
                    <div class=" panel panel-default ">
                        
                                <div class="panel-body colorFont">
                                    <%-- <form runat="server">--%>
                                    <asp:Repeater ID="rptBots" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-striped table-bordered" width="100%" id="dataTables-BotDashboard" >
                                                <thead class="mastercolor colorSidebar" align="center">
                                                    <tr class="TableHeaderFont">
                                                        <th scope="col"  runat="server" class=" font-weight-bold poptxt">Status&nbsp;</th>
                                                        <th scope="col" class=" font-weight-bold tpoptxt">Bot Name
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold poptxt">Machine Name
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold poptxt">Req Served In Session
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold poptxt">Executing Automation 
                                                        </th>
                                                         <%if ((Session["Role"].ToString()) == "Admin" || (Session["Role"].ToString()) == "SuperAdmin")
                                                            { %>
                                                         <th scope="col" class=" font-weight-bold poptxt">Group Id 
                                                        </th>
                                                        <% } %>
                                                         <%if ((Session["Role"].ToString()) == "SuperAdmin")
                                                            { %>
                                                         <th scope="col" class=" font-weight-bold poptxt">Tenant Id 
                                                        </th>
                                                        <% } %>
                                                        <th scope="col" class=" font-weight-bold poptxt">Action
                                                        </th>
                                                    </tr>
                                                </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" id="TableData" class="repeaterrow  tabrowcolor">
                                                <td style="text-align: center; align-content: center; align-items: center; align-self: center; padding-left: 10px;" align="center">
                                                    <%--<div name ="divStatus" style="align-self:center; align-content:center; background-color: #999999;" class="circleBase type" ></div>--%>
                                                    <button class="btn btn-circle btn-default" name="divStatus" style="background-color: #999999; width: 17px; height: 25px"
                                                        type="button">
                                                    </button>
                                                </td>
                                                <td class="odd gradeX" style="text-align: center;">
                                                    <dt><asp:Label ID="lblBotName"  name="tofindbot" Style="" runat="server" Text='<%# Eval("botname") %>' Height="12px" /></dt>
                                                </td>
                                                <td class="odd gradeX" style="text-align: center;">
                                                    <asp:Label ID="lblMachineName" runat="server" Style="" Text='<%# Eval("machinename") %>' />
                                                </td>
                                                <td class="odd gradeX" style="text-align: center">
                                                    <asp:Label ID="lblTotalRequestServed" Style="" runat="server" />
                                                </td>
                                                <td class="odd gradeX" style="text-align: center;">
                                                    <asp:Label ID="lblProcessName" Style="" runat="server" />
                                                </td>
                                                <%if ("Admin" == Convert.ToString(Session["Role"]) || "SuperAdmin" == Convert.ToString(Session["Role"]))
                                                    { %>
                                                <td class="odd gradeX" style="text-align: center;">
                                                    <asp:Label ID="lblGroupId" Style="" runat="server" Text='<%# Eval("groupid") %>' />
                                                </td>
                                                <% } %>
                                                <%if (Convert.ToString(Session["Role"]) == "SuperAdmin")
                                                    { %>
                                                <td class="odd gradeX" style="text-align: center;">
                                                    <asp:Label ID="lblTenantId" Style="" runat="server" Text='<%# Eval("tenantid") %>' />
                                                </td>
                                                <% } %>
                                                <td style="text-align: center; align-content: center; align-items: center;" data-toggle="popover" data-trigger="hover" data-content="Some content">
                                                    <%--<td style="text-align: left; align-content: center; align-items: center; padding-left: 20px; padding-top: 2px; padding-bottom: 2px;">--%>

                                                    <asp:ImageButton ID="StartButton" runat="server" CssClass="btnStart" ImageUrl="~/Images/play3.png" ImageAlign="Middle" CommandName="Start" CommandArgument='<%#Eval("botname") + ","+Eval("machinename") %>' OnCommand="StartProcess" />
                                                    <asp:ImageButton ID="StopButton" runat="server" CssClass="btnStop" ImageUrl="~/Images/stop3.png" ImageAlign="Middle" CommandName="Stop" CommandArgument='<%#Eval("botname") + ","+Eval("machinename") %>' OnCommand="StopProcess" />
                                                    <asp:ImageButton ID="InfoButton" runat="server" CssClass="info " ImageUrl="~/Images/info3.png" ImageAlign="Middle" CommandArgument='<%#Eval("botname") + ","+Eval("machinename") %>' OnCommand="LoadBotLogs" />
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>

                                    <div id="output1">
                                        <textarea id="output" cols="5" rows="10" hidden="hidden"></textarea>
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                        <asp:HiddenField ID="HiddenField3" runat="server" />
                                    </div>
                                </div>

                          </div>
                     </div>
                </div>
            </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <!-- Logs For Bot Modal-->
                        <asp:UpdatePanel ID="one" runat="server">
                            <ContentTemplate>
                                <div class="modal fade " id="botlogModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false" >
                                    <div class="modal-dialog modal-xl" role="document" style="width:290%" >
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title h4 font-weight-bold poptxt" id="exampleModalLabel">Bot Log</h5>
                                                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true">×</span>
                                                </button>
                                            </div>

                                            <div class="modal-body " runat="server">
                                                <asp:Repeater ID="Repeater2" runat="server">
                                                    <HeaderTemplate>
                                                        <table class="table table-striped table-bordered table-hover" id="dataTables-BotLogs" >
                                                            <thead class="mastercolor colorSidebar" align="center">
                                                                <tr class="TableHeaderFont">
                                                                    <th scope="col" class=" font-weight-bold poptxt">UserName</th>
                                                                    <th scope="col" class=" font-weight-bold poptxt">Machine Name
                                                                    </th>
                                                                    <th scope="col" class=" font-weight-bold poptxt">Bot Name
                                                                    </th>
                                                                    <th scope="col" class=" font-weight-bold poptxt">Process Name
                                                                    </th>
                                                                    <th scope="col" class=" font-weight-bold poptxt">Time Stamp
                                                                    </th>
                                                                    <th scope="col" class=" font-weight-bold poptxt">Log Level
                                                                    </th>
                                                                    <th scope="col" class=" font-weight-bold poptxt">Message Value
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" id="TableData" class="repeaterrow pre-scrollable poptxtdata">

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
                                                                <asp:Label ID="Label1" Style="" runat="server" Text='<%# Eval("LogLevel") %>' />
                                                            </td>
                                                            <td class="odd gradeX" style="text-align: center;">
                                                                <asp:Label ID="Label2" Style="" runat="server" Text='<%# Eval("MessageValue") %>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </table>
                                                    </FooterTemplate>



                                                </asp:Repeater>
                                            </div>
                                            <div class="modal-footer">
                                                <button class="btn btn-danger" type="button" data-dismiss="modal">Cancel</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
    
    <% }
        catch (Exception ex)
        {
            Console.WriteLine("Piysuh : "+ex.Message);
            // serviceInterface.insertLog("Logging Out Forcefully","Exception : "+ex.Message,0,0);
            //serviceInterface.insertActivityLog(Convert.ToString(Session["DomainName"]), Convert.ToString(Session["UserName"]), Convert.ToString(Session["GroupName"]), "Logged Out From BotDashBoard Page", Convert.ToInt32(Session["GroupId"]), Convert.ToInt32(Session["TenantId"]));
        }%>
      
    <!-- /.container-fluid -->

    <%--        </div>--%>
    <!-- End of Main Content -->

    
    <script type="text/javascript">

        function showModal() {
            $('#botlogModal').modal('show');
        }

        $(".rotate").click(function () {
            $(this).addClass("down");
        });

        $(document).ready(function () {
            $(".info").css("visibility", "hidden");
            $(".repeaterrow").hover(function () {
                $(this).find(".info").css("visibility", "visible"); //.fadeToggle(50); //locating the associated .slet and not the all one.
                //$(".info").css("visibility","visible");
            });
            $(".repeaterrow").mouseleave(function () {
                $(".info").css("visibility", "hidden");
            })

            $("#page-wrapper").delay(100).fadeIn(300);
            $('#dataTables-BotDashboard').DataTable({
                responsive: true,
                destroy: true
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
                    $('#dataTables-BotDashboard').DataTable({
                        responsive: true,
                        destroy:true
                    });
                    $(".info").css("visibility", "hidden");
                    $(".repeaterrow").hover(function () {
                        $(this).find(".info").css("visibility", "visible"); //.fadeToggle(50); //locating the associated .slet and not the all one.
                        //$(".info").css("visibility","visible");
                    });
                    $(".repeaterrow").mouseleave(function () {
                        $(".info").css("visibility", "hidden");
                    })
                }
            });
        };

        function previousSchedule() {

            window.location = "ScheduleDetails.aspx#Previous";
        }
        function failedSchedule() {
            window.location = "ScheduleDetails.aspx#Failed";
        }
        function scheduledSchedule() {
            window.location = "ScheduleDetails.aspx#Scheduled";
        }
    </script>

     <script type="text/javascript" src="../scripts/signalr.js/jquery.signalr.min.js"></script>
    <script src="../signalr/hubs"></script>

    
    <script>
        let a = 0;
        var output;
        $(function () {
            console.log("hi piyushpiyushpiyushpiyushpiyushpiyush");
            console.log(" nnnnnnnnnnnnnnpiyush : " + $.connection.myhub);
            
            var chat = $.connection.myHub;    //connection to hub object is created.
            $.connection.logging = true;
            // create a function that the hub can call back to display messages.
            //var sr;
            chat.client.addNewMessageToPage = function (x) {
                
                console.log("hi piyush : " + a);
                console.log("Stream reader : " + x);
                console.log("Stream reader : " + x);
                a++;
                //output.value += x.body + '<br />'
                //console.log("Output : "+output.value);


                var obj = JSON.parse(x);

                console.log("JSON reader : " + obj.RoboColor);
                console.log("JSON reader roboExecutionStatus : " + obj.roboExecutionStatus);
                if (obj.roboExecutionStatus == 1) //launching - dark grey
                {
                    obj.RoboColor = '#FFCC00';
                }
                else if (obj.roboExecutionStatus == 2) {//launch failed - dark red
                    obj.RoboColor = '#990000';
                }
                else if (obj.roboExecutionStatus == 3) { //ready & idle - Yellow
                    //obj.RoboColor = '#FFCC00';
                    obj.RoboColor = '#008000';
                }
                else if (obj.roboExecutionStatus == 4) {//processing Automation - Green
                    obj.RoboColor = '#008000';
                }
                else if (obj.roboExecutionStatus == 5) {//Automation Completed - Yellow
                    obj.RoboColor = '#FFCC00';
                }
                else if (obj.roboExecutionStatus == 6) {//RobotAutomationFailed - brown red 
                    obj.RoboColor = '#800000';
                }
                else if (obj.roboExecutionStatus == 6) {//RobotStopping -  red 
                    obj.RoboColor = '#FF0000';
                }

                var spans = document.getElementsByName('tofindbot');
                var l = spans.length;

                //Iteration of SPAN in DOM  
                for (var i = 0; i < l; i++) {
                    var strText = spans[i].innerText;
                    var robotName = obj.RobotName;

                    if (strText == robotName) {
                        var strid = spans[i].id;
                        var divs = document.getElementsByName('divStatus');
                        divs[i].style.backgroundColor = obj.RoboColor;

                        /*Code for toggling play and Stop Button */
                        var dcx = document.getElementsByClassName('btnStart');
                        var dcy = document.getElementsByClassName('btnStop');
                        //dcx[i].style.backgroundColor = obj.RoboColor;
                        dcx[i].style.display = "none";
                        if (obj.RoboColor == "#FF0000") {
                            divs[i].style.backgroundColor = "#FFCC00";
                            dcx.style.display = "inline";
                        }
                        if (obj.RoboColor == "#008000") {
                            dcy[i].style.display = "inline"

                        }
                        /*Code change Complete */

                        var reslblMachine = strid.replace("lblBotName", "lblMachineName");
                        var lblMachine = document.getElementById(reslblMachine);
                        if (lblMachine != null) {
                            lblMachine.innerText = obj.MachineName;
                        }

                        var reslblProcess = strid.replace("lblBotName", "lblProcessName");
                        var lblProcess = document.getElementById(reslblProcess);
                        if (lblProcess != null) {
                            lblProcess.innerText = obj.ProcessName;
                        }

                        var reslblTotalRequest = strid.replace("lblBotName", "lblTotalRequestServed");
                        var lblTotalRequest = document.getElementById(reslblTotalRequest);
                        if (lblTotalRequest != null) {
                            lblTotalRequest.innerText = obj.TotalRequestServed;
                        }

                        break;
                    }

                }
            };
            $.connection.hub.logging = true;    //logs generated by debug

            //hub starts connection with client
            $.connection.hub.start().done(function () {
                console.log("Connection starting");
                //chat.server.send();
                chat.server.joinGroup('<%= Session["TenantId"] %>');
            });
        });
    </script>
    
</asp:Content>

