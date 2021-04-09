﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ControlTower/MasterPageSkin.master" AutoEventWireup="true" CodeFile="AssetManagement.aspx.cs" Inherits="ControlTower_AssetManagement1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" >
    </asp:ScriptManager>
     <div class="container-fluid">
        <div id="page-wrapper">
        <%--<div style="width:100%">--%>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                    <div id="DIVAddUsers" class="card shadow"  runat="server">
                        <div class="card-header py-3">
                            <h2 class="m-0 font-weight-bold poptxt">
                                <asp:Literal Id = "LitADDASSET" runat="server" Text="<%$Resources:content,LitADDASSET%>"></asp:Literal>
                            </h2>
                        </div>
                        

                        <div class="card-body">
                            <div>
                                <div class="panel panel-default colorFont TableHeaderFont">

                                     <div class="form-group">
                                        <asp:Label ID="lblAssetName" class=" font-weight-bold poptxt" runat="server">
                                           <asp:Literal Id = "LitAssetName" runat="server" Text="<%$Resources:content,LitAssetName%>"></asp:Literal> 
                                            <span class="text-danger">*</span></asp:Label>
                                        <asp:TextBox ID="txtAssetName" class="form-control input-sm" runat="server" placeholder="<%$Resources:content,LitAssetName%>"></asp:TextBox>
                                    </div>

                               

                                    <div class="form-group">
                                        <asp:Label ID="lblAssetValue" class=" font-weight-bold poptxt" runat="server">
                                            <asp:Literal Id = "LitAssetValue" runat="server" Text="<%$Resources:content,LitAssetValue%>"></asp:Literal>  
                                            <span class="text-danger">*</span></asp:Label>
                                        <asp:TextBox ID="txtAssetValue" class="form-control input-sm" runat="server"  placeholder="<%$Resources:content,LitAssetValue%>"></asp:TextBox>
                                    </div>
                                    </td>
                            <div class="form-group" align="center">
                                <asp:Button ID="Button1" runat="server" class="btn btn-primary colorSidebar" Text="<%$Resources:content,LitSave%>" OnClick="btnSave_Asset" Width="49%" />
                                <asp:Button ID="Button2" runat="server" class="btn btn-danger" Text="<%$Resources:content,LitClear%>"  Width="49%" OnClick="btnClear_Asset" />
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
                              <h2 class="m-0 font-weight-bold poptxt">
                                  <asp:Literal Id = "LitASSETS" runat="server" Text="<%$Resources:content,LitASSETS%>"></asp:Literal>
                   <%--<asp:ImageButton ID="ImageButton2" runat="server" Class="rotate refreshbtn" ImageUrl="~/Images/refresh3.png" ImageAlign="Right" OnCommand="refreshUsers" />--%>

                              </h2>
                          </div>

                          <div class="card-body">
                              <div>

                                  <div class="panel-body">
                                      <asp:Repeater ID="GVAssets" runat="server">
                                          <HeaderTemplate>
                                              <table class="table table-striped table-bordered table-hover table-responsive-lg" id="dataTables-users" width="100%">
                                                  <thead class="colorSidebar" align="center">
                                                      <tr class="TableHeaderFont">
                                                          <th scope="col" class=" font-weight-bold poptxt">
                                                              <asp:Literal Id = "LitAssetName" runat="server" Text="<%$Resources:content,LitAssetName%>"></asp:Literal>
                                                          </th>
                                                          <th scope="col" class=" font-weight-bold poptxt">
                                                              <asp:Literal Id = "LitAssetValue" runat="server" Text="<%$Resources:content,LitAssetValue%>"></asp:Literal>
                                                          </th>
                                                          <th scope="col" class=" font-weight-bold poptxt">
                                                              <asp:Literal Id = "LitDelete" runat="server" Text="<%$Resources:content,LitDelete%>"></asp:Literal>
                                                          </th>
                                                      </tr>
                                                  </thead>
                                          </HeaderTemplate>
                                          <ItemTemplate>
                                              <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="poptxtdata">
                                                 
                                                  <td class="odd gradeX" style="text-align: center;">
                                                      <asp:Label ID="lblAssetName" runat="server" Text='<%# Eval("assetname") %>' />
                                                  </td>
                                                   <td class="odd gradeX" style="text-align: center;">
                                                      <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("value") %>' />
                                                  </td>
                                               
                                                
                                                  <td style="text-align: center; align-content: center; align-items: center; padding-left: 6px; padding-top: 2px; padding-bottom: 2px;">

                                                      <asp:ImageButton ID="btnDeleteAsset" data-target="#modalDelete" runat="server" ImageUrl="~/Images/Delete3.png" ImageAlign="Middle" Text="Delete" CommandArgument='<%#Eval("id")+","+ Eval("tenantId")+","+ Eval("groupid")+","+ Eval("Assetname")%>' OnCommand="btnDelete_Click" />

                                                  </td>

                                                  <%--   <% } %>--%>
                                              </tr>
                                          </ItemTemplate>
                                          <FooterTemplate>
                                              </table>
                                          </FooterTemplate>
                                      </asp:Repeater>
                                   <%--   <div id="output1">
                                          <textarea id="output" cols="6" rows="10" hidden="hidden"></textarea>
                                      </div>--%>
                                      <!-- /.table-responsive -->

                                  </div>

                                  <!-- /.panel-body -->
                              </div>
                              </div>
                          </div>
                      </ContentTemplate>
                 </asp:UpdatePanel>
              <asp:UpdatePanel runat="server">
                                  <ContentTemplate>
                                      <div class="modal fade " id="modalDelete" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
                                          <div class="modal-dialog modal-lg" role="document">
                                              <div class="modal-content ">
                                                  <div class="modal-header">
                                                      <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabel1">
                                                          <asp:Literal Id = "LitDeleteAsset" runat="server" Text="<%$Resources:content,LitDeleteAsset%>"></asp:Literal> 

                                                      </h5>
                                                      <asp:Button class="close" runat="server" CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" type="button" BackColor="White" BorderStyle="None" aria-label="Close" data-dismiss="modal"></asp:Button>
                                                  </div>
                                                  <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                     <asp:Literal Id = "LitAreyousureyouwantto" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal>
                                                      <span class=" font-weight-bold text-danger">
                                                          <asp:Literal Id = "LitpmDelete" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal>

                                                      </span>
                                                <br />
                                                      <div class="form-group">

                                                          <div class="container">

                                                              <div class="row">
                                                                  <br />
                                                              </div>

                                                              <div class="row">
                                                                  <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6" align="right">
                                                                      <asp:Literal Id = "LitAUUserId" runat="server" Text="<%$Resources:content,LitAUUserId%>"></asp:Literal> 
                                                                  </div>
                                                                  <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6 font-weight-bold  text-danger" align="left">
                                                                      <asp:Label ID="lblId" runat="server" hidden> </asp:Label>
                                                                      <asp:Label ID="lblAssetId" runat="server"> </asp:Label>
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
                                                                  <button class="btn btn-primary btn-block" type="button" data-dismiss="modal">
                                                                      <asp:Literal Id = "LitClose" runat="server" Text="<%$Resources:content,LitClose%>"></asp:Literal>
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
                                      </div>
                                  </ContentTemplate>
                              </asp:UpdatePanel>
                              <%--End Delete user--%>





                              <%-- Delete user  Second PopUp Modal Start--%>
                              <asp:UpdatePanel runat="server">
                                  <ContentTemplate>
                                      <div class="modal fade " id="modalDeleteSecondPopUp" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelDeleteSecondPopUp" aria-hidden="true">
                                          <div class="modal-dialog modal-lg" role="document">
                                              <div class="modal-content ">
                                                  <div class="modal-header">
                                                      <h5 class="modal-title h5 font-weight-bold text-danger" id="exampleModalLabelDeleteSecondPopUp">
                                                          <asp:Literal Id = "LitDeleteUserPermanently" runat="server" Text="<%$Resources:content,LitDeleteUserPermanently%>"></asp:Literal>

                                                      </h5>
                                                      <asp:Button CssClass="ccolor" Style="font-size: larger" Font-Bold="true" Text="X" BackColor="White" BorderStyle="None" runat="server" OnClick="btnXdelete_clickHideBgPop"></asp:Button>

                                                      <%-- <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true">×</span>
                                                </button>--%>
                                                  </div>

                                                  <div class="modal-body TableHeaderFont  poptxt" style="text-align: center" runat="server">
                                                      <asp:Literal Id = "LitYouareabouttodeletetheuserpermanentlyDeleteduserwillnotberecovered" runat="server" Text="<%$Resources:content,LitYouareabouttodeletetheuserpermanentlyDeleteduserwillnotberecovered%>"></asp:Literal>
                                                      <br />
                                                      <asp:Literal Id = "Literal1" runat="server" Text="<%$Resources:content,LitAreyousureyouwantto%>"></asp:Literal> 
                                                      <span class=" font-weight-bold text-danger">
                                                         <asp:Literal Id = "Literal2" runat="server" Text="<%$Resources:content,LitpmDelete%>"></asp:Literal> 

                                                      </span>

                                                <br />
                                                      <div class="form-group">

                                                          <div class="container">

                                                              <div class="row">
                                                                  <br />
                                                              </div>

                                                              <div class="row">
                                                                  <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6" align="right">
                                                                     <asp:Literal Id = "LitAUUserId1" runat="server" Text="<%$Resources:content,LitAUUserId%>"></asp:Literal>
                                                                  </div>
                                                                  <div class="col-sm-6 col-6 col-md-6 col-lg-6 col-xl-6 font-weight-bold  text-danger" align="left">
                                                                      <asp:Label ID="lblIdSecondPopUp" runat="server" hidden> </asp:Label>
                                                                      <asp:Label ID="lblAssetSecondPopUp" runat="server"> </asp:Label>
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
                                                                  <asp:Button ID="Button3" runat="server" class="btn btn-danger btn-block" Text="<%$Resources:content,LitDelete%>" OnClick="ModalPopUpBtnDelete_ClickSecondPopUp" />
                                                              </div>
                                                              <div class="col-sm-3 col-3 col-md-3 col-lg-3 col-xl-3 ">
                                                                  <asp:Button ID="btnCloseSecondPopUp" runat="server" class="btn btn-primary btn-block" Text="<%$Resources:content,LitClose%>" OnClick="ModalPopUpBtnClose_ClickSecondPopUp" />

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
                                      </div>
                                  </ContentTemplate>
                              </asp:UpdatePanel>
          
           </div>
            </div>
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

        $(".rotate").click(function () {
            $(this).addClass("down");
        })

        function openModalDelete() {
          
            $('#modalDelete').modal('show');
        };

        function openModalHideDelete() {
            $('body').removeClass().removeAttr('style'); $('.modal-backdrop').remove();
        };


        function openModalDeleteSecondPopUp() {
            $('#modalDeleteSecondPopUp').modal('show');
        };

        function openModalGroupChange() {
            $('#changeGroup123').modal('show');
        }
    </script>
</asp:Content>

