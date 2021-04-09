<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashboardUI.aspx.cs" Inherits="FrontOfficeBotWebApp.DashboardUI" %>

<!DOCTYPE html>

<html >
<%--<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>--%>
    <head>
    <!-- <meta charset="utf-8" /> -->
    <!-- <meta http-equiv="x-ua-compatible" content="ie=edge, chrome=1" /> -->
    <!-- <meta name="viewport" content="width=device-width, initial-scale=1" /> -->
    <title>Executor</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" crossorigin="*">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js" crossorigin="*"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js" crossorigin="*"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js"
            crossorigin="*"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" crossorigin="*"></script>
    <!-- <link href="style.css" rel="stylesheet" /> -->
    <!-- <link -->
    <!-- rel="stylesheet" -->
    <!-- href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" -->
    <!-- integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" -->
    <!-- crossorigin="anonymous" -->
    <!-- /> -->
    <!-- <link -->
    <!-- href="https://fonts.googleapis.com/css?family=Roboto:300,400,700" -->
    <!-- rel="stylesheet" -->
    <!-- /> -->
    <style>
         div.scroll { 
                margin:4px, 4px; 
                padding:4px; 
                /*background-color: green;*/ 
                /*width: 500px; */
                height: 880px; 
                overflow-x: hidden; 
                overflow-y: auto; 
                text-align:justify; 
            } 
        .noborder {
           border-color:white;
        }
        .wellcolor {
            background-color: white;
            
          
            border-width:0.05px;
         }
        .padd {
                 padding:5px;
        }

        table {
            border-collapse: collapse;
            width: 100%;
        }

        th, td {
          
            text-align: left;
            padding: 3px;
            vertical-align: top;
        }

        label {
            font-weight: normal !important;
        }

        select {
            width: 150px;
            margin: 10px;
        }

            select:focus {
                min-width: 150px;
                width: auto;
            }

        h5 {
            color: blue;
        }

        footer {
            text-align: center;
            padding: 15px;
            background-color: gainsboro;
            color: gray;
            font-size:large;
            font-weight:500;
        }

        #cover-spin {
    position:fixed;
    width:100%;
    left:0;right:0;top:0;bottom:0;
    background-color: rgba(255,255,255,0.7);
    z-index:9999;
    display:none;
}

@-webkit-keyframes spin {
	from {-webkit-transform:rotate(0deg);}
	to {-webkit-transform:rotate(360deg);}
}

@keyframes spin {
	from {transform:rotate(0deg);}
	to {transform:rotate(360deg);}
}

#cover-spin::after {
    content:'';
    display:block;
    position:absolute;
    left:48%;top:40%;
    width:40px;height:40px;
    border-style:solid;
    border-color:black;
    border-top-color:transparent;
    border-width: 4px;
    border-radius:50%;
    -webkit-animation: spin .8s linear infinite;
    animation: spin .8s linear infinite;
}
    </style>
</head>

