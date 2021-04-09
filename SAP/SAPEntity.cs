using CommonLibrary;
using Logger;
using sapfewse;
using System;
using System.Diagnostics;

namespace Bot.Activity.SAP
{
    public class SAPEntity: CommonLibrary.Interfaces.IApplicationInterface
    {
        public string AppId { get; set; }
        public string sapPath { get; set; }
        public string rotEntry { get; set; }
        public string connectString { get; set; }
        public GuiApplication SapGuiApp { get; set; }
        public GuiConnection SapConnection { get; set; }
        public GuiSession SapSession { get; set; }
        public int iTimeInSec { get; set; }
        public int ProcessId { get; set; }


        public SAPEntity()
        {
            iTimeInSec = 5;
        }
        public void Close()
        {
          //  throw new NotImplementedException();
        }

        //connection string "Access to ERP ECC 6.0 EHP7"
        //language "EN"
        //pwd "welcome1"
        //user "mmuser1"
        // Client id "800"

        //Company code "1000"

        private void KillSAPWindow(GuiConnection SapConnection, Process process)
        {
            try
            {
                Log.Logger.LogData("KillSAPWindow " +  "process start", LogLevel.Info);
                if (SapConnection.Sessions.Count > 0)
                {
                    GuiSession session = SapConnection.Children.ElementAt(0) as GuiSession;
                    GuiMainWindow SapMainWindow = (GuiMainWindow)session.FindById("wnd[0]");
                    SapMainWindow.Close();
                    ((GuiButton)session.FindById("wnd[1]/usr/btnSPOP-OPTION1")).Press();
                    process.Kill();
                  
                }
                CheckAndKilllProcesses();
                Log.Logger.LogData("KillSAPWindow " + "process end", LogLevel.Info);
            }
            catch (Exception ex)
            {
                CheckAndKilllProcesses();
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }

        public void CheckAndKilllProcesses()
        {
            try
            {
                Process[] AllProcesses = Process.GetProcessesByName("saplogon");

                foreach (Process SapProcess in AllProcesses)
                {
                    SapProcess.Kill();
                }
                AllProcesses = null;
            }
            catch (Exception ex)
            {
                Log.Logger.LogData("CheckAndKillProcesses"+ "Error -> SAP -> CheckAndKilllProcesses" , LogLevel.Error);
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }

        }
        public void SapGuiApp_DestroySession(GuiSession Session)
        {
            
            if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
            {
                ThreadInvoker.Instance.RunByUiThread(() =>
                {
                    ScrapWindowHelper.StopAndMakeScrapeWindowInvisible(1);
                });
                SAPEntity sapEntity = (SAPEntity)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

                SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Remove(AppId);
                // wininstance.Close();
               
            }
        }
    }
}
