<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="Control_Tower_CompleteLogs, App_Web_2o2bniex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <asp:ScriptManager runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="panel1" runat="server" style="width: 100%">
        <ContentTemplate>
           <div class="container-fluid">
                <div id="page-wrapper">
                    <!-- DataTales Example -->
                    <div class="card shadow mb-4">
                        <div class="card-header py-3">
                            <h2 class="m-0 font-weight-bold poptxt">Detail Logs
                       <asp:ImageButton ID="ImageButton3" runat="server" Class="rotate refreshbtn" ImageUrl="~/Images/refresh3.png" ImageAlign="Right"  OnCommand="ReloadCompleteLogs" />
                            </h2>
                        </div>
                        <div class="card-body">
                            <div>
                                <div class="panel-body colorFont" style="width: 100%">
                                    <br />
                                    <asp:Repeater ID="GVCompleteLogs" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-striped table-bordered table-hover" id="datatable-Processes" width="100%">
                                        <thead class="mastercolor colorSidebar" align="center">
                                            <tr class="poptxt TableHeaderFont">
                                                <th scope="col" class=" font-weight-bold ">Log</th>
                                                <th scope="col" class=" font-weight-bold ">Detail log
                                                </th>
                                                <th scope="col" class=" font-weight-bold ">Group Id
                                                </th>
                                                <th scope="col" class=" font-weight-bold ">Tenant Id
                                                </th>
                                                <th scope="col" class=" font-weight-bold ">Date
                                                </th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="padding: 0px; margin: 0px; border: .25px solid #E5E5E5;" class="poptxtdata">

                                        
                                        <td class="odd gradeX " style="text-align: center;" onclick="Version('<%# Eval("Message") %>')">
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("Message") %>' />
                                        </td>
                                        <td class="odd gradeX " style="text-align: center;" onclick="Version('<%# Eval("DetailLog") %>')">
                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("DetailLog") %>' />
                                        </td>
                                        <td class="odd gradeX " style="text-align: center;" onclick="Version('<%# Eval("groupid") %>')">
                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("groupid") %>' />
                                        </td>
                                        <td class="odd gradeX " style="text-align: center;" onclick="Version('<%# Eval("tenantId") %>')">
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("tenantId") %>' />
                                        </td>
                                        <td class="odd gradeX " style="text-align: center;" onclick="Version('<%# Eval("Date") %>')">
                                            <asp:Label ID="CreateDate" runat="server" Text='<%# Eval("Date", "{0:dd/MMM/yyyy HH:mm:ss}") %>' />
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
                               
   
       </ContentTemplate>
        </asp:UpdatePanel>
    <script type="text/javascript">
      $(document).ready(function () {
            //$(".repeaterrow").hover(function () {
            //    $(this).find(".info").css("visibility", "visible"); //.fadeToggle(50); //locating the associated .slet and not the all one.
            //    //$(".info").css("visibility","visible");
            //});
            $("#page-wrapper").delay(300).fadeIn(300);
            $('#datatable-Processes').DataTable({

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
                    $('#datatable-Processes').DataTable({
                        destroy: true,
                        responsive: true
                    });


                }
            });
        };
        </script>








</asp:Content>

