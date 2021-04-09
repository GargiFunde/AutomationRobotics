using CommonLibrary;
using CommonLibrary.Interfaces;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;

namespace Bot.Activity.WinDriverPlugin
{
    public class WindowsInstance : IApplicationInterface
    {
        public int ProcessId { get; set; }
      
        public string ApplicationId { get; set; }

        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4823/";
        public WindowsDriver<WindowsElement> _driver;
        public WindowsInstance( string AppExePath)
        {
            var options = new AppiumOptions();

            options.AddAdditionalCapability("ms:experimental-webdriver", true);
            options.AddAdditionalCapability("ms:waitForAppLaunch", "10");
            //options.AddAdditionalCapability("app", @"excel.exe");
            options.AddAdditionalCapability("app", AppExePath);
         
            options.AddAdditionalCapability("deviceName", "WindowsPC");
           
            //Process.Start(@"WinDriver\WinAppDriver");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = @"WinDriver\WinAppDriver";           
            process.StartInfo = startInfo;
            process.Start();                                  //Process Starting using WinApp Driver
            //Thread.Sleep(2000);
            _driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);

            //Added By Piyush
            //Thread.Sleep(8000);
            //SelectHelper.ApplicationProcessId = process.Id;
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        public void Close()
        {
            _driver.Quit();
        }
    }
}
