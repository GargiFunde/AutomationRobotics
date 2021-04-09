using CommonLibrary;
using CommonLibrary.Interfaces;
using Logger;
using sapfewse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.ComponentModel.Composition;
using System.Activities.Presentation.Services;
using System.Activities.Presentation.Model;
using System.Windows;
using SapROTWr;

namespace Bot.Activity.SAP
{
    [Export(typeof(ICustomPluginInterface))]
    public sealed class SAPPlugin : ICustomPluginInterface
    {
        public double X { get; set; }
        public double Y { get; set; }
        private static SAPPlugin instance = null;
        private bool readystate = false;
        public bool NonScrapeMode { get; set; }
        private static readonly object padlock = new object();
        
        int iOpenFile = 0;
        int iReset = 0;
        int iIECreated = 0;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        
       // SAPHelper IEHelper = new SAPHelper();

        //private readonly KeyboardHookListener m_KeyboardHookManager;
        //private readonly MouseHookListener m_MouseHookManager;

        ControlHighlighterWpfForm cf = new ControlHighlighterWpfForm();
        private SAPPlugin()
        {
           // AssemblyName = "InternetExplorer";
            //if (iOpenFile > 0)
            //{
            //    for (int i = 0; i <= iOpenFile; i++)
            //    {
            //        SelectHelper.LaunchScraping -= LaunchScraping;
            //    }
            //}
            //iOpenFile = 0;
            SelectHelper.LaunchScraping += LaunchScraping;
            iOpenFile = iOpenFile + 1;

            if (iReset > 0)
            {
                for (int i = 0; i <= iReset; i++)
                {
                    SelectHelper.Reset -= Reset;
                }
            }
            iReset = 0;
            SelectHelper.Reset += Reset;
            iReset = iReset + 1;
                                
            cf.Visibility = Visibility.Hidden;
        }
       
       
        public int CountStringOccurrences(string text, string pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }
        
