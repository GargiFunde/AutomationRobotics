<%@ page language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="DemoMasterPage2_AssignQueueToRobot, App_Web_shozqevm" %>

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
         <div id="bootstrapAlertMsgInfo" class="alert alert-info" role='alert'>
             <button type='button' class='close' data-dismiss='alert' aria-label='Close'>
                 <span aria-hidden="true">&times;</span>
             </button>
             1. Select Bot Name and Queue to it.<br />
             2. After assigning queue to robot,go to
                                   
             <a href='./AssignRobotToUser.aspx' class='alert-link'>ASSIGNROBOTTOUSER </a>
             to configure your robot with user.
                                    
       
         </div>
        <!-- DataTables Example -->
       <div id="DivAddQueue" class ="card shadow mb-4" runat="server">
            <div class="card-header py-3">
                <h2 class="m-0 font-weight-bold poptxt">ADD QUEUE TO ROBOT
                </h2>
            </div>

            <div class="card-body">
               <div>
                        <div class="col-md-12 col-lg-12 col-sm-12 col-xm-12" id="msgBox" runat="server">
                                </div>
                                <div class="card-body TableHeaderFont">
                                    <div class="form-group">
                                        <asp:Label ID="Label4" runat="server" Text="Label" class=" font-weight-bold poptxt">
                        Bot Name<span class="text-danger">*</span>
                                        </asp:Label>
                                        <asp:DropDownList ID="DrpBots" runat="server" class="form-control input-sm" AppendDataBoundItems="True"
                                            AutoPostBack="false">
                                            <asp:ListItem Value="Select">--Select--</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label ID="Label5" runat="server" Text="Label" class=" font-weight-bold poptxt">
                        Queue Name<span class="text-danger">*</span>
                                        </asp:Label>
                                        <asp:DropDownList ID="DrpQueues" runat="server" class="form-control input-sm" AppendDataBoundItems="True">
                                            <asp:ListItem Value="Select">--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <br />
                                    <div class="form-group" align="center">
                                        <asp:Button ID="btnSave" runat="server" class="btn btn-primary colorSidebar" Text="Save" OnClick="Save_Click" Width="49%" />
                                        <asp:Button ID="Button3" runat="server" class="btn btn-danger" Text="Cancel" OnClick="BtnCancelUser" Width="49%" />
                                    </div>
                                     </div>
                                </div>
                </div>
                </div>
         <%--  </ContentTemplate> 
     </asp:UpdatePanel>--%>
        <br />
