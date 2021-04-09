<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="DemoMasterPage2_QueueDetails, App_Web_2o2bniex" enabletheming="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <!-- Begin Page Content -->

    <div class="container-fluid">
        <asp:ScriptManager runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                
     <div class="alert alert-success" role="alert" runat="server" visible="true">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
           Check Queue Mapping and the Time of Execution of the Process. You can also <span class="text-danger">Delete</span> the Queue by Clicking  <i class="fas fa-trash-alt alert-danger"></i> Button.
        </div>

                <div id="page-wrapper" class="container-fluid">
                    <div class="card shadow mb-4">
                        <div class="card-header py-3">
                            <h2 class="m-0 font-weight-bold poptxt">QUEUES
                   <asp:ImageButton ID="ImageButton1" runat="server" Class="rotate refreshbtn " ImageUrl="~/Images/refresh3.png" ImageAlign="Right"  OnCommand="ImageButton3_Command" />
                            </h2>
                        </div>
                        <div class="card-body">
                            <div class="row" align="center">
                                    <div class="col-sm-12">
                                        <asp:Button ID="btnQueue" runat="server" CssClass="btn btn-success buttoncss" Text="Scheduled Queue's" OnClick="btnQueue_Click" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnProcess" runat="server" CssClass="btn btn-dark buttoncss" Text="Scheduled Processes " OnClick="btnProcess_Click"  />&nbsp;&nbsp;&nbsp;
                                    </div>
                       </div>
                            <div>
                                <div class="card-body">
                                    <div class="panel panel-default colorFont">

                                        <div class="panel-body">
                                            <asp:Repeater ID="GrvSchedules" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-striped table-bordered" id="dataTables-schedules" width="100%">
                                                        <thead class="mastercolor colorSidebar" align="center">
                                                            <tr class="TableHeaderFont">
                                                                <th scope="col" class=" font-weight-bold poptxt">Bot Name
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold poptxt">Queue Name
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold poptxt">Cron Expression
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold text-danger">Delete
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr class="poptxtdata" style="padding: 0px; margin: 0px; border: .25px solid ;" id="mainData " >
                                                        <td class="odd gradeX " style="text-align: center;" onclick="Open('<%# Eval("botname")%>')">
                                                            <asp:Label ID="lblBotName" name="tofindbot" runat="server" Text='<%# Eval("botname") %>' Height="12px" />
                                                        </td>
                                                        <td class="odd gradeX " style="text-align: center;" onclick="Open('<%# Eval("botname") %>')">
                                                            <asp:Label ID="lblQueueName" runat="server" Text='<%# Eval("queuename") %>' />
                                                        </td>
                                                        <td class="odd gradeX " style="text-align: center;" onclick="Open('<%# Eval("botname") %>')">
                                                            <asp:Label ID="lblCronExpr" runat="server" Text='<%# Eval("StringChronExpression")  %>' />
                                                        <td id="IddeleteButton" style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;" runat="server">
                                                            <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Images/delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("scheduleid")+","+Eval("queuename")+","+ Eval("botname")+","+ Eval("chronExpression")%>' OnCommand="btnDelete_Click" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>

                                            <asp:Repeater ID="processRepeater" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-striped table-bordered" id="dataTables-schedules" width="100%">
                                                        <thead class="mastercolor colorSidebar" align="center">
                                                            <tr class="TableHeaderFont">

                                                                <th scope="col" class=" font-weight-bold poptxt">Bot Name
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold poptxt">Process Name
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold poptxt">Cron Expression
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold text-danger">Delete
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr class="poptxtdata" style="padding: 0px; margin: 0px; border: .25px solid ;" id="mainData " >
                                                        <td class="odd gradeX " style="text-align: center;" onclick="Open('<%# Eval("botname")%>')">
                                                            <asp:Label ID="lblBotName" name="tofindbot" runat="server" Text='<%# Eval("botname") %>' Height="12px" />
                                                        </td>
                                                        <td class="odd gradeX " style="text-align: center;" onclick="Open('<%# Eval("botname") %>')">
                                                            <asp:Label ID="lblQueueName" runat="server" Text='<%# Eval("processname") %>' />
                                                        </td>
                                                        <td class="odd gradeX " style="text-align: center;" onclick="Open('<%# Eval("botname") %>')">
                                                            <asp:Label ID="lblCronExpr" runat="server" Text='<%# Eval("StringChronExpression")  %>' />
                                                        <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">

                                                            <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Images/delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("scheduleid")+","+Eval("processname")+","+ Eval("botname")+","+ Eval("chronExpression")%>' OnCommand="btnProcessDelete_Click" />
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
                                    </div>






                                    <div class="modal fade " id="modalDelete" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
                                        <div class="modal-dialog modal-lg" role="document">
                                            <div class="modal-content ">
                                                <div class="modal-header">
                                                    <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">Delete Queue Details</h5>
                                                    <asp:Button runat="server" data-dismiss="modal" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" />
                                                    <%-- <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">×</span>
                                                    </button>--%>
                                                </div>

                                                <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                    Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?
                                                                          
                                         <br />
                                                    <br />
                                                    <div class="form-group">
                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Schedule Id :
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblScheduleIdDelete" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Queue Name :
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblQueueNameDelete" runat="server"> </asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Bot Name :
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblBotNameDelete" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Chron:
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblChronExpressionDelete" runat="server"> </asp:Label>
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


                                    <div class="modal fade " id="modalProcessDelete" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
                                        <div class="modal-dialog modal-lg" role="document">
                                            <div class="modal-content ">
                                                <div class="modal-header">
                                                    <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">Delete Process Details</h5>
                                                    <asp:Button runat="server" data-dismiss="modal" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" />
                                                    <%-- <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">×</span>
                                                    </button>--%>
                                                </div>

                                                <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                    Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?
                                                                          
                                         <br />
                                                    <br />
                                                    <div class="form-group">
                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Schedule Id :
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblScheduleIdDelete1" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Process Name :
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblprocessname" runat="server"> </asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Bot Name :
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblbotname" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Chron:
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblchron" runat="server"> </asp:Label>
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
                                                                    <asp:Button ID="Button2" runat="server" class="btn btn-danger btn-block" Text="Delete" align="center" OnClick="ModalPopUpBtnProcessDelete_Click" />
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




                                    <div class="modal fade " id="modalProcessDeleteSecondPopUp" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelDeleteSecondPopUp" aria-hidden="true">
                                        <div class="modal-dialog modal-lg" role="document">
                                            <div class="modal-content ">
                                                <div class="modal-header">
                                                    <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">Delete Process Details Permanently</h5>

                                                    <asp:Button ID="Button3" runat="server" class="close" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop" />
                                                    <%--<button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">×</span>
                                                        a              
                                                    </button>--%>
                                                </div>

                                                <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                    You are about to delete the process details permanently.Deleted process details will not be recovered.<br />
                                                    Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?

                                                <br />
                                                    <br />
                                                    <div class="form-group">
                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Schedule Id :
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblScheduleId" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Process Name :
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblprocessNameSec" runat="server"> </asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Bot Name :
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblBotNameSec" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Chron:
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblChronSec" runat="server"> </asp:Label>
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
                                                                    <asp:Button ID="Button4" runat="server" class="btn btn-danger btn-block" Text="Delete" align="center" OnClick="ModalPopUpBtnProcessDelete_ClickSecondPopUp" />
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Button ID="Button5" runat="server" class="btn btn-primary btn-block" Text="Close" OnClick="ModalPopUpBtnClose_ClickSecondPopUp" />
                                                                    <%--<asp:Button ID="btnCloseSecondPopUp" runat="server" class="btn btn-primary btn-block" Text="Close" OnClick="ModalPopUpBtnClose_ClickSecondPopUp" />--%>
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


                                    <div class="modal fade " id="modalDeleteSecondPopUp" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelDeleteSecondPopUp" aria-hidden="true">
                                        <div class="modal-dialog modal-lg" role="document">
                                            <div class="modal-content ">
                                                <div class="modal-header">
                                                    <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">Delete Queue Details Permanently</h5>

                                                    <asp:Button ID="btnXdelete" runat="server" class="close" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop" />
                                                    <%--<button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">×</span>
                                                        a              
                                                    </button>--%>
                                                </div>

                                                <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                    You are about to delete the queue details permanently.Deleted queue details will not be recovered.<br />
                                                    Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?

                                                <br />
                                                    <br />
                                                    <div class="form-group">
                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Schedule Id :
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblScheduleIdDeleteSecondPopUp" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Queue Name :
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblQueueNameDeleteSecondPopUp" runat="server"> </asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Bot Name :
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblBotNameDeleteSecondPopUp" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    Chron:
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblChronExpressionDeleteSecondPopUp" runat="server"> </asp:Label>
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
                                                                    <asp:Button ID="Button1" runat="server" class="btn btn-danger btn-block" Text="Delete" align="center" OnClick="ModalPopUpBtnDelete_ClickSecondPopUp" />
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Button ID="btnCloseSecondPopUp" runat="server" class="btn btn-primary btn-block" Text="Close" OnClick="ModalPopUpBtnClose_ClickSecondPopUp" />
                                                                    <%--<asp:Button ID="btnCloseSecondPopUp" runat="server" class="btn btn-primary btn-block" Text="Close" OnClick="ModalPopUpBtnClose_ClickSecondPopUp" />--%>
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
           //swal("Here's a message!");
           $("#page-wrapper").delay(100).fadeIn(300);
           $('#dataTables-schedules').DataTable({
               responsive: true
           });
       });

       function openModalHideDelete() {
           $('body').removeClass().removeAttr('style'); $('.modal-backdrop').remove();
       }


       var prm = Sys.WebForms.PageRequestManager.getInstance();
       if (prm != null) {
           prm.add_endRequest(function (sender, e) {
               if (sender._postBackSettings.panelsToUpdate != null) {
                   $(".rotate").click(function () {
                       $(this).toggleClass("down");
                   })
                   $("#page-wrapper").show();
                   $('#dataTables-schedules').DataTable({
                       destroy: true,
                       responsive: true
                   });
               }
           });
       };
       function openModalDelete() {
           $('#modalDelete').modal('show');

       }
       function openModalDeleteSecondPopUp() {
           $('#modalDeleteSecondPopUp').modal('show');
       };

       function openModalProcessDelete() {
           $('#modalProcessDelete').modal('show');

       }
       function openModalProcessDeleteSecondPopUp() {
           $('#modalProcessDeleteSecondPopUp').modal('show');
       };
       $(".rotate").click(function () {
           // alert("clicked");
           $(this).addClass("down");
       })

    </script>
    <script type="text/javascript">
        function Open(BotName) {
            //Window.location.replace("BotLogs.aspx");
            window.location.replace("BotLogs.aspx?BotName=" + BotName + "");
        }
    </script>

</asp:Content>