<body style="overflow-x: hidden;padding:1% ;font-family:Verdana;">
    <form runat="server" >
    <!--  <div style="text-align: center; margin-top: 25px">
        <input
          type="text"
          placeholder="Username"
          style="margin-bottom: 8px"
          id="username"
        /><br />
        <input
          type="password"
          placeholder="Password"
          style="margin-bottom: 8px"
          id="password"
        /><br />
        <input
          type="text"
          placeholder="Folder Name"
          style="margin-bottom: 8px"
          id="folderName"
        /><br />
        <input
          type="text"
          placeholder="File Name"
          style="margin-bottom: 8px"
          id="fileName"
        /><br />
        <button onclick="runbot()" type="button" class="btn btn-primary">
          Go!</button
        >&nbsp;&nbsp;&nbsp;&nbsp;
        <button onclick="test()" type="button" class="btn btn-primary">
          Test</button
        ><br />

        <div
          id="result"
          style="margin: 25px; color: blue; font-size: 18px"
        ></div>
      </div> -->
    <!-- div for UI  -->
    <!-- <div > -->
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div id="cover-spin"></div>
    <div id="pagestart">
        <div class="row">
            <div class="col-sm-3 ">
                    <div class="row">
                           <div class="col-sm-4"></div>
                          <div class="col-sm-1">
                   <asp:Image runat="server" ImageUrl="~/E2ELogo01.ico" Width="100" Height="50" ImageAlign="Middle" />
                         </div>
                          </div>
                <div class="well wellcolor" style="padding:30px;">
                    
         
                   <div class="row">        
                    <h5><b>Automation Group Name :</b></h5>
                    </div>
                    <div class="row">  
                        <div class="col-sm-4 padd" >
                            <label for="dropBusinessProcess"><b>Business Process:</b></label>
                        </div>
                        <div class="col-sm-8 padd" >
                            <select class="form-control" name="BusinessProcess" id="dropBusinessProcess" >
                                    <option value="Null">--Select--</option>
                                    <option value="New Requisition">New Requisition</option>
                                    <option value="Old Requisition">Old Requisition</option>
                                </select>
                        </div>
                         
                        </div>
                     <div class="row">  
                        <div class="col-sm-4 padd">
                            <label id="lblRequisitionType" for="dropRequisitionType"><b>Requisition Type:</b></label>
                        </div>
                        <div class="col-sm-8 padd">
                            <select class="form-control" name="RequisitionType" id="dropRequisitionType">
                                    <option value="Null">--Select--</option>    
                                    <option value="CPU">CPU</option>
                                    <option value="Disk">Disk</option>
                                    <option value="Keyboard">Keyboard</option>
                                    <option value="Memory">Memory</option>
                                    <option value="Monitor">Monitor</option>
                                    <option value="Mouse">Mouse</option>
                                </select>
                        </div>
                         
                        </div>
                    <div class="row">  
                        <div class="col-sm-4 padd">
                            <label id="lblServices" for="dropServices"><b>Services:</b></label>
                        </div>
                        <div class="col-sm-8 padd">
                            <select class="form-control" name="Services" id="dropServices">
                                     <option value="Null">--Select--</option>
                                    <option value="IT Services">IT Services</option>
                                    <option value="Business Services">Business Services</option>
                       
                                </select>
                        </div>
                         
                        </div>
                     <div class="row">  
                        <div class="col-sm-4 padd" >
                            <label id="lblDescription" for="dropDescription"><b>Description</b></label>
                        </div>
                        
                        <div class="col-sm-8 padd" >
                             <textarea class="form-control" id="description" name="Description" cols="5"  rows="4" placeholder="Enter text here..."></textarea>
                        </div>
                         
                        </div>

                     <div class="row">  
                        <div class="col-sm-4 padd">
                             <label id="lblIncidentNo" for="IncidentNo" style="visibility:hidden"><b>Incident:</b></label>
                        </div>
                        <div class="col-sm-8 padd">
                              <textarea  id="IncidentNo" name="IncidentNo" form="usrform" rows="1" style="width: 100%; max-width: 100%;visibility:hidden;" class="form-control" placeholder="Enter Incident No.."></textarea>
                        </div>
                         
                        </div>

                    <div class="row">  
                        <div class="col-sm-6 " style="text-align:right;padding:2%">
                              <button  class="btn btn-primary" type="button" onclick="GoBtnClick()"> Go </button>
                        </div>
                        <div class="col-sm-6 " style="text-align:left;padding:2%" >
                             <button class="btn btn-primary" type="button" onclick="ResetDetails()">Reset</button>
                        </div>
                         
                        </div>

                 <%--   <table>
                        <tr>
                            <td>
                                <label for="dropBusinessProcess"><b>Business Process:</b></label>
                            </td>
                            <td>
                                
                                <select class="form-control" name="BusinessProcess" id="dropBusinessProcess" >
                                    <option value="New Requisition">New Requisition</option>
                                    <option value="Old Requisition">Old Requisition</option>
                                </select>
                            </td>
                        </tr>


                        <tr>
                            <td>
                                <label id="lblRequisitionType" for="dropRequisitionType"><b>Requisition Type:</b></label>
                            </td>
                            <td>
                                <select class="form-control" name="RequisitionType" id="dropRequisitionType">
                                    <option value="CPU">CPU</option>
                                    <option value="Disk">Disk</option>
                                    <option value="Keyboard">Keyboard</option>
                                    <option value="Memory">Memory</option>
                                    <option value="Monitor">Monitor</option>
                                    <option value="Mouse">Mouse</option>
                                </select>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <label id="lblServices" for="dropServices"><b>Services:</b></label>
                            </td>
                            <td>
                                <select class="form-control" name="Services" id="dropServices">
                                    <option value="IT Services">IT Services</option>
                                    <option value="Business Services">Business Services</option>
                 
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblDescription" for="dropDescription"><b>Description:</b></label>
                            </td>
                            <td>
                                <textarea class="form-control" id="description" name="Feedback" cols="5"  rows="4" >Enter text here...</textarea>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblIncidentNo" for="IncidentNo" style="visibility:hidden"><b>Incident:</b></label>
                            </td>
                            <td>
                                <textarea  id="IncidentNo" name="IncidentNo" form="usrform" rows="1" style="width: 100%; max-width: 100%;visibility:hidden;">Enter Incident No..</textarea>
                            </td>
                        </tr>
                        <tr>
                            <td style="float:right">

                                <button class="btn btn-primary" type="button" onclick="GoBtnClick()"> Go </button>
                            </td>

                            <td>
                                <button class="btn btn-primary" type="button">Reset</button>
                            </td>

                        </tr>


                    </table>
