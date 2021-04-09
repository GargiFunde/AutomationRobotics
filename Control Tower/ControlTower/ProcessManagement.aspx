<%@ Page Title="" Language="C#" MasterPageFile="~/ControlTower/MasterPageSkin.master" AutoEventWireup="true" CodeFile="ProcessManagement.aspx.cs" Inherits="DemoMasterPage2_ProcessManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        window.setTimeout(function () {
            $(".alertHide").fadeTo(200, 0).slideUp(500, function () {
                $(this).remove();
            });
        }, 4000);
    </script>



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Begin Page Content -->
    <div class="container-fluid" style="width: 100%">
        <%--Alert For SuccessFull upload Process--%>
         <div id="alertDeletedSuccess" class="alert alert-success " role="alert" runat="server" visible="false">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
             <asp:Literal Id="LitProcess" runat="server" Text="<%$Resources:content,LitProcess%>"></asp:Literal>  
            <strong>
             <asp:Literal Id="LitDeleted" runat="server" Text="<%$Resources:content,LitDeleted%>"></asp:Literal> 
            </strong>
            <asp:Literal Id="LitSuccessfully" runat="server" Text="<%$Resources:content,LitSuccessfully%>"></asp:Literal>.
        </div>

        <div class="alert alert-success" role="alert" runat="server" visible="true">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
             <asp:Literal Id="LitUploadProcessbyclicking" runat="server" Text="<%$Resources:content,LitUploadProcessbyclicking%>"></asp:Literal>
            <strong><i class="fas fa-cloud-upload-alt alert-warning"></i></strong>
            <asp:Literal Id="LitButtonDeleteProcessbyclicking" runat="server" Text="<%$Resources:content,LitButtonDeleteProcessbyclicking%>"></asp:Literal> <i class="fas fa-trash-alt alert-danger"></i>
            <asp:Literal Id="LitButtonUploadProcessbyClicking" runat="server" Text="<%$Resources:content,LitButtonUploadProcessbyClicking%>"></asp:Literal> <i class="fas fa-code-branch"></i>
            <asp:Literal Id="LitButton" runat="server" Text="<%$Resources:content,LitButton%>"></asp:Literal>
        </div>

        <asp:ScriptManager runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="panel1" runat="server" style="width: 100%">
            <ContentTemplate>

                <div id="page-wrapper">
                    <!-- DataTales Example -->
                    <div class="card shadow mb-4">
                        <div class="card-header py-3">
                            <h2 class="m-0 font-weight-bold poptxt">
                                <asp:Literal Id="LitpmPROCESSES" runat="server" Text="<%$Resources:content,LitpmPROCESSES%>"></asp:Literal>
                       <asp:ImageButton ID="ImageButton3" runat="server" Class="rotate refreshbtn" ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="ReloadProcesses" />
                            </h2>
                        </div>
                        <div class="card-body">
                        <div class="card-body">
                            <div>
                                <div id="IDaddUploadProcess" class="card-header py-3" runat="server">
                                    <asp:Button ID="btnAddUploadProcess" data-toggle="modal" data-target="#botlogModal" runat="server" align="center" class="btn  btn-block btn-primary btn-lg" Text="<%$Resources:content,LitAddUploadProcess%>" /><%--Add/Upload Process--%>
                                </div>

                                <div class="panel-body colorFont" style="width: 100%">
                                    <br />
                                    <%-- <form runat="server">--%>
                                    <asp:Repeater ID="GVUserToBotMapping" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-striped table-bordered " id="datatable-Processes" width="100%">
                                                <thead class="mastercolor colorSidebar" align="center">
                                                    <tr class="TableHeaderFont">
                                                        <th scope="col" class=" font-weight-bold poptxt"> <asp:Literal Id="LitProcessId" runat="server" Text="<%$Resources:content,LitProcessId%>"></asp:Literal></th>
                                                        <th scope="col" class=" font-weight-bold poptxt"> <asp:Literal Id="LitProcessName" runat="server" Text="<%$Resources:content,LitProcessName%>"></asp:Literal>
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold poptxt"> <asp:Literal Id="LitProcessVersion" runat="server" Text="<%$Resources:content,LitProcessVersion%>"></asp:Literal>
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold poptxt">  <asp:Literal Id="LitCreatedBy" runat="server" Text="<%$Resources:content,LitCreatedBy%>"></asp:Literal>
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold poptxt">  <asp:Literal Id="LitCreatedDate" runat="server" Text="<%$Resources:content,LitCreatedDate%>"></asp:Literal>
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold poptxt">  <asp:Literal Id="LitUpload" runat="server" Text="<%$Resources:content,LitUpload%>"></asp:Literal>
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold text-danger"> <asp:Literal Id="LitDelete" runat="server" Text="<%$Resources:content,LitDelete%>"></asp:Literal>
                                                        </th>
                                                        <th scope="col" class=" font-weight-bold text-info">   <asp:Literal Id="LitVersion" runat="server" Text="<%$Resources:content,LitVersion%>"></asp:Literal>
                                                        </th>
                                                    </tr>
                                                </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="poptxtdata">

                                                <td class="odd gradeX" style="text-align: center;" onclick="Version('<%# Eval("processname") %>')">
                                                    <asp:Label ID="Label1" class="font-weight-bold" runat="server" Text='<%# Eval("processid") %>' />
                                                </td>
                                                <td class="odd gradeX " style="text-align: center;" onclick="Version('<%# Eval("processname") %>')">
                                                    <asp:Label ID="Label2" class="font-weight-bold" runat="server" Text='<%# Eval("processname") %>' />
                                                </td>
                                                <td class="odd gradeX " style="text-align: center;" onclick="Version('<%# Eval("processname") %>')">
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("processversion") %>' />
                                                </td>
                                                <td class="odd gradeX " style="text-align: center;" onclick="Version('<%# Eval("processname") %>')">
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("createdby") %>' />
                                                </td>
                                                <td class="odd gradeX " style="text-align: center;" onclick="Version('<%# Eval("processname") %>')">
                                                    <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("createddate", "{0:dd/MMM/yyyy HH:mm:ss}") %>' />
                                                </td>

                                                <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                                    <asp:ImageButton ID="Upload" data-toggle="modal" data-target="#botlogModal" CssClass="colorFont" Text="Upload" runat="server" ImageUrl="~/Images/Upload3.png" ImageAlign="Middle" />

                                                </td>
                                                <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                                    <asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/Images/delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("processid")+","+ Eval("processname")%>' OnCommand="btnDelete_Click" />
                                                </td>
                                                <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                                    <asp:ImageButton ID="btnVersion" runat="server" ImageUrl="~/Images/Version3.png" ImageAlign="Middle" CommandArgument='<%# Eval("processid") + "," + Eval("processname") %>' OnCommand="LoadProcessesforVersion" />
                                                </td>
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

                                <%--Log Modal--%>
                            </div>
                        </div>
                    </div>
                </div>
                </div>


                <%-- Delete Process Modal Start--%>






                <%--DELETE UpdateVersion MODAL--%>

                <%--COMPLETE DELETE UpdateVersion MODAL--%>
          

