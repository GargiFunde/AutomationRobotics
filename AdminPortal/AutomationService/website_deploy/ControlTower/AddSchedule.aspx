<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageSkin.master" autoeventwireup="true" inherits="DemoMasterPage2_AddSchedule, App_Web_2o2bniex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <%--<script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>--%>
          
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <asp:ScriptManager runat="server">
                    </asp:ScriptManager>
     <asp:UpdatePanel runat="server" style="width:100%">
                        <ContentTemplate>

                            <div class="alert alert-success" role="alert" runat="server" visible="true">
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                               1. Select Bot Name. <br />
                               2. Select Queue Name.<br />
                               3. Set the Schedule Time/Appointment Time.<br />
                               4. Set Stop Schedule Time(Optional).
                            </div>

    <!-- Begin Page Content -->
    <div class="container-fluid">
       
        <div id="page-wrapper">
        <!-- DataTables Example -->
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h2 class="m-0 font-weight-bold poptxt">ADD SCHEDULES
                </h2>
            </div>

            <div class="row" align="center">
                            <div class="col-sm-12">
                                <br />
                                <asp:Button ID="btnQueue" runat="server" CssClass="btn btn-success " Text="Schedule Using Queue" OnClick="btnQueue_Click" />&nbsp;&nbsp;&nbsp; 
                    <asp:Button ID="btnProcess" runat="server" CssClass="btn btn-dark" Text="Schedule Using Process" OnClick="btnProcess_Click" />&nbsp;&nbsp;&nbsp;
                            </div>
                        </div>
                        <div class="row" align="center">
                            <div class="col-sm-12 modal-title h4 font-weight-bold text-primary">
                                <br />
                                <asp:Label ID="lblPageName" runat="server" Text=""></asp:Label>
                                <br />
                            </div>
                        </div>
            <div class="card-body TableHeaderFont">
                <div class="table">
                            <div class="form-group">
                                <asp:Label ID="lblBotName" class="h4 font-weight-bold text-secondary" runat="server" Text="Bot Name :  ">Bot Name<span class="text-danger">*</span> :</asp:Label>
                                    <asp:DropDownList ID="DrpBots" runat="server" class="form-control animated--grow-in" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>

                                    <br />

                                    <% if (Queue_Click == true)
                                        { %>
                                    <asp:Label ID="lbQueueName" class="h4 font-weight-bold text-secondary" runat="server" Text="Queue Name :  ">Queue Name<span class="text-danger">*</span> :</asp:Label></td>
                                   
                                        <asp:DropDownList ID="DrpQueues" runat="server" class="form-control animated--grow-in" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                    <%}
                                    else
                                    { %>
                                    <asp:Label ID="lbProcessName" class="h4 font-weight-bold text-secondary" runat="server" Text="Process Name :  ">Process Name<span class="text-danger">*</span> :</asp:Label></td>
                                    <asp:DropDownList ID="DrpProcess" runat="server" class="form-control animated--grow-in" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <% } %>

                                    <br />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Panel ID="pnlSchedule"  class=" font-weight-bold TableHeaderFont poptxt" runat="server" GroupingText="Appointment Time">
                                  
                                                <asp:Panel ID="Panel1" runat="server" Font-Bold="False" >
                                                    <asp:RadioButtonList ID="RadioButtonList1" RepeatDirection="Horizontal" align="center" runat="server" class=" font-weight-bold poptxt" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" >&nbsp;Daily&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                        <asp:ListItem>&nbsp; Weekly&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                        <asp:ListItem>&nbsp; Monthly&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                        <asp:ListItem>&nbsp; Advanced&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </asp:Panel>
                                            </td>
                                                <asp:Panel ID="Dailypnl" runat="server" align="center">
                                                    At &nbsp;
                                                            <asp:DropDownList ID="HoursComboBox"  class="ddltext form-control-sm  animated--grow-in  dropdown-toggle"   runat="server">
                                                                <asp:ListItem Value="Select">--Select--</asp:ListItem>
                                                                <asp:ListItem>1</asp:ListItem>
                                                                <asp:ListItem>2</asp:ListItem>
                                                                <asp:ListItem>3</asp:ListItem>
                                                                <asp:ListItem>4</asp:ListItem>
                                                                <asp:ListItem>5</asp:ListItem>
                                                                <asp:ListItem>6</asp:ListItem>
                                                                <asp:ListItem>7</asp:ListItem>
                                                                <asp:ListItem>8</asp:ListItem>
                                                                <asp:ListItem>9</asp:ListItem>
                                                                <asp:ListItem>10</asp:ListItem>
                                                                <asp:ListItem>11</asp:ListItem>
                                                                <asp:ListItem>12</asp:ListItem>
                                                                <asp:ListItem>13</asp:ListItem>
                                                                <asp:ListItem>14</asp:ListItem>
                                                                <asp:ListItem>15</asp:ListItem>
                                                                <asp:ListItem>16</asp:ListItem>
                                                                <asp:ListItem>17</asp:ListItem>
                                                                <asp:ListItem>18</asp:ListItem>
                                                                <asp:ListItem>19</asp:ListItem>
                                                                <asp:ListItem>20</asp:ListItem>
                                                                <asp:ListItem>21</asp:ListItem>
                                                                <asp:ListItem>22</asp:ListItem>
                                                                <asp:ListItem>23</asp:ListItem>
                                                                <asp:ListItem>00</asp:ListItem>
                                                            </asp:DropDownList>
                                                    &nbsp;hours and&nbsp;
                                                            <asp:DropDownList ID="MinutesComboBox" class=" ddltext form-control-sm  animated--grow-in   dropdown-toggle "   runat="server">
                                                                <asp:ListItem Value="Select">--Select--</asp:ListItem>
                                                                <asp:ListItem>0</asp:ListItem>
                                                                <asp:ListItem>1</asp:ListItem>
                                                                <asp:ListItem>2</asp:ListItem>
                                                                <asp:ListItem>3</asp:ListItem>
                                                                <asp:ListItem>4</asp:ListItem>
                                                                <asp:ListItem>5</asp:ListItem>
                                                                <asp:ListItem>6</asp:ListItem>
                                                                <asp:ListItem>7</asp:ListItem>
                                                                <asp:ListItem>8</asp:ListItem>
                                                                <asp:ListItem>9</asp:ListItem>
                                                                <asp:ListItem>10</asp:ListItem>
                                                                <asp:ListItem>11</asp:ListItem>
                                                                <asp:ListItem>12</asp:ListItem>
                                                                <asp:ListItem>13</asp:ListItem>
                                                                <asp:ListItem>14</asp:ListItem>
                                                                <asp:ListItem>15</asp:ListItem>
                                                                <asp:ListItem>16</asp:ListItem>
                                                                <asp:ListItem>17</asp:ListItem>
                                                                <asp:ListItem>18</asp:ListItem>
                                                                <asp:ListItem>19</asp:ListItem>
                                                                <asp:ListItem>20</asp:ListItem>
                                                                <asp:ListItem>21</asp:ListItem>
                                                                <asp:ListItem>22</asp:ListItem>
                                                                <asp:ListItem>23</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>28</asp:ListItem>
                                                                <asp:ListItem>29</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                                <asp:ListItem>32</asp:ListItem>
                                                                <asp:ListItem>33</asp:ListItem>
                                                                <asp:ListItem>34</asp:ListItem>
                                                                <asp:ListItem>35</asp:ListItem>
                                                                <asp:ListItem>36</asp:ListItem>
                                                                <asp:ListItem>37</asp:ListItem>
                                                                <asp:ListItem>38</asp:ListItem>
                                                                <asp:ListItem>39</asp:ListItem>
                                                                <asp:ListItem>40</asp:ListItem>
                                                                <asp:ListItem>41</asp:ListItem>
                                                                <asp:ListItem>42</asp:ListItem>
                                                                <asp:ListItem>43</asp:ListItem>
                                                                <asp:ListItem>44</asp:ListItem>
                                                                <asp:ListItem>45</asp:ListItem>
                                                                <asp:ListItem>46</asp:ListItem>
                                                                <asp:ListItem>47</asp:ListItem>
                                                                <asp:ListItem>48</asp:ListItem>
                                                                <asp:ListItem>49</asp:ListItem>
                                                                <asp:ListItem>50</asp:ListItem>
                                                                <asp:ListItem>51</asp:ListItem>
                                                                <asp:ListItem>52</asp:ListItem>
                                                                <asp:ListItem>53</asp:ListItem>
                                                                <asp:ListItem>55</asp:ListItem>
                                                                <asp:ListItem>56</asp:ListItem>
                                                                <asp:ListItem>57</asp:ListItem>
                                                                <asp:ListItem>58</asp:ListItem>
                                                                <asp:ListItem>59</asp:ListItem>
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                    &nbsp;minutes<br />
                                                </asp:Panel>
                                                <asp:Panel ID="Weeklpnl" runat="server" class=" font-weight-bold" Visible="False" HorizontalAlign="Center">
                                                    <br /><br />
                                                    <asp:Label ID="Label1" runat="server" class=" font-weight-bold" Text="On every :  "></asp:Label>
                                                    <br /><br />
                                                    <asp:CheckBox ID="MondayCheckBox" runat="server" Text="&nbsp;Monday" />
                                                    &nbsp;&nbsp;
                                                    <asp:CheckBox ID="TuesdayCheckBox" runat="server" Text="&nbsp; Tuesday" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:CheckBox ID="WednesdayCheckBox" runat="server" Text="&nbsp; Wednesday" />
                                                    <br />
                                                    <asp:CheckBox ID="ThursdayCheckBox" runat="server" Text="&nbsp;Thursday" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:CheckBox ID="FridayCheckBox" runat="server" Text="&nbsp; Friday" />
                                                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:CheckBox ID="SaturdayCheckBox" runat="server" Text="&nbsp; Saturday" />

                                                    <br />
                                                    <asp:CheckBox ID="SundayCheckBox" runat="server" Text="&nbsp; Sunday" />
                                                </asp:Panel>
                                                <asp:Panel ID="Monthlypnl" runat="server" Visible="False">
                                                    <br />
                                                    Of every&nbsp;
                                                            <asp:DropDownList ID="MonthUpDown" class=" btn-sm dropdown-toggle ddltext" Style="background-color: #1d20e3; color: antiquewhite;" runat="server">
                                                                <asp:ListItem Value="Select">--Select--</asp:ListItem>
                                                                <asp:ListItem>1</asp:ListItem>
                                                                <asp:ListItem>2</asp:ListItem>
                                                                <asp:ListItem>3</asp:ListItem>
                                                                <asp:ListItem>4</asp:ListItem>
                                                                <asp:ListItem>5</asp:ListItem>
                                                                <asp:ListItem>6</asp:ListItem>
                                                                <asp:ListItem>7</asp:ListItem>
                                                                <asp:ListItem>8</asp:ListItem>
                                                                <asp:ListItem>9</asp:ListItem>
                                                                <asp:ListItem>10</asp:ListItem>
                                                                <asp:ListItem>11</asp:ListItem>
                                                                <asp:ListItem>12</asp:ListItem>
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                    &nbsp; month(s)
                                                </asp:Panel>
                                                <asp:Panel ID="Advancepnl" runat="server" Visible="False" HorizontalAlign="center">
                                                   <br /> Cron Expression: 
                                                           <br /> <br />
                                                    <asp:TextBox ID="txtCroneExpression" runat="server" Width="200px"></asp:TextBox>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>

                            <br /> <br />

                            <div class="form-group" >
                                <asp:Panel ID="Panel2" runat="server" class=" font-weight-bold poptxt" >
                                    <asp:CheckBox ID="CheckBox1"  class="h4 font-weight-bold poptxt"  runat="server" Text="&nbsp; Stop after" />
                                    <br />
                                    <div align="center">
                                    <asp:TextBox ID="txtUserName" runat="server" align="center" class=" btn-sm dropdown-toggle ddltext " ></asp:TextBox>
                                    Days &nbsp;&nbsp; &nbsp;<asp:DropDownList ID="DropDownList3" class="form-control-sm  animated--grow-in ddltext dropdown-toggle "  runat="server">
                                        <asp:ListItem Value="Select">--Select--</asp:ListItem>
                                        <asp:ListItem>0</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>13</asp:ListItem>
                                        <asp:ListItem>14</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>16</asp:ListItem>
                                        <asp:ListItem>17</asp:ListItem>
                                        <asp:ListItem>18</asp:ListItem>
                                        <asp:ListItem>19</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>21</asp:ListItem>
                                        <asp:ListItem>22</asp:ListItem>
                                        <asp:ListItem>23</asp:ListItem>
                                        <asp:ListItem>00</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;hours  and
                                                <asp:DropDownList ID="DropDownList4" class="form-control-sm  animated--grow-in ddltext dropdown-toggle "  runat="server">
                                                    <asp:ListItem Value="Select">--Select--</asp:ListItem>
                                                    <asp:ListItem>0</asp:ListItem>
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                    <asp:ListItem>7</asp:ListItem>
                                                    <asp:ListItem>8</asp:ListItem>
                                                    <asp:ListItem>9</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                    <asp:ListItem>24</asp:ListItem>
                                                    <asp:ListItem>25</asp:ListItem>
                                                    <asp:ListItem>26</asp:ListItem>
                                                    <asp:ListItem>27</asp:ListItem>
                                                    <asp:ListItem>28</asp:ListItem>
                                                    <asp:ListItem>29</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>31</asp:ListItem>
                                                    <asp:ListItem>32</asp:ListItem>
                                                    <asp:ListItem>32</asp:ListItem>
                                                    <asp:ListItem>33</asp:ListItem>
                                                    <asp:ListItem>34</asp:ListItem>
                                                    <asp:ListItem>35</asp:ListItem>
                                                    <asp:ListItem>36</asp:ListItem>
                                                    <asp:ListItem>37</asp:ListItem>
                                                    <asp:ListItem>38</asp:ListItem>
                                                    <asp:ListItem>39</asp:ListItem>
                                                    <asp:ListItem>40</asp:ListItem>
                                                    <asp:ListItem>41</asp:ListItem>
                                                    <asp:ListItem>42</asp:ListItem>
                                                    <asp:ListItem>43</asp:ListItem>
                                                    <asp:ListItem>44</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                    <asp:ListItem>46</asp:ListItem>
                                                    <asp:ListItem>47</asp:ListItem>
                                                    <asp:ListItem>48</asp:ListItem>
                                                    <asp:ListItem>49</asp:ListItem>
                                                    <asp:ListItem>50</asp:ListItem>
                                                    <asp:ListItem>51</asp:ListItem>
                                                    <asp:ListItem>52</asp:ListItem>
                                                    <asp:ListItem>53</asp:ListItem>
                                                    <asp:ListItem>55</asp:ListItem>
                                                    <asp:ListItem>56</asp:ListItem>
                                                    <asp:ListItem>57</asp:ListItem>
                                                    <asp:ListItem>58</asp:ListItem>
                                                    <asp:ListItem>59</asp:ListItem>
                                                    <asp:ListItem></asp:ListItem>
                                                </asp:DropDownList>
                                    &nbsp;minutes<br />
                                        </div>
                                </asp:Panel>
                            </div>
                            <div>
                                <br />
                                </div>
                            <asp:Panel ID="Panel3" runat="server" GroupingText=" " align="center">
                                <asp:Button type="submit" class="btn btn-primary btn-lg" ID="btnCreateSchedule"  runat="server" Text="Create Schedule" OnClick="btnCreateScheduleNow" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                         <asp:Button type="submit" class="btn btn-danger btn-lg" ID="Button2" runat="server" Text="Cancel" OnClick="BtnCancelSchedule" />
                                <!-- <asp:Label ID="CronExpressionlbl" runat="server"></asp:Label> -->
                            </asp:Panel>
                            </div>
                                    
                                </div>
                                <!-- /.row (nested) -->
                            </div>
                             </ContentTemplate>
                    </asp:UpdatePanel>
                             <%-- </div>
                      
                    <!-- /.panel-body -->
                </div>
                <!-- /.panel -->
            </div>
            <!-- /.col-lg-12 -->
        </div>
    </div>

        </div>--%>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#page-wrapper").delay(300).fadeIn(500);
            function Confirm() {
                $(".Weeklpnl").fadeIn(500).fadeOut(500);
            };
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $(".rotate").click(function () {
                        $(this).toggleClass("down");
                    })
                    $("#page-wrapper").show();
                    
                }
            });
        };

        $(".rotate").click(function () {


            $(this).addClass("down");

        })
    </script>




</asp:Content>