--%>





                </div>

                <div class="well wellcolor"  >
                    <div class="row" style=" margin-bottom: 3%;">        
                    <h5><b>Key Fields:</b></h5>
                    </div>
                    <div class="row" style=" margin-bottom: 2%;">  
                        <div class="col-sm-6 padd">
                               <label for="lblNumber"><b>Number:</b></label>
                        </div>
                        <div class="col-sm-6 padd" style="text-align:left;">
                              <label id="lblNumber"></label>
                        </div>
                         
                        </div>

                     <div class="row" style=" margin-bottom: 2%;">  
                        <div class="col-sm-6 padd">
                                <label for="lblOpened"><b>Opened:</b></label>
                        </div>
                        <div class="col-sm-6 padd" >
                              <label id="lblOpened"></label>
                        </div>
                         
                        </div>
                     <div class="row">  
                        <div class="col-sm-6 padd">
                                 <label for="lblShortdescription"><b>description:</b></label>
                        </div>
                        <div class="col-sm-6 padd" >
                             <label id="lblShortdescription"></label>
                        </div>
                         
                        </div>
                     <div class="row" style=" margin-bottom: 2%;">  
                        <div class="col-sm-6 padd">
                                 <label for="lblCaller"><b>Caller:</b></label>
                        </div>
                        <div class="col-sm-6 padd" >
                             <label id="lblCaller"></label>
                        </div>
                         
                        </div>
                     <div class="row" style=" margin-bottom: 2%;">  
                        <div class="col-sm-6 padd">
                                 <label for="lblPriority"><b>Priority:</b></label>
                        </div>
                        <div class="col-sm-6 padd" >
                            <label id="lblPriority"></label>
                        </div>
                         
                        </div>
                    <div class="row" style=" margin-bottom: 2%;">  
                        <div class="col-sm-6 padd">
                                <label for="lblState"><b>State:</b></label>
                        </div>
                        <div class="col-sm-6 padd" >
                            <label id="lblState"></label>
                        </div>
                         
                        </div>
                     <div class="row" style=" margin-bottom: 2%;">  
                        <div class="col-sm-6 padd">
                                <label for="lblCategory"><b>Category:</b></label>
                        </div>
                        <div class="col-sm-6 padd" >
                             <label id="lblCategory"></label>
                        </div>
                         
                        </div>
                     <div class="row" style=" margin-bottom: 2%;">  
                        <div class="col-sm-6 padd">
                                <label for="lblAssignmentGroup"><b>Assignment Group:</b></label>
                        </div>
                        <div class="col-sm-6 padd" >
                            <label id="lblAssignmentGroup"></label>
                        </div>
                         
                        </div>
                    <div class="row" style=" margin-bottom: 2%;">  
                        <div class="col-sm-6 padd">
                                 <label for="lblAssignedto"><b>Assigned to:</b></label>
                        </div>
                        <div class="col-sm-6 padd" >
                            <label id="lblAssignedto"></label>
                        </div>
                         
                        </div>

                    <div class="row" style=" margin-bottom: 2%;">  
                        <div class="col-sm-6 padd">
                                 <label for="lblUpdated"><b>Updated:</b></label>
                        </div>
                        <div class="col-sm-6 padd" >
                            <label id="lblUpdated"></label>
                        </div>
                         
                        </div>

                     <div class="row" style=" margin-bottom: 2%;">  
                        <div class="col-sm-6 padd">
                                 <label for="lblUpdatedby"><b>Updated by:</b></label>
                        </div>
                        <div class="col-sm-6 padd" >
                           <label id="lblUpdatedby"></label>
                        </div>
                         
                        </div>

                   <%-- <h5><b>Key Fields:</b></h5>
                    <table>
                        <tr>
                            <td>
                                <label for="lblNumber"><b>Number:</b></label>
                            </td>
                            <td>
                                <label id="lblNumber"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblOpened"><b>Opened:</b></label>
                            </td>
                            <td>
                                <label id="lblOpened"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblShortdescription"><b>Short description:</b></label>
                            </td>
                            <td>
                                <label id="lblShortdescription"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblCaller"><b>Caller:</b></label>
                            </td>
                            <td>
                                <label id="lblCaller"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblPriority"><b>Priority:</b></label>
                            </td>
                            <td>
                                <label id="lblPriority"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblState"><b>State:</b></label>
                            </td>
                            <td>
                                <label id="lblState"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblCategory"><b>Category:</b></label>
                            </td>
                            <td>
                                <label id="lblCategory"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblAssignmentGroup"><b>Assignment Group:</b></label>
                            </td>
                            <td>
                                <label id="lblAssignmentGroup"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblAssignedto"><b>Assigned to:</b></label>
                            </td>
                            <td>
                                <label id="lblAssignedto"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblUpdated"><b>Updated:</b></label>
                            </td>
                            <td>
                                <label id="lblUpdated"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblUpdatedby"><b>Updated by:</b></label>
                            </td>
                            <td>
                                <label id="lblUpdatedby"></label>
                            </td>
                        </tr>

                    </table>--%>
                </div>

            </div>
            <div class="col-sm-9">
                <div class="row">
                    <div class="col-sm-12 panel panel-default text-left wellcolor" >
                         <div class="row">        
                          <h5><b>Employee Details :</b></h5>
                        </div>
                         <div class="row">  
                        <div class="col-sm-2 padd">
                                 <img border="0" alt="W3Schools" src="person.jpg" width="100" height="100">
                        </div>
                        <div class="col-sm-10 padd">
                             <div class="row"> 
                                 <div class="col-sm-2 padd">
                                     <label for="lblempName"><b>Customer Name:</b></label>
                                 </div>
                                 <div class="col-sm-2 padd">
                                      <asp:label runat="server" id="lblempName"></asp:label>
                                 </div>
                                 <div class="col-sm-2 padd">
                                     <label for="lblEmployeeType"><b>Customer Type:</b></label>
                                 </div>
                                 <div class="col-sm-2 padd" >
                                      <asp:label runat="server" id="lblEmployeeType"></asp:label>
                                 </div>
                                 <div class="col-sm-2 padd">
                                     <label for="lblOrganisationalunit"><b>Organisational unit:</b></label>
                                 </div>
                                  <div class="col-sm-2 padd">
                                     <asp:label runat="server" id="lblOrganisationalunit"></asp:label>
                                 </div>
                                  
                              </div>
                            <%----------Row1------------%>
                            <div class="row"> 
                                 <div class="col-sm-2 padd">
                                     <label for="lblempid"><b>Customer Id:</b></label>
                                 </div>
                                 <div class="col-sm-2 padd">
                                      <asp:label runat="server" id="lblempid"></asp:label>
                                 </div>
                                 <div class="col-sm-2 padd">
                                      <label for="lblManager"><b>Manager:</b></label>
                                 </div>
                                 <div class="col-sm-2 padd">
                                       <asp:label runat="server" id="lblManager"></asp:label>
                                 </div>
                                 <div class="col-sm-2 padd">
                                     <label for="lblSubDivision"><b>Sub-Division:</b></label>
                                 </div>
                                  <div class="col-sm-2 padd">
                                     <asp:label runat="server" id="lblSubDivision"></asp:label>
                                 </div>
                                  
                              </div>

                              <%----------Row2------------%>
                             <div class="row"> 
                                 <div class="col-sm-2 padd">
                                    <label for="lblCountry"><b>Country:</b></label>
                                 </div>
                                 <div class="col-sm-2 padd">
                                       <asp:label runat="server" id="lblCountry"></asp:label>
                                 </div>
                                 <div class="col-sm-2 padd">
                                     <label for="lblOffice"><b>Office:</b></label>
                                 </div>
                                 <div class="col-sm-2 padd">
                                      <asp:label runat="server" id="lblOffice"></asp:label>
                                 </div>
                                 <div class="col-sm-2 padd">
                                     <label for="lblAddress"><b>Address:</b></label>
                                 </div>
                                  <div class="col-sm-2 padd">
                                      <asp:label runat="server" id="lblAddress"></asp:label>
                                 </div>
                                  
                              </div>
                         </div>

                        </div>


                       <%-- <table class="panel panel-default text-left wellcolor" >
                            <tr>
                                <td style="text-align:left">
                                    <h5><b>Employee Details :</b></h5>
                                </td>
                                <td>
                                    <!-- <h4><b>Employee Deatils :</b></h4> -->
                                </td>
                                <td>
                                    <!-- <h4><b>Employee Deatils :</b></h4> -->
                                </td>
                                <td>
                                    <!-- <h4><b>Employee Deatils :</b></h4> -->
                                </td>
                                <td>
                                    <!-- <h4><b>Employee Deatils :</b></h4> -->
                                </td>
                                <td>
                                    <!-- <h4><b>Employee Deatils :</b></h4> -->
                                </td>
                                <td>
                                    <!-- <h4><b>Employee Deatils :</b></h4> -->
                                </td>
                            </tr>
                            <!-- --------------------------------------------------------------------------------------------------------------------- -->
                            <tr>
                                <td rowspan="3">
                                    <img border="0" alt="W3Schools" src="person.jpg" width="100" height="100">
                                </td>
                                <td>
                                    <label for="lblempName"><b>Customer Name:</b></label>
                                </td>
                                <td>
                                    <asp:label runat="server" id="lblempName1"></asp:label>
                                </td>
                                <td>
                                    <label for="lblEmployeeType"><b>Customer Type:</b></label>
                                </td>
                                <td>
                                    <asp:label runat="server" id="lblEmployeeType1"> </asp:label>
                                </td>
                                <td>
                                    <label for="lblOrganisationalunit"><b>Organisational unit:</b></label>
                                </td>
                                <td>
                                    <asp:label runat="server" id="lblOrganisationalunit1"></asp:label>
                                </td>
                            </tr>

                            <!-- --------------------------------------------------------------------------------------------------------------------- -->
                            <tr>
                                <td>
                                    <label for="lblempid1"><b>Customer Id:</b></label>
                                </td>
                                <td>
                                    <asp:label runat="server" id="lblempid1"></asp:label>
                                </td>
                                <td>
                                    <label for="lblManager1"><b>Manager:</b></label>
                                </td>
                                <td>
                                    <asp:label runat="server" id="lblManager1"></asp:label>
                                </td>
                                <td>
                                    <label for="lblSubDivision1"><b>Sub-Division:</b></label>
                                </td>
                                <td>
                                    <asp:label runat="server" id="lblSubDivision1"></asp:label>
                                </td>
                            </tr>
                            <!-- --------------------------------------------------------------------------------------------------------------------- -->
                            <tr>
                                <td>
                                    <label for="lblCountry"><b>Country:</b></label>
                                </td>
                                <td>
                                    <asp:label runat="server" id="lblCountry1"></asp:label>
                                </td>
                                <td>
                                    <label for="lblOffice"><b>Office:</b></label>
                                </td>
                                <td>
                                    <asp:label runat="server" id="lblOffice1"></asp:label>
                                </td>
                                <td>
                                    <label for="lblAddress"><b>Address:</b></label>
                                </td>
                                <td>
                                    <asp:label runat="server" id="lblAddress1"></asp:label>
                                </td>
                            </tr>

                        </table>--%>
                    </div>
                </div>



                <div class="col-sm-8 well wellcolor panel panel-default text-left wellcolor noborder">
                     <div class="row"  style=" margin-bottom: 2%;">        
                          <h5><b>Update Details :</b></h5>
                        </div>
                     <div class="row" >
                         <div class="col-sm-2 padd"  style=" margin-bottom: 2%;">
                               <label for="Fname">First name:</label>
                         </div>
                         <div class="col-sm-4 padd"  style=" margin-bottom: 2%;">
                             <asp:TextBox class="form-control" runat="server" type="text" id="Fname" name="Fname" />
                         </div>
                          <div class="col-sm-2 padd"  style=" margin-bottom: 2%;">
                               <label for="Lname">Last name:</label>
                         </div>
                         <div class="col-sm-4 padd"  style=" margin-bottom: 2%;">
                            <asp:TextBox class="form-control" runat="server" type="text" id="Lname" name="Lname"/>  
                         </div>
                     </div>
                   
                    <%----------1 Row------%>
                     <div class="row">
                         <div class="col-sm-2 padd"  style=" margin-bottom: 2%;">
                                <label for="MobileNo">Mobile No:</label>
                         </div>
                         <div class="col-sm-4 padd"  style=" margin-bottom: 2%;">
                             <asp:TextBox class="form-control" runat="server" type="text" id="MobileNo" name="MobileNo"/>
                         </div>
                          <div class="col-sm-2 padd"  style=" margin-bottom: 2%;">
                                <label for="OfficeNo">Office RoomNo:</label>
                         </div>
                         <div class="col-sm-4 padd"  style=" margin-bottom: 2%;">
                            <asp:TextBox class="form-control" runat="server" type="text" id="OfficeNo" name="OfficeNo"/> 
                         </div>
                     </div>
                   <%----------2 Row------%>
                      <div class="row">
                         <div class="col-sm-2 padd"  style=" margin-bottom: 2%;">
                               <label for="Interest">Interest:</label>
                         </div>
                         <div class="col-sm-4 padd"  style=" margin-bottom: 2%;">
                             <asp:TextBox class="form-control" runat="server" type="text" id="Interest" name="Interest"/>
                         </div>
                       </div>
                     <%----------3 Row------%>
                      <div class="row">
                         <div class="col-sm-6 padd" style="text-align:right">
                               <asp:button class="btn btn-primary" runat="server" type="button" Text="Update" OnClientClick="UpdateDtlsBtnClick()" OnClick="EditDetails_Click"  ></asp:button>
                         </div>
                         <div class="col-sm-6 padd" style="text-align:left">
                             <button class="btn btn-primary" type="button">Cancel</button>
                         </div>
                       </div>

                   <%-- <table class="panel panel-default text-left wellcolor noborder">
                        <tr>
                            <td>
                                <h5><b>Update Details :</b></h5>
                            </td>
                            <td>
                               
                            </td>
                            <td>
                               
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="Fname">First name:</label>
                            </td>
                            <td>
                                <asp:TextBox class="form-control" runat="server" type="text" id="Fname1" name="Fname" />
                            </td>

                            <td>
                                <label for="Lname">Last name:</label>
                            </td>
                            <td>
                                <asp:TextBox class="form-control" runat="server" type="text" id="Lname1" name="Lname"/>
                            </td>
                           
                        </tr>

                        <tr>

                            <td>
                                <label for="MobileNo">Mobile No:</label>
                            </td>
                            <td>
                                <asp:TextBox class="form-control" runat="server" type="text" id="MobileNo1" name="MobileNo"/>
                            </td>

                            <td>
                                <label for="OfficeNo">Office RoomNo:</label>
                            </td>
                            <td>
                                <asp:TextBox class="form-control" runat="server" type="text" id="OfficeNo1" name="OfficeNo"/>
                            </td>
                            
                        </tr>

                        <tr>

                            <td>
                                <label for="Interest">Interest:</label>
                            </td>
                            <td>
                                <asp:TextBox class="form-control" runat="server" type="text" id="Interest1" name="Interest"/>
                            </td>

                            <td>
                              
                            </td>
                            <td>
                              
                            </td>
                            
                        </tr>

                        <tr>

                            <td>
                                
                            </td>
                            <td style="float:right">

                            
                            </td>

                            <td>
                             
                            </td>
                            <td>
                               
                            </td>
                            
                        </tr>

                    </table>--%>

                    <div class="row well wellcolor panel panel-default  wellcolor noborder" style=" margin-bottom: 2%;">
                          <h5><b>Notes :</b></h5>
                       </div>
                    <div class="row">
                        <div class="col-sm-2 padd" style=" margin-bottom: 2%;">
                               <label for="Feedback">Feedback:</label>
                         </div>
                         <div class="col-sm-10 padd"  style=" margin-bottom: 2%;" >
                             <textarea class="form-control" id="Feedback" name="Feedback" rows="4" style="width: 100%; max-width: 100%;" placeholder="Enter text here..."  ></textarea>
                         </div>
                    </div>

                     <div class="row">
                        <div class="col-sm-2 padd" >
                                <label for="Complaint">Complaint:</label>
                         </div>
                         <div class="col-sm-10 padd"  style=" margin-bottom: 2%;">
                             <textarea class="form-control" id="Complaint" name="Complaint" rows="4" style="width: 100%; max-width: 100%;" placeholder="Enter text here..."  ></textarea>
                         </div>
                    </div>
                     <div class="row">
                        <div class="col-sm-6 padd" style="text-align:right" >
                                   <asp:button runat="server" class="btn btn-primary" type="button" OnClick="SubmitFeedback_Click" Text="Submit" ></asp:button>
                         </div>
                         <div class="col-sm-6 padd" style="text-align:left">
                                    <button type="button" class="btn btn-primary">Cancel</button>
                         </div>
                    </div>
                    <%--<table class="panel panel-default text-left wellcolor noborder">
                        <tr>
                            <td style="text-align:left">
                                <h5><b>Notes :</b></h5>
                            </td>
                            <td>
                               
                            </td>

                        </tr>


                        <tr>
                            <td width="100">
                                <label for="Feedback">Feedback:</label>
                            </td>
                            <td>
                                <textarea class="form-control" id="Feedback1" name="Feedback" rows="4" style="width: 100%; max-width: 100%;">Enter text here...</textarea>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <label for="Complaint">Complaint:</label>
                            </td>
                            <td>
                                <textarea class="form-control" id="Complaint1" name="Complaint" rows="4" style="width: 100%; max-width: 100%;">Enter text here...</textarea>
                            </td>

                        </tr>
                        <tr>

                            <td>
                               
                            </td>
                            <td style="float:center">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:button class="btn btn-primary" runat="server" type="button" OnClick="SubmitFeedback_Click" Text="Submit" ></asp:button>

                                <button class="btn btn-primary" type="button">Cancel</button>
                            </td>



                        </tr>

                    </table>--%>
                </div>
                <div class="col-sm-4" >
                    <div class="well wellcolor scroll">
                        <div class="row" style="padding:5%;">
                            <h5><b>Details :</b></h5>
                        </div>
                        <div class="row" style="padding:5%;">
                             <asp:DataGrid 
                                id="dgBooks" 
                                runat="server" 
                                BorderColor="White" 
                                BorderWidth="2px"
                                AutoGenerateColumns="False"
                                width="100%" AlternatingItemStyle-Wrap="True">

                                <HeaderStyle 
                                  HorizontalAlign="Center" 
                                  ForeColor="Black" 
                                  BackColor="White" 
                                  Font-Bold=true
                                  CssClass="TableHeader" BorderColor="White" /> 

                                <ItemStyle
                                  BackColor="White" 
                                  cssClass="TableCellNormal" />

                                <AlternatingItemStyle 
                                  BackColor="#FFFFFF" 
                                  cssClass="TableCellAlternating" />
                                

                                <Columns>
                                  <asp:BoundColumn HeaderText="Feedback" DataField="Feedback" />
                                  <asp:BoundColumn HeaderText="Complaint" DataField="Complaint" 
                                                   ItemStyle-HorizontalAlign="Center" />
                                  <asp:BoundColumn HeaderText="Logged Date" DataField="LoggedDate"
                                                   ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                              </asp:DataGrid>

                        </div>

                       <%-- <table>
                            <tr>
                                <td>
                                    <h5><b>Details :</b></h5>
                                </td>
                                <td>
                                  
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                 
                                </td>
                            </tr>
                            <tr>
                              


                                <td>

                                       <asp:DataGrid 
                                id="dgBooks1" 
                                runat="server" 
                                BorderColor="White" 
                                BorderWidth="2px"
                                AutoGenerateColumns="False"
                                width="100%" AlternatingItemStyle-Wrap="True">

                                <HeaderStyle 
                                  HorizontalAlign="Center" 
                                  ForeColor="Black" 
                                  BackColor="White" 
                                  Font-Bold=true
                                  CssClass="TableHeader" BorderColor="White" /> 

                                <ItemStyle
                                  BackColor="White" 
                                  cssClass="TableCellNormal" />

                                <AlternatingItemStyle 
                                  BackColor="#FFFFFF" 
                                  cssClass="TableCellAlternating" />
                                

                                <Columns>
                                  <asp:BoundColumn HeaderText="Feedback" DataField="Feedback" />
                                  <asp:BoundColumn HeaderText="Complaint" DataField="Complaint" 
                                                   ItemStyle-HorizontalAlign="Center" />
                                  <asp:BoundColumn HeaderText="Logged Date" DataField="LoggedDate"
                                                   ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                              </asp:DataGrid>


                                    </td>






                            </tr>

                       <%--     <tr>
                                <td>
                                    <label id="lbltxtfeedback">Special sound and Switch control button blue LED indicator,5-6 hours playtime easy to operating full directivr</label>
                                </td>
                                <td>
                                    <label id="lbltblComplaint"></label>
                                </td>
                                <td>
                                    <label id="lbltblDate">Mon 08-06-20; 6:40 PM</label>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <label id="lbltxtfeedback">Special sound and Switch control button blue LED indicator,5-6 hours playtime easy to operating full directivr</label>
                                </td>
                                <td>
                                    <label id="lbltblComplaint"> Its not good enough.</label>
                                </td>
                                <td>
                                    <label id="lbltblDate">Mon 08-06-20; 6:40 PM</label>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <label id="lbltxtfeedback">Special sound and Switch control button blue LED indicator,5-6 hours playtime easy to operating full directivr</label>
                                </td>
                                <td>
                                    <label id="lbltblComplaint"> Its not good enough.</label>
                                </td>
                                <td>
                                    <label id="lbltblDate">Mon 08-06-20; 6:40 PM</label>
                                </td>

                            </tr>
                            
                        </table>--%>
                    </div>
                </div>
               


              <%--  <div class="row">
                    <div class="col-sm-12">
                       
                    </div>
                </div>--%>



            </div>

        </div>
        <!-- </div> -->

        <footer class="container-fluid text-center">
            <span>innowise.us</span>
        </footer>

    </div>
    <script src="Scripts/jquery-3.4.1.min.js"></script>
    <script src="Scripts/jquery.signalR-2.2.2.min.js"></script>
    <!--<script src="Scripts/jquery-3.1.1.min.js"></script>
    <script type="text/javascript" src="lib/signalr.js/jquery.signalR.min.js"></script>-->
    <script src="signalr/hubs"></script>


    <script crossorigin="anonymous">
        function test() {
            debugger;
            $("#result").html(`
                                        <div class="spinner-border text-success" role="status">
                              <span class="sr-only">Loading...</span>
</div>`);
            $.get("http://localhost:8091/ExecutorController/api/executor/test", function (data) {
                debugger;

                $("#result").html(data);
                setTimeout(() => {
                    $("#result").html("");
                }, 3000);
            });
        }

        function runbot() {

            getvalfromconfig();
            $("#result").html(`
                                        <div class="spinner-border text-success" role="status">
                              <span class="sr-only">Loading...</span>
</div>`);
            //< !-- var u = $("#username").val(); -->
            //< !-- var p = $("#password").val(); -->
            //< !-- var folderName = $("#folderName").val(); -->
            //< !-- var fileName = $("#fileName").val(); -->
            //< !-- var u = "akash felix"; -->
            //< !-- var p = "InnoWise@123"; -->
            //< !-- var folderName = "Main"; -->
            //< !-- var fileName = "Main.xaml"; -->
          //  alert("in runbot " + User + "-" + Pass + "-" + Pfolder + "-" + Pfile + "-" + strjson);
            var u = User;
            var p = Pass;
            var folderName = Pfolder;
            var fileName = Pfile;
            var inputargs = strjson;
            console.log(
                `username : ${u}  password : ${p}  folderName : ${folderName} fileName ${fileName}`
            );
            $.post("http://localhost:8091/ExecutorControllerB/api/executor/runbot", {
                username: u,
                password: p,
                folderName: folderName,
                fileName: fileName,
                inputargs: inputargs,
            }).done(function (data) {
                $("#result").html(data);
                setTimeout(() => {
                    $("#result").html("");
                }, 10000);
            });
        }



        function editdetails() {
            var xhr = new XMLHttpRequest();
            xhr.open('GET', 'UpdateDetails.xml', true);

            xhr.timeout = 2000; // time in milliseconds

            xhr.onload = function () {
                if (this.readyState == 4 && this.status == 200) {
                    var xmlDoc = this.responseXML;
                    var x;
                    var txt = "";
                    x = xmlDoc.getElementsByTagName("FirstName")[0].childNodes[0];
                    //alert(x.nodeValue);
                    x.nodeValue = "Prashant";
                    x = xmlDoc.getElementsByTagName("FirstName")[0].childNodes[0];
                    //alert(x.nodeValue);

                }

            }

            xhr.ontimeout = function (e) {
                // XMLHttpRequest timed out. Do something here.
            };

            xhr.send(null);
        }


        function getupdatedetails() {
            var xhr = new XMLHttpRequest();
            xhr.open('GET', 'UpdateDetails.xml', true);

            xhr.timeout = 2000; // time in milliseconds

            xhr.onload = function () {
                // Request finished. Do processing here.
                var xmlDoc = this.responseXML; // <- Here's your XML file
                updatexmlstr = new XMLSerializer().serializeToString(xmlDoc.documentElement);
                updatexmlstr = "<?xml version='1.0' encoding='utf-8'?>" + updatexmlstr;
             //   alert("in update read " + updatexmlstr);
                xmlDoc = $.parseXML(updatexmlstr),
                    $xml = $(xmlDoc),
                    $FirstName = $xml.find("FirstName").text();
                $LastName = $xml.find("LastName").text();
                $MobileNo = $xml.find("MobileNo").text();
                $OfficeRoomNo = $xml.find("OfficeRoomNo").text();
                $Interest = $xml.find("Interest").text();

                $("#Fname").val($FirstName);
                $("#Lname").val($LastName);
                $("#MobileNo").val($MobileNo);
                $("#OfficeNo").val($OfficeRoomNo);
                $("#Interest").val($Interest);


            };

            xhr.ontimeout = function (e) {
                // XMLHttpRequest timed out. Do something here.
            };

            xhr.send(null);

        }


        function readconfig() {

            var xhr = new XMLHttpRequest();
            xhr.open('GET', 'Process.xml', true);

            xhr.timeout = 2000; // time in milliseconds

            xhr.onload = function () {
                // Request finished. Do processing here.
                var xmlDoc = this.responseXML; // <- Here's your XML file
                xmlstr = new XMLSerializer().serializeToString(xmlDoc.documentElement);
                xmlstr = "<?xml version='1.0' encoding='utf-8'?>" + xmlstr;
             //   alert("in read " + xmlstr);

            };

            xhr.ontimeout = function (e) {
                // XMLHttpRequest timed out. Do something here.
            };

            xhr.send(null);

        }


        function openvm() {

            var vmurl = "http://<%=RobotIP%>/Myrtille/?__EVENTTARGET=&__EVENTARGUMENT=&server=<%=RobotServer%>&domain=<%=RobotDomain%>[optional]&user=<%=RobotMachine%>&passwordHash=<%=RobotPassword%>&connect=Connect%21"
            window.open(vmurl);

        }

        //reset
        function ResetDetails()
        {
            
                $("#lblNumber").text("");
            
                $("#lblOpened").text("");
            
                $("#lblShortdescription").text("");
            
                $("#lblCaller").text("");
          
                $("#lblPriority").text("");
           
                $("#lblState").text("");
           
                $("#lblCategory").text("");
        
                $("#lblAssignmentGroup").text("");
            
                $("#lblAssignedto").text("");
              
                $("#lblUpdated").text("");
         
                $("#lblUpdatedby").text("");
           
        }

        function getvalfromconfig() {





            //$.ajax({
            //    type: "GET",
            //    url: "",
            //    dataType: "xml",

            //    error: function (e) {
            //        alert("An error occurred while processing XML file");
            //        console.log("XML reading Failed: ", e);
            //    },
            //    success: function (response) {
            //        alert("success");
            //        console.log("XML reading Failed: ", e);
            //    }

            //});


         //   alert("in get val from config" + xmlstr);
                //alert(xmlstr);
            //  var xml = "<?xml version='1.0' encoding='utf-8'?>" + xmlstr;

            //   var xml = "<?xml version='1.0' encoding='utf-8'?><Details><Username>akash felix</Username><Password>InnoWise@123</Password><ProcessA><FolderName>TestExec</FolderName><FileName>Main.xaml</FileName></ProcessA><ProcessB><FolderName>Main2</FolderName><FileName>Main.xaml</FileName></ProcessB><ProcessC><FolderName>Main3</FolderName><FileName>Main.xaml</FileName></ProcessC><ProcessD><FolderName>Main4</FolderName><FileName>Main.xaml</FileName></ProcessD></Details>",
          
            xmlDoc = $.parseXML(xmlstr),
                $xml = $(xmlDoc),
                $UserName = $xml.find("Username").text();
            $Password = $xml.find("Password").text();






          //  alert(procid);

            $ProcessFolderName = $xml.find(procid).find("FolderName").text();
            $ProcessFileName = $xml.find(procid).find("FileName").text();
            User = $UserName;
            Pass = $Password;
            Pfolder = $ProcessFolderName;
            Pfile = $ProcessFileName;

          //  alert($UserName + "-" + $Password + "-" + $ProcessFolderName + "-" + $ProcessFileName);

        }

        var User;
        var Pass;
        var Pfolder;
        var Pfile;
        var procid;
        var strjson;
        var xmlstr;
        var updatexmlstr;

        //< !-- function for go button-- >

        function GoBtnClick() {
            $('#cover-spin').show(0);
            var procname = $("#dropBusinessProcess").val();
            switch (procname) {

                case "New Requisition":
                    procid = "ProcessA";
                    var jsonarr = {

                        'in_RequisitionType': $("#dropRequisitionType").val(),
                        'in_Services': $("#dropServices").val(),
                        'in_ShortDescription': $("#description").val()


                    };
                    break;
                case "Old Requisition":
                    procid = "ProcessB";
                    var jsonarr = {

                        'in_IncidentNumber': $("#IncidentNo").val(),



                    };
                    break;

                default: break;

            }




            strjson = JSON.stringify(jsonarr);




            runbot();

        }
        //< !-- function for update details button-- >

        function UpdateDtlsBtnClick() {
            procid = "ProcessC";

            var jsonarr = {

                'FirstName': $("#Fname").val(),
                'LastName': $("#Lname").val(),
                'MobileNo': $("#MobileNo").val(),
                'OfficeRoomNo': $("#OfficeNo").val(),
                'Interest': $("#Interest").val()


            };

            strjson = JSON.stringify(jsonarr);

          //  alert("in method itself " + strjson);
         
          //  editdetails();
            // runbot();
        }
        //< !-- function for update notes button-- >

        function UpdateNotesBtnClick() {
            procid = "ProcessD";

            var jsonarr = {

                'Feedback': $("#Feedback").val(),

                'Complaint': $("#Complaint").val()


            };

            strjson = JSON.stringify(jsonarr);
            runbot();
        }
        //< !-- function load emp dtls-- >

        function LoadEmpDtls() {
            procid = "ProcessE";

            var jsonarr = {

                'CustomerName': $("#lblempName").val(),
                'CustomerType': $("#lblCustomerType").val(),
                'OrganisationalUnit': $("#lblOrganisationalunit").val(),
                'CustomerId': $("#lblempid").val(),
                'Manager': $("#lblManager").val(),
                'SubDivision': $("#lblSubDivision").val(),
                'Country': $("#lblCountry").val(),
                'Office': $("#lblOffice").val(),
                'Address': $("#lblAddress").val()


            };

            strjson = JSON.stringify(jsonarr);
            runbot();
        }



        //function signintoallapps()
        //{
        //    procid = "ProcessE";

        //    var jsonarr = {};

        //    strjson = JSON.stringify(jsonarr);
        //    runbot();

        //}

        //window.onload = function () {
        //    //alert("in window load");
        //    signintoallapps();
        //}

        $(document).ready(function () {

            //alert("in document ready");
            readconfig();
            
         //   getupdatedetails();

            $("#dropBusinessProcess").change(function () {
               // alert("Dropdown val changed");
                var dropval = $("#dropBusinessProcess").val();
                if (dropval == "Old Requisition") {


                    $("#dropServices").hide();
                    $("#dropRequisitionType").hide();
                    $("#description").hide();
                    $("#lblServices").hide();
                    $("#lblRequisitionType").hide();
                    $("#lblDescription").hide();
                    $("#lblIncidentNo").css("visibility", "visible");
                    $("#IncidentNo").css("visibility", "visible");
                }
                else if (dropval == "New Requisition") {

                    $("#dropServices").show();
                    $("#dropRequisitionType").show();
                    $("#description").show();
                    $("#lblServices").show();
                    $("#lblRequisitionType").show();
                    $("#lblDescription").show();
                    $("#lblIncidentNo").css("visibility", "hidden");
                    $("#IncidentNo").css("visibility", "hidden");
                }
            });


            $(function () {
                console.log(" Connection Hub : " + $.connection.myHub);
                var chat = $.connection.myHub; //connection to hub object is created.
                $.connection.logging = true;
                // create a function that the hub can call back to display messages.
                //var sr;
                chat.client.addNewMessageToPage = function (x) {
                    alert("Add message to page received" + x);
                    var jsonobj = JSON.parse(x);
                    for (var k in jsonobj) {
                        if (k == "out_Number") {
                            $("#lblNumber").text(jsonobj.out_Number);
                        }
                        else if (k == "out_Opened") {
                            $("#lblOpened").text(jsonobj.out_Opened);
                        }
                        else if (k == "out_ShortDescription") {
                            $("#lblShortdescription").text(jsonobj.out_ShortDescription);
                        }
                        else if (k == "out_Caller") {
                            $("#lblCaller").text(jsonobj.out_Caller);
                        }
                        else if (k == "out_Priority") {
                            $("#lblPriority").text(jsonobj.out_Priority);
                        }
                        else if (k == "out_State") {
                            $("#lblState").text(jsonobj.out_State);
                        }
                        else if (k == "out_Category") {
                            $("#lblCategory").text(jsonobj.out_Category);
                        }
                        else if (k == "out_AssignmentGroup") {
                            $("#lblAssignmentGroup").text(jsonobj.out_AssignmentGroup);
                        }
                        else if (k == "out_AssignedTo") {
                            $("#lblAssignedto").text(jsonobj.out_AssignedTo);
                        }
                        else if (k == "out_Updated") {
                            $("#lblUpdated").text(jsonobj.out_Updated);
                        }
                        else if (k == "out_UpdatedBy") {
                            $("#lblUpdatedby").text(jsonobj.out_UpdatedBy);
                        }
                    }
                    $('#cover-spin').hide();
                };


                $.connection.hub.logging = true; //logs generated by debug

                //hub starts connection with client
                $.connection.hub.start().done(function () {
                    console.log("Connection starting");



                });

                $.connection.hub.disconnected(function () {
                    console.log("ReConnection starting");
                    setTimeout(function () {
                        $.connection.hub.start();
                    }, 5000); // Restart connection after 5 seconds.
                });



            });

            //window.open("http://52.186.11.76/Myrtille", 'targetWindow', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=500,height=300,left=5000');
           // window.open("http://52.186.11.76/Myrtille");
        
        });


    </script>

    <script>


    </script>
        </form>
</body>
</html>