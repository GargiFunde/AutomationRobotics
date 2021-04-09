// <copyright file=SapEngine company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:59</date>
// <summary></summary>

//using Logger;
//using sapfewse;
//using saprotwr.net;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.InteropServices;
//using System.Text.RegularExpressions;
//using System.Threading;

//namespace Bot.Activity.SAP
//{
    //internal class SAPEngine
    //{

    //    private string fileName = string.Empty;
    //    private saprotwr.net.CSapROTWrapper sapROTWrapper;
    //    private GuiApplication engine;
    //    private object SapGuilRot;
    //    private int initialNumber;
    //    private string lastRecordedFile;

    //    public int GetNextFileNumber
    //    {
    //        get
    //        {
    //            return Interlocked.Increment(ref this.initialNumber);
    //        }
    //    }

        // public event EventHandler<CapturedSapControlDetails> ControlCapturedEvent = delegate { };

        //public event EventHandler<string> RecordingStoppedEvent = delegate { };

        //public SAPEngine(CSapROTWrapper sapROTWrapper)
        //{
        //    //this.logger.Debug((object)"Method:SAPEngine - Enter Method");
        //    this.sapROTWrapper = sapROTWrapper;
        //    this.SapGuilRot = ((ISapROTWrapper)sapROTWrapper).GetROTEntry("SAPGUI");
        //    this.engine = this.SapGuilRot.GetType().InvokeMember("GetScriptingEngine", BindingFlags.InvokeMethod, (Binder)null, this.SapGuilRot, (object[])null) as GuiApplication;
        //    //this.logger.Debug((object)"Method:SAPEngine - Exit Method");
        //}

        //public void StartRecording(int connectionId, int sessionId)
        //{
        //    //this.logger.Debug((object)"Enter Method");
        //    if (((ISapCollectionTarget)((_Dsapfewse)this.engine).Connections).Count >= connectionId)
        //    {
        //        GuiConnection guiConnection = ((ISapCollectionTarget)((_Dsapfewse)this.engine).Connections).ElementAt(connectionId) as GuiConnection;
        //        if (((ISapCollectionTarget)((ISapConnectionTarget)guiConnection).Sessions).Count > sessionId)
        //        {
        //            GuiSession guiSession = ((ISapCollectionTarget)((ISapConnectionTarget)guiConnection).Sessions).ElementAt(sessionId) as GuiSession;
        //            SwitchToThisWindow(new IntPtr(((ISapWindowTarget)((ISapSessionTarget)guiSession).ActiveWindow).Handle), false);
        //            this.lastRecordedFile = string.Format("Session{0}{1}", (object)this.GetNextFileNumber, (object)".vbs");
        //            string lastRecordedFile = this.lastRecordedFile;
        //            ((ISapSessionTarget)guiSession).RecordFile = lastRecordedFile;
        //           // int num = 1;
        //            ((ISapSessionTarget)guiSession).Record = true;
        //            this.fileName = this.lastRecordedFile;
        //        }
        //    }
        //    //this.logger.Debug((object)"Exit Method");
        //}

        //public void StopRecording(int connectionId, int sessionId)
        //{
        //    try
        //    {
        //        //this.logger.Debug((object)"Enter Method");
        //        if (((ISapCollectionTarget)((_Dsapfewse)this.engine).Connections).Count >= connectionId)
        //        {
        //            GuiConnection guiConnection = ((ISapCollectionTarget)((_Dsapfewse)this.engine).Connections).ElementAt(connectionId) as GuiConnection;
        //            if (((ISapCollectionTarget)((ISapConnectionTarget)guiConnection).Sessions).Count > sessionId)
        //            {
        //                GuiSession guiSession = ((ISapCollectionTarget)((ISapConnectionTarget)guiConnection).Sessions).ElementAt(sessionId) as GuiSession;
        //                if (((ISapSessionTarget)guiSession).Record)
        //                    ((ISapSessionTarget)guiSession).Record=false;
        //            }
        //        }
        //        // ISSUE: reference to a compiler-generated field
        //        this.RecordingStoppedEvent((object)this, this.lastRecordedFile);
        //        this.lastRecordedFile = string.Empty;
        //        //this.logger.Debug((object)"Exit Method");
        //    }
        //    catch (Exception ex)
        //    {
        //        if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SAP\\SAP GUI\\Scripts", this.lastRecordedFile)))
        //        {
        //            // ISSUE: reference to a compiler-generated field
        //            this.RecordingStoppedEvent((object)this, this.lastRecordedFile);
        //            this.lastRecordedFile = string.Empty;
        //        }
        //        //this.logger.Error((object)string.Format("{0}", (object)ex.Message), ex);
        //    }
        //}

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        //public void StartScraping(int connectionId, int sessionId)
        //{
        //    //this.logger.Debug((object)"Enter Method");
        //    if (((ISapCollectionTarget)((_Dsapfewse)this.engine).Connections).Count >= connectionId)
        //    {
        //        GuiConnection guiConnection = ((ISapCollectionTarget)((_Dsapfewse)this.engine).Connections).ElementAt(connectionId) as GuiConnection;
        //        if (((ISapCollectionTarget)((ISapConnectionTarget)guiConnection).Sessions).Count > sessionId)
        //        {
        //            GuiSession guiSession = ((ISapCollectionTarget)((ISapConnectionTarget)guiConnection).Sessions).ElementAt(sessionId) as GuiSession;
        //            if (!((ISapWindowTarget)((ISapSessionTarget)guiSession).ActiveWindow).ElementVisualizationMode)
        //            {
        //                SwitchToThisWindow(new IntPtr(((ISapWindowTarget)((ISapSessionTarget)guiSession).ActiveWindow).Handle), false);
        //                ((ISapWindowTarget)((ISapSessionTarget)guiSession).ActiveWindow).ElementVisualizationMode =true;
        //                // ISSUE: method pointer
        //                ((ISapSessionEvents_Event)guiSession).Hit += Session_Hit;

        //            }
        //        }
        //    }
        //    //this.logger.Debug((object)"Exit Method");
        //}

        //public void StopScraping(int connectionId, int sessionId)
        //{
        //    //this.logger.Debug((object)"Enter Method");
        //    if (((ISapCollectionTarget)((_Dsapfewse)this.engine).Connections).Count >= connectionId)
        //    {
        //        GuiConnection guiConnection = ((ISapCollectionTarget)((_Dsapfewse)this.engine).Connections).ElementAt(connectionId) as GuiConnection;
        //        if (((ISapCollectionTarget)((ISapConnectionTarget)guiConnection).Sessions).Count > sessionId)
        //        {
        //            GuiSession guiSession = ((ISapCollectionTarget)((ISapConnectionTarget)guiConnection).Sessions).ElementAt(sessionId) as GuiSession;
        //            if (((ISapWindowTarget)((ISapSessionTarget)guiSession).ActiveWindow).ElementVisualizationMode)
        //            {
        //                ((ISapWindowTarget)((ISapSessionTarget)guiSession).ActiveWindow).ElementVisualizationMode = false;
        //                // ISSUE: method pointer
        //                ((ISapSessionEvents_Event)guiSession).Hit -= Session_Hit;
        //            }
        //        }
        //    }
        //    //this.logger.Debug((object)"Exit Method");
        //}

        //private void Session_Hit(GuiSession Session, GuiComponent Component, string InnerObject)
        //{
        //    //this.logger.Debug((object)"Enter Method");
        //   // CapturedSapControlDetails sapControlDetails = new CapturedSapControlDetails();
        //    //sapControlDetails.ConnectionId = 0;
        //    //sapControlDetails.SessionId = 0;
        //    string str1 = ((ISapComponentTarget)Component).Id.Substring(((ISapComponentTarget)Component).Id.IndexOf('w'));
        //  //  sapControlDetails.ControlId = str1;
        //    string type = ((ISapComponentTarget)Component).Type;
        //  //  sapControlDetails.ControlType = type;
        //    int num = 0;
        //  //  sapControlDetails.RowNumber = num;
        //    string empty = string.Empty;
        //  //  sapControlDetails.CellName = empty;
        //    SAPControlProperties e = new SAPControlProperties();
        //    string[] strArray = ((ISapComponentTarget)Component).Id.Split('/');
        //    Func<string, bool> func = (Func<string, bool>)(a => a.StartsWith("tbl"));
        //    Func<string, bool> predicate;
        //    // if (((IEnumerable<string>)strArray).Any<string>(predicate))
        //    if (((IEnumerable<string>)strArray).Any<string>(func))
        //    {
        //        Match match = new Regex("(\\d),(\\d)").Match(((ISapComponentTarget)Component).Id);
        //        if (match != null && match.Groups.Count == 3)
        //        {
        //            e.CellName = match.Groups[1].Value;
        //            e.RowNumber = Convert.ToInt32(match.Groups[2].Value);
        //            e.ControlType = "GuiTableControl";
        //            string str2 = e.ControlId.Substring(0, e.ControlId.LastIndexOf('/'));
        //            e.ControlId = str2;
        //        }
        //    }
        //    else if (Component is GuiShell && ((ISapShell)(Component as GuiShell)).SubType.Equals("GridView"))
        //    {
        //        e.ControlType = "GuiGridView";
        //        if (!string.IsNullOrEmpty(InnerObject) && Component is GuiGridView && InnerObject.StartsWith("Cell"))
        //        {
        //            MatchCollection matchCollection = new Regex("(\\d+|\\w+)").Matches(InnerObject);
        //            int int32 = Convert.ToInt32(matchCollection[1].ToString().Trim('"'));
        //            string str2 = matchCollection[2].ToString().Trim('"');
        //            e.CellName = str2;
        //            e.RowNumber = int32;
        //        }
        //    }
        //    else if (Component is GuiTree)
        //    {
        //        e.ControlType = "GuiTree";
        //        if (!string.IsNullOrEmpty(InnerObject))
        //            e.CellName = InnerObject.ToString();
        //    }
        //    else if (Component is GuiShell)
        //    {
        //        //this.logger.Info((object)((ISapShell)(Component as GuiShell)).get_SubType());
        //        e.ControlType = string.Format("Gui{0}", (object)((ISapShell)(Component as GuiShell)).SubType);
        //    }
        //    // ISSUE: reference to a compiler-generated field
        //   // this.ControlCapturedEvent((object)this, e);
        //    //this.logger.Debug((object)"Exit Method");
        //}
   // }
//}
