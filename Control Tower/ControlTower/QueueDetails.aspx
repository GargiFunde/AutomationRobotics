<%@ Page Title="" Language="C#" MasterPageFile="~/ControlTower/MasterPageSkin.master" AutoEventWireup="true" CodeFile="QueueDetails.aspx.cs" Inherits="DemoMasterPage2_QueueDetails" EnableTheming="true" %>

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
            <asp:Literal Id="LitCheckQueueMappingandtheTimeofExecutionoftheProcessYoucanalso" runat="server" Text="<%$Resources:content,LitCheckQueueMappingandtheTimeofExecutionoftheProcessYoucanalso%>"></asp:Literal>   <span class="text-danger">
              <asp:Literal Id="LitDelete" runat="server" Text="<%$Resources:content,LitDelete%>"></asp:Literal>                                                                                                                                                                                                                  
            </span> 
          <asp:Literal Id="LittheQueuebyClicking" runat="server" Text="<%$Resources:content,LittheQueuebyClicking%>"></asp:Literal> 
         <i class="fas fa-trash-alt alert-danger"></i> 
          <asp:Literal Id="LitButton" runat="server" Text="<%$Resources:content,LitButton%>"></asp:Literal>.
        </div>

                <div id="page-wrapper" class="container-fluid">
                    <div class="card shadow mb-4">
                        <div class="card-header py-3">
                            <h2 class="m-0 font-weight-bold poptxt"> <asp:Literal Id="LitQUEUES" runat="server" Text="<%$Resources:content,LitQUEUES%>"></asp:Literal>
                   <asp:ImageButton ID="ImageButton1" runat="server" Class="rotate refreshbtn " ImageUrl="~/Images/refresh3.png" ImageAlign="Right"  OnCommand="ImageButton3_Command" />
                            </h2>
                        </div>
                        <div class="card-body">
                            <div class="row" align="center">
                                    <div class="col-sm-12">
                                        <asp:Button ID="btnQueue" runat="server" CssClass="btn btn-success buttoncss" Text="<%$Resources:content,LitScheduledQueues%>" OnClick="btnQueue_Click" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnProcess" runat="server" CssClass="btn btn-dark buttoncss" Text="<%$Resources:content,LitScheduledProcesses%>" OnClick="btnProcess_Click"  />&nbsp;&nbsp;&nbsp;
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
                                                                <th scope="col" class=" font-weight-bold poptxt">
                                                                    <asp:Literal Id="LitBotName" runat="server" Text="<%$Resources:content,LitBotName%>"></asp:Literal>
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold poptxt">
                                                                    <asp:Literal Id="LitQueueName" runat="server" Text="<%$Resources:content,LitQueueName%>"></asp:Literal>
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold poptxt">
                                                                    <asp:Literal Id="LitCronExpression" runat="server" Text="<%$Resources:content,LitCronExpression%>"></asp:Literal>
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold text-danger">
                                                                   <asp:Literal Id="LitDelete" runat="server" Text="<%$Resources:content,LitDelete%>"></asp:Literal> 
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

                                                                <th scope="col" class=" font-weight-bold poptxt">
                                                                   <asp:Literal Id="LitBotName" runat="server" Text="<%$Resources:content,LitBotName%>"></asp:Literal> 
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold poptxt">
                                                                   <asp:Literal Id="LitProcessName" runat="server" Text="<%$Resources:content,LitProcessName%>"></asp:Literal> 
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold poptxt">
                                                                    <asp:Literal Id="LitCronExpression" runat="server" Text="<%$Resources:content,LitCronExpression%>"></asp:Literal> 
                                                                </th>
                                                                <th scope="col" class=" font-weight-bold text-danger">
                                                                   <asp:Literal Id="LitDelete" runat="server" Text="<%$Resources:content,LitDelete%>"></asp:Literal> 
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
                                                    <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">
                                                       <asp:Literal Id="LitDeleteQueueDetails" runat="server" Text="<%$Resources:content,LitDeleteQueueDetails%>"></asp:Literal> 
                                                    </h5>
                                                    <asp:Button runat="server" data-dismiss="modal" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" />
                                                    <%-- <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">×</span>
                                                    </button>--%>
                                                </div>

                                                <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                   <asp:Literal Id="LitAreyousureyouwantto" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal> 
                                                    <span class=" font-weight-bold text-danger">
                                                       <asp:Literal Id="LitpmDelete" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal>   
                                                    </span>
                                                                          
                                         <br />
                                                    <br />
                                                    <div class="form-group">
                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Literal Id="LitScheduleId" runat="server" Text="<%$Resources:content,LitScheduleId%>"></asp:Literal>   
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblScheduleIdDelete" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Literal Id="LitQueueName" runat="server" Text="<%$Resources:content,LitQueueName%>"></asp:Literal> :    
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblQueueNameDelete" runat="server"> </asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Literal Id="LitBotName" runat="server" Text="<%$Resources:content,LitqdBotName%>"></asp:Literal>  
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblBotNameDelete" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Literal Id="LitChron" runat="server" Text="<%$Resources:content,LitChron%>"></asp:Literal>  
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
                                                                    <asp:Button ID="btnDelete" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" align="center" OnClick="ModalPopUpBtnDelete_Click" />
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <button class="btn btn-primary btn-block" type="button" data-dismiss="modal">
                                                                        <asp:Literal Id="LitClose" runat="server" Text="<%$Resources:content,LitClose%>"></asp:Literal>  

                                                                    </button>
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
                                                    <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">
                                                       
                                                         <asp:Literal Id="LitDeleteProcessDetails" runat="server" Text="<%$Resources:content,LitDeleteProcessDetails%>"></asp:Literal>  
                                                         </h5>
                                                                    <asp:Button runat="server" data-dismiss="modal" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" />
                                                    <%-- <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">×</span>
                                                    </button>--%>
                                   
                                                </div>

                                                <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                    <asp:Literal Id="LitAreyousureyouwantto1" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal>   
                                                    <span class=" font-weight-bold text-danger">
                                                        <asp:Literal Id="LitpmDelete1" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal> </span>
                                                                          
                                         <br />
                                                    <br />
                                                    <div class="form-group">
                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Literal Id="LitScheduleId1" runat="server" Text="<%$Resources:content,LitScheduleId%>"></asp:Literal>
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblScheduleIdDelete1" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                   <asp:Literal Id="LitpmProcessName" runat="server" Text="<%$Resources:content,LitpmProcessName%>"></asp:Literal> 
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblprocessname" runat="server"> </asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                     <asp:Literal Id="LitqdBotName" runat="server" Text="<%$Resources:content,LitqdBotName%>"></asp:Literal>
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblbotname" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                   <asp:Literal Id="LitChron1" runat="server" Text="<%$Resources:content,LitChron%>"></asp:Literal>
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
                                                                    <asp:Button ID="Button2" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" align="center" OnClick="ModalPopUpBtnProcessDelete_Click" />
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <button class="btn btn-primary btn-block" type="button" data-dismiss="modal">
                                                                        <asp:Literal Id="LitClose1" runat="server" Text="<%$Resources:content,LitClose%>"></asp:Literal>

                                                                    </button>
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
                                                    <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">
                                                        <asp:Literal Id="LitDeleteProcessDetailsPermanently" runat="server" Text="<%$Resources:content,LitDeleteProcessDetailsPermanently%>"></asp:Literal> 

                                                    </h5>

                                                    <asp:Button ID="Button3" runat="server" class="close" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop" />
                                                    <%--<button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">×</span>
                                                        a              
                                                    </button>--%>
                                                </div>

                                                <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                     <asp:Literal Id="LitYouareabouttodeletetheprocessdetailspermanentlyDeletedprocessdetailswillnotberecovered" runat="server" Text="<%$Resources:content,LitYouareabouttodeletetheprocessdetailspermanentlyDeletedprocessdetailswillnotberecovered%>"></asp:Literal><br />
                                                     <asp:Literal Id="LitAreyousureyouwantto2" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal>  
                                                    <span class=" font-weight-bold text-danger">
                                                      <asp:Literal Id="LitpmDelete2" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal>   

                                                    </span>

                                                <br />
                                                    <br />
                                                    <div class="form-group">
                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Literal Id="LitScheduleId2" runat="server" Text="<%$Resources:content,LitScheduleId%>"></asp:Literal>
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblScheduleId" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                     <asp:Literal Id="LitpmProcessName1" runat="server" Text="<%$Resources:content,LitpmProcessName%>"></asp:Literal>
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblprocessNameSec" runat="server"> </asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                   <asp:Literal Id="LitqdBotName1" runat="server" Text="<%$Resources:content,LitqdBotName%>"></asp:Literal> 
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblBotNameSec" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                   <asp:Literal Id="LitChron2" runat="server" Text="<%$Resources:content,LitChron%>"></asp:Literal>
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
                                                                    <asp:Button ID="Button4" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>"  align="center" OnClick="ModalPopUpBtnProcessDelete_ClickSecondPopUp" />
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Button ID="Button5" runat="server" class="btn btn-primary btn-block" Text="<%$Resources:content,LitClose%>" OnClick="ModalPopUpBtnClose_ClickSecondPopUp" />
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
                                                    <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">
                                                        <asp:Literal Id="LitDeleteQueueDetailsPermanently" runat="server" Text="<%$Resources:content,LitDeleteQueueDetailsPermanently%>"></asp:Literal>

                                                    </h5>

                                                    <asp:Button ID="btnXdelete" runat="server" class="close" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop" />
                                                    <%--<button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">×</span>
                                                        a              
                                                    </button>--%>
                                                </div>

                                                <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                    <asp:Literal Id="LitYouareabouttodeletethequeuedetailspermanentlyDeletedqueuedetailswillnotberecovered" runat="server" Text="<%$Resources:content,LitYouareabouttodeletethequeuedetailspermanentlyDeletedqueuedetailswillnotberecovered%>"></asp:Literal><br />
                                                    <asp:Literal Id="LitAreyousureyouwantto3" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal> 
                                                    <span class=" font-weight-bold text-danger">
                                                      <asp:Literal Id="LitpmDelete3" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal>    </span>

                                                <br />
                                                    <br />
                                                    <div class="form-group">
                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                   <asp:Literal Id="LitScheduleId3" runat="server" Text="<%$Resources:content,LitScheduleId%>"></asp:Literal>
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblScheduleIdDeleteSecondPopUp" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                   <asp:Literal Id="LitQueueName2" runat="server" Text="<%$Resources:content,LitQueueName%>"></asp:Literal>
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblQueueNameDeleteSecondPopUp" runat="server"> </asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="container">
                                                            <div class="row">
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Literal Id="LitqdBotName2" runat="server" Text="<%$Resources:content,LitqdBotName%>"></asp:Literal>
                                                                </div>
                                                                <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                                    <asp:Label ID="lblBotNameDeleteSecondPopUp" runat="server"> </asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Literal Id="LitChron3" runat="server" Text="<%$Resources:content,LitChron%>"></asp:Literal> 
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
                                                                    <asp:Button ID="Button1" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" align="center" OnClick="ModalPopUpBtnDelete_ClickSecondPopUp" />
                                                                </div>
                                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                                    <asp:Button ID="btnCloseSecondPopUp" runat="server" class="btn btn-primary btn-block" Text="<%$Resources:content,LitClose%>" OnClick="ModalPopUpBtnClose_ClickSecondPopUp" />
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

