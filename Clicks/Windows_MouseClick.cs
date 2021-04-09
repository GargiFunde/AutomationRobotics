using Bot.Activity.WinDriverPlugin;
using CommonLibrary;
using Logger;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Clicks
{
    public enum Clicks
    {
        Single = 1, Double = 2,Right=3
    }

    [ToolboxBitmap("Resources/Windows_MouseClick.png")]
    [Designer(typeof(Windows_MouseClick_ActivityDesigner))]
    public class Windows_MouseClick : BaseNativeActivity
    {
        
        [Category("Input")]
        [DisplayName("Xpath")]
        [Description("Get the Xpath ")]
        [RequiredArgument]
        public InArgument<string> xpath { get; set; }


        [Category("Input Paramaters")]
        [RequiredArgument]
        [DisplayName("Application ID To Attach")]
        
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [Category("Input")]
        [DisplayName("Click")]
        [Description("Select Single or Double Click")]
        [RequiredArgument]
        public Clicks Clk { get; set; }

      

        protected override void Execute(NativeActivityContext context)
        {
            string xp = xpath.Get(context);
            string AppId = ApplicationIDToAttach.Get(context);
            WindowsElement element=null;
            WindowsInstance window = null;
            try
            {
                //  WindowsInstance window = (WindowsInstance)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId]; ;
                if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                {
                    window = (WindowsInstance)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                }

                if (xp != null)
                {
                     element = window._driver.FindElementByXPath(xp);
                   // window._driver.Mouse.MouseMove(element.Coordinates);
                }

               
                    switch (Clk)
                    {
                        case Clicks.Single: element.Click(); break;

                        case Clicks.Double: window._driver.Mouse.DoubleClick(null); break;
                    case Clicks.Right: window._driver.Mouse.ContextClick(element.Coordinates); break;

                    default: Log.Logger.LogData("Please select click option", LogLevel.Error); break;
                    }












                //next try


            }
            catch (Exception e)
            {
                Log.Logger.LogData("Exception is" + e.Message, Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
                //else
                //{
                //    throw;
                //}
              
            }
        }
    }
}
