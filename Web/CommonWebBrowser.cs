using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Firefox;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

namespace Bot.Activity.Web
{
    public class CommonWebBrowser
    {
        public string ApplicationId { get; set; }
        public string SearchUrl { get; set; }
        public RemoteWebDriver driver { get; set; }
       

        public CommonWebBrowser(string strType)
        {
            if (strType.ToUpper().Contains("IE")) 
            {

                //InternetExplorerOptions options = new InternetExplorerOptions();
                //options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                //options.IgnoreZoomLevel = true;
                //options.UnhandledPromptBehavior = UnhandledPromptBehavior.Accept;
                //options.EnablePersistentHover = true;
                //options.EnableNativeEvents = false;
                ////options.EnsureCleanSession = true;    // this cleansession did the trick
                //options.BrowserAttachTimeout = new TimeSpan(0, 0, 3);
                //var driverService = InternetExplorerDriverService.CreateDefaultService();
                //driverService.HideCommandPromptWindow = true;
                //driver = new InternetExplorerDriver(driverService,options);

                //ashu
                InternetExplorerOptions options = new InternetExplorerOptions();
                options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                options.IgnoreZoomLevel = true;
                options.UnhandledPromptBehavior = UnhandledPromptBehavior.Accept;
                options.EnablePersistentHover = true;
                options.EnableNativeEvents = false;
                //options.EnsureCleanSession = true;    // this cleansession did the trick
                options.BrowserAttachTimeout = new TimeSpan(0, 0, 3);
                var driverService = InternetExplorerDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                driver = new InternetExplorerDriver(driverService, options);


            }
            else if (strType.ToUpper().Contains("EDGE"))
            {
                var driverService = EdgeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                driver = new EdgeDriver(driverService);
            }
            else if (strType.ToUpper().Contains("CHROME"))
            {
                Process proc = new Process();
                proc.StartInfo.FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                proc.StartInfo.Arguments = "--new-window --remote-debugging-port=9222 --user-data-dir=C:\\Temp";
                proc.Start();
                ChromeOptions options = new ChromeOptions();
                options.DebuggerAddress = "127.0.0.1:9222";
                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                driver = new ChromeDriver(driverService, options);
              


                //ChromeOptions options = new ChromeOptions();
                //// options.AddArguments("-incognito");
                //// options.AddArguments("chrome.switches", "disable-extensions");


                //options.AddArgument("start-maximized");

                //options.AddUserProfilePreference("disable-popup-blocking", "true");
                //// //To Disable any browser notifications
                //options.AddExcludedArgument("enable-automation");
                //options.AddAdditionalCapability("useAutomationExtension", false);
                //options.AddArguments("disable-notifications");
                //// //To disable yellow strip info bar which prompts info /*/*messages*/*/
                //options.AddArguments("disable-infobars");
                //var driverService = ChromeDriverService.CreateDefaultService();
                //driverService.HideCommandPromptWindow = true;
                //// options.setExperimentalOption("prefs", prefs);
                //driver = new ChromeDriver(driverService,options);



                //driver.get("chrome://settings/clearBrowserData");
                //Thread.Sleep(5000);
                //driver.SwitchTo().ActiveElement();
                //driver.FindElement(By.CssSelector("* /deep/ #clearBrowsingDataConfirm")).Click();
                //Thread.Sleep(5000);

                //driver.Navigate().GoToUrl("chrome://settings/clearBrowserData");
                //driver.SwitchTo().ActiveElement();
                //driver.FindElement(By.XPath("//settings-ui")).SendKeys(Keys.Enter);
                //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                //wait.Until(wd => wd.Url.StartsWith("chrome://settings"));


                //driver.Manage().Cookies.DeleteAllCookies(); //delete all cookies
                //Thread.Sleep(7000);


                //driver.get("chrome://settings/clearBrowserData");
                //Thread.Sleep(5000);
                //driver.SwitchTo().ActiveElement();
                //driver.FindElement(By.CssSelector("* /deep/ #clearBrowsingDataConfirm")).Click();
                //Thread.Sleep(5000);



            }
            else if (strType.ToUpper().Contains("FIREFOX"))
            {
                FirefoxOptions options = new FirefoxOptions();
                options.AddArgument("start-maximized");
                
                var driverService = FirefoxDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                driver = new FirefoxDriver(driverService, options);

            }
            else if (strType.ToUpper().Contains("SAFARI"))
            {
                //SafariOptions options = new SafariOptions();
                //options.AddAdditionalCapability("useAutomationExtension", false);
                //var driverService = SafariDriverService.CreateDefaultService();
                //driverService.HideCommandPromptWindow = true;
                //driver = new SafariDriver(driverService,options);
              
            }
            else if(strType.ToUpper().Contains("PHANTOM"))
            {
                //var driverService = PhantomJSDriverService.CreateDefaultService();
                //driverService.HideCommandPromptWindow = true;

                //var driver = new PhantomJSDriver(driverService);
            }

            var firingDriver = new EventFiringWebDriver(driver);

            firingDriver.Navigating += FiringDriver_Navigating;
            firingDriver.Navigated += FiringDriver_Navigated;

        }

        public object switchTo()
        {
            throw new NotImplementedException();
        }

        private void FiringDriver_Navigated(object sender, WebDriverNavigationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void FiringDriver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            
            //throw new NotImplementedException();  
        }

       public void LaunchWebBrowser(string url , int iTimeInSec)
        {
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(iTimeInSec);
            driver.Navigate().GoToUrl(url); //("http://www.google.com/");
        }
    }
}