<%-- <asp:UpdatePanel runat="server">
          <ContentTemplate>--%>

 <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h2 class="m-0 font-weight-bold poptxt">ROBOTS
                   <asp:ImageButton ID="ImageButton2" runat="server" Class="rotate refreshbtn " ImageUrl="~/Images/refresh3.png" ImageAlign="Right"  OnCommand="ImageButton3_Command" />

                </h2>
            </div>

            <div class="card-body">
                <div>

                    <div class="panel panel-default colorFont">

                            
                                <div class="panel-body">

                                    <asp:Repeater ID="GVQueBotMapping" runat="server" >
                                        <HeaderTemplate>
                                            <table class="table table-striped table-bordered table-hover" id="dataTables-botqueuemapping" width="100%">
                                                <thead class="colorSidebar">
                                                    <tr class="poptxt TableHeaderFont">
                                                        <th scope="col" style="font-weight: normal; text-align: center" class=" font-weight-bold ">Bot Name
                                                        </th>
                                                        <th scope="col" style="font-weight: normal; text-align: center" class=" font-weight-bold ">Tenant Name
                                                        </th>
                                                        <th scope="col" style="font-weight: normal; text-align: center" class=" font-weight-bold ">Queue Name
                                                        </th>
                                                        <th scope="col" style="font-weight: normal; text-align: center" class=" font-weight-bold ">Created By
                                                        </th>
                                                        <th scope="col" style="font-weight: normal; text-align: center" class=" font-weight-bold ">Create Date
                                                        </th>
                                                        <th scope="col" style="font-weight: normal; text-align: center" class=" font-weight-bold text-danger">Delete
                                                        </th>

                                                    </tr>
                                                </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="poptxtdata"  >
                                                <td class="odd gradeX " style="text-align: center" onclick="Open('<%# Eval("botname")+","+ Eval("defaultqueuename")%>')" >
                                                    <asp:Label  ID="lblBotName" runat="server" Text='<%# Eval("botname") %>'  />
                                                </td>
                                                <td class="odd gradeX " style="text-align: center" onclick="Open('<%# Eval("botname")+","+ Eval("defaultqueuename")%>')">
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("tenantname") %>' />
                                                </td>
                                                <td class="odd gradeX " style="text-align: center" onclick="Open('<%# Eval("botname")+","+ Eval("defaultqueuename")%>')">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("defaultqueuename") %>' />
                                                </td>
                                                <td class="odd gradeX " style="text-align: center" onclick="Open('<%# Eval("botname")+","+ Eval("defaultqueuename")%>')">
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("createdby") %>' />
                                                </td>
                                                <td class="odd gradeX " style="text-align: center" onclick="Open('<%# Eval("botname")+","+ Eval("defaultqueuename")%>')">
                                                    <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("createddate" , "{0:dd/MMM/yyyy HH:mm:ss}") %>' />
                                                </td>

                                                <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;" >


                                                    <asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/Images/delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("id")+","+Eval("botname")+","+ Eval("defaultqueuename")%>' CommandName="Delete" OnCommand="btnDelete_Click" />


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
     </div>
              

                            
                        
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                  <asp:UpdatePanel runat="server">
          <ContentTemplate>
                 <div class="modal fade" id="UpdateQueuepopUp" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false" aria-labelledby="exampleModalLabel2" aria-hidden="true">
                                        <div class="modal-dialog modal-lg" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="m-0 font-weight-bold poptxt" id="exampleModalLabel2">Update Queue</h5>
                                                     <asp:Button runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXclosePopup"   ><%--    data-dismiss="modal" aria-label="Close"--%> <%--OnClick="btnXclosePopup"--%>
                                                    </asp:Button>
                                                   
                                                </div>

                                                <div class="modal-body poptxt"  runat="server" >
                                                    <div class="container">
                                                        <table width="100%" align="center">
                                                          
                                                            <tr style="height: 25px" >
                                                                <td align="left" style="text-align: right">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lblBotNamepop" runat="server"><b>Bot Name*&nbsp;&nbsp;</b></asp:Label>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtBotName" class="form-control input-sm" runat="server" ></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                
                                                                
                                                                <td class="auto-style16"></td>

                                                            </tr>
                                                           
                                                             <tr style="height: 25px" >
                                                              <td align="left" style="text-align: right">
                                                                    <div class="form-group">
                                                                        <asp:HiddenField id="lblQuenameHidden" runat="server" ClientIDMode="Static" />
                                                                        <asp:Label ID="lblQueueNamepop" runat="server"><b>Queue Name*&nbsp;&nbsp;</b></asp:Label>
                                                                    </div>

                                                                </td>
                                                                <td>
                                                                    <div class="form-group">
                                                                        <asp:DropDownList runat="server" ID="ddlQueueName" class="form-control input-sm" AutoPostBack="true" AppendDataBoundItems="True" OnSelectedIndexChanged="checkForQueueName_Click">
                                                                         <asp:ListItem Value="Select">--Select--</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                    </div>
                                                                </td>
                                                                
                                                            
                                                                <td class="auto-style16"></td>

                                                            </tr>
                                                           
                                                                    </table>


                                                                </td>
                                                                <td></td>
                                                            </tr>


                                                        </table>




                                                    </div>

                                                    <div class="modal-footer">
                                                       <table width="100%">
                                                                        <tr >
                                                                            <td width="15%"></td>
                                                                            <td width="35%">
                                                                                <div class="form-group" >
                                                                                    <asp:Button ID="Button5" runat="server" class="btn btn-primary colorSidebar" Text="Update" OnClick="UpdateQueue_click" Width="160px" />
                                                                                    <%--<asp:Button ID="Button3" runat="server" class="btn btn-primary colorSidebar"  Text="Save" OnClick="Button1_Click" width="150px"/>--%>
                                                                                </div>
                                                                            </td>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                            <td width="35%">
                                                                                <div class="form-group">
                                                                                    <asp:Button ID="Button6" runat="server" class="btn btn-danger" Text="Clear" Width="150px"  OnClick="btnClear_click" /><%--data-dismiss="modal"--%>
                                                                               
                                                                                    </div>
                                                                            </td>
                                                                            <td width="15%"></td>
                                                                        </tr>
                                                                    </table>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

              </ContentTemplate>
                        </asp:UpdatePanel>





                    <!-- /.panel-body -->
                    <%--DELETE ROBOT MODAL--%>
                    <asp:UpdatePanel runat="server">

                        <ContentTemplate>
                            <div class="modal fade " id="modalDelete" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
                                <div class="modal-dialog modal-lg" role="document">
                                    <div class="modal-content ">
                                        <div class="modal-header">
                                            <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">Delete Queue To Robot</h5>
                                            <asp:Button class="close" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" data-dismiss="modal" >
                                              
                                            </asp:Button>
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
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                            Robot Name :
                                                        </div>
                                                        <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2" align="left">
                                                            <asp:Label ID="lblRobotName" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                                            <asp:Label ID="lblId" Class="text-success" runat="server" hidden> </asp:Label>
                                                        </div>
                                                        <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                        </div>
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                            Queue Name :
                                                        </div>
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="left">
                                                            <asp:Label ID="lblQueueName" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--COMPLETE DELETE MODAL--%>

                    <%--DELETE ROBOT MODAL Second PopUp--%>
                    <asp:UpdatePanel runat="server">

                        <ContentTemplate>
                            <div class="modal fade " id="modalDeleteSecondPopUp" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelDeleteSecondPopUp" aria-hidden="true">
                                <div class="modal-dialog modal-lg" role="document">
                                    <div class="modal-content ">
                                        <div class="modal-header">
                                            <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">Delete Queue To Robot Permanently</h5>
                                            <asp:Button class="close" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop">
                                              
                                            </asp:Button>
                                            <%--<button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">×</span>
                                            </button>--%>
                                        </div>

                                        <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                            You are about to delete the queue to robot permanently.Deleted queue to robot will not be recovered.<br />
                                            Are you sure you want to <span class=" font-weight-bold text-danger">Delete </span>?

                                                <br />
                                            <br />
                                            <div class="form-group">
                                                <div class="container">
                                                    <div class="row">
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                            Robot Name :
                                                        </div>
                                                        <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2" align="left">
                                                            <asp:Label ID="lblRobotNameSecondPopUp" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                                            <asp:Label ID="lblIdSecondPopUp" Class="text-success" runat="server" hidden> </asp:Label>
                                                        </div>
                                                        <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                        </div>
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                            Queue Name :
                                                        </div>
                                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="left">
                                                            <asp:Label ID="lblQueueNameSecondPopUp" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
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
                    <%--COMPLETE DELETE MODAL Second PopUp --%>
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
            $("#bootstrapAlertMsgInfo").delay(800).fadeIn();
            $("#msgBox").delay(800).fadeIn();
            $("#page-wrapper").delay(200).fadeIn(200);
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
                        destroy:true
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
        function openUpdate() {
            $('#UpdateQueuepopUp').modal('show');
        };

    </script>

    
<script type="text/javascript"> 
    function hide_MsgSuccess() {


        document.getElementById('bootstrapAlertMsgInfo').style.display = 'none';
    }

    </script>

      <script type="text/javascript">
          
          function Open(data) {

              spltdata = data.split(',');
              BotName = spltdata[0];
              Queuename = spltdata[1];

              
              //Window.location.replace("BotLogs.aspx");
              $('#UpdateQueuepopUp').modal('show');
              document.getElementById("<%=txtBotName.ClientID%>").readOnly = "readonly";
              
              document.getElementById("<%=txtBotName.ClientID%>").value = BotName;
              //alert($("#lblQuenameHidden").val());
              document.getElementById("<%=lblQuenameHidden.ClientID%>").value = Queuename;
             
              

              //window.location.replace("BotLogs.aspx?BotName=" + BotName + "");
          }
    </script>


</asp:Content>
