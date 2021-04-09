<%@ Page Title="" Language="C#" MasterPageFile="~/ControlTower/MasterPageSkin.master" AutoEventWireup="true" CodeFile="AddRole.aspx.cs" Inherits="Control_Tower_AddRole" %>

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
                             <asp:Literal Id = "LitARADDROLE" runat="server" Text="<%$Resources:content,LitARADDROLE%>"></asp:Literal> 
                        </h2>   
                    </div>
                    <div class="card-body TableHeaderFont">
                        <div>
                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" class=" font-weight-normal poptxt"><b>
                                   <asp:Literal Id = "LitARRoleName" runat="server" Text="<%$Resources:content,LitARRoleName%>"></asp:Literal> 
                                    <span class="text-danger">*&nbsp;&nbsp;</span></b></asp:Label>
                                <asp:TextBox ID="txtRole" class="form-control input-sm animated--grow-in" runat="server"></asp:TextBox>
                            </div>
                             <div class="form-group">
                                <asp:Label ID="Label4" runat="server" class=" font-weight-normal poptxt"><b>
                                     <asp:Literal Id = "LitGroupName" runat="server" Text="<%$Resources:content,LitGroupName%>"></asp:Literal> 
                                    <span class="text-danger">*&nbsp;&nbsp;</span></b></asp:Label>
                                 <asp:DropDownList ID="DrpGroupName" runat="server" class="form-control animated--grow-in" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" AppendDataBoundItems="True">
                                            <asp:ListItem>--Select--</asp:ListItem>
                                        </asp:DropDownList>
                            </div>
                            <div class="form-group" align="center">
                                <asp:Button ID="btnSaveRole" runat="server" class="btn btn-primary colorSidebar" Text="<%$Resources:content,LitSave%>" OnClick="BtnSaveRole" Width="49%" />
                                <asp:Button ID="btnCancelRole" runat="server" class="btn btn-danger" Text="<%$Resources:content,LitCancel%>" OnClick="BtnCancelRole" Width="49%" />
                            </div>
                            <br />
                        </div>
                    </div>
                </div>
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h2 class="m-0 font-weight-bold poptxt">
                            <asp:Literal Id = "LitROLES" runat="server" Text="<%$Resources:content,LitROLES%>"></asp:Literal>
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
                                                   <asp:Literal Id = "LitROLES" runat="server" Text="<%$Resources:content,LitGroupName%>"></asp:Literal> 
                                                </th>
                                                <th scope="col" class=" font-weight-bold ">
                                                  <asp:Literal Id = "LitRole" runat="server" Text="<%$Resources:content,LitRole%>"></asp:Literal>  
                                                </th>
                                                <th scope="col" class=" font-weight-bold ">
                                                   <asp:Literal Id = "LitCreatedBy" runat="server" Text="<%$Resources:content,LitCreatedBy%>"></asp:Literal> 
                                                </th>
                                                <th scope="col" class=" font-weight-bold ">
                                                   <asp:Literal Id = "LitCreatedDate" runat="server" Text="<%$Resources:content,LitCreatedDate%>"></asp:Literal>
                                                </th>
                                                <th scope="col" class=" font-weight-bold ">
                                                  <asp:Literal Id = "LitAccess" runat="server" Text="<%$Resources:content,LitAccess%>"></asp:Literal>   
                                                </th>
                                                <th scope="col" class=" font-weight-bold text-danger">
                                                   <asp:Literal Id = "LitDelete" runat="server" Text="<%$Resources:content,LitDelete%>"></asp:Literal> 
                                                </th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="poptxtdata">

                                         <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="Label1" name="RolesID" runat="server" Text='<%# Eval("groupname") %>' Height="12px" />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="lblRolesID" name="RolesID" runat="server" Text='<%# Eval("rolename") %>' Height="12px" />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("createdby") %>' />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("createddate" )%>' />
                                        </td>
                                        <td class="odd gradeX" style="text-align: center;">
                                            <asp:ImageButton ID="btnAccess" runat="server" ImageUrl="~/Images/access.png" ImageAlign="Middle" Width="16%" CommandArgument='<%# Eval("roleid")+","+ Eval("tenantid") + ","+ Eval("rolename") + ","+ Eval("groupid")  %>' OnCommand="ImageButton1_Command" Text="Access" />
                                        </td>
                                        <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                            <asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/Images/delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("roleid")+","+Eval("rolename")+ ","+ Eval("rolename")+ ","+ Eval("groupid")%>' CommandName="Delete" OnCommand="btnDelete_Click" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div id="output1">l
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
                                   <asp:Literal Id = "LitDeleteGroup" runat="server" Text="<%$Resources:content,LitDeleteGroup%>"></asp:Literal>  

                                </h5>
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
                                    <asp:Literal Id = "Literal1" runat="server" Text="<%$Resources:content,LitDeleteGroup%>"></asp:Literal>

                                </h5>
                                <asp:Button ID="Button6" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                            </div>

                            <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                               <asp:Literal Id = "LitYouareabouttodeletethegrouppermanentlyDeletedgroupwillnotberecovered" runat="server" Text="<%$Resources:content,LitYouareabouttodeletethegrouppermanentlyDeletedgroupwillnotberecovered%>"></asp:Literal>
                                <br />
                                 <asp:Literal Id = "LitAreyousureyouwantto1" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal>  
                                <span class=" font-weight-bold text-danger">
                                    <asp:Literal Id = "LitpmDelete1" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal>  </span>

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
                                    <asp:Literal Id = "LitpmDelete2" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal> </span>
                                                                          
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
                                <h5 class="modal-title h5 font-weight-bold poptxt" id="exampleModalLabel2">
                                   <asp:Literal Id = "LitUpdateProcessFile" runat="server" Text="<%$Resources:content,LitUpdateProcessFile%>"></asp:Literal>  

                                </h5>
                                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">×</span>
                                </button>
                            </div>

                            <div class="modal-body" style="align-content: center" runat="server">
                                 <asp:HiddenField runat="server" ID="_HGroupid" />
                                <asp:HiddenField runat="server" ID="_HRoleid" />
                                <asp:HiddenField runat="server" ID="_HTenantID" />
                                <asp:HiddenField runat="server" ID="_RoleName" />
                                

                                <%--card begins--%>
                                <div class="card shadow">
                                    <div >
                                        
                                                              <asp:Table ID="Table1" CssClass="table table-striped table-bordered table-hover poptxt" runat="server">
                                                                  <asp:TableHeaderRow align="center" CssClass="TableHeaderFont">
                                                                      <asp:TableHeaderCell>
                                                                          <span class="text-dark">
                                                                              <asp:Literal Id = "LitPageAccess" runat="server" Text="<%$Resources:content,LitPageAccess%>"></asp:Literal> 

                                                                          </span>
                                                                          <asp:CheckBox ID="chkHeader" name="sample" onclick="javascript:CheckUncheckall(this);" class="selectall" runat="server" />
                                                                      </asp:TableHeaderCell>
                                                                      <asp:TableHeaderCell>
                                                                          <span class="text-success" >
                                                                         <asp:Literal Id = "LitRead" runat="server" Text="<%$Resources:content,LitRead%>"></asp:Literal>  

                                                                                           </span></asp:TableHeaderCell>
                                                                      <asp:TableHeaderCell>
                                                                          <span class="text-muted" >
                                                                          <asp:Literal Id = "LitCreate" runat="server" Text="<%$Resources:content,LitCreate%>"></asp:Literal>   

                                                                                           </span></asp:TableHeaderCell>
                                                                      <asp:TableHeaderCell>
                                                                          <span class="text-warning" ">
                                                                          <asp:Literal Id = "LitEdit" runat="server" Text="<%$Resources:content,LitEdit%>"></asp:Literal>

                                                                                           </span></asp:TableHeaderCell>
                                                                      <asp:TableHeaderCell>
                                                                          <span class="text-danger" ">
                                                                          <asp:Literal Id = "LitDelete" runat="server" Text="<%$Resources:content,LitDelete%>"></asp:Literal>

                                                                                           </span></asp:TableHeaderCell>
                                                                  </asp:TableHeaderRow>



                                                                  <asp:TableRow ID="DashBoardRow" align="center">
                                                                      <asp:TableHeaderCell>
                                                                          <span class="poptxt font-weight-bold">
                                                                             <asp:Literal Id = "LitBotDashboard" runat="server" Text="<%$Resources:content,LitBotDashboard%>"></asp:Literal> 

                                                                          </span>&nbsp;&nbsp;
                                                                    <asp:CheckBox ID="CheckBox1" name="sample" onclick="javascript:CheckUncheckDashBoard(this);" class="selectall" runat="server" />
                                                                      </asp:TableHeaderCell>
                                                                      <asp:TableCell>

                                                                          <asp:CheckBox ID="chkBotDashboardR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotDashboardC"  Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotDashboardE" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotDashboardD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="ProcessManagemenRow" align="center">
                                                                      <asp:TableHeaderCell><span>
                                                                          <asp:Literal Id = "LitProcessManagement" runat="server" Text="<%$Resources:content,LitProcessManagement%>"></asp:Literal> 

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox13" name="sample" onclick="javascript:CheckUncheckProcessManagemen(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
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

                                                                  <asp:TableRow ID="QueueDetailsRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                          <asp:Literal Id = "LitQueueDetails" runat="server" Text="<%$Resources:content,LitQueueDetails%>"></asp:Literal> 

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox2" name="sample" onclick="javascript:CheckUncheckQueueDetail(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkQueueD" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>


                                                                  <asp:TableRow ID="AddSchedulesRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                          <asp:Literal Id = "LitAddSchedule" runat="server" Text="<%$Resources:content,LitAddSchedule%>"></asp:Literal> 

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox3" name="sample" onclick="javascript:CheckUncheckAddSchedules(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddScheduleR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="ChkAddScheduleC" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddScheduleE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddScheduleD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="ScheduleDetailsRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                          <asp:Literal Id = "LitScheduleDetails" runat="server" Text="<%$Resources:content,LitScheduleDetails%>"></asp:Literal>

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox11" name="sample" onclick="javascript:CheckUncheckScheduleDetails(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkScheduleDetailsR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="ChkScheduleDetailsC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="ChkScheduleDetailsE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="ChkScheduleDetailsD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>



                                                                  <asp:TableRow ID="AddUserRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                          <asp:Literal Id = "LitAddUser" runat="server" Text="<%$Resources:content,LitAddUser%>"></asp:Literal>

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox4" name="sample" onclick="javascript:CheckUncheckAddUser(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
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

                                                                  <asp:TableRow ID="AddRobotRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                         <asp:Literal Id = "LitAddRobot" runat="server" Text="<%$Resources:content,LitAddRobot%>"></asp:Literal>

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox5" name="sample" onclick="javascript:CheckUncheckAddRobot(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddRobotR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddRobotC" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddRobotE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAddRobotD" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="AssignQueueToBotRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                          <asp:Literal Id = "LitAssignQueueToBot" runat="server" Text="<%$Resources:content,LitAssignQueueToBot%>"></asp:Literal>

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox7" name="sample" onclick="javascript:CheckUncheckAssignQueueToBot(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
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

                                                                  <asp:TableRow ID="AssignBotToUserRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                        <asp:Literal Id = "LitAssignBotToUser" runat="server" Text="<%$Resources:content,LitAssignBotToUser%>"></asp:Literal>  

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox8" name="sample" onclick="javascript:CheckUncheckAssignBotToUser(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
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

                                                                  <asp:TableRow ID="QueueManagementRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                           <asp:Literal Id = "LitQueueManagement" runat="server" Text="<%$Resources:content,LitQueueManagement%>"></asp:Literal>

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox6" name="sample" onclick="javascript:CheckUncheckQueueManagement(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
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

                                                                  <asp:TableRow ID="figurationManagementRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                          <asp:Literal Id = "LitConfigurationManagement" runat="server" Text="<%$Resources:content,LitConfigurationManagement%>"></asp:Literal>

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox12" name="sample" onclick="javascript:CheckUncheckfigurationManagement(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
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

                                                                  <asp:TableRow ID="PromoteDemoteAutomationRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                         <asp:Literal Id = "LitPromoteDemoteAutomation" runat="server" Text="<%$Resources:content,LitPromoteDemoteAutomation%>"></asp:Literal> 

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox15" name="sample" onclick="javascript:CheckUncheckPromoteDemoteAutomation(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkPromoteDemoteR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkPromoteDemoteC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkPromoteDemoteE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkPromoteDemoteD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>


                                                                   <asp:TableRow id="BotLogRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                         <asp:Literal Id = "LitBotLog" runat="server" Text="<%$Resources:content,LitBotLog%>"></asp:Literal>  

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox9" name="sample" onclick="javascript:CheckUncheckBotLog(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotLogR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotLogC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotLogE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkBotLogD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  
                                                                  <asp:TableRow ID="AuditTrailRow" align="center">
                                                                      <asp:TableHeaderCell><span >
                                                                          <asp:Literal Id = "LitAuditTrail" runat="server" Text="<%$Resources:content,LitAuditTrail%>"></asp:Literal> 

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox10" name="sample" onclick="javascript:CheckUncheckAuditTrail(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAuditTrailR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAuditTrailC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAuditTrailE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkAuditTrailD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="DetaillLogRow" align="center">
                                                                      <asp:TableHeaderCell><span>
                                                                          <asp:Literal Id = "LitDetailLogs" runat="server" Text="<%$Resources:content,LitDetailLogs%>"></asp:Literal>  

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox14" name="sample" onclick="javascript:CheckUncheckDetaillLog(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkDetailLogR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkDetailLogC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkDetailLogE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkDetailLogD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>

                                                                  <asp:TableRow ID="ReportsRow" align="center">
                                                                      <asp:TableHeaderCell><span class="text-primary">
                                                                          <asp:Literal Id = "LitReports" runat="server" Text="<%$Resources:content,LitReports%>"></asp:Literal>  

                                                                                           </span>&nbsp;&nbsp;<asp:CheckBox ID="CheckBox16" name="sample" onclick="javascript:CheckUncheckReports(this);" class="selectall" runat="server" /></asp:TableHeaderCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkReportsR" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkReportsC" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkReportsE" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                      <asp:TableCell>
                                                                          <asp:CheckBox ID="chkReportsD" Enabled="false" runat="server" />
                                                                      </asp:TableCell>
                                                                  </asp:TableRow>
                                                              </asp:Table>
                                       <%-- <asp:Table ID="Table1" CssClass="table table-bordered table-hover TableHeaderFont poptxt" align="center" runat="server">
                                            <asp:TableHeaderRow align="center">
                                                <asp:TableHeaderCell> <span class="text-dark" >Page Access</span></asp:TableHeaderCell>
                                                <asp:TableHeaderCell><span class="text-success" >Read</span></asp:TableHeaderCell>
                                                <asp:TableHeaderCell><span class="text-muted" >Write</span></asp:TableHeaderCell>
                                                <asp:TableHeaderCell><span class="text-warning" ">Edit</span></asp:TableHeaderCell>
                                                <asp:TableHeaderCell><span class="text-danger" ">Delete</span></asp:TableHeaderCell>
                                            </asp:TableHeaderRow>


                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span class=" font-weight-bold">BotDashboard</span></asp:TableHeaderCell>
                                                <asp:TableCell >

                                                    <asp:CheckBox ID="chkBotDashboardR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotDashboardC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotDashboardE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotDashboardD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span >Queue Details</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkQueueR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkQueueC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkQueueE" runat="server" />
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
                                                    <asp:CheckBox ID="ChkAddScheduleC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddScheduleE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddScheduleD" runat="server" />
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
                                                    <asp:CheckBox ID="chkAddRobotE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddRobotD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span class="text-primary">Queue Management</span></asp:TableHeaderCell>
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


                                           <%-- <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span>Add Group</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddGroupR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddGroupC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddGroupE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAddGroupD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>--%>


                                           <%--  <asp:TableRow align="center">
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
                                                <asp:TableHeaderCell><span>Assign Bot To User</span></asp:TableHeaderCell>
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
                                                <asp:TableHeaderCell><span>Bot Log</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotLogR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotLogC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotLogE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkBotLogD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>


                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span>Audit Trail</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAuditTrailR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAuditTrailC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAuditTrailE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkAuditTrailD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>



                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span >Schedule Details</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkScheduleDetailsR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="ChkScheduleDetailsC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="ChkScheduleDetailsE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="ChkScheduleDetailsD" runat="server" />
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
                                                <asp:TableHeaderCell><span class="text-primary">Detail Log</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkDetailLogR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkDetailLogC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkDetailLogE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkDetailLogD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>



                                            <asp:TableRow align="center">
                                                <asp:TableHeaderCell><span >Promote Demote Automation</span></asp:TableHeaderCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkPromoteDemoteR" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkPromoteDemoteC" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkPromoteDemoteE" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:CheckBox ID="chkPromoteDemoteD" runat="server" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>--%>


                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-danger" data-dismiss="modal"> <asp:Literal Id = "LitClose1" runat="server" Text="<%$Resources:content,LitClose%>"></asp:Literal>  </button>
                                <asp:Button ID="btnSaveAccess" CssClass="btn btn-success" runat="server" Text="<%$Resources:content,LitSave%>" OnClientClick="javascript:po();" OnCommand="btnSaveAccess_Command" />

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
      <script language="javascript" type="text/javascript">
        function CheckUncheckall(chk) {
            var chks = document.getElementById("<% = Table1.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
               
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }

        function CheckUncheckDashBoard(chk) {
            var chks = document.getElementById("<% = DashBoardRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                 
                if (chks[i].type == "checkbox" && !chks[i].disabled) 
                     chks[i].checked = chk.checked;
                
               
            }
        }
        function CheckUncheckQueueDetail(chk) {
            var chks = document.getElementById("<% = QueueDetailsRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAddSchedules(chk) {
            var chks = document.getElementById("<% = AddSchedulesRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAddUser(chk) {
            var chks = document.getElementById("<% = AddUserRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAddRobot(chk) {
            var chks = document.getElementById("<% = AddRobotRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckQueueManagement(chk) {
            var chks = document.getElementById("<% = QueueManagementRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAssignQueueToBot(chk) {
            var chks = document.getElementById("<% =  AssignQueueToBotRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAssignBotToUser(chk) {
            var chks = document.getElementById("<% =  AssignBotToUserRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckBotLog(chk) {
            var chks = document.getElementById("<% =  BotLogRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckAuditTrail(chk) {
            var chks = document.getElementById("<% = AuditTrailRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckScheduleDetails(chk) {
            var chks = document.getElementById("<% = ScheduleDetailsRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }

        function CheckUncheckfigurationManagement(chk) {
            var chks = document.getElementById("<% = figurationManagementRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }

        function CheckUncheckProcessManagemen(chk) {
            var chks = document.getElementById("<% = ProcessManagemenRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckDetaillLog(chk) {
            var chks = document.getElementById("<% = DetaillLogRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
        function CheckUncheckPromoteDemoteAutomation(chk) {
            var chks = document.getElementById("<% = PromoteDemoteAutomationRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
           function  CheckUncheckReports(chk) {
            var chks = document.getElementById("<% = ReportsRow.ClientID %>").getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].type == "checkbox" && !chks[i].disabled) chks[i].checked = chk.checked;
            }
        }
       

    </script>

</asp:Content>