        public static SAPPlugin Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new SAPPlugin();
                           
                        }
                    }
                }
                return instance;
            }
        }
       
        //this fuction will be called only by Designer ***
        public void LaunchScraping(object sender, LaunchScrapingEventArgs e)
        {
            string environment = string.Empty;
            string connectionString = string.Empty;
            string ApplicationId = string.Empty;
            int iTimeInSec = 0;
            bool launchResult = false;
            var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
            IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(System.Activities.Activity));
            SelectHelper.CurrentScrapMode = ScrapMode.SAP;
            foreach (ModelItem item in modelItems)
            {
                if (item.ItemType.FullName == "Bot.Activity.SAP.SAP_OpenApplication")
                {
                    SAP_OpenApplication owa = (SAP_OpenApplication)item.GetCurrentValue();
                    ApplicationId = owa.ApplicationID.Expression.ToString();
                    owa.sapEntity = new SAPEntity();
                    owa.sapEntity.AppId = owa.ApplicationID.Expression.ToString();
                    owa.sapEntity.sapPath = owa.SapPath.Expression.ToString();//  "C:\\Program Files (x86)\\SAP\\FrontEnd\\SAPgui\\saplogon.exe";
                    owa.sapEntity.rotEntry = owa.SAPROTEntry.Expression.ToString();
                    owa.sapEntity.connectString = owa.ConnectionString.Expression.ToString();
                    //owa.sapEntity.userName = owa.UserName.Expression.ToString();
                    //owa.sapEntity.pwd = owa.Password.Expression.ToString();
                    //owa.sapEntity.clientid = owa.ClientId.Expression.ToString();
                    //owa.sapEntity.companyname = owa.CompanyName.Expression.ToString();
                    //owa.sapEntity.companycode = owa.CompanyCode.Expression.ToString();
                    owa.sapEntity.iTimeInSec = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
                   // owa.sapEntity.language = owa.Language.Expression.ToString();
                    if (ApplicationId == e.ApplicationID)
                    {
                        launchResult = LaunchSap(owa.sapEntity, launchResult, false);
                        break;
                    }
                }
            }
        }

        public bool LaunchSap(SAPEntity sapEntity, bool launchResult, bool NoScrape = false, string TitleOrUrl = "")
        {
            TimeSpan timeSpan = new TimeSpan(sapEntity.iTimeInSec * 1000);
            NonScrapeMode = NoScrape;
            try
            {
                
                if (NoScrape == true)
                {
                    SelectHelper.StartSimulation = false;
                     TitleOrUrl = ""; //need this in all plugins so that below loop should not execute in scraping mode
                }
                if ((TitleOrUrl != "") && (!string.IsNullOrEmpty(TitleOrUrl)))
                {
                }
                else if (sapEntity.AppId != string.Empty) //scraping time
                {
                    if (!SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(sapEntity.AppId))
                    {
                        OpenSap(sapEntity);
                        int elapsed = 0;
                        while (elapsed < timeSpan.TotalMilliseconds)
                        {
                            if (sapEntity.SapGuiApp != null)
                            {
                                SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(sapEntity.AppId, sapEntity);
                                break;
                            }
                            Thread.Sleep(1000);
                            elapsed += 1000;
                        }
                    }
                    else
                    {
                        sapEntity = (SAPEntity)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[sapEntity.AppId];
                    }
                }
                else
                {
                    Logger.Log.Logger.LogData("Please enter ApplicationID or window title", LogLevel.Error);
                }
                 
                SelectHelper.CurrentPlugin = this;
                launchResult = true;
            }catch(Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            //});
            return launchResult;
        }
        public void Reset(object sender, ResetEventArgs e)
        {
            string SearchUrl = string.Empty;
            string ApplicationId = string.Empty;
            int iTimeInSec = 0;
           
            var modelService = SelectHelper._wfDesigner.Context.Services.GetService<ModelService>();
            IEnumerable<ModelItem> modelItems = modelService.Find(modelService.Root, typeof(System.Activities.Activity));

            foreach (ModelItem item in modelItems)
            {
                if (item.ItemType.FullName == "SAP.OpenSAPApplication")
                {
                    //OpenMainframeApplication owa = (OpenMainframeApplication)item.GetCurrentValue();
                    //ApplicationId = owa.ApplicationID.Expression.ToString();
                    //if (ApplicationId == e.ApplicationID)
                    //{
                    //    SearchUrl = owa.SearchUrl.Expression.ToString();
                    //    // ApplicationId = owa.TimeOutInSecond.Expression.ToString();
                    //    iTimeInSec = Convert.ToInt32(owa.TimeOutInSecond.Expression.ToString());
                    //    ResetIE(ApplicationId, SearchUrl, iTimeInSec);
                    //}
                }
            }
        }
        private bool ResetSAP(string AppId, string searchUrl, int timeInSec )
        {
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {

                        //IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        //IEWATIN.GoTo(searchUrl);
                        //IEWATIN.WaitForComplete(5000);
                        
                        //SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId] = IEWATIN;
                        return true;
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
                return false;
            }
        }


        IntPtr Handle = IntPtr.Zero;
      

        public void OpenSap(SAPEntity sapEntity)
        {
            // GuiApplication launchResult = null;
            //b
            try
            {
                SapROTWr.CSapROTWrapper sapROTWrapper = null;
                object SapGuilRot = null;

                object engine = null;
                if (File.Exists(sapEntity.sapPath))
                {
                    System.Diagnostics.Process.Start(sapEntity.sapPath);
                    DateTime StartTime = DateTime.Now;
                    while (SapGuilRot == null && sapEntity.iTimeInSec >= (DateTime.Now - StartTime).TotalSeconds)
                    {
                        SapGuilRot = new CSapROTWrapper().GetROTEntry(sapEntity.rotEntry);
                        System.Threading.Thread.Sleep(1000);
                        if (SapGuilRot == null)
                        {
                            continue;
                        }

                        sapROTWrapper = new SapROTWr.CSapROTWrapper();
                        SapGuilRot = sapROTWrapper.GetROTEntry(sapEntity.rotEntry);
                        engine = SapGuilRot.GetType().InvokeMember("GetScriptingEngine", System.Reflection.BindingFlags.InvokeMethod, null, SapGuilRot, null);
                        
                        System.Threading.Thread.Sleep(1000);
                       
                        sapEntity.SapGuiApp = new GuiApplication();
                       
                        sapEntity.SapConnection = sapEntity.SapGuiApp.OpenConnection(sapEntity.connectString, Sync: true); //creates connection
                        sapEntity.SapSession = (GuiSession)sapEntity.SapConnection.Sessions.Item(0); //creates the Gui session off the connection you made
                                                                                                     //SapSession = (GuiSession)SapConnection.Children.ElementAt(0);
                                                                                                     //GuiSession session = (GuiSession)SAPHelper.SapConnection.Children.ElementAt(0);
                        sapEntity.SapGuiApp.DestroySession += sapEntity.SapGuiApp_DestroySession;
                    }
                }
                else
                {
                    //Error
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception....." + ex);
                //Error
            }

        }

      

        public void StartScraping( string AppId)
        {

            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
            {
                SAPEntity sapEntity = (SAPEntity)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                if ((sapEntity == null) || (sapEntity.SapGuiApp == null))
                {
                    Log.Logger.LogData("SAP Instance is not loadded", LogLevel.Error);
                }

                StartScraping(sapEntity.SapConnection, sapEntity.SapSession);
            }
           
        }

        public void StopScraping( string AppId)
        {
            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
            {
                SAPEntity sapEntity = (SAPEntity)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                if ((sapEntity == null) || (sapEntity.SapGuiApp == null))
                {
                    Log.Logger.LogData("SAP Instance is not loadded", LogLevel.Error);
                }

                StopScraping(sapEntity.SapConnection, sapEntity.SapSession);
            }
          
        }

        public void StartScraping(GuiConnection SapConnection, GuiSession SapSession)
        {
            if ((SelectHelper.StartSimulation == true) && (SelectHelper.CurrentScrapMode == ScrapMode.SAP))
            {
                if (!((ISapWindowTarget)((ISapSessionTarget)SapSession).ActiveWindow).ElementVisualizationMode)
                {
                    SwitchToThisWindow(new IntPtr(((ISapWindowTarget)((ISapSessionTarget)SapSession).ActiveWindow).Handle), false);
                    ((ISapWindowTarget)((ISapSessionTarget)SapSession).ActiveWindow).ElementVisualizationMode = true;
                    // ISSUE: method pointer
                    ((ISapSessionEvents_Event)SapSession).FocusChanged += SAPPlugin_FocusChanged;
                    ((ISapSessionEvents_Event)SapSession).Hit += SAPPlugin_Hit;
                }
            }

        }


        public void StopScraping(GuiConnection SapConnection, GuiSession SapSession)
        {
            if (SapSession != null && (((ISapSessionTarget)SapSession).ActiveWindow != null) && ((ISapWindowTarget)((ISapSessionTarget)SapSession).ActiveWindow).ElementVisualizationMode)
            {
                ((ISapWindowTarget)((ISapSessionTarget)SapSession).ActiveWindow).ElementVisualizationMode=false;
                // ISSUE: method pointer
                ((ISapSessionEvents_Event)SapSession).Hit -= SAPPlugin_Hit;
            } 
        }
        public void login(SAPEntity sapEntity)
        {
            GuiTextField client  = (GuiTextField)sapEntity.SapSession.ActiveWindow.FindByName("RSYST-MANDT", "GuiTextField");
            GuiTextField login  = (GuiTextField)sapEntity.SapSession.ActiveWindow.FindByName("RSYST-BNAME", "GuiTextField");
            GuiTextField pass  = (GuiTextField)sapEntity.SapSession.ActiveWindow.FindByName("RSYST-BCODE", "GuiPasswordField");
            GuiTextField language  = (GuiTextField)sapEntity.SapSession.ActiveWindow.FindByName("RSYST-LANGU", "GuiTextField");

           // client.SetFocus();
          //  client.Text = sapEntity.clientid;
          //  login.SetFocus();
          //  login.Text = sapEntity.userName;
          //  pass.SetFocus();
          //  pass.Text = sapEntity.pwd;
          //  language.SetFocus();
          //  language.Text = sapEntity.language; //"EN";  

            //Press the green checkmark button which is about the same as the enter key 
            GuiButton btn = (GuiButton)sapEntity.SapSession.FindById("/app/con[0]/ses[0]/wnd[0]/tbar[0]/btn[0]");
           // btn.SetFocus(); 
            btn.Press();

            //Login finish-----------
            //Automation part need to do by scraping
            System.Threading.Thread.Sleep(1000);
            GuiOkCodeField searchbar = (GuiOkCodeField)sapEntity.SapSession.FindById("/app/con[0]/ses[0]/wnd[0]/tbar[0]/okcd");

          //  searchbar.SetFocus();
          //  searchbar.Text = sapEntity.companyname;

            GuiButton enter = (GuiButton)sapEntity.SapSession.FindById("/app/con[0]/ses[0]/wnd[0]/tbar[0]/btn[0]");
           // enter.SetFocus();
            enter.Press();
            System.Threading.Thread.Sleep(1000);

            GuiTextField cmpcode = (GuiTextField)sapEntity.SapSession.FindById("/app/con[0]/ses[0]/wnd[1]/usr/ctxtBKPF-BUKRS");
          //  cmpcode.SetFocus();
         //   cmpcode.Text = sapEntity.companycode;
        //    cmpcode.SetFocus();

            GuiButton Ccodeenter = (GuiButton)sapEntity.SapSession.FindById("/app/con[0]/ses[0]/wnd[1]/tbar[0]/btn[0]");
        //    Ccodeenter.SetFocus();
            Ccodeenter.Press();

            GuiTextField vendor = (GuiTextField)sapEntity.SapSession.FindById("/app/con[0]/ses[0]/wnd[0]/usr/tabsTS/tabpMAIN/ssubPAGE:SAPLFDCB:0010/ctxtINVFO-ACCNT");
         //   vendor.SetFocus();
            vendor.Text = "IndigenTechnologies";
        }

       // System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle();
        private void SAPPlugin_FocusChanged(GuiSession Session, GuiVComponent NewFocusedControl)
        {
            if (SelectHelper.StartSimulation == true)
            {
                cf.Visibility = Visibility.Visible;
                cf.Width = NewFocusedControl.Width;
                cf.Height = NewFocusedControl.Height;
                cf.Top = NewFocusedControl.Top;
                cf.Left = NewFocusedControl.Left;
            }
            else
            {
                cf.Visibility = Visibility.Hidden;
            }
            //throw new NotImplementedException();
        }
       
        private void SAPPlugin_Hit(GuiSession Session, GuiComponent Component, string InnerObject)
        {
          
            if ((SelectHelper.StartSimulation == true)&&(SelectHelper.CurrentScrapMode == ScrapMode.SAP))
            {


                //this.logger.Debug((object)"Enter Method");
                SAP_ControlProperties sapControlDetails = new SAP_ControlProperties();
                //sapControlDetails.ConnectionId = 0;
                //sapControlDetails.SessionId = 0;
                string str1 = ((ISapComponentTarget)Component).Id.Substring(((ISapComponentTarget)Component).Id.IndexOf('w'));
                sapControlDetails.ControlId = str1;
                string type = ((ISapComponentTarget)Component).Type;
                sapControlDetails.ControlType = type;
                int num = 0;
                sapControlDetails.RowNumber = num;
                string empty = string.Empty;
                sapControlDetails.CellName = empty;
                SAP_ControlProperties e = sapControlDetails;
                string[] strArray = ((ISapComponentTarget)Component).Id.Split('/');
                Func<string, bool> func = (Func<string, bool>)(a => a.StartsWith("tbl"));
                Func<string, bool> predicate;
                // if (((IEnumerable<string>)strArray).Any<string>(predicate))
                if (((IEnumerable<string>)strArray).Any<string>(func))
                {
                    Match match = new Regex("(\\d),(\\d)").Match(((ISapComponentTarget)Component).Id);
                    if (match != null && match.Groups.Count == 3)
                    {
                        e.CellName = match.Groups[1].Value;
                        e.RowNumber = Convert.ToInt32(match.Groups[2].Value);
                        e.ControlType = "GuiTableControl";
                     //   string str2 = e.ControlId.Substring(0, e.ControlId.LastIndexOf('/'));
                      //  e.ControlId = str2;
                    }
                }
                else if (Component is GuiShell && ((ISapShell)(Component as GuiShell)).SubType.Equals("GridView"))
                {
                    e.ControlType = "GuiGridView";
                    if (!string.IsNullOrEmpty(InnerObject) && Component is GuiGridView && InnerObject.StartsWith("Cell"))
                    {
                        MatchCollection matchCollection = new Regex("(\\d+|\\w+)").Matches(InnerObject);
                        int int32 = Convert.ToInt32(matchCollection[1].ToString().Trim('"'));
                        string str2 = matchCollection[2].ToString().Trim('"');
                        e.CellName = str2;
                        e.RowNumber = int32;
                    }
                }
                else if (Component is GuiTree)
                {
                   
                    e.ControlType = "GuiTree";
                    //if (!string.IsNullOrEmpty(InnerObject))
                    //    e.CellName = InnerObject.ToString();
                   string keyDetails = InnerObject.ToString();
                    char[] delimeter = { '?' };
                    string[] Keys = keyDetails.Split(delimeter);




                    sapfewse.GuiTree guiTree = (sapfewse.GuiTree)Session.FindById(str1);
                    foreach (string key in guiTree.GetAllNodeKeys()) {
                        if (key != null)
                        {
                            if (key == Keys[0])
                            {
                                e.NodeKey = key;
                            }
                        }
                     
                    }



                }
                else if (Component is GuiShell)
                {
                    //this.logger.Info((object)((ISapShell)(Component as GuiShell)).get_SubType());
                    e.ControlType = string.Format("Gui{0}", (object)((ISapShell)(Component as GuiShell)).SubType);
                }
                // ISSUE: reference to a compiler-generated field
                AddClickedControl( e);
                //this.logger.Debug((object)"Exit Method");
            }
        }

        private bool AddClickedControl(ActivityExtended spp)
        {           
            try
            {          
                SelectHelper.CurrentPluginScrapeProperties.Add(spp);
                ScrapingEventArgs scrapingEventArgs = new ScrapingEventArgs();
                scrapingEventArgs.ActivityCollection = SelectHelper.CurrentPluginScrapeProperties;
                SelectHelper.OnScraping(scrapingEventArgs);
                SelectHelper.StartSimulation = true;
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
                //Log.Logger.LogData(ex.Message, LogLevel.Error, RequestNo);
            }

            return false;
        }
        public void CloseApplication(string AppId)
        {

            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
            {
                SAPEntity sapEntity = (SAPEntity)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                if ((sapEntity == null) || (sapEntity.SapGuiApp == null))
                {
                    Log.Logger.LogData("SAP Instance is not loadded", LogLevel.Error);
                }
               else if (sapEntity.SapSession != null)
               {
                    sapEntity.SapSession.ActiveWindow.Close();
                }
            }
         
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
