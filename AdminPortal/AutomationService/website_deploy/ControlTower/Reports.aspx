<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="pages_DemoMasterPage2_SanketCharts, App_Web_yn0cbmo4" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="css/tooltip.css" type="text/css" media="screen" />
    <!-- Custom fonts for this template-->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet">
    <!-- Custom styles for this template-->
    <link href="css/sb-admin-2.min.css" rel="stylesheet">

    <script src="http://cdn.syncfusion.com/js/assets/external/jquery-1.10.2.min.js" type="text/javascript"></script>
    <!-- Essential JS UI widget -->
    <script src="http://cdn.syncfusion.com/17.2.0.46/js/web/ej.web.all.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-3.3.1.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.2/Chart.bundle.js" type="text/javascript"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid" style="font-family:Helvetica">

        <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <!-- Page Heading -->
        <%--<div class="card-header py-3">
            <h2 class="m-0 font-weight-bold text-primary">Reports</h2>
        </div>--%>
        <%--<h1 class="h3 mb-2 text-gray-800">Reports</h1>--%>
        <!-- Content Row -->
        <div class="row">


            <div class="col-xl-6 col-lg-5">
                <asp:UpdatePanel ID="updpnlWeekly" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="WeeklyHidden" style="display: none">
                            <div id="weeklytype"><%= TypeWeekly %></div>
                            <div id="weeklylabels"><%=Newtonsoft.Json.JsonConvert.SerializeObject(Labelsweekly)%></div>
                            <div id="weeklylegend"><%=Newtonsoft.Json.JsonConvert.SerializeObject(Legendweekly)%></div>
                            <div id="weeklydata"><%=Newtonsoft.Json.JsonConvert.SerializeObject(Dataweekly)%></div>
                        </div>
                        <!-- Area Chart -->
                        <div class="card shadow mb-4" id="id1">
                            <div class="card-header py-3">
                                <h5 class="m-0 font-weight-bold poptxt" style="font-size:x-large">Weekly Status</h5>
                               <br>
                                <div class="row">

                                    <div class="col-xl-6 col-lg-6">
                                        <asp:DropDownList ID="ddlWeeklyChart" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlWeeklyChart_SelectedIndexChanged">
                                            <asp:ListItem Selected="True">line</asp:ListItem>
                                            <asp:ListItem>bar</asp:ListItem>
                                            <asp:ListItem>doughnut</asp:ListItem>
                                            <asp:ListItem>pie</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xl-6 col-lg-6">

                                        <asp:DropDownList ID="ddlStatusWeekly" AutoPostBack="true" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlStatusWeekly_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>


                      <div class="card-body">
                        <div class="chart-area" style="height: auto">

                             <div id="container">
                                        <br />
                                        <canvas id="canvas"></canvas>
                                    </div> 
                        </div>
                    </div>

                           
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlWeeklyChart" />
                        <asp:AsyncPostBackTrigger ControlID="ddlStatusWeekly" />
                    </Triggers>

                </asp:UpdatePanel>
                <script type="text/javascript">

                    window.onload = function () {
                        DisplayMonthlyChart();
                        DisplayWeeklyChart();
                        DisplayDailyChart();
                    };


                    //On UpdatePanel Refresh
                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    if (prm != null) {
                        prm.add_endRequest(function (sender, e) {
                            if (sender._postBackSettings.panelsToUpdate != null) {
                                DisplayWeeklyChart();

                            }
                        });
                    };

                    function DisplayWeeklyChart() {
                        var weeklytype = document.getElementById("weeklytype").innerHTML;
                        var weeklylabels1 = document.getElementById("weeklylabels").innerHTML;
                        var weeklylabel = document.getElementById("weeklylegend").innerHTML;
                        var weeklydata1 = document.getElementById("weeklydata").innerHTML;

                        var a = weeklydata1.substr(1).slice(0, -1);
                        var weeklydata = a.split(',').map(function (item) {
                            return parseInt(item);
                        });

                        var b = weeklylabels1.substr(1).slice(0, -1).replace(/"/g, "");
                        var weeklylabels = b.split(',').map(function (item) {
                            return item;
                        });

                        var ctx = document.getElementById("canvas").getContext('2d');
                        var myChart = new Chart(ctx, {
                            type: weeklytype,
                            data: {
                                labels: weeklylabels,
                                datasets: [{
                                    label: weeklylabel,
                                    data: weeklydata,
                                    backgroundColor: [
                                        'rgba(123, 92, 250)',
                                        'rgba(255, 92, 250)',
                                        'rgba(3, 155, 229)',
                                        'rgba(167, 255, 235)',
                                        'rgba(66, 112, 150)',
                                        'rgba(151, 112, 150)',
                                        'rgba(27, 154, 67)',
                                    ],
                                    borderColor: [
                                        'rgba(123, 92, 250,10)',
                                        'rgba(255, 92, 250,1)',
                                        'rgba(3, 155, 229)',
                                        'rgba(167, 255, 235)',
                                        'rgba(66, 112, 150)',
                                        'rgba(151, 112, 150)',
                                        'rgba(27, 154, 67)',
                                    ],
                                }]
                            },


                            options: {


                                scales: {
                                    yAxes: [{
                                        ticks: {

                                            beginAtZero: true,
                                            barThickness: 6,
                                        }
                                    }],

                                    xAxes: [{
                                        barPercentage: 2,
                                        barThickness: 20,
                                        maxBarThickness: 20,
                                        minBarLength: 2,
                                        gridLines: {
                                            offsetGridLines: true
                                        }
                                    }]

                                }
                            }


                        });

                    };

                </script>



            </div>


            <div class="col-xl-6 col-lg-5">            
                 <asp:UpdatePanel ID="updpnlDaily" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                         <div id="DailyHidden" style="display: none">
                            <div id="Dailytype"><%= TypeDaily %></div>
                            <div id="Dailylabels"><%=Newtonsoft.Json.JsonConvert.SerializeObject(Labelsdaily)%></div>
                            <div id="Dailylegend"><%=Newtonsoft.Json.JsonConvert.SerializeObject(Legenddaily)%></div>
                            <div id="Dailydata"><%=Newtonsoft.Json.JsonConvert.SerializeObject(Datadaily)%></div>
                        </div>
                           
                        <div class="card shadow mb-4">
                            <!-- Card Header - Dropdown -->
                            <div class="card-header py-3">
                                <h6 class="m-0 font-weight-bold poptxt" style="font-size:x-large">Daily Status</h6>
                                <br>
                                <asp:DropDownList ID="ddlDailyChart" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlDailyChart_SelectedIndexChanged">
                                    <asp:ListItem Selected="True">pie</asp:ListItem>
                                    <asp:ListItem>bar</asp:ListItem>
                                    <asp:ListItem>doughnut</asp:ListItem>
                                    <%--<asp:ListItem>line</asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                            
                            <!-- Card Body -->
                           
                       <div class="card-body">
                        <div class="chart-area" style="height: auto">
                            <div id="container2">
                                <br />
                              <canvas id="canvasdaily"></canvas>
                              </div> 
                        </div>
                    </div>
                                   <%-- <div class="card-body" >
                                        <div class="chart-area">
                                            <br />
                                            <div id="container2">
                                                <canvas id="canvasdaily"></canvas>

                                            </div>

                                        </div>
                                    </div>--%>
                                    
                                
                             

                        </div>
                  </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDailyChart" />
                        
                    </Triggers>

                </asp:UpdatePanel>

                <script type="text/javascript">

                   
                    //On UpdatePanel Refresh
                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    if (prm != null) {
                        prm.add_endRequest(function (sender, e) {
                            if (sender._postBackSettings.panelsToUpdate != null) {
                                DisplayDailyChart();

                            }
                        });
                    };



                    function DisplayDailyChart() {

                        var dailytype = document.getElementById("Dailytype").innerHTML;
                        var dailylabels1 = document.getElementById("Dailylabels").innerHTML;
                        var dailylabel1 = document.getElementById("Dailylegend").innerHTML;
                        var dailydata1 = document.getElementById("Dailydata").innerHTML;


                        var a = dailydata1.substr(1).slice(0, -1);
                        var dailydata = a.split(',').map(function (item) {
                            return parseInt(item);
                        });

                        var b = dailylabels1.substr(1).slice(0, -1).replace(/"/g, "");
                        var dailylabels = b.split(',').map(function (item) {
                            return item;
                        });

                        var c = dailylabel1.substr(1).slice(0, -1).replace(/"/g, "");
                        var dailylabel = c.split(',').map(function (item) {
                            return item;
                        });
                       
                        var ctx = document.getElementById("canvasdaily").getContext('2d');
                        var myChart = new Chart(ctx, {

                            type: dailytype,

                            data: {
                                labels: dailylabels,
                                datasets: [{
                                    label: dailylabel, <%--'<%=Legend%>',--%>
                                    data: dailydata,
                                    backgroundColor: [

                                        'rgba(27, 154, 67)',
                                        'rgba(240, 173, 78)',
                                        'rgba(2, 117, 216)',

                                    ],

                                    borderWidth: 2

                                }]
                            },
                            options: {
                                //cutoutPercentage: 80,

                                scales: {
                                    yAxes: [{
                                        ticks: {
                                            beginAtZero: true,
                                            gridThickness: 0,
                                            tickLength: 0,
                                            lineThickness: 0,

                                        }
                                    }],
                                    xAxes: [{
                                        barPercentage: 2,
                                        barThickness: 20,
                                        maxBarThickness: 20,
                                        minBarLength: 2,
                                        gridThickness: 0,
                                        tickLength: 0,
                                        lineThickness: 0,

                                        gridLines: {
                                            offsetGridLines: true,

                                        }
                                    }]
                                }
                            }



                        });


                    };
                </script>

            </div>

        </div>



        <div class="row">

            <div class="col-xl-12 col-lg-12">
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                         <div id="MonthlyHidden" style="display: none" >
                            <div id="Monthlytype"><%= TypeMonthly %></div>
                            <div id="Monthlylabels"><%=Newtonsoft.Json.JsonConvert.SerializeObject(LabelsMonthly)%></div>
                            <div id="Monthlylegend"><%=Newtonsoft.Json.JsonConvert.SerializeObject(LegendMonthly)%></div>
                            <div id="Monthlydata"><%=Newtonsoft.Json.JsonConvert.SerializeObject(DataMonthly)%></div>
                        </div>


                <!-- Area Chart -->
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold poptxt" style="font-size:x-large">Monthly Status</h6>
                        <%--<asp:DropDownList ID="ddlChart" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlChart_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                        <br />

                        <%--<asp:DropDownList ID="ddlChart" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlChart_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                        <div class="row">
                            <div class="col-xl-6 col-lg-6">
                                <asp:DropDownList ID="ddlMonthlyChart" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlMonthlyChart_SelectedIndexChanged">
                                    <asp:ListItem Selected="True">bar</asp:ListItem>
                                    <asp:ListItem>doughnut</asp:ListItem>
                                    <asp:ListItem>pie</asp:ListItem>
                                    <asp:ListItem>line</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-xl-6 col-lg-6s">
                                <asp:DropDownList ID="ddlStatusMonthly" AutoPostBack="true" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlStatusMonthly_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                        </div>
                    </div>

                    <div class="card-body">
                        <div class="chart-area" style="height: auto">

                            <div id="containerMonthly">
                                <canvas id="canvasMonthly"></canvas>
                            </div>

                            

                        </div>


                    </div>
                </div>
            </div>
                         </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlMonthlyChart" />
                         <asp:AsyncPostBackTrigger ControlID="ddlStatusMonthly" />
                        
                    </Triggers>

                </asp:UpdatePanel>
                <script type="text/javascript">
                    


                    //On UpdatePanel Refresh
                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    if (prm != null) {
                        prm.add_endRequest(function (sender, e) {
                            if (sender._postBackSettings.panelsToUpdate != null) {
                                DisplayMonthlyChart();

                            }
                        });
                    };


                    function DisplayMonthlyChart() {

                        var monthlytype = document.getElementById("Monthlytype").innerHTML;
                        var monthlylabels1 = document.getElementById("Monthlylabels").innerHTML;
                        var monthlylabel = document.getElementById("Monthlylegend").innerHTML;
                        var monthlydata1 = document.getElementById("Monthlydata").innerHTML;


                        var a = monthlydata1.substr(1).slice(0, -1);
                        var monthlydata = a.split(',').map(function (item) {
                            return parseInt(item);
                        });

                        var b = monthlylabels1.substr(1).slice(0, -1).replace(/"/g, "");
                        var monthlylabels = b.split(',').map(function (item) {
                            return item;
                        });

                       
                        var ctx = document.getElementById("canvasMonthly").getContext('2d');
                        var myChart = new Chart(ctx, {
                            type: monthlytype,
                            data: {
                                labels: monthlylabels,
                                datasets: [{
                                    label: monthlylabel, <%--'<%=Legend%>',--%>
                                    data: monthlydata,

                                    backgroundColor: [
                                        'rgba(123, 92, 250)',
                                        'rgba(16, 156, 150)',
                                        'rgba(151, 149, 108)',
                                        'rgba(66, 112, 150)',
                                        'rgba(151, 112, 150)',
                                        'rgba(215, 204, 200)',
                                        'rgba(251, 233, 231)',
                                        'rgba(255, 171, 145)',
                                        'rgba(255, 245, 157)',
                                        'rgba(230, 238, 156)',
                                        'rgba(197, 225, 165)',
                                        'rgba(165, 214, 167)',
                                        'rgba(128, 203, 196)',
                                        'rgba(128, 222, 234)',
                                        'rgba(129, 212, 250)',
                                        'rgba(144, 202, 249)',
                                        'rgba(159, 168, 218)',
                                        'rgba(179, 157, 219)',
                                        'rgba(206, 147, 216)',
                                        'rgba(244, 143, 177)',
                                        'rgba(38, 50, 56)',
                                        'rgba(176, 190, 197)',
                                        'rgba(16, 156, 150)',
                                        'rgba(151, 149, 108)',
                                        'rgba(66, 112, 150)',
                                        'rgba(151, 112, 150)',
                                        'rgba(18, 212, 150)',
                                        'rgba(255, 92, 250)',
                                        'rgba(123, 92, 250)',
                                        'rgba(16, 156, 150)',
                                        'rgba(151, 149, 108)',
                                        'rgba(66, 112, 150)',
                                        'rgba(151, 112, 150)',
                                        'rgba(18, 212, 150)',
                                        'rgba(255, 92, 250)',


                                    ],
                                    borderColor:
                                        'rgba(103,118,234)',
                                    //backgroundColor: [
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',
                                    //    'rgba(103,118,234)',

                                    //],
                                    //borderColor: [
                                    //    'rgba(255,99,132,1)',
                                    //    'rgba(54, 162, 235, 1)',
                                    //    'rgba(255, 206, 86, 1)',
                                    //    'rgba(75, 192, 192, 1)',
                                    //    'rgba(153, 102, 255, 1)',
                                    //    'rgba(255, 159, 64, 1)',
                                    //    'rgba(255,99,132,1)',

                                    //],
                                    //borderWidth: 1
                                }]
                            },
                            options: {


                                scales: {
                                    yAxes: [{
                                        ticks: {
                                            beginAtZero: true,

                                        }
                                    }],
                                    xAxes: [{
                                        barPercentage: 2,
                                        barThickness: 20,
                                        maxBarThickness: 20,
                                        minBarLength: 2,

                                        gridLines: {
                                            offsetGridLines: true,

                                        }
                                    }]
                                }
                            }
                        });
                    };
                            </script>
        </div>


        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />

        <%--<div class="row">

            <div class="col-xl-12 col-lg-12">

                <!-- Area Chart -->
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold text-primary">BAR CHART</h6>
                        <%--<asp:DropDownList ID="ddlChart" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlChart_SelectedIndexChanged">
                        </asp:DropDownList>--%>
        <br />
        <%-- <asp:DropDownList ID="ddlChart" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlChart_SelectedIndexChanged">
                        </asp:DropDownList>

                       
                    </div>

                    

                    <div class="card-body">
                        <div class="chart-area">  
    
                            <div id="container1" >
                                  
                              

               

               <asp:Chart ID="Chart1"  runat="server" Palette="Berry">
                                <Titles>
                                    <asp:Title ShadowOffset="3" Name="Items" />
                                </Titles>
                                <Legends>
                                    <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default"
                                        LegendStyle="Row" />
                                </Legends>
                                <Series>
                                    <asp:Series Name="LOGS" ToolTip="DATE : #VALX  !!  COUNT : #VALY" XValueMember="date" YValueMembers="TimePerDay" ChartArea="ChartArea1" ChartType="column" YValuesPerPoint="6">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderWidth="0" BackColor="#f5f5f5" />
                                </ChartAreas>
                            </asp:Chart>
               </div>
            </div>
</div></div></div>--%>
    </div>
</asp:Content>

<%-- <asp:Table ID="Tbl_chart" runat="server" CssClass="table  table-bordered table-condensed table-responsive">
                                <asp:TableRow>
                                    <asp:TableCell>--%>


<%-- <asp:Chart ID="Chart1" Style="width: 95%; height: 100%" runat="server" Palette="Berry">
                                <Titles>
                                    <asp:Title ShadowOffset="3" Name="Items" />
                                </Titles>
                                <Legends>
                                    <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default"
                                        LegendStyle="Row" />
                                </Legends>
                                <Series>
                                    <asp:Series Name="LOGS" ToolTip="DATE : #VALX  !!  COUNT : #VALY" XValueMember="date" YValueMembers="TimePerDay" ChartArea="ChartArea1" ChartType="column" YValuesPerPoint="6">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderWidth="0" BackColor="#f5f5f5" />
                                </ChartAreas>
                            </asp:Chart>--%>

<%-- <asp:Chart  ID="Chart1" runat="server"  Height="500px" Width="700px" Palette="None" class="col-lg-12">
                                            <Series>                                 
                                                <asp:Series Name="Series1" ToolTip="Value of X:#VALX;   Value of Y:#VALY" XValueMember="date" YValueMembers="TimePerDay" ChartArea="ChartArea2" ChartType="column" YValuesPerPoint="6" >
                                                </asp:Series>
                                            </Series>
                                             <ChartAreas>
                                                <asp:ChartArea Area3DStyle-Enable3D="true" Name="ChartArea2">
                                                </asp:ChartArea>
                                           </ChartAreas>
                                            <Titles>
                                                <asp:Title Docking="Top" Font="Arial, 12pt" Name="Title1" >
                                                </asp:Title>
                                            </Titles>
                                            
                                        </asp:Chart>--%>
<%--</asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>--%>