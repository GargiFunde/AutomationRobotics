// <copyright file=WindowsPropertyCreator company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:15:03</date>
// <summary></summary>


using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Bot.Activity.WinDriverPlugin
{
    public class WindowsPropertyCreator
    {
        
         public void CreateWindowsPropertyObject(AutomationElement automationElement)
        {
            try
            {
                
                Windows_ControlProperties spp = new Windows_ControlProperties();
                spp.ControlId = automationElement.Current.AutomationId;
                spp.ControlName = automationElement.Current.Name;
                spp.ClassName = automationElement.Current.ClassName;
                spp.ControlType = automationElement.Current.LocalizedControlType;
                if (automationElement.Current.ControlType != null)
                {
                    try
                    {
                        spp.ControlType = automationElement.Current.ControlType.ProgrammaticName;
                        if (automationElement.Current.ControlType.ProgrammaticName.ToLower() == "controltype.button")
                        {
                            spp.IsEventField = true;
                        }
                    }catch(Exception ) { }
                }
                
               //spp.FrameworkId = automationElement.Current.FrameworkId;
               // spp.WindowTitle = SelectHelper.ScrapingWindowTitle;//WindowsScrapControl(automationElement);
                SelectHelper.CurrentPluginScrapeProperties.Add(spp);
                ScrapingEventArgs scrapingEventArgs = new ScrapingEventArgs();
                scrapingEventArgs.ActivityCollection = SelectHelper.CurrentPluginScrapeProperties;
                SelectHelper.OnScraping(scrapingEventArgs);

            }
            catch (Exception)
            {
                Logger.Log.Logger.LogData("Error while creating WindowsPropertyObject", Logger.LogLevel.Error);
            }
        }
    }
}
