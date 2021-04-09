<%@ Page Title="" Language="C#" MasterPageFile="~/ControlTower/MasterPageSkin.master" AutoEventWireup="true" CodeFile="AddRobot.aspx.cs" Inherits="DemoMasterPage2_AddRobot" %>

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
                        <h2 class="m-0 font-weight-bold poptxt">
                            <asp:Literal Id = "LitADADDROBOT" runat="server" Text="<%$Resources:content,LitADADDROBOT%>"></asp:Literal>   
                        </h2>
                    </div>

                    <div class="card-body">
                        <div>
                            <div class="panel panel-default colorFont">
                                <div class="form-group">
                                    <asp:Label ID="Label3" runat="server" class="TableHeaderFont poptxt font-weight-bold ">
                                        <asp:Literal Id = "LitBotName" runat="server" Text="<%$Resources:content,LitBotName%>"></asp:Literal> 
                                        <span class="text-danger">*</span></asp:Label>
                                    <asp:TextBox ID="txtBotName" class="form-control input-sm animated--grow-in" runat="server"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" class=" font-weight-bold TableHeaderFont poptxt">
                                        <asp:Literal Id = "LitMachineName" runat="server" Text="<%$Resources:content,LitMachineName%>"></asp:Literal> 
                                        <span class="text-danger">*</span></asp:Label>
                                    <asp:TextBox ID="txtMachineName" class="form-control input-sm" runat="server"></asp:TextBox>
                                    <br />
                                    <div class="form-group">
                                        <asp:Label ID="Label4" runat="server" class=" font-weight-bold TableHeaderFont poptxt">
                                            <asp:Literal Id = "LitBotUserId" runat="server" Text="<%$Resources:content,LitBotUserId%>"></asp:Literal> 
                                            <span class="text-danger">*</span></asp:Label>
                                        <asp:TextBox ID="txtBotId" class="form-control input-sm" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="Label5" runat="server" class=" font-weight-bold TableHeaderFont poptxt">
                                           <asp:Literal Id = "LitBotPassword" runat="server" Text="<%$Resources:content,LitBotPassword%>"></asp:Literal> 
                                            <span class="text-danger">*</span></asp:Label>
                                        <asp:TextBox ID="txtPwd" class="form-control input-sm" runat="server" TextMode="Password"></asp:TextBox>
                                    </div>

                                    <br />

                                    <div class="form-group" align="center">
                                        <asp:Button ID="btnsave" runat="server" class="btn btn-primary colorSidebar" Text="<%$Resources:content,LitSave%>"  OnClick="btnsave_Click" Width="49%" />
                                        <asp:Button ID="Button2" runat="server" class="btn btn-danger" Text="<%$Resources:content,LitClear%>"  OnClick="BtnCancelRobot" Width="49%" />
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
                        <h2 class="m-0 font-weight-bold text-primary">
                            <asp:Literal Id = "LitADADDROBOT1" runat="server" Text="<%$Resources:content,LitADADDROBOT%>"></asp:Literal>
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
                                                    <th scope="col" class=" font-weight-bold ">
                                                        <asp:Literal Id = "LitStatus" runat="server" Text="<%$Resources:content,LitStatus%>"></asp:Literal>
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">
                                                        <asp:Literal Id = "LitBotName" runat="server" Text="<%$Resources:content,LitBotName%>"></asp:Literal>
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">
                                                        <asp:Literal Id = "LitBotKey" runat="server" Text="<%$Resources:content,LitBotKey%>"></asp:Literal>
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">
                                                        <asp:Literal Id = "LitMachineName" runat="server" Text="<%$Resources:content,LitMachineName%>"></asp:Literal>
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">
                                                        <asp:Literal Id = "LitCreatedBy" runat="server" Text="<%$Resources:content,LitCreatedBy%>"></asp:Literal>
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold ">
                                                       <asp:Literal Id = "LitCreateDate" runat="server" Text="<%$Resources:content,LitCreateDate%>"></asp:Literal> 
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold text-danger">
                                                       <asp:Literal Id = "LitDelete" runat="server" Text="<%$Resources:content,LitDelete%>"></asp:Literal> 
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
                                <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">
                                    <asp:Literal Id = "LitDeleteRobot" runat="server" Text="<%$Resources:content,LitDeleteRobot%>"></asp:Literal>

                                </h5>
                                <asp:Button class="close" runat="server" CssClass="ccolor" data-dismiss="modal" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close"></asp:Button>
                            </div>

                            <div class="modal-body  TableHeaderFont  poptxt " style="text-align: center" runat="server">
                               <asp:Literal Id = "LitAreyousureyouwantto" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal>  
                                <span class=" font-weight-bold text-danger">
                                   <asp:Literal Id = "LitpmDelete" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal>  

                                </span>
                                                                          
                                         <br />
                                <br />
                                <div class="form-group">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                               <asp:Literal Id = "LitRobotName" runat="server" Text="<%$Resources:content,LitRobotName%>"></asp:Literal> 
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2" align="left">
                                                <asp:Label ID="lblRobotName" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                                <asp:Label ID="lblId" Class="text-success" runat="server" hidden> </asp:Label>
                                            </div>
                                            <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                              <asp:Literal Id = "LitARMachineName" runat="server" Text="<%$Resources:content,LitARMachineName%>"></asp:Literal>  
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
                                                <asp:Button ID="btnDelete" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>"  align="center" OnClick="ModalPopUpBtnDelete_Click" />
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                <button class="btn btn-primary btn-block" type="button" data-dismiss="modal">
                                                  <asp:Literal Id = "LitClose" runat="server" Text="<%$Resources:content,LitClose%>"></asp:Literal>  

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
                                <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">
                                   <asp:Literal Id = "LitDeleteRobotPermanently" runat="server" Text="<%$Resources:content,LitDeleteRobotPermanently%>"></asp:Literal> 

                                </h5>
                                <asp:Button class="close" runat="server" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop"></asp:Button>
                            </div>

                            <div class="modal-body   TableHeaderFont  poptxt" style="text-align: center" runat="server">
                               <asp:Literal Id = "LitYouareabouttodeletetherobotpermanentlyDeletedrobotwillnotberecovered" runat="server" Text="<%$Resources:content,LitYouareabouttodeletetherobotpermanentlyDeletedrobotwillnotberecovered%>"></asp:Literal>  
                                <br />
                                <asp:Literal Id = "LitAreyousureyouwantto1" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal>  
                                <span class=" font-weight-bold text-danger">
                                    <asp:Literal Id = "LitpmDelete1" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal>
                                    
                                </span>

                                <br />
                                <br />
                                <div class="form-group">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                <asp:Literal Id = "LitRobotName1" runat="server" Text="<%$Resources:content,LitRobotName%>"></asp:Literal> 
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2" align="left">
                                                <asp:Label ID="lblRobotNameSecondPopUp" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                                <asp:Label ID="lblIdSecondPopUp" Class="text-success" runat="server" hidden> </asp:Label>
                                            </div>
                                            <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                <asp:Literal Id = "LitARMachineName1" runat="server" Text="<%$Resources:content,LitARMachineName%>"></asp:Literal>  
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
                                            <span class=" font-weight-bold text-danger">
                                              <asp:Literal Id = "LitNote" runat="server" Text="<%$Resources:content,LitNote%>"></asp:Literal>  

                                            </span>
                                            </br>
                                                            <br />
                                            <asp:Literal Id = "LitWhiledeletingthisrobotyouwillbealsodeletingallthemappingstothisrobot" runat="server" Text="<%$Resources:content,LitWhiledeletingthisrobotyouwillbealsodeletingallthemappingstothisrobot%>"></asp:Literal> 
                                            </br>
                                                            <asp:Literal Id = "LitAreyousureyouwanttodeleteitfrom" runat="server" Text="<%$Resources:content,LitAreyousureyouwanttodeleteitfrom %>"></asp:Literal>  
                                                             
                                                        <span class="h6 font-weight-bold text-danger">
                                                          <asp:Literal Id = "LitBots" runat="server" Text="<%$Resources:content,LitBots%>"></asp:Literal>  

                                                        </span><span class="h6 font-weight-bold text-danger">
                                                           <asp:Literal Id = "LitADScheduleDetails" runat="server" Text="<%$Resources:content,LitADScheduleDetails%>"></asp:Literal> 

                                                               </span><span class="h6 font-weight-bold text-danger">
                                                                   <asp:Literal ID="LitUserBotMapping" runat="server" Text="<%$Resources:content,LitUserBotMapping%>"></asp:Literal>
                                                                    

                                                                                                                                                                                                   </span>
                                            <span class="h6 font-weight-bold text-danger">
                                                <asp:Literal ID="LitBotDefaultQueue" runat="server" Text="<%$Resources:content,LitBotDefaultQueue%>"></asp:Literal> 

                                            </span><span class="h6 font-weight-bold text-danger">
                                                <asp:Literal ID="LitQrtzTriggers" runat="server" Text="<%$Resources:content,LitQrtzTriggers%>"></asp:Literal>

                                                   </span><span class="h6 font-weight-bold text-danger">
                                                     <asp:Literal ID="LitQrtzJobDetailsand" runat="server" Text="<%$Resources:content,LitQrtzJobDetailsand%>"></asp:Literal>   

                                                          </span>
                                            <span class="h6 font-weight-bold text-danger">
                                               <asp:Literal ID="LitQrtzCronTriggers" runat="server" Text="<%$Resources:content,LitQrtzCronTriggers%>"></asp:Literal> 

                                            </span>
                                        </div>



                                        <div class="modal-body h6  text-primary" style="text-align: center" runat="server">
                                            <span class="h5 font-weight-bold text-danger">
                                               <asp:Literal ID="LitCounts" runat="server" Text="<%$Resources:content,LitCounts%>"></asp:Literal>  

                                            </span>
                                            <br />
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold">
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                               <asp:Literal ID="LitSchedule" runat="server" Text="<%$Resources:content,LitSchedule%>"></asp:Literal> 
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
                                                <asp:Button ID="btnDeleteSecondPopUp" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" runat="server" OnClick="ModalPopUpBtnDelete_ClickSecondPopUp" />
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Button ID="btnCloseSecondPopUp" runat="server" class="btn btn-primary btn-block" Text="<%$Resources:content,LitClose%>" OnClick="ModalPopUpBtnClose_ClickSecondPopUp" />
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
