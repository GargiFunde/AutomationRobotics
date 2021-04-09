//using CommonLibrary;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using System;
//using System.Activities;
//using System.Collections.Generic;
//using System.Web.UI.HtmlControls;

//namespace Bot.Activity.Web
//{
//    public class GetXPath : BaseNativeActivity
//    {
//        private String generateXPATH(IWebElement childElement, String current)
//        {
//            String childTag = childElement.getTagName();
//            if (childTag.Equals("html"))
//            {
//                return "/html[1]" + current;
//            }
//            IWebElement parentElement = childElement.FindElement(By.XPath(".."));
//            List<IWebElement> childrenElements = parentElement.FindElements(By.XPath("*"));
//            int count = 0;
//            for (int i = 0; i < childrenElements.size(); i++)
//            {
//                IWebElement childrenElement = childrenElements.get(i);
//                String childrenElementTag = childrenElement.getTagName();
//                if (childTag.equals(childrenElementTag))
//                {
//                    count++;
//                }
//                if (childElement.equals(childrenElement))
//                {
//                    return generateXPATH(parentElement, "/" + childTag + "[" + count + "]" + current);
//                }
//            }
//            return null;
//        }

//        protected override void Execute(NativeActivityContext context)
//        {
//            //Web webBrowser1 = null;
//            //bool launchResult = false;

//            //string LaunchUrl = string.Empty;
//            //string AppId = string.Empty;
//            //string strWaitUntilContainText = string.Empty;
//            //int iTimeInSec = 0;
//            //AppId = "appId";
//            //LaunchUrl = "www.google.com";

//            //webBrowser1.LaunchIE("1", "Chrome", "www.google.com", iTimeInSec, launchResult, true, strWaitUntilContainText);

//            IWebDriver driver = new ChromeDriver("Path to Chrome Driver");

//            // This will open up the URL 
//            driver.Url = "https://www.geeksforgeeks.org/";

//            //string piyush = driver.FindElement(By.nam);

//            //HtmlElement element = this.webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);

//            //var savedId = element.Id;
//            //var uniqueId = Guid.NewGuid().ToString();
//            //element.Id = uniqueId;

//            //var doc = new HtmlAgilityPack.HtmlDocument();
//            //doc.LoadHtml(element.Document.GetElementsByTagName("html")[0].OuterHtml);
//            //element.Id = savedId;

//            //var node = doc.GetElementbyId(uniqueId);
//            //var xpath = node.XPath;
//        }
//    }
//}
