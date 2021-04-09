<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl1.ascx.cs" Inherits="RobotsController.WebUserControl1" %>
<style type="text/css">
    .auto-style1 {
        width: 200px;
    }
</style>

    
<body style="font-size: medium">
<asp:Panel ID="Panel1" runat="server" Height="229px" Width="300px" BorderWidth="1px">
    <asp:Panel ID="RoboTopDiv" runat="server">

     <table style="width: 300px">
        <tr style="height:35px">
            <td style="width:20px; top: 0px; clip: rect(0px, auto, auto, auto);"><asp:Button ID="btnStatus0"  runat="server" style="height: 26px" Text="Round" Font-Size="12px" /></td>
            <td><asp:Label ID="Label1" ColumnSpan="2" runat="server" style="text-align: center" Text="Name:"  Font-Size="12px" ></asp:Label></td>
            <td></td>
            <td></td>
            <td style="padding: 0px; margin: 0px; text-align: right; top: 0px;">&nbsp;<asp:Button ID="Button1" runat="server" Text="&lt;"  Font-Size="12px" BorderStyle="None" Height="24px" Width="20px" /></td>
        </tr>
    </table>
    </asp:Panel>
 <asp:Panel ID="RoboMiddleDiv" runat="server">

    <table  style="width: 300px">
        <tr style="height:35px" >
             <td style="width:100px"><asp:Label ID="lblProcessName" runat="server" Text="Automation :"  Font-Size="12px" ></asp:Label></td>
            <td class="auto-style1">Test Autoation</td>
        </tr>
        <tr style="height:35px">
            <td style="width:100px"> <asp:Label ID="lblRequestServed" runat="server" Text="Request Served :"  Font-Size="12px" ></asp:Label>
            <td class="auto-style1">1000</td>
        </tr>
        <tr style="height:35px">
            <td style="width:100px"> <asp:Label ID="lblRequestTime" runat="server" Text="Last Request Time:"  Font-Size="12px" ></asp:Label>
            <td class="auto-style1">1 minute</td>
        </tr>
        <tr style="height:35px">
            <td style="width:100px"> <asp:Label ID="Label2" runat="server" Text="Last Request Time:"  Font-Size="12px" ></asp:Label>
            <td class="auto-style1">1 minute</td>
        </tr>
    </table> 
 </asp:Panel>

  <div id="RoboBottomDiv">
   <table  style="width: 300px">
    <tr style="height:35px">
        <td></td>
        <td><asp:Button ID="Button3" runat="server" height="26px" Text="Start" width="70px"  Font-Size="12px" /></td>
        <td> <asp:Button ID="Button4" runat="server" height="26px" Text="Stop" width="70px"  Font-Size="12px" /></td>
        <td> <asp:Button ID="Button5" runat="server" Text="Upgrade"  Font-Size="12px" /></td>
            <td></td>
    </tr>
</table>  
  </div>
   
</asp:Panel>
</body>
