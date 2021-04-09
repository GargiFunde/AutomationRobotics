<%@ page title="" language="C#" masterpagefile="~/ControlTower/MasterPageskin.master" autoeventwireup="true" inherits="Control_Tower_Default, App_Web_2o2bniex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <html lang="en" class=" js flexbox flexboxlegacy canvas canvastext webgl no-touch geolocation postmessage websqldatabase indexeddb hashchange history draganddrop websockets rgba hsla multiplebgs backgroundsize borderimage borderradius boxshadow textshadow opacity cssanimations csscolumns cssgradients cssreflections csstransforms csstransforms3d csstransitions fontface generatedcontent video audio localstorage sessionstorage webworkers applicationcache svg inlinesvg smil svgclippaths cssscrollbar"><head>
    <title>E2E Bot : Bot Dashboard</title>    
	<!-- HTML5 Shim and Respond.js IE10 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 10]>
		<script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
		<script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
		<![endif]-->

      <!-- Meta -->
      <meta charset="utf-8">
      <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui">
      <meta http-equiv="X-UA-Compatible" content="IE=edge">
      <meta name="description" content="Gradient Able Bootstrap admin template made using Bootstrap 4 and it has huge amount of ready made feature, UI components, pages which completely fulfills any dashboard needs.">
      <meta name="keywords" content="bootstrap, bootstrap admin template, admin theme, admin dashboard, dashboard template, admin template, responsive">
      <meta name="author" content="Phoenixcoded">
      <!-- Favicon icon -->
      <link rel="icon" href="files/assets/images/favicon.ico" type="image/x-icon">
      <!-- Google font-->
	  <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700,800" rel="stylesheet">
	  <!-- Required Fremwork -->
      <link rel="stylesheet" type="text/css" href="files/bower_components/bootstrap/css/bootstrap.min.css">
      <!-- waves.css -->
      <link rel="stylesheet" href="files/assets/pages/waves/css/waves.min.css" type="text/css" media="all">
      <!-- feather icon -->
      <link rel="stylesheet" type="text/css" href="files/assets/icon/feather/css/feather.css">      
      <!-- themify-icons line icon -->
      <link rel="stylesheet" type="text/css" href="files/assets/icon/themify-icons/themify-icons.css">
	  <!-- ico font -->
      <link rel="stylesheet" type="text/css" href="files/assets/icon/icofont/css/icofont.css">
      <!-- Font Awesome -->
      <link rel="stylesheet" type="text/css" href="files/assets/icon/font-awesome/css/font-awesome.min.css">
	  <!-- Data Table Css -->
      <link rel="stylesheet" type="text/css" href="files/bower_components/datatables.net-bs4/css/dataTables.bootstrap4.min.css">
      <link rel="stylesheet" type="text/css" href="files/assets/pages/data-table/css/buttons.dataTables.min.css">
      <link rel="stylesheet" type="text/css" href="files/bower_components/datatables.net-responsive-bs4/css/responsive.bootstrap4.min.css">

      
      <!-- Style.css -->
      <link rel="stylesheet" type="text/css" href="files/assets/css/style.css">
      <link rel="stylesheet" type="text/css" href="files/assets/css/pages.css">
	  <!-- <link rel="stylesheet" type="text/css" href="files/assets/css/widget.css"> -->

	 
  </head>
  <body themebg-pattern="theme1">
  <!-- [ Pre-loader ] start -->
  <div class="loader-bg" style="display: none;">
      <div class="loader-bar"></div>
  </div>
  <!-- [ Pre-loader ] end -->
  <div id="pcoded" class="pcoded iscollapsed" nav-type="st2" theme-layout="vertical" vertical-placement="left" vertical-layout="wide" pcoded-device-type="tablet" vertical-nav-type="offcanvas" vertical-effect="overlay" vnavigation-view="view1" fream-type="theme1" layout-type="light">
      <div class="pcoded-overlay-box"></div>
      <div class="pcoded-container navbar-wrapper">
          <!-- [ Header ] start -->
          <nav class="navbar header-navbar pcoded-header iscollapsed" header-theme="theme1" pcoded-header-position="relative">
              <div class="navbar-wrapper">
                  <div class="navbar-logo">
                      <a class="mobile-menu waves-effect waves-light" id="mobile-collapse" href="#!">
                          <i class="feather icon-toggle-right"></i>
                      </a>
                      <a href="index.html">
                          <img class="img-fluid" src="files/assets/images/logo.png" alt="Theme-Logo">
                      </a>
                      <a class="mobile-options waves-effect waves-light">
                          <i class="feather icon-more-horizontal"></i>
                      </a>
                  </div>
                  <div class="navbar-container container-fluid">
                      <ul class="nav-left">
                          <li class="header-search">
                              <div class="main-search morphsearch-search">
                                  <div class="input-group">
                                        <span class="input-group-prepend search-close">
										<i class="feather icon-x input-group-text"></i>
									</span>
                                      <input type="text" class="form-control" placeholder="Enter Keyword">
                                      <span class="input-group-append search-btn">
										<i class="feather icon-search input-group-text"></i>
									</span>
                                  </div>
                              </div>
                          </li>
                          <li>
                              <a href="#!" onclick="javascript:toggleFullScreen()" class="waves-effect waves-light">
                                  <i class="full-screen feather icon-maximize"></i>
                              </a>
                          </li>
                      </ul>
                      <ul class="nav-right">
                          <li class="user-profile header-notification">
                              <div class="dropdown-primary dropdown">
                                  <div class="dropdown-toggle" data-toggle="dropdown">
                                      <img src="files/assets/images/avatar-4.png" class="img-radius" alt="User-Profile-Image">
                                      <span>Sunder Rao</span>
                                      <i class="feather icon-chevron-down"></i>
                                  </div>
                                  <ul class="show-notification profile-notification dropdown-menu" data-dropdown-in="fadeIn" data-dropdown-out="fadeOut">
                                      <li>
                                          <a href="#!">
                                              <i class="feather icon-settings"></i> Settings

                                          </a>
                                      </li>
                                      <li>
                                          <a href="user-profile.html">
                                              <i class="feather icon-user"></i> Profile

                                          </a>
                                      </li>
                                      <li>
                                          <a href="email-inbox.html">
                                              <i class="feather icon-mail"></i> My Messages

                                          </a>
                                      </li>
                                      <li>
                                          <a href="auth-lock-screen.html">
                                              <i class="feather icon-lock"></i> Lock Screen

                                          </a>
                                      </li>
                                      <li>
                                          <a href="auth-normal-sign-in.html">
                                              <i class="feather icon-log-out"></i> Logout

                                          </a>
                                      </li>
                                  </ul>
                              </div>
                          </li>
                      </ul>
                  </div>
              </div>
          </nav>
          <!-- [ Header ] end -->

          <!-- [ chat user list ] start -->
          
          <!-- [ chat user list ] end -->

          <!-- [ chat message ] start -->
          
          <!-- [ chat message ] end -->
          <div class="pcoded-main-container" style="margin-top: 0px;">
              <div class="pcoded-wrapper">
                  <!-- [ navigation menu ] start -->
                  <nav class="pcoded-navbar" navbar-theme="themelight1" active-item-theme="theme1" sub-item-theme="theme2" active-item-style="style0" pcoded-navbar-position="absolute">
                      <div class="slimScrollDiv" style="position: relative; overflow: hidden; width: 100%; height: 100%;"><div class="pcoded-inner-navbar main-menu" style="overflow: hidden; width: 100%; height: 100%;">
                          <div class="">
                              <div class="main-menu-header">
                                  <!-- <img class="img-menu-user img-radius" src="files/assets/images/avatar-4.png" alt="User-Profile-Image"> -->
                                  <div class="user-details">
                                      <!-- <p id="more-details">Sunder Rao<i class="feather icon-chevron-down m-l-10"></i></p> -->
                                  </div>
                              </div>
                              <div class="main-menu-content">
                                  <ul>
                                      <li class="more-details">
                                          <a href="user-profile.html">
                                              <i class="feather icon-user"></i>View Profile
                                          </a>
                                          <a href="#!">
                                              <i class="feather icon-settings"></i>Settings
                                          </a>
                                          <a href="auth-normal-sign-in.html">
                                              <i class="feather icon-log-out"></i>Logout
                                          </a>
                                      </li>
                                  </ul>
                              </div>
                          </div>
                          <!-- <div class="pcoded-navigation-label">Navigation</div> -->
                          <ul class="pcoded-item pcoded-left-item" item-border="true" item-border-style="solid" subitem-border="true">
							  <li class="active">
                                  <a href="BotDashboard.html" class="waves-effect waves-dark">
									<span class="pcoded-micon">
										<i class="feather icon-globe"></i>
									</span>
                                      <span class="pcoded-mtext">Bot Dashboard</span>
                                  </a>
                              </li>
							  <li class="">
                                  <a href="ProcessManagement.html" class="waves-effect waves-dark">
									<span class="pcoded-micon">
										<i class="feather icon-cpu"></i>
									</span>
                                      <span class="pcoded-mtext">Process Management</span>
                                  </a>
                              </li>
                              <li class="pcoded-hasmenu" dropdown-icon="style1" subitem-icon="style1">
                                  <a href="javascript:void(0)" class="waves-effect waves-dark">
                                      <span class="pcoded-micon"><i class="feather icon-calendar"></i></span>
                                      <span class="pcoded-mtext">Scheduler</span>
                                  </a>
                                  <ul class="pcoded-submenu">
                                      <li class="">
                                          <a href="ScheduleDetails.html" class="waves-effect waves-dark">
                                              <span class="pcoded-mtext">Schedule Details</span>
                                          </a>
                                      </li>
                                      <li class="">
                                          <a href="AddSchedule.html" class="waves-effect waves-dark">
                                              <span class="pcoded-mtext">Add Schedule</span>
                                          </a>
                                      </li>
                                  </ul>
                              </li>
                              <li class="pcoded-hasmenu" dropdown-icon="style1" subitem-icon="style1">
                                  <a href="javascript:void(0)" class="waves-effect waves-dark">
                                      <span class="pcoded-micon"><i class="feather icon-users"></i></span>
                                      <span class="pcoded-mtext">User Management</span>                                      
                                  </a>
                                  <ul class="pcoded-submenu">
									  <li class="">
                                          <a href="AddUser.html" class="waves-effect waves-dark">
                                              <span class="pcoded-mtext">Add User</span>
                                          </a>
                                      </li>
									  <li class="">
                                          <a href="AddRobot.html" class="waves-effect waves-dark">
                                              <span class="pcoded-mtext">Add Robot</span>
                                          </a>
                                      </li>
									  <li class="">
                                          <a href="AssignQueue.html" class="waves-effect waves-dark">
                                              <span class="pcoded-mtext">Assign Queue To Bot</span>
                                          </a>
                                      </li>
									  <li class="">
                                          <a href="AssignBot.html" class="waves-effect waves-dark">
                                              <span class="pcoded-mtext">Assign Bot To User</span>
                                          </a>
                                      </li>
                                  </ul>
                              </li>
                              <li class="">
                                  <a href="QueueManagement.html" class="waves-effect waves-dark">
									<span class="pcoded-micon">
										<i class="feather icon-wind"></i>
									</span>
                                      <span class="pcoded-mtext">Queue Management</span>
                                  </a>
                              </li>
							  <li class="">
                                  <a href="Configuration.html" class="waves-effect waves-dark">
									<span class="pcoded-micon">
										<i class="feather icon-settings"></i>
									</span>
                                      <span class="pcoded-mtext">Configuration</span>
                                  </a>
                              </li>
							  <li class="">
                                  <a href="PromoteDepromote.html" class="waves-effect waves-dark">
									<span class="pcoded-micon">
										<i class="feather icon-layers"></i>
									</span>
                                      <span class="pcoded-mtext">Promote Depromote</span>
                                  </a>
                              </li>
                              <li class="pcoded-hasmenu" dropdown-icon="style1" subitem-icon="style1">
                                  <a href="javascript:void(0)" class="waves-effect waves-dark">
									<span class="pcoded-micon">
										<i class="feather icon-activity"></i>
									</span>
                                      <span class="pcoded-mtext">Troubleshoot</span>
                                  </a>
                                  <ul class="pcoded-submenu">
                                      <li class="">
                                          <a href="BotLog.html" class="waves-effect waves-dark">
                                              <span class="pcoded-mtext">Bot Log</span>
                                          </a>
                                      </li>
                                      <li class="">
                                          <a href="AuditLog.html" class="waves-effect waves-dark">
                                              <span class="pcoded-mtext">Audit Log</span>
                                          </a>
                                      </li>
                                  </ul>
                              </li>
							  <li class="">
                                  <a href="Reports.html" class="waves-effect waves-dark">
									<span class="pcoded-micon">
										<i class="feather icon-bar-chart"></i>
									</span>
                                      <span class="pcoded-mtext">Reports</span>
                                  </a>
                              </li>
                          </ul>
                      </div><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 3px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px; height: 558px;"></div><div class="slimScrollRail" style="width: 3px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div>
                  </nav>
                  <!-- [ navigation menu ] end -->
                  <div class="pcoded-content">
                      <!-- [ breadcrumb ] start -->
                      <div class="page-header">
                          <div class="page-block">
                              <div class="row align-items-center">
                                  <div class="col-md-8">
                                      <div class="page-header-title">
                                          <h4 class="m-b-10">Bots</h4>
                                      </div>
                                      <ul class="breadcrumb">
                                          <li class="breadcrumb-item">
                                              <a href="index.html">
                                                  <i class="feather icon-home"></i>
                                              </a>
                                          </li>
                                          <li class="breadcrumb-item"><a href="#!">Bot Dashboard</a>
                                          </li>                                          
                                      </ul>
                                  </div>
                              </div>
                          </div>
                      </div>
                      <!-- [ breadcrumb ] end -->
                        <div class="pcoded-inner-content">
                            <!-- Main-body start -->
                            <div class="main-body">
                                <div class="page-wrapper">
                                    <!-- Page-body start -->
                                    <div class="page-body">
                                        <!-- Basic table card start -->
                                        <div class="card">
                                            <div class="card-header">
                                                <h5>List of Available Bots</h5>
                                                <!-- <span>use class <code>table</code> inside table element</span> -->
                                                <div class="card-header-right">                                                             
												<ul class="list-unstyled card-option">                                                                 
													<li class="first-opt"><i class="feather icon-chevron-left open-card-option"></i></li>                                                                 
													<li><i class="feather icon-maximize full-card"></i></li>                                                                 
													<li><i class="feather icon-minus minimize-card"></i></li>                                                                 
													<li><i class="feather icon-refresh-cw reload-card"></i></li>                                                                 
													<!-- <li><i class="feather icon-trash close-card"></i></li>                                                                  -->
													<li><i class="feather icon-chevron-left open-card-option"></i></li>                                                             
												</ul>                                                         
												</div>
                                            </div>
                                            <div class="card-block table-border-style">
                                                <div class="table-responsive dt-responsive">
                                                    <div id="dom-jqry_wrapper" class="dataTables_wrapper dt-bootstrap4 no-footer"><div class="row"><div class="col-xs-12 col-sm-12 col-sm-12 col-md-6"><div class="dataTables_length" id="dom-jqry_length"><label>Show <select name="dom-jqry_length" aria-controls="dom-jqry" class="form-control input-sm"><option value="10">10</option><option value="25">25</option><option value="50">50</option><option value="100">100</option></select> entries</label></div></div><div class="col-xs-12 col-sm-12 col-md-6"><div id="dom-jqry_filter" class="dataTables_filter"><label>Search:<input type="search" class="form-control input-sm" placeholder="" aria-controls="dom-jqry"></label></div></div></div><div class="row"><div class="col-xs-12 col-sm-12"><table id="dom-jqry" class="table table-striped table-bordered nowrap dataTable no-footer" role="grid" aria-describedby="dom-jqry_info">
                                                        <thead>
                                                            <tr role="row"><th class="sorting_asc" tabindex="0" aria-controls="dom-jqry" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Status: activate to sort column descending" style="width: 57px;">Status</th><th class="sorting" tabindex="0" aria-controls="dom-jqry" rowspan="1" colspan="1" aria-label="Bot Name: activate to sort column ascending" style="width: 135px;">Bot Name</th><th class="sorting" tabindex="0" aria-controls="dom-jqry" rowspan="1" colspan="1" aria-label="Machine Name: activate to sort column ascending" style="width: 143px;">Machine Name</th><th class="sorting" tabindex="0" aria-controls="dom-jqry" rowspan="1" colspan="1" aria-label="Req Served In Section: activate to sort column ascending" style="width: 183px;">Req Served In Section</th><th class="sorting" tabindex="0" aria-controls="dom-jqry" rowspan="1" colspan="1" aria-label="Executing Automation: activate to sort column ascending" style="width: 188px;">Executing Automation</th><th class="sorting" tabindex="0" aria-controls="dom-jqry" rowspan="1" colspan="1" aria-label="Action: activate to sort column ascending" style="width: 60px;">Action</th></tr>
                                                        </thead>
                                                        <tbody>
                                                            
                                                            
                                                                                                                        
                                                        <tr role="row" class="odd">
                                                                <td class="text-center sorting_1">
																	<div class="block_red"></div>
																</td>
                                                                <td>Bot2</td>
                                                                <td>SERVER00001</td>
                                                                <td></td>
                                                                <td></td>
                                                                <td class="toggler">
																	<a href="#"><i class="feather icon-play play playBtn"></i></a>
																	<a href="#"><i class="feather icon-stop-circle stop stopBtn" style="display:none"></i></a>
																</td>
                                                            </tr><tr role="row" class="even">
                                                                <td class="text-center sorting_1">
																	<div class="block_green"></div>
																</td>
                                                                <td>Robot_OrderMgmt</td>
                                                                <td>DESKTOP-1BQD86A</td>
                                                                <td></td>
                                                                <td></td>
                                                                <td class="toggler">
																	<a href="#"><i class="feather icon-play play playBtn"></i></a>
																	<a href="#"><i class="feather icon-stop-circle stop stopBtn" style="display:none"></i></a>
																</td>
                                                            </tr><tr role="row" class="odd">
                                                                <td class="text-center sorting_1">
																	<div class="block_grey"></div>
																</td>
                                                                <td>Demo_Bot</td>
                                                                <td>IWPUNLPT0077</td>
                                                                <td></td>
                                                                <td></td>
                                                                <td class="toggler">
																	<a href="#"><i class="feather icon-play play playBtn"></i></a>
																	<a href="#"><i class="feather icon-stop-circle stop stopBtn" style="display:none"></i></a>
																</td>
                                                            </tr></tbody>                                                        
                                                    </table></div></div><div class="row"><div class="col-xs-12 col-sm-12 col-md-5"><div class="dataTables_info" id="dom-jqry_info" role="status" aria-live="polite">Showing 1 to 3 of 3 entries</div></div><div class="col-xs-12 col-sm-12 col-md-7"><div class="dataTables_paginate paging_simple_numbers" id="dom-jqry_paginate"><ul class="pagination"><li class="paginate_button page-item previous disabled" id="dom-jqry_previous"><a href="#" aria-controls="dom-jqry" data-dt-idx="0" tabindex="0" class="page-link">Previous</a></li><li class="paginate_button page-item active"><a href="#" aria-controls="dom-jqry" data-dt-idx="1" tabindex="0" class="page-link">1</a></li><li class="paginate_button page-item next disabled" id="dom-jqry_next"><a href="#" aria-controls="dom-jqry" data-dt-idx="2" tabindex="0" class="page-link">Next</a></li></ul></div></div></div></div>
                                                </div>
                                            </div>
                                        </div>
                                        
                                    </div>
                                    <!-- Page-body end -->
                                </div>
                            </div>
                            <!-- Main-body end -->

                            <div id="styleSelector">
								
                            <div class="selector-toggle"><a href="javascript:void(0)" class="waves-effect waves-light"></a></div><ul><li><p class="selector-title main-title st-main-title"><b>Select </b>Your Desired Style</p><span class="text-muted">Live customizer with tons of options</span></li><li><p class="selector-title">Main layouts</p></li><li><div class="theme-color"><a href="#" data-toggle="tooltip" title="" class="navbar-theme waves-effect waves-light" navbar-theme="themelight1" data-original-title="light Navbar"><span class="head"></span><span class="cont"></span></a><a href="#" data-toggle="tooltip" title="" class="navbar-theme waves-effect waves-light" navbar-theme="theme1" data-original-title="Dark Navbar"><span class="head"></span><span class="cont"></span></a><a href="#" data-toggle="tooltip" title="" class="Layout-type waves-effect waves-light" layout-type="light" data-original-title="light Layout"><span class="head"></span><span class="cont"></span></a><a href="#" data-toggle="tooltip" title="" class="Layout-type waves-effect waves-light" layout-type="dark" data-original-title="Dark Layout"><span class="head"></span><span class="cont"></span></a><a href="#" data-toggle="tooltip" title="" class="Layout-type waves-effect waves-light" layout-type="reset" data-original-title="Reset Default"><i class="feather icon-power"></i></a></div></li></ul><ul><li><p class="selector-title">Header color</p></li><li class="theme-option"><div class="theme-color"><a href="#" class="header-theme waves-effect waves-light" header-theme="theme1" active-item-color="theme1"><span class="head"></span><span class="cont"></span></a><a href="#" class="header-theme waves-effect waves-light" header-theme="theme2" active-item-color="theme2"><span class="head"></span><span class="cont"></span></a><a href="#" class="header-theme waves-effect waves-light" header-theme="theme3" active-item-color="theme3"><span class="head"></span><span class="cont"></span></a><a href="#" class="header-theme waves-effect waves-light" header-theme="theme4" active-item-color="theme4"><span class="head"></span><span class="cont"></span></a><a href="#" class="header-theme waves-effect waves-light" header-theme="theme5" active-item-color="theme5"><span class="head"></span><span class="cont"></span></a><a href="#" class="header-theme waves-effect waves-light" header-theme="theme6" active-item-color="theme6"><span class="head"></span><span class="cont"></span></a></div></li><li><p class="selector-title">Active link color</p></li><li class="theme-option"><div class="theme-color"><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme1"> </a><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme2"> </a><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme3"> </a><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme4"> </a><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme5"> </a><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme6"> </a><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme7"> </a><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme8"> </a><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme9"> </a><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme10"> </a><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme11"> </a><a href="#" class="active-item-theme small waves-effect waves-light" active-item-theme="theme12"> </a></div></li><li><p class="selector-title">Menu Caption Color</p></li><li class="theme-option"><div class="theme-color"><a href="#" class="leftheader-theme small waves-effect waves-light" menu-caption="theme1"> </a><a href="#" class="leftheader-theme small waves-effect waves-light" menu-caption="theme2"> </a><a href="#" class="leftheader-theme small waves-effect waves-light" menu-caption="theme3"> </a><a href="#" class="leftheader-theme small waves-effect waves-light" menu-caption="theme4"> </a><a href="#" class="leftheader-theme small waves-effect waves-light" menu-caption="theme5"> </a><a href="#" class="leftheader-theme small waves-effect waves-light" menu-caption="theme6"> </a><a href="#" class="leftheader-theme small waves-effect waves-light" menu-caption="theme7"> </a><a href="#" class="leftheader-theme small waves-effect waves-light" menu-caption="theme8"> </a><a href="#" class="leftheader-theme small waves-effect waves-light" menu-caption="theme9"> </a></div></li></ul></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
	
    
    <!-- Required Jquery -->
    <script type="text/javascript" src="files/bower_components/jquery/js/jquery.min.js"></script>
    <script type="text/javascript" src="files/bower_components/jquery-ui/js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="files/bower_components/popper.js/js/popper.min.js"></script>
    <script type="text/javascript" src="files/bower_components/bootstrap/js/bootstrap.min.js"></script>
    <!-- waves js -->
    <script src="files/assets/pages/waves/js/waves.min.js"></script>
    <!-- jquery slimscroll js -->
    <script type="text/javascript" src="files/bower_components/jquery-slimscroll/js/jquery.slimscroll.js"></script>
    
    <!-- modernizr js -->
    <script type="text/javascript" src="files/bower_components/modernizr/js/modernizr.js"></script>
    <script type="text/javascript" src="files/bower_components/modernizr/js/css-scrollbars.js"></script>
	
	<!-- <!-- waves js --> --&gt;
    <!-- <script src="files/assets/pages/waves/js/waves.min.js"></script> -->
	<!-- Data Table Css -->
      <script src="files/bower_components/datatables.net/js/jquery.dataTables.min.js"></script>
	  <script src="files/bower_components/datatables.net-buttons/js/dataTables.buttons.min.js"></script>
	  <script src="files/assets/pages/data-table/js/jszip.min.js"></script>
	  <script src="files/assets/pages/data-table/js/pdfmake.min.js"></script>
	  <script src="files/assets/pages/data-table/js/vfs_fonts.js"></script>
	  <script src="files/bower_components/datatables.net-buttons/js/buttons.print.min.js"></script>
	  <script src="files/bower_components/datatables.net-buttons/js/buttons.html5.min.js"></script>
	  <script src="files/bower_components/datatables.net-bs4/js/dataTables.bootstrap4.min.js"></script>
	  <script src="files/bower_components/datatables.net-responsive/js/dataTables.responsive.min.js"></script>
	  <script src="files/bower_components/datatables.net-responsive-bs4/js/responsive.bootstrap4.min.js"></script>

    <!-- Custom js -->
	<script src="files/assets/pages/data-table/js/data-table-custom.js"></script>
    <script src="files/assets/js/pcoded.min.js"></script>
    <script src="files/assets/js/vertical/vertical-layout.min.js"></script>
    <script src="files/assets/js/jquery.mCustomScrollbar.concat.min.js"></script>
    <script type="text/javascript" src="files/assets/js/script.min.js"></script>
	

	<script type="text/javascript">
		$(".toggler").click(function() {
    
        // Reset all images
        $(".toggler i.stop").hide();
        $(".toggler i.play").show();
        
        // Now toggle the ones in this .toggler
        $("i", this).toggle();
    });
	</script>



</body></html>
</asp:Content>

