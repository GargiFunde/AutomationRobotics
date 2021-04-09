using Bot.Activity.Web;
using CommonLibrary;
using OpenQA.Selenium;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HotKeys
{
    [Designer(typeof(Web_GetText_ActivityDesigner))]
    [ToolboxBitmap("Resources/Web_GetText.png")]
    public class Web_GetText : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [RequiredArgument]
        [DisplayName("Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }



        [Category("Input")]
        [DisplayName("Xpath [priority 1] ")]
        [Description("Elemet xpath")]
        public InArgument<string> Xpath { get; set; }

        [Category("Input")]
        [DisplayName("IdContains [priority 2]")]
        [Description("Elemet Id Contains")]
        public InArgument<string> IdContains { get; set; }

        [Category("Input")]
        [DisplayName("IdStartWith [priority 3]")]
        [Description("Element Id Start With")]
        public InArgument<string> IdStartWith { get; set; }

        [Category("Input")]
        [DisplayName("Element Class [priority 4]")]
        [Description("Element Class [priority 4 with IdStartWith and IdContains]")]
        public InArgument<string> ElementClass { get; set; }


        [Category("Input")]
        [DisplayName("Following Sibling [priority 5]")]
        [Description("Following Sibling or preceding Sibling")]
        public InArgument<string> FollowingSibling { get; set; }

        [Category("Input")]
        [DisplayName("preceding Sibling [priority 6]")]
        [Description("preceding Sibling or Following Sibling")]
        public InArgument<string> precedingSibling { get; set; }


        [Category("Output")]
        [DisplayName("GetTextResult")]
        [Description("gettext")]
        public OutArgument<string> GetText { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            // string result = string.Empty;
            string AppId = ApplicationIDToAttach.Get(context);
            string xpath = Xpath.Get(context);
            IWebElement Element = null;
            CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

            try
            {
                if (Xpath.Get(context) != null)
                {
                    Thread.Sleep(3000);
                    if (commonWebBrowser_Current.driver.FindElements(By.XPath(xpath)).Count == 0)
                    {
                        int size = commonWebBrowser_Current.driver.FindElements(By.TagName("iframe")).Count;
                        for (int iFrameCounter = 0; iFrameCounter < size; iFrameCounter++)
                        {
                            commonWebBrowser_Current.driver.SwitchTo().Frame(iFrameCounter);
                            if (commonWebBrowser_Current.driver.FindElements(By.XPath(xpath)).Count > 0)
                            {
                                //  Console.WriteLine("***************found the element in iframe:" + iFrameCounter.ToString());
                                // perform the actions on element here
                                string result = commonWebBrowser_Current.driver.FindElement(By.XPath(xpath)).Text;
                                if (result.Equals(""))
                                {
                                    result = commonWebBrowser_Current.driver.FindElement(By.XPath(xpath)).GetAttribute("value");
                                }

                                GetText.Set(context, result);

                                //RemoteWebElement element1 = (RemoteWebElement)IEWATIN.driver.FindElementByXPath(sXPath);
                                //element1.SendKeys(value);

                            }
                            commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                        }
                    }
                    else
                    {

                        string result = commonWebBrowser_Current.driver.FindElement(By.XPath(xpath)).Text;
                        GetText.Set(context, result);
                    }

                    //***************************************************************



                    //  commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                }

                if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) != null)
                {

                    Thread.Sleep(3000);
                    string result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and contains(@class,'" + ElementClass.Get(context) + "')]")).Text;

                    if (result.Equals(""))
                    {
                        result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and contains(@class,'" + ElementClass.Get(context) + "')]")).GetAttribute("value");
                    }

                    GetText.Set(context, result);



                }

                if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) == null)
                {

                    Thread.Sleep(3000);
                    string result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]")).Text;

                    if (result.Equals(""))
                    {
                        result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]")).GetAttribute("value");
                    }

                    GetText.Set(context, result);



                    commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                }

                if (IdContains.Get(context) != null && IdStartWith.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Thread.Sleep(3000);
                    string result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@id,'" + IdContains.Get(context) + "')]")).Text;

                    if (result.Equals(""))
                    {
                        result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@id,'" + IdContains.Get(context) + "')]")).GetAttribute("value");
                    }

                    GetText.Set(context, result);


                }

                if (IdStartWith.Get(context) != null && IdContains.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Thread.Sleep(3000);
                    string result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id,'" + IdStartWith.Get(context) + "')]")).Text;

                    if (result.Equals(""))
                    {
                        result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id,'" + IdStartWith.Get(context) + "')]")).GetAttribute("value");
                    }

                    GetText.Set(context, result);

                    commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                }
                if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && FollowingSibling.Get(context) != null)
                {
                    Thread.Sleep(3000);
                    string result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/following-sibling::" + FollowingSibling.Get(context) + "")).Text;

                    if (result.Equals(""))
                    {
                        result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/following-sibling::" + FollowingSibling.Get(context) + "")).GetAttribute("value");
                    }

                    GetText.Set(context, result);


                    commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                }

                if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && precedingSibling.Get(context) != null)
                {
                    Thread.Sleep(3000);
                    string result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/preceding-sibling::" + precedingSibling.Get(context) + "")).Text;

                    if (result.Equals(""))
                    {
                        result = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/preceding-sibling::" + precedingSibling.Get(context) + "")).GetAttribute("value");
                    }

                    GetText.Set(context, result);


                    commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                }

            }
            catch (Exception e)
            {
                Logger.Log.Logger.LogData("Exception in " + this.DisplayName + ": " + e.Message, Logger.LogLevel.Error);

                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

    }
}
