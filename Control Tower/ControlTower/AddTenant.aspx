<%@ Page Title="" Language="C#" MasterPageFile="~/ControlTower/MasterPageSkin.master" AutoEventWireup="true" CodeFile="AddTenant.aspx.cs" Inherits="DemoMasterPage2_AddTenant" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    

  
   <script type="text/javascript">

       function openModal() {

           $('#AddNewTenant').modal('show');

       }
       function closeModal() {
           location.reload();
       }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


      <asp:ScriptManager runat="server">
                    </asp:ScriptManager>
     
    <!-- Begin Page Content -->
    <div id="page-wrapper" class="container-fluid">
        
                  
        <!-- Page Heading -->
        <%--<h1 class="h3 mb-2 text-gray-800">Add Tenant</h1>--%>
        
        <!-- DataTables Example -->
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

           
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h2 class="m-0 font-weight-bold poptxt"> <asp:Literal Id="LitADDTENANTS" runat="server" Text="<%$Resources:content,LitADDTENANTS%>"></asp:Literal> 
                </h2>
            </div>
            <div class="card-body">
                <div>

                   
                            <div class="form-group">
                                <asp:Label ID="lblTenantName" runat="server" class="TableHeaderFont font-weight-bold poptxt">
                                    <asp:Literal Id="LitTenantName" runat="server" Text="<%$Resources:content,LitTenantName%>"></asp:Literal>
                                    <span class="text-danger">*</span>&nbsp;&nbsp;</asp:Label>
                             <asp:TextBox ID="txtTenantName" class="form-control" runat="server"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lblOwner" class="TableHeaderFont font-weight-bold poptxt" runat="server">
                                    <asp:Literal Id="LitOwner" runat="server" Text="<%$Resources:content,LitOwner%>"></asp:Literal>
                                    <span class="text-danger">*</span>&nbsp;&nbsp;</asp:Label>
                             <asp:TextBox ID="txtOwner" class="form-control input-sm" runat="server"></asp:TextBox>
                            </div>

                            <div class="form-group" align="center">
                                <asp:Button ID="btnShow" runat="server" class="btn btn-primary colorSidebar" Text="<%$Resources:content,LitSave%>" OnClick="Save_Click" Width="49%" />
                                <asp:Button ID="btnCancel" runat="server" class="btn btn-danger" Text="<%$Resources:content,LitClear%>" Width="49%" OnClick="btnCancel_Click"  />
                            </div>

                            <div class="form-group">
                            </div>
                            </div>
                </div>
            </div>
                  </br>
                 <div class="card shadow mb-4">
                    <div class="card-header py-3">
               <h2 class="m-0 font-weight-bold poptxt"> 
                   <asp:Literal Id="LitTENANTS" runat="server" Text="<%$Resources:content,LitTENANTS%>"></asp:Literal>
                   <asp:ImageButton ID="ImageButton2" runat="server" Class="rotate refreshbtn " ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="ImageButton3_Command" />

                </h2>
            </div>
            <div class="card-body">
                <div>
                            <div class="panel-body table-responsive">
                                <asp:Repeater ID="GVRoles" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-bordered table-hover " id="dataTables-users" width="100%">
                                            <thead class="colorSidebar" align="center">
                                                <tr class="TableHeaderFont">
                                                    <th scope="col" class=" font-weight-bold poptxt">
                                                        <asp:Literal Id="LitTENANTS" runat="server" Text="<%$Resources:content,LitTenantId%>"></asp:Literal>
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold poptxt">
                                                        <asp:Literal Id="Literal1" runat="server" Text="<%$Resources:content,LitTenantName%>"></asp:Literal>
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold poptxt">
                                                        <asp:Literal Id="Literal2" runat="server" Text="<%$Resources:content,LitOwner%>"></asp:Literal>
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold poptxt">
                                                        <asp:Literal Id="Literal3" runat="server" Text="<%$Resources:content,LitCreatedBy%>"></asp:Literal> 
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold poptxt">
                                                        <asp:Literal Id="Literal4" runat="server" Text="<%$Resources:content,LitCreatedDate%>"></asp:Literal> 
                                                    </th>
                                                    <th scope="col" class=" font-weight-bold text-danger">
                                                        <asp:Literal Id="Literal5" runat="server" Text="<%$Resources:content,LitDelete%>"></asp:Literal>
                                                    </th>

                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="poptxtdata">
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="lblRolesID" name="RolesID" runat="server" Text='<%# Eval("tenantid") %>'  />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="lblRolesName" runat="server" Text='<%# Eval("tenantname") %>' />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("owner") %>' />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("createdby") %>' />
                                            </td>
                                            <td class="odd gradeX" style="text-align: center;">
                                                <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("createddate") %>' />
                                            </td>

                                            <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                                <asp:ImageButton ID="Delete" Text="Delete" runat="server" ImageUrl="~/Images/delete3.png" ImageAlign="Middle" CommandName="Delete" CommandArgument='<%#Eval("tenantid")+","+Eval("tenantname")%>' OnCommand="btnDelete_Click" />
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
                            <br />
   
                </div>
                <!-- /.panel -->



             </div>


                <div class="modal fade" id="AddNewTenant" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false" aria-labelledby="exampleModalLabel2" aria-hidden="true">
                                        <div class="modal-dialog modal-lg" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="m-0 font-weight-bold poptxt" id="exampleModalLabel2">
                                                        <asp:Literal Id="LitAddNewTenant" runat="server" Text="<%$Resources:content,LitAddNewTenant%>"></asp:Literal> 

                                                    </h5>
                                                     <asp:Button runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close"   OnClick="btnXclosePopup"  ><%--data-dismiss="modal" aria-label="Close"--%> <%--OnClick="btnXclosePopup"--%>
                                                    </asp:Button>
                                                    <%--<button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">×</span>
                                                    </button>--%>
                                                </div>

                                                <div class="modal-body poptxt" style="align-content: center" runat="server" >
                                                    <div class="container">
                                                        <table width="100%" align="center">
                                                            <tr>
                                                                <td>
                                                                    <br />
                                                                </td>
                                                            </tr>

                                                            <tr style="height: 25px" >
                                                                <td align="left" style="text-align: right">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label8" runat="server"><b>
                                                                           <asp:Literal Id="LitUserName" runat="server" Text="<%$Resources:content,LitUserName%>"></asp:Literal>  
                                                                            *&nbsp;&nbsp;</b></asp:Label>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtUser" class="form-control input-sm" runat="server"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                
                                                                <td align="right" style="text-align: right">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label9" runat="server"><b>
                                                                            <asp:Literal Id="LitPassword" runat="server" Text="<%$Resources:content,LitPassword%>"></asp:Literal>
                                                                            *&nbsp;&nbsp;</b></asp:Label>
                                                                    </div>

                                                                </td>
                                                                <td>
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtPwd" class="form-control input-sm" runat="server" TextMode="Password"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="auto-style16"></td>

                                                            </tr>
                                                            <tr style="height: 25px">
                                                                <td align="left" class="auto-style14" style="text-align: right">

                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label10" runat="server"><b>
                                                                         <asp:Literal Id="LitDomainName" runat="server" Text="<%$Resources:content,LitDomainName%>"></asp:Literal>
                                                                            *&nbsp;&nbsp;&nbsp;</b></asp:Label>
                                                                    </div>

                                                                </td>
                                                                <td class="auto-style14">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtDomain" class="form-control input-sm" runat="server"  ReadOnly></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td align="left" class="auto-style14" style="text-align: right">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label11" runat="server"><b>
                                                                             <asp:Literal Id="LitRoleType" runat="server" Text="<%$Resources:content,LitRoleType%>"></asp:Literal>
                                                                            *&nbsp;&nbsp;&nbsp;</b></asp:Label>
                                                                    </div>


                                                                </td>
                                                                <td class="auto-style14">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="DrpRoleType" runat="server"  class="form-control input-sm"  Text="Admin" ReadOnly> <%--AppendDataBoundItems="True"--%> 

                                                                            <%--<asp:ListItem Value="Admin">Admin</asp:ListItem>--%>
                                                                            <%--<asp:ListItem Value="Developer">Developer</asp:ListItem>
                                                    <asp:ListItem Value="Controller">Controller</asp:ListItem>
                                                    <asp:ListItem Value="Controller">Tester</asp:ListItem>--%>
                                                                        </asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="auto-style16"></td>
                                                            </tr>

                                                            <tr>
                                                                <td></td>
                                                                <td class="auto-style2" colspan="3">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="15%"></td>
                                                                            <td width="35%">
                                                                                <div class="form-group">
                                                                                    <asp:Button ID="Button1" runat="server" class="btn btn-primary colorSidebar" Text="<%$Resources:content,LitSaveAdminDetails%>" OnClick="CreateNewUser" Width="160px" />
                                                                                    <%--<asp:Button ID="Button3" runat="server" class="btn btn-primary colorSidebar"  Text="Save" OnClick="Button1_Click" width="150px"/>--%>
                                                                                </div>
                                                                            </td>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                            <td width="35%">
                                                                                <div class="form-group">
                                                                                    <asp:Button ID="Button3" runat="server" class="btn btn-danger" Text="<%$Resources:content,LitClear%>" Width="150px"  OnClick="cancelPopup" /><%--data-dismiss="modal"--%>
                                                                               
                                                                                    </div>
                                                                            </td>
                                                                            <td width="15%"></td>
                                                                        </tr>
                                                                    </table>


                                                                </td>
                                                                <td></td>
                                                            </tr>


                                                        </table>




                                                    </div>

                                                    <div class="modal-footer">
                                                        <%-- <asp:Button ID="Button2" runat="server" class="btn btn-primary" Text="Save ZIP" align="center" Width="150px" OnClick="Button1_Click" />--%>

                                                        <%-- <button class="btn btn-danger" type="button" data-dismiss="modal">Cancel</button>--%>





                                                        <%--  <div class="panel-body" style="text-align:center">
                       
                                                          </div>--%>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>



                <div class="modal fade " id="modalDelete" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false" aria-labelledby="exampleModalLabel1" aria-hidden="true" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog modal-lg" role="document" style="width: 600px; height: 140px;">
                                <div class="modal-content ">
                                    <div class="modal-header">
                                        <h5 class="modal-title h4 font-weight-bold text-danger" id="exampleModalLabel1">
                                          <asp:Literal Id="LitDeleteTenant" runat="server" Text="<%$Resources:content,LitDeleteTenant%>"></asp:Literal> 

                                        </h5>
                                       
                                         <asp:Button runat="server" data-dismiss="modal" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" ><%--data-dismiss="modal" aria-label="Close"--%> <%--OnClick="btnXclosePopup"--%>
                                                    </asp:Button>
                                        
                                    </div>

                                    <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                   <asp:Literal Id="LitAreyousureyouwantto" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal>
                                        <span class=" font-weight-bold text-danger">
                                            <asp:Literal Id="LitpmDelete" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal>

                                                                 </span>
                                        <br />
                                        <div class="form-group">

                                            <div class="container">
                                                <div class="row">
                                                    <br />
                                                </div>

                                                <div class="row">

                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                        <asp:Literal Id="Literal6" runat="server" Text="<%$Resources:content,LitTATenantId%>"></asp:Literal>
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanIdDelete" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                    </div>
                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                       <asp:Literal Id="LitTATenantName" runat="server" Text="<%$Resources:content,LitTATenantName%>"></asp:Literal> 
                                                    </div>
                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanNameDelete" runat="server"> </asp:Label>
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="row">
                                                <br />
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                </div>
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                    <asp:Button ID="btnDelete" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" OnClick="CommandBtnDelete_Click" />
                                                </div>
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
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
                   


                
                        <div class="modal fade " id="modalDeleteSecondPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelSecondPopUp" aria-hidden="true" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog modal-lg" role="document" style="width: 900px; height: 140px;" >
                                <div class="modal-content ">
                                    <div class="modal-header">
                                        <h5 class="modal-title h4 font-weight-bold text-danger" id="exampleModalLabelSecondPopUp">
                                           <asp:Literal Id="Literal7" runat="server" Text="<%$Resources:content,LitDeleteTenant%>"></asp:Literal> 

                                        </h5>
                                      
                                         <asp:Button ID="Button6" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                                        <%-- <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">×</span>
                                        </button>--%>
                                    </div>

                                    <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                          <asp:Literal Id="LitYouareabouttodeletethetenantpermanentlyDeletedtenantwillnotberecovered" runat="server" Text="<%$Resources:content,LitYouareabouttodeletethetenantpermanentlyDeletedtenantwillnotberecovered%>"></asp:Literal> <br />
                                               <asp:Literal Id="LitAreyousureyouwantto1" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal>
                                        <span class=" font-weight-bold text-danger">
                                            <asp:Literal Id="LitpmDelete1" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal>

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
                                                        <asp:Literal Id="Literal8" runat="server" Text="<%$Resources:content,LitTATenantId%>"></asp:Literal>
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanIdDeleteSecondPopUp" runat="server"> </asp:Label>
                                                    </div>
                                                   <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                    </div>
                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                        <asp:Literal Id="Literal9" runat="server" Text="<%$Resources:content,LitTATenantName%>"></asp:Literal> 
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanNameDeleteSecondPopUp" runat="server"> </asp:Label>
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
                                                        <asp:Literal Id="LitCounts" runat="server" Text="<%$Resources:content,LitCounts%>"></asp:Literal> 

                                                    </span>
                                                    <br />
                                                </div>


                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        <asp:Literal Id="LitATGroups" runat="server" Text="<%$Resources:content,LitATGroups%>"></asp:Literal> 
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblGroups" runat="server"> </asp:Label>
                                                    </div>

                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        <asp:Literal Id="Literal10" runat="server" Text="<%$Resources:content,LitATUsers%>"></asp:Literal>  
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblUsers" runat="server"> </asp:Label>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                       <asp:Literal Id="LitATBots" runat="server" Text="<%$Resources:content,LitATBots%>"></asp:Literal>  
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblBot" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        <asp:Literal Id="LitATProcesses" runat="server" Text="<%$Resources:content,LitATProcesses%>"></asp:Literal>   
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblProcess" runat="server"> </asp:Label>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        <asp:Literal Id="Literal11" runat="server" Text="<%$Resources:content,LitATProcessUploads%>"></asp:Literal>    
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblProcessUpload" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        <asp:Literal Id="Literal12" runat="server" Text="<%$Resources:content,LitATRQDetails%>"></asp:Literal>
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblRQDetails" runat="server"> </asp:Label>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                       <asp:Literal Id="Literal13" runat="server" Text="<%$Resources:content,LitATScheduleDetails%>"></asp:Literal> 
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblScheduleDeatils" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        <asp:Literal Id="Literal14" runat="server" Text="<%$Resources:content,LitATUserBotMapping%>"></asp:Literal> 
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblUserBotMapping" runat="server"> </asp:Label>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                       <asp:Literal Id="Literal15" runat="server" Text="<%$Resources:content,LitATBotDefaultQueue%>"></asp:Literal> 
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblBotdefaultqueue" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        <asp:Literal Id="Literal16" runat="server" Text="<%$Resources:content,LitATConfigurationParameters%>"></asp:Literal>  
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblconfigurationparameters" runat="server"> </asp:Label>
                                                    </div>

                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4 col-4 col-md-4 col-lg-4 col-xl-4 font-weight-bold">
                                                        <asp:Literal Id="Literal17" runat="server" Text="<%$Resources:content,LitATMessagingDetails%>"></asp:Literal>  
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 text-success">
                                                        <asp:Label ID="lblMessagingdetails" runat="server"> </asp:Label>
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
                                                <asp:Button ID="Button2" runat="server" class="btn btn-danger btn-block"  Text="<%$Resources:content,LitDelete%>" OnClick="CommandBtnDelete_ClickSecondPopUp" />
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2">
                                                <asp:Button ID="btnEnableDisable" runat="server" class="btn btn-warning"  OnClick="CommandBtnDisable_Click"  />
                                                <%--<asp:Button ID="btnEnableDisable" runat="server" OnClick="CommandBtnDisable_Click" />--%>
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2">
                                              <asp:Button ID="Button5" runat="server" class="btn btn-primary btn-block"  Text="<%$Resources:content,LitClose%>" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                                            </div>
                                            <divclass="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                       
                           

                
                        <%-- Delete tentant 3  Second PopUp Modal Start--%>
                     
                        <div class="modal fade " id="modalDeleteThirdPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel3" aria-hidden="true">
                            <div class="modal-dialog modal-lg" role="document" style="width: 600px; height: 140px;">
                                <div class="modal-content ">
                                    <div class="modal-header">
                                        <h5 class="modal-title h4 font-weight-bold text-danger" id="exampleModalLabel3">
                                      <asp:Literal Id="Literal18" runat="server" Text="<%$Resources:content,LitDeleteTenant%>"></asp:Literal> 

                                        </h5>
                                        <asp:Button ID="Button7" runat="server" CssClass="ccolor" style="font-size:larger" Font-Bold="true" Text="X" type="button"  BackColor="White" BorderStyle="None" aria-label="Close" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                                    </div>

                                    <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                        <asp:Literal Id="Literal19" runat="server" Text="<%$Resources:content,LitYouareabouttodeletethetenantpermanentlyDeletedtenantwillnotberecovered%>"></asp:Literal>
                                        <br />
                                                 <asp:Literal Id="Literal20" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal>
                                        <span class=" font-weight-bold text-danger">
                                            <asp:Literal Id="LitpmDelete2" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal> </span>

                                                <br />
                                        <div class="form-group">

                                            <div class="container">
                                                <div class="row">
                                                    <br />
                                                </div>

                                                <div class="row">

                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                        <asp:Literal Id="Literal21" runat="server" Text="<%$Resources:content,LitTATenantId%>"></asp:Literal>
                                                    </div>
                                                    <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanIdDeleteThird" runat="server"> </asp:Label>
                                                    </div>
                                                    <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                                    </div>
                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                         <asp:Literal Id="Literal22" runat="server" Text="<%$Resources:content,LitTATenantName%>"></asp:Literal> 
                                                    </div>
                                                    <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                        <asp:Label ID="lblTenanNameDeleteThird" runat="server"> </asp:Label>
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="row">
                                                <br />
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                </div>
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                    <asp:Button ID="Button4" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" OnClick="ModalPopUpBtnDelete_ClickThirdPopUp" />
                                                </div>
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                            <asp:Button ID="btnCloseSecondPopUp" runat="server" class="btn btn-primary btn-block" Text="<%$Resources:content,LitClose%>" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                                                </div>
                                                <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                   
                        <%--End Delete  3 PopUp user--%>

                 </ContentTemplate>
        </asp:UpdatePanel>            
                        </div>

    
               
                        






            </div>
            <!-- /.col-lg-12 -->
        
    <!-- /#wrapper -->


    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script>
        $(document).ready(function () {
            $("#page-wrapper").delay(500).fadeIn(500);
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

        //$(".rotate").click(function () {
        //    $(this).addClass("down");
        //})


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
            //$("#modalDeleteSecondPopUp").modal('hide');

        }

    </script>
</asp:Content>

