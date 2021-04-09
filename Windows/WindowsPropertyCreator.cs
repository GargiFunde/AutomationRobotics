// <copyright file=WindowsPropertyCreator company=E2E BOTS>
using CommonLibrary;
using System;
using System.Windows.Automation;

namespace Bot.Activity.Windows
{
    public class WindowsPropertyCreator
    {
         public void CreateWindowsPropertyObject(AutomationElement automationElement, AutomationElement automationNextElement, AutomationElement automationPrevElement, AutomationElement automationParentElement)
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
#pragma warning disable CS0168 // The variable 'ex1' is declared but never used
                    }catch(Exception ex1) { }
#pragma warning restore CS0168 // The variable 'ex1' is declared but never used
                }
                
                spp.FrameworkId = automationElement.Current.FrameworkId;
                spp.WindowTitle = SelectHelper.ScrapingWindowTitle;//WindowsScrapControl(automationElement);

                if (automationPrevElement != null)
                {
                    spp.PreviousSiblingId = automationPrevElement.Current.AutomationId;
                    spp.PreviousSiblingName = automationPrevElement.Current.Name;
                    spp.PreviousSiblingClassName = automationPrevElement.Current.ClassName;
                }
                //  spp.PreviousSiblingControlType = automationElement.Current.LocalizedControlType;
                if (automationNextElement != null)
                {
                    spp.NextSiblingId = automationNextElement.Current.AutomationId;
                    spp.NextSiblingName = automationNextElement.Current.Name;
                    spp.NextSiblingClassName = automationNextElement.Current.ClassName;
                }
                if (automationParentElement != null)
                {
                    spp.ParentId = automationParentElement.Current.AutomationId;
                    spp.ParentName = automationParentElement.Current.Name;
                    spp.ParentClassName = automationParentElement.Current.ClassName;
                    //spp.ChildIdRegx = automationParentElement.Current.AutomationId;
                    //spp.ChildNameRegx = automationParentElement.Current.Name;
                    //spp.ChildClassNameRegx = automationParentElement.Current.ClassName;
                }
                SelectHelper.CurrentPluginScrapeProperties.Add(spp);
                ScrapingEventArgs scrapingEventArgs = new ScrapingEventArgs();
                scrapingEventArgs.ActivityCollection = SelectHelper.CurrentPluginScrapeProperties;
                SelectHelper.OnScraping(scrapingEventArgs);

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Logger.Log.Logger.LogData("Error while creating WindowsPropertyObject", Logger.LogLevel.Error);
            }
        }
    }
}
