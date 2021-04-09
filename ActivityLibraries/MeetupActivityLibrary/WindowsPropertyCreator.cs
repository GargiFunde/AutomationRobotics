// <copyright file=WindowsPropertyCreator company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:19:13</date>
// <summary></summary>

//using CommonLibrary;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Automation;

//namespace Core.ActivityLibrary
//{
//    public class WindowsPropertyCreator
//    {
//        public string GetWindowTitle(int processId)
//        {
//            return Process.GetProcessById(processId).MainWindowTitle;
//        }
//        public string WindowsScrapControl(AutomationElement selectedElement)
//        {
//            try
//            {
//                string _windowTitle = string.Empty;
//                int iCount = 0;
//                Process process = Process.GetProcessById(selectedElement.Current.ProcessId);
//                while ((process.MainWindowHandle == IntPtr.Zero) && (iCount < 50))
//                {
//                    iCount++;
//                    Thread.Sleep(100);
//                    process.Refresh();
//                }
//                var window = AutomationElement.FromHandle(process.MainWindowHandle);
//                AutomationElement titleBar = window.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TitleBar));
//                if (titleBar != null)
//                {
//                    //if (titleBar.Current != null)
//                    //{
//                    _windowTitle = titleBar.Current.Name;
//                    //}

//                }
//                if(string.IsNullOrEmpty(_windowTitle))
//                {
//                    _windowTitle = process.MainWindowTitle;
//                }
//                return _windowTitle;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public void CreateWindowsPropertyObject(AutomationElement automationElement)
//        {
//            try
//            {
//                WindowsControlProperties spp = new WindowsControlProperties();
//                spp.ControlId = automationElement.Current.AutomationId;
//                spp.ControlName = automationElement.Current.Name;
//                spp.ClassName = automationElement.Current.ClassName;
//                spp.ControlType = automationElement.Current.ControlType.ProgrammaticName;
//                spp.FrameworkId = automationElement.Current.FrameworkId;
//                spp.WindowTitle = SelectHelper.ScrapingWindowTitle;//WindowsScrapControl(automationElement);
//                SelectHelper.CurrentPluginScrapeProperties.Add(spp);
//                ScrapingEventArgs scrapingEventArgs = new ScrapingEventArgs();
//                scrapingEventArgs.ActivityCollection = SelectHelper.CurrentPluginScrapeProperties;
//                SelectHelper.OnScraping(scrapingEventArgs);

//            }
//            catch (Exception ex)
//            {
//                Logger.Log.Logger.LogData("Error while creating WindowsPropertyObject", Logger.LogLevel.Error);
//            }
//        }
//    }
//}
