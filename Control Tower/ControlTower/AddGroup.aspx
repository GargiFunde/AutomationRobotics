<%@ Page Title="" Language="C#" MasterPageFile="~/ControlTower/MasterPageSkin.master" AutoEventWireup="true" CodeFile="AddGroup.aspx.cs" Inherits="DemoMasterPage2_AddGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
    <!-- Begin Page Content -->
     <div id="page-wrapper" class="container-fluid">

        <!-- Page Heading -->
        <%--<h1 class="h3 mb-2 text-gray-800">Process Management</h1>--%>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <!-- DataTables Example -->
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h2 class="m-0 font-weight-bold poptxt">
                             <asp:Literal Id = "LitADDGROUP" runat="server" Text="<%$Resources:content,LitADDGROUP%>"></asp:Literal> 
                        </h2>   
                    </div>
                    <div class="card-body TableHeaderFont">
                        <div>
                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" class=" font-weight-normal poptxt"><b>
                                     <asp:Literal Id = "LitGroupName" runat="server" Text="<%$Resources:content,LitGroupName%>"></asp:Literal>
                                    <span class="text-danger">*&nbsp;&nbsp;</span></b></asp:Label>
                                <asp:TextBox ID="txtGroup" class="form-control input-sm animated--grow-in" runat="server"></asp:TextBox>
                            </div>

                            <div class="form-group" align="center">
                                <asp:Button ID="btnSaveGroup" runat="server" class="btn btn-primary colorSidebar" Text="<%$Resources:content,LitSave%>" OnClick="BtnSaveGroup" Width="49%" />
                                <asp:Button ID="btnCancelGroup" runat="server" class="btn btn-danger" Text="<%$Resources:content,LitCancel%>" OnClick="BtnCancelGroup" Width="49%" />
                            </div>
                            <br />
                        </div>
                    </div>
                </div>
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h2 class="m-0 font-weight-bold poptxt">
                            <asp:Literal Id = "LitGROUPS" runat="server" Text="<%$Resources:content,LitGROUPS%>"></asp:Literal>
                   <asp:ImageButton ID="ImageButton1" runat="server" Class="rotate refreshbtn " ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="ImageButton3_Command" />

                        </h2>
                    </div>
                    <div class="card-body">
                        <div>
                            <asp:Repeater ID="GVRoles" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-striped table-bordered table-hover" id="dataTables-users" width="100%">
                                        <thead class="colorSidebar" align="center">
                                            <tr class="poptxt TableHeaderFont">
                                                <th scope="col" class=" font-weight-bold ">
                                                    <asp:Literal Id = "LitGroupName" runat="server" Text="<%$Resources:content,LitGroupName%>"></asp:Literal> 
                                                </th>
                                                <th scope="col" class=" font-weight-bold ">
                                                    <asp:Literal Id = "LitCreatedBy" runat="server" Text="<%$Resources:content,LitCreatedBy%>"></asp:Literal> 
                                                </th>
                                                <th scope="col" class=" font-weight-bold ">
                                                    <asp:Literal Id = "LitCreatedDate" runat="server" Text="<%$Resources:content,LitCreatedDate%>"></asp:Literal> 
                                                </th>
                                                <%--<th scope="col" class=" font-weight-bold ">Access 
                                                </th>--%>
                                                <th scope="col" class=" font-weight-bold text-danger">
                                                    <asp:Literal Id = "LitDelete" runat="server" Text="<%$Resources:content,LitDelete%>"></asp:Literal>
                                                </th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="poptxtdata">

                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="lblRolesID" name="RolesID" runat="server" Text='<%# Eval("groupname") %>' Height="12px" />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("createdby") %>' />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("createddate" )%>' />
                                        </td>
                                       <%-- <td class="odd gradeX" style="text-align: center;">
                                            <asp:ImageButton ID="btnAccess" runat="server" ImageUrl="~/Images/access.png" ImageAlign="Middle" Width="16%" CommandArgument='<%# Eval("groupid")+","+ Eval("tenantid") + ","+ Eval("groupname") %>' OnCommand="ImageButton1_Command" Text="Access" />
                                        </td>--%>
                                        <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                            <asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/Images/delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("groupid")+","+Eval("groupname")%>' CommandName="Delete" OnCommand="btnDelete_Click" />
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

            </ContentTemplate>
        </asp:UpdatePanel>

        <%--DELETE ROBOT MODAL--%>
        <asp:UpdatePanel runat="server">

            <ContentTemplate>
                <div class="modal fade " id="modalDelete" data-backdrop="static" data-keyboard="false"  tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">
                                    <asp:Literal Id = "LitDeleteGroup" runat="server" Text="<%$Resources:content,LitDeleteGroup%>"></asp:Literal> </h5>
                                 <asp:Button runat="server" data-dismiss="modal" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" ><%--data-dismiss="modal" aria-label="Close"--%> <%--OnClick="btnXclosePopup"--%>
                                                    </asp:Button>
                            </div>

                            <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                <asp:Literal Id = "LitAreyousureyouwantto" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal>
                                <span class=" font-weight-bold text-danger">
                                    <asp:Literal Id = "LitpmDelete" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal> </span>
                                                                          
                                         <br />
                                <br />
                                <div class="form-group">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6" align="right">
                                                <asp:Literal Id = "LitAGGroupName" runat="server" Text="<%$Resources:content,LitAGGroupName%>"></asp:Literal>
                                            </div>
                                            <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6" align="left">
                                                <asp:Label ID="lblGroupName" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                                <asp:Label ID="lblId" Class="text-success" runat="server" hidden> </asp:Label>
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
                                                <asp:Button ID="btnDelete" runat="server" class="btn btn-danger btn-block"  Text="<%$Resources:content,LitDelete%>" align="center" OnClick="ModalPopUpBtnDelete_Click" />
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                <button class="btn btn-primary btn-block" type="button" data-dismiss="modal"> <asp:Literal Id = "LitClose" runat="server" Text="<%$Resources:content,LitClose%>"></asp:Literal></button>
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
        <%--DELETE GROUP MODAL END--%>

        <%--DELETE ROBOT MODAL Second PopUp--%>

        <asp:UpdatePanel runat="server">

            <ContentTemplate>
                <div class="modal fade " id="modalDeleteSecondPopUp" data-backdrop="static" data-keyboard="false"  tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelSecondPopUp" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document" style="width: 900px; height: 140px;">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h4 font-weight-bold text-danger" id="exampleModalLabelSecondPopUp">
                                    <asp:Literal Id = "LitDeleteGroup1" runat="server" Text="<%$Resources:content,LitDeleteGroup%>"></asp:Literal> 

                                </h5>
                                <asp:Button ID="Button6" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                            </div>

                            <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                <asp:Literal Id = "LitYouareabouttodeletethegrouppermanentlyDeletedgroupwillnotberecovered" runat="server" Text="<%$Resources:content,LitYouareabouttodeletethegrouppermanentlyDeletedgroupwillnotberecovered%>"></asp:Literal> 
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
                                            <br />
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                <asp:Literal Id = "LitAGGroupId" runat="server" Text="<%$Resources:content,LitAGGroupId%>"></asp:Literal> 
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                <asp:Label ID="lblGroupIdSecondPopUp" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                               <asp:Literal Id = "LitAGGroupName1" runat="server" Text="<%$Resources:content,LitAGGroupName%>"></asp:Literal>  
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                <asp:Label ID="lblGroupNameSecondPopUp" runat="server"> </asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <br />
                                        </div>
                                        <div class="row">
                                            <br />
                                        </div>
                                        <div class="modal-body h6  text-primary" style="text-align: center" runat="server">
                                            <span class="h5 font-weight-bold text-danger">
                                                <asp:Literal Id = "LitCounts" runat="server" Text="<%$Resources:content,LitCounts%>"></asp:Literal>  

                                            </span>
                                            <br />
                                        </div>


                                        <div class="row">
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                 <asp:Literal Id = "LitMessagingDetails" runat="server" Text="<%$Resources:content,LitMessagingDetails%>"></asp:Literal> 
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                <asp:Label ID="lblMessagingdetails" runat="server"> </asp:Label>
                                            </div>

                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                               <asp:Literal Id = "LitAGUsers" runat="server" Text="<%$Resources:content,LitAGUsers%>"></asp:Literal>  
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                <asp:Label ID="lblUsers" runat="server"> </asp:Label>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                               <asp:Literal Id = "LitAGBots" runat="server" Text="<%$Resources:content,LitAGBots%>"></asp:Literal>  
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                <asp:Label ID="lblBot" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                               <asp:Literal Id = "LitAGProcesses" runat="server" Text="<%$Resources:content,LitAGProcesses%>"></asp:Literal>  
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                <asp:Label ID="lblProcess" runat="server"> </asp:Label>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                               <asp:Literal Id = "LitAGProcessUploads" runat="server" Text="<%$Resources:content,LitAGProcessUploads%>"></asp:Literal>  
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                <asp:Label ID="lblProcessUpload" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                <asp:Literal Id = "LitAGRQDetails" runat="server" Text="<%$Resources:content,LitAGRQDetails%>"></asp:Literal>   
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                <asp:Label ID="lblRQDetails" runat="server"> </asp:Label>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                <asp:Literal Id = "LitAGScheduleDetails" runat="server" Text="<%$Resources:content,LitAGScheduleDetails%>"></asp:Literal>  
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                <asp:Label ID="lblScheduleDeatils" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                               <asp:Literal Id = "LitAGUserBotMapping" runat="server" Text="<%$Resources:content,LitAGUserBotMapping%>"></asp:Literal>
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                <asp:Label ID="lblUserBotMapping" runat="server"> </asp:Label>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                               <asp:Literal Id = "LitAGBotDefaultQueue" runat="server" Text="<%$Resources:content,LitAGBotDefaultQueue%>"></asp:Literal> 
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                <asp:Label ID="lblBotdefaultqueue" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                <asp:Literal Id = "LitAGConfigurationParameters" runat="server" Text="<%$Resources:content,LitAGConfigurationParameters%>"></asp:Literal> 
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                <asp:Label ID="lblconfigurationparameters" runat="server"> </asp:Label>
                                            </div>

                                        </div>


                                    </div>

                                </div>

                                <div class="row">
                                    <br />
                                </div>


                                <div class="row">
                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                    </div>
                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2">
                                        <asp:Button ID="Button2" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" OnClick="ModalPopUpBtnDelete_ClickSecondPopUp" />
                                    </div>
                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2">
                                        <asp:Button ID="btnEnableDisable" runat="server" OnClick="CommandBtnDisable_Click" />
                                    </div>
                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2">
                                        <asp:Button ID="Button5" runat="server" class="btn btn-primary btn-block" Text="<%$Resources:content,LitClose%>" OnClick="ModalPopUpBtnClose_ClickSecondPopUp" />
                                    </div>
                                    <%--<divclass="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            </div>--%>
                                </div>
                            </div>
                        </div>
                    </div></div>
                    <%--  </div>
                        </div>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--End Delete Second PopUp user--%>
        <%-- Delete tentant 3  Second PopUp Modal Start--%>
        <asp:UpdatePanel runat="server">

            <ContentTemplate>
                <div class="modal fade " id="modalDeleteThirdPopUp" data-backdrop="static" data-keyboard="false"  tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel3" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel3">
                                    <asp:Literal Id = "LitAGDeleteGroupPermanently" runat="server" Text="<%$Resources:content,LitAGDeleteGroupPermanently%>"></asp:Literal> 

                                </h5>
                                <asp:Button ID="Button7" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                            </div>

                            <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                <asp:Literal Id = "LitAGYouareabouttodeletethegrouppermanentlyDeletedgroupwillnotberecovered" runat="server" Text="<%$Resources:content,LitAGYouareabouttodeletethegrouppermanentlyDeletedgroupwillnotberecovered%>"></asp:Literal> 
                                <br />
                               <asp:Literal Id = "LitAreyousureyouwantto2" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal> 
                                <span class=" font-weight-bold text-danger">
                                    <asp:Literal Id = "LitpmDelete2" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal>  </span>
                                                                          
                                         <br />
                                <br />
                                <div class="form-group">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6" align="right">
                                                <asp:Literal Id = "LitAGGroupName2" runat="server" Text="<%$Resources:content,LitAGGroupName%>"></asp:Literal>
                                            </div>
                                            <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6" align="left">
                                                <asp:Label ID="lblGroupNameThirdPopUp" Class="font-weight-bold text-danger" runat="server"> </asp:Label>
                                                <asp:Label ID="lblIdThirdPopUp" Class="text-success" runat="server" hidden> </asp:Label>
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
                                                <asp:Button ID="Button1" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" align="center" OnClick="ModalPopUpBtnDelete_ClickThirdPopUp" />
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
        <%--DELETE GROUP MODAL 3 PopUp END --%>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal fade" id="UpdateVersion" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel2" aria-hidden="true">
                    <div class="modal-dialog " role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title h5 font-weight-bold poptxt" id="exampleModalLabel2">Update Process File</h5>
                                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">×</span>
                                </button>
                            </div>

                            <div class="modal-body" style="align-content: center" runat="server">
                                <asp:HiddenField runat="server" ID="_HGroupid" />
                                <asp:HiddenField runat="server" ID="_HTenantID" />
                                <asp:HiddenField runat="server" ID="_Role" />

                                <%--card begins--%>
                                <div class="card shadow">
                                    <div >

                                        <asp:Table ID="Table1" CssClass="table table-bordered table-hover TableHeaderFont poptxt" align="center" runat="server">
                                            <asp:TableHeaderRow align="center">
                                                <asp:TableHeaderCell> <span class="text-dark" >Page Access</span></asp:TableHeaderCell>
                                                <asp:TableHeaderCell><span class="text-success" >Read</span></asp:TableHeaderCell>
                                                <asp:TableHeaderCell><span class="text-muted" >Create</span></asp:TableHeaderCell>
                                                <asp:TableHeaderCell><span class="text-warning" ">Edit</span></asp:TableHeaderCell>
                                                <asp:TableHeaderCell><span class="text-danger" ">Delete</span></asp:TableHeaderCell>
                                            </asp:TableHeaderRow>


                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span class=" font-weight-bold">BotDashboard</span></asp:TableHeaderCell>
                                                <asp:TableCell >

                                                    <asp:CheckBox ID="chkBotDashboardR" runat="server"  />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotDashboardC" enabled="false" runat="server"  />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotDashboardE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotDashboardD" enabled="false"  runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                             <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span class="text-primary">Process Management</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkProcessManagementR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkProcessManagementC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkProcessManagementE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkProcessManagementD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span >Queue Details</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkQueueR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkQueueC" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkQueueE" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkQueueD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>


                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span>Add Schedules</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddScheduleR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="ChkAddScheduleC"  runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddScheduleE" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddScheduleD" enabled="false" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                             <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span >Schedule Details</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkScheduleDetailsR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="ChkScheduleDetailsC" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="ChkScheduleDetailsE" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="ChkScheduleDetailsD" enabled="false" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                           

                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span >Add User</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddUserR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddUserC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddUserE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddUserD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span >Add Robot</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddRobotR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddRobotC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddRobotE" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddRobotD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                             <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span>Assign Queue To Bot</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAssignQueueBotR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAssignQueueBotC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAssignQueueBotE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAssignQueueBotD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                              <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span>Assign Robot To User</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAssignBotUserR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAssignBotUserC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAssignBotUserE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAssignBotUserD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell>Queue Management</asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkQueueManagementR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkQueueManagementC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkQueueManagementE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkQueueManagementD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span >Configuration Management</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkConfigurationR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkConfigurationC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkConfigurationE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkConfigurationD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span >Promote Demote Automation</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkPromoteDemoteR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkPromoteDemoteC" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkPromoteDemoteE" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkPromoteDemoteD" enabled="false" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                       
                                           
                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span>Bot Log</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotLogR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotLogC" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotLogE" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotLogD" enabled="false" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span>Audit Trail</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAuditTrailR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAuditTrailC" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAuditTrailE" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAuditTrailD" enabled="false" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                         <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span class="text-primary">Detail Log</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkDetailLogR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkDetailLogC" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkDetailLogE" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkDetailLogD" enabled="false" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                           <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span class="text-primary">Reports</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="CheckBox5" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="CheckBox6" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="CheckBox7" enabled="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="CheckBox8" enabled="false" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                         </asp:Table>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                                <asp:Button ID="btnSaveAccess" CssClass="btn btn-success" runat="server" Text="Save All" OnClientClick="javascript:po();" OnCommand="btnSaveAccess_Command" />

                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <!-- /.panel-body -->

    </div>

    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script>
        function openModal() {
            $('#UpdateVersion').modal('show');
        }
    </script>
    <script>
        function po() {

            $('#UpdateVersion').modal('hide');
        }
    </script>
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

    </script>
</asp:Content>