</div>


          










               <div class="modal fade" id="modalDelete" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">
                                    <asp:Literal Id="LitDeleteProcess" runat="server" Text="<%$Resources:content,LitDeleteProcess%>"></asp:Literal></h5>

                                <asp:Button ID="btnXmodalDelete" data-dismiss="modal" CssClass="ccolor" runat="server" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close"></asp:Button>
                            </div>
                            <div class="modal-body TableHeaderFont poptxt" style="text-align: center" runat="server">
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
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                               <asp:Literal Id="LitpmProcessId" runat="server" Text="<%$Resources:content,LitpmProcessId%>"></asp:Literal>
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                <asp:Label ID="lblProcessId" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Literal Id="LitpmProcessName" runat="server" Text="<%$Resources:content,LitpmProcessName%>"></asp:Literal>
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                <asp:Label ID="lblProcessName" runat="server"> </asp:Label>
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
                                            <asp:Button ID="btnDelete" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" OnClick="ModalPopUpBtnDelete_Click" />
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                            <button class="btn btn-primary btn-block" type="button" data-dismiss="modal" aria-label="Close" >
                                                 <asp:Literal Id="LitClose" runat="server" Text="<%$Resources:content,LitClose%>"></asp:Literal>
                                            </button>
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%-- &nbsp;&nbsp;&nbsp;&nbsp;--%>
                        </div>
                    </div>
                </div>
                <%--MODAL POPUP for Upload Process--%>
               
                <%-- Delete Process Modal Start  Second PopUp--%>
                <div class="modal fade" id="modalDeleteSecondPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelDeleteSecondPopUp" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">
                                    <asp:Literal Id="LitDeleteProcessPermanently" runat="server" Text="<%$Resources:content,LitDeleteProcessPermanently%>"></asp:Literal>

                                </h5>
                                <asp:Button ID="btnXdelete" runat="server" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop" />
                            </div>
                            <div class="modal-body Delete Process Permanently poptxt" style="text-align: center" runat="server">
                                <asp:Literal Id="LitYouareabouttodeletetheprocesspermanentlyDeletedprocesswillnotberecovered" runat="server" Text="<%$Resources:content,LitYouareabouttodeletetheprocesspermanentlyDeletedprocesswillnotberecovered%>"></asp:Literal>
                                <br />
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
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Literal Id="Literal1" runat="server" Text="<%$Resources:content,LitpmProcessId%>"></asp:Literal> 
                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger">
                                                <asp:Label ID="lblProcessIdSecondPopUp" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Literal Id="Literal2" runat="server" Text="<%$Resources:content,LitpmProcessName%>"></asp:Literal>
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                <asp:Label ID="lblProcessNameSecondPopUp" runat="server"> </asp:Label>
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
                                            <asp:Button ID="btnDeleteSecondpopup" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" OnClick="ModalPopUpBtnDelete_ClickSecondPopUp" />
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
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

                <!-- Update Version of Process Modal-->

                 <div class="modal fade" data-backdrop="static" data-keyboard="false" id="UpdateVersion" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel2" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title h4 font-weight-bold poptxt" id="exampleModalLabel2"> <asp:Literal Id="LitUpdateProcessFile" runat="server" Text="<%$Resources:content,LitUpdateProcessFile%>"></asp:Literal></h5>
                                <asp:Button class="close" runat="server" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close" data-dismiss="modal"></asp:Button>
                            </div>

                            <div class="modal-body TableHeaderFont" style="align-content: center" runat="server" >
                                <%--card begins--%>
                                <div class="card shadow">
                                    <div class="container">
                                        <div class="row">   
                                            <br />
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3  font-weight-bold poptxt">
                                                <asp:Literal Id="LitpmProcessId2" runat="server" Text="<%$Resources:content,LitpmProcessId%>"></asp:Literal> 
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3  font-weight-bold text-success">
                                                <asp:Label ID="Pid" runat="server" Text="" />
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3  font-weight-bold poptxt">
                                                 <asp:Literal Id="Literal3" runat="server" Text="<%$Resources:content,LitpmProcessName%>"></asp:Literal> 
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3  font-weight-bold text-success">
                                                <asp:Label ID="Pname" runat="server" Text="" />
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3  font-weight-bold poptxt">
                                                  <asp:Literal Id="LitSelectVersion" runat="server" Text="<%$Resources:content,LitSelectVersion%>"></asp:Literal>  

                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3  font-weight-bold text-success">
                                                <asp:DropDownList ID="DropDownList1" runat="server" OnDataBound="ddlArea_DataBound" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" isPostback="true">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger">
                                                <%--<asp:Label ID="lblErrorMsg" runat="server"></asp:Label>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                         <div class="col-sm-12 col-12 col-md-12 col-lg-12 col-xl-12 font-weight-bold  text-danger text-center" >
                                                <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
                                            </div>
                                        </div>


                                    <div class="row">
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3  font-weight-bold poptxt">
                                            <asp:Literal Id="LitIsLatest" runat="server" Text="<%$Resources:content,LitIsLatest%>"></asp:Literal> 
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3  font-weight-bold text-success">
                                            <asp:CheckBox ID="CheckBox2" runat="server" Checked="True" />
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 h5 font-weight-bold text-primary">
                                            <asp:Button ID="btnUpdateZip" class="btn btn-primary btn-block" Text="<%$Resources:content,LitSaveZIP%>"  align="center" runat="server" OnClick="btnUpdateZip_Click" />

                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 h5 font-weight-bold text-success">
                                            <asp:Button ID="Button6" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitClose%>" align="center" OnClick="ModalPopUpBtnClose_ClickPopUp" />
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%--card ends--%>

                            <div class="card shadow">
                                <div class="modal-footer">
                                    <div style="margin-left: auto; margin-right: auto;">
                                        <div class="panel-body" style="text-align: center">
                                            <asp:Repeater ID="GVForVersion" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-striped table-bordered table-hover" id="dataTables-botqueuemapping">
                                                        <thead>
                                                            <tr class="TableHeaderFont">
                                                                <th scope="col" class="font-weight-bold text-primary">
                                                                    <asp:Literal Id="LitProcessName" runat="server" Text="<%$Resources:content,LitProcessName%>"></asp:Literal>
                                                                </th>
                                                                <th scope="col" class="font-weight-bold text-primary">
                                                                   <asp:Literal Id="LitProcessVersion" runat="server" Text="<%$Resources:content,LitProcessVersion%>"></asp:Literal>
                                                                </th>
                                                                <th scope="col" class="font-weight-bold text-danger">
                                                                 <asp:Literal Id="LitDeleted" runat="server" Text="<%$Resources:content,LitDeleted%>"></asp:Literal>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;"class="txtFont poptxtdata">
                                                        <td class="odd gradeX" style="text-align: center;">
                                                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("processname") %>' />
                                                        </td>

                                                        <td class="odd gradeX" style="text-align: center;">
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("processversion") %>' />
                                                        </td>

                                                        <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">
                                                            <asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/Images/delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("processid")+","+ Eval("processname")+","+ Eval("processversion")%>' OnCommand="btnDelete_ClickUpdateVersion" />

                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                </div>


        <div class="modal fade " id="modalDeleteUpdateVersion" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelUpdateVersion" aria-hidden="true">
             <div class="modal-dialog modal-lg" role="document">
                 <div class="modal-content ">
                     <div class="modal-header">
                         <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelUpdateVersion">
                              <asp:Literal Id="Literal4" runat="server" Text="<%$Resources:content,LitDeleteProcess%>"></asp:Literal></h5>
                         <asp:Button ID="Button4" runat="server" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop" />
                     </div>

                     <div class="modal-body TableHeaderFont text-primary" style="text-align: center" runat="server">
                         <asp:Literal Id="Literal5" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal> 
                         <span class=" font-weight-bold text-danger">
                             <asp:Literal Id="Literal6" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal>  </span>                      
                                <br />
                         <br />
                         <div class="form-group">
                             <div class="container">
                                 <div class="row">
                                     <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                        <asp:Literal Id="Literal7" runat="server" Text="<%$Resources:content,LitpmProcessId%>"></asp:Literal>
                                     </div>
                                     <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger" align="left">
                                         <asp:Label ID="lblProcessIdUpdateVersion" runat="server"> </asp:Label>
                                     </div>
                                     <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                     </div>
                                     <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                         <asp:Literal Id="Literal8" runat="server" Text="<%$Resources:content,LitpmProcessName%>"></asp:Literal>
                                     </div>
                                     <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger" align="left">
                                         <asp:Label ID="lblProcessNameUpdateVersion" runat="server"> </asp:Label>
                                     </div>

                                     <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                     </div>
                                 </div>
                                 <div class="row">
                                     <br></br>
                                 </div>
                                 <div class="row">
                                     <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6" align="right">
                                         <asp:Literal Id="LitProcessVersion" runat="server" Text="<%$Resources:content,LitpmProcessVersion%>"></asp:Literal>
                                     </div>
                                     <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6 font-weight-bold  text-danger" align="left">
                                         <asp:Label ID="lblProcessVersion" runat="server"> </asp:Label>
                                     </div>
                                 </div>
                             </div>


                             <div class="row">
                                 <br></br>
                             </div>

                             <div class="row">
                                 <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                 </div>
                                 <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                     <asp:Button ID="btnDeleteUpdateVersion" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" align="center" OnClick="ModalPopUpBtnDelete_ClickUpdateVersion" />
                                 </div>
                                 <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                     <asp:Button ID="btnCloseUpdateVersion" runat="server" class="btn btn-primary btn-block" Text="<%$Resources:content,LitClose%>" OnClick="ModalPopUpBtnClose_ClickUpdateVersion" />
                                 </div>
                                 <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                 </div>
                             </div>
                         </div>
                     </div>
                 </div>
             </div>
         </div>


              <div class="modal fade " id="modalDeleteUpdateVersionSecondPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelUpdateVersionSecondPopUp" aria-hidden="true">
                    <div class="modal-dialog modal-lg" role="document">
                        <div class="modal-content ">
                            <div class="modal-header">
                                <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelUpdateVersionSecondPopUp">
                                    <asp:Literal Id="LitDeleteProcessPermanently1" runat="server" Text="<%$Resources:content,LitDeleteProcessPermanently%>"></asp:Literal></h5>
                                <asp:Button ID="Button3" runat="server" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close" OnClick="btnXdelete_clickHideBgPop" />
                            </div>

                            <div class="modal-body TableHeaderFont  text-primary" style="text-align: center" runat="server">
                                <asp:Literal Id="LitYouareabouttodeletetheprocesspermanentlyDeletedprocesswillnotberecovered1" runat="server" Text="<%$Resources:content,LitYouareabouttodeletetheprocesspermanentlyDeletedprocesswillnotberecovered%>"></asp:Literal>.<br />
                                <asp:Literal Id="LitAreyousureyouwantto2" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal> <span class=" font-weight-bold text-danger">
                                    <asp:Literal Id="LitDelete" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal> </span>
                                <br />
                                <br />
                                <div class="form-group">
                                    <div class="container">
                                        <div class="row">
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                <asp:Literal Id="Literal9" runat="server" Text="<%$Resources:content,LitpmProcessId%>"></asp:Literal>                                            </div>
                                            <div class="col-sm-2 col-2 col-md-2 col-lg-2 col-xl-2 font-weight-bold  text-danger" align="left">
                                                <asp:Label ID="lblProcessIdUpdateVersionSecondPopUp" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3" align="right">
                                                <asp:Literal Id="Literal10" runat="server" Text="<%$Resources:content,LitpmProcessName%>"></asp:Literal> 
                                            </div>
                                            <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 font-weight-bold  text-danger" align="left">
                                                <asp:Label ID="lblProcessNameUpdateVersionSecondPopUp" runat="server"> </asp:Label>
                                            </div>
                                            <div class="col-sm-1 col-1 col-md-1 col-lg-1 col-xl-1">
                                            </div>
                                        </div>
                                        <div class="row">
                                            <br></br>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6" align="right">
                                                <asp:Literal Id="Literal11" runat="server" Text="<%$Resources:content,LitpmProcessVersion%>"></asp:Literal> 
                                            </div>
                                            <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6 font-weight-bold  text-danger" align="left">
                                                <asp:Label ID="lblProcessVersionSecondPopUp" runat="server"> </asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        </br>
                                        <br />
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            <asp:Button ID="btnclosesecond" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" align="center" OnClick="ModalPopUpBtnDelete_ClickSecondPopUpDeleteUpdateVersion" />
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                            <asp:Button ID="Button5" runat="server" class="btn btn-primary btn-block" Text="<%$Resources:content,LitClose%>" OnClick="ModalPopUpBtnClose_ClickUpdateVersion" />
                                        </div>
                                        <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="modal fade" id="botlogModal" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content ">
                    <div class="modal-header">
                        <h5 class="modal-title h4 font-weight-bold poptxt" id="exampleModalLabel">
                            <asp:Literal Id="LitpmPROCESSES1" runat="server" Text="<%$Resources:content,LitpmPROCESSES%>"></asp:Literal> </h5>
                        <asp:Button runat="server" class="close" CssClass="ccolor" data-dismiss="modal" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close">
                            <%-- <span aria-hidden="true">×</span>--%>
                        </asp:Button>
                    </div>
                    <div class="modal-body TableHeaderFont " style="align-content: center" runat="server">

                        <div class="input-group mb-3" align="center">
                            <div class="modal-body h6 font-weight-bold  poptxt" style="text-align: center" runat="server">
                                <asp:Label ID="LabelZIPFile" runat="server"  align="right"> 
                                     <asp:Literal Id="LitPleaseSelectZipFile" runat="server" Text="<%$Resources:content,LitPleaseSelectZipFile%>"></asp:Literal>
                                </asp:Label>
                            </div>
                            <div class="modal-body h6  text-success">
                                <asp:FileUpload ID="FileUploadZip" Width="100%" runat="server" />

                            </div>
                        </div>
                    </div>
                    <%--<div class="modal-dialog">--%>
                    <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked="true" />
                        <asp:Label ID="Label1" runat="server" Text="Label">
                            <asp:Literal Id="LitUseLatest" runat="server" Text="<%$Resources:content,LitUseLatest%>"></asp:Literal> &nbsp;&nbsp;</asp:Label>
                        <br />
                        <br />
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSave" runat="server" class="btn btn-primary" Text="<%$Resources:content,LitSaveZIP%>" align="center" OnClick="SaveZIP_Click" />
                        <asp:Button ID="Button1" runat="server" class="btn btn-danger" Text="<%$Resources:content,LitCancel%>" align="center" data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
       

        <!-- /.container-fluid -->
        <script>



            $(document).ready(function () {
                $("#page-wrapper").delay(100).fadeIn(300);
                $('#datatable-Processes').DataTable({
                    responsive: true
                })

            });



            function destroy() {
                table = $('#datatable-Processes').DataTable({
                    responsive: true
                });
            };

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        $(".rotate").click(function () {
                            $(this).toggleClass("down");
                        })
                        $("#page-wrapper").show();
                        $('#datatable-Processes').DataTable({
                            destroy: true,
                            responsive: true
                        });
                    }
                });
            };
            // Show modal on page load
            function openModalDeleteSecondPopUpUpdateVersion() {
                $('#modalDeleteUpdateVersionSecondPopUp').modal('show');
            }
            function openModalDeleteSecondPopUp() {
                $('#modalDeleteSecondPopUp').modal('show');
            };

            function openModalDeleteUpdateVersion() {
                $('#modalDeleteUpdateVersion').modal('show');
            }
            function openModalDelete() {
                $('#modalDelete').modal('show');
            }
            function ClosePopupModalDelete() {
                $('#modalDelete').modal('hide');
            }
            function openModalHideDelete() {
                $('body').removeClass().removeAttr('style'); $('.modal-backdrop').remove();
            }
            $(".rotate").click(function () {

                // alert("clicked");
                $(this).addClass("down");

            })
        </script>
        <script type="text/javascript">
            function openModal() {
                $('#UpdateVersion').modal('show');
            }

        </script>
        <script>

</script>

        <style type="text/javascript">
            .modal-footer {
                justify - content: center;
            }
        </style>
</asp:Content>

