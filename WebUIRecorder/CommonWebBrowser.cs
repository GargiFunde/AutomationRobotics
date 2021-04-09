using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace WebUIRecorder
//{
//    class CommonWebBrowser
//    {
//    }
//}


using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;

namespace WebUIRecorder
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
                ChromeOptions options = new ChromeOptions();
                // options.AddArguments("-incognito");
                // options.AddArguments("chrome.switches", "disable-extensions");


                options.AddArgument("start-maximized");
                //options.AddArgument("--enable-automation");

                options.AddUserProfilePreference("disable-popup-blocking", "false");

                // //To Disable any browser notifications
                options.AddExcludedArgument("enable-automation");

                options.AddAdditionalCapability("useAutomationExtension", false);
                options.AddArguments("disable-notifications");

                // //To disable yellow strip info bar which prompts info /*/*messages*/*/
                options.AddArguments("disable-infobars");

                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                // options.setExperimentalOption("prefs", prefs);
                driver = new ChromeDriver(driverService, options);

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
            else if (strType.ToUpper().Contains("PHANTOM"))
            {
                //var driverService = PhantomJSDriverService.CreateDefaultService();
                //driverService.HideCommandPromptWindow = true;

                //var driver = new PhantomJSDriver(driverService);
            }

            var firingDriver = new EventFiringWebDriver(driver);

            firingDriver.Navigating += FiringDriver_Navigating;
            firingDriver.Navigated += FiringDriver_Navigated;

        }

        private void FiringDriver_Navigated(object sender, WebDriverNavigationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void FiringDriver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {

            //throw new NotImplementedException();  
        }

        public void LaunchWebBrowser(string url, int iTimeInSec)
        {
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(iTimeInSec);
            driver.Navigate().GoToUrl(url); //("http://www.google.com/");
            //driver.Navigate().;
        }
    }
}
