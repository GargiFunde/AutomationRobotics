using Logger;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;

/***
 * 
 * **Note**: Need to modify xpath before use in activity. Change is add "/ " at the starting of xpath.
 * old xpath eg."/Pane[@ClassName=""Shell_TrayWnd""][@Name=""Taskbar""]/Button[@ClassName=""Start""][@Name=""Start""]"" 
 * new xpath -"//Pane[@ClassName=""Shell_TrayWnd""][@Name=""Taskbar""]/Button[@ClassName=""Start""][@Name=""Start""]"" 
 * 
 */
namespace Clicks
{

    [ToolboxBitmap("Resources/Windows_MouseClick.png")]
    [Designer(typeof(Windows_MouseClick_Generic_ActivityDesigner))]
    public class Windows_MouseClick_Generic : BaseNativeActivity
    {

        [Category("Input")]
        [DisplayName("Element Xpath")]
        [Description("Get the element Xpath ")]
        public InArgument<string> ElementXpath { get; set; }

        [Category("Input")]
        [DisplayName("Element Name")]
        [Description("Get the element name ")]
        public InArgument<string> ElementName { get; set; }

        [Category("Input")]
        [DisplayName("Click")]
        [Description("Select Single or Double Click")]
        [RequiredArgument]
        public Clicks Clk { get; set; }

        public WindowsDriver<WindowsElement> _driver;

        protected override void Execute(NativeActivityContext context)
        {
            string xp = ElementXpath.Get(context);
            string elementName = ElementName.Get(context);

            WindowsElement element = null;
            try
            {
                 WindowsInstance("Root");
          
                if (elementName != null && xp == null)
                {
                    element = _driver.FindElementByName(elementName);
                }
                else if (elementName != null && xp != null)
                {
                    element = _driver.FindElementByName(elementName);
                }
                else if (xp != null && elementName == null)
                {
                    //test
                    element = _driver.FindElementByXPath(xp);
                }
                else
                {

                }

                if (element != null)
                {
                    switch (Clk)
                    {
                        case Clicks.Single: element.Click(); break;
                        case Clicks.Double:
                            element.Click();
                            element.Click(); break;
                        case Clicks.Right: _driver.Mouse.ContextClick(element.Coordinates); break;
                        default: Log.Logger.LogData("Please select click option", LogLevel.Error); break;
                    }
                }

            }
            catch (Exception e)
            {
                Log.Logger.LogData(e.Message + " in activity Generic_MouseClick", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }
        public enum Clicks
        {
            Single = 0, Double = 1, Right = 2
        }
        public void WindowsInstance(string AppExePath)
        {
            var options = new AppiumOptions();

            options.AddAdditionalCapability("ms:experimental-webdriver", true);
            // options.AddAdditionalCapability("ms:waitForAppLaunch", "10");
            options.AddAdditionalCapability("app", AppExePath);
            options.AddAdditionalCapability("deviceName", "WindowsPC");

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = @"WinDriver\WinAppDriver";
            process.StartInfo = startInfo;
            process.Start();                                  //Process Starting using WinApp Driver

            _driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
    }
}
