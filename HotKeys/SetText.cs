using Bot.Activity.Web;
using CommonLibrary;
using Logger;
using OpenQA.Selenium;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotKeys
{


    [ToolboxBitmap("Resources/SetText.png")]
    [Designer(typeof(SetText_ActivityDesigner))]
    public class SetText : BaseNativeActivity
    {

        [Category("Input Paramaters")]
        [RequiredArgument]
        [DisplayName("Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }


        [Category("Input")]
        [DisplayName("InputText")]
        [Description("Enter test to set")]
        public InArgument<string> text { get; set; }

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

        
        protected override void Execute(NativeActivityContext context)
        {
            String result = string.Empty;
            string txt = text.Get(context);
            string AppId = ApplicationIDToAttach.Get(context);
            string xpath = string.Empty;
            //string cssselector = CssSelector.Get(context);
            xpath = Xpath.Get(context);

            CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

            try
            {
                if (Xpath.Get(context) != null)
                {
                    Thread.Sleep(2000);
                    if (commonWebBrowser_Current.driver.FindElements(By.XPath(xpath)).Count == 0)
                    {
                        int size = commonWebBrowser_Current.driver.FindElements(By.TagName("iframe")).Count;
                        for (int iFrameCounter = 0; iFrameCounter < size; iFrameCounter++)
                        {
                            commonWebBrowser_Current.driver.SwitchTo().Frame(iFrameCounter);
                            if (commonWebBrowser_Current.driver.FindElements(By.XPath(xpath)).Count > 0)
                            {

                                commonWebBrowser_Current.driver.FindElement(By.XPath(xpath)).Click();
                                Thread.Sleep(2000);
                                commonWebBrowser_Current.driver.FindElement(By.XPath(xpath)).Clear();
                                commonWebBrowser_Current.driver.FindElement(By.XPath(xpath)).SendKeys(txt);


                            }

                            commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                        }

                    }

                    else
                    {



                        commonWebBrowser_Current.driver.FindElement(By.XPath(xpath)).Click();
                        Thread.Sleep(1000);
                        commonWebBrowser_Current.driver.FindElement(By.XPath(xpath)).Clear();
                        commonWebBrowser_Current.driver.FindElement(By.XPath(xpath)).SendKeys(txt);
                        commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                    }

                    //***************************************************************



                    //  commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                }

                if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) != null)
                {

                    Thread.Sleep(2000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and contains(@class,'" + ElementClass.Get(context) + "')]")).Click();
                    Thread.Sleep(2000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and contains(@class,'" + ElementClass.Get(context) + "')]")).Clear();
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and contains(@class,'" + ElementClass.Get(context) + "')]")).SendKeys(txt);
                    commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) == null)
                {

                    Thread.Sleep(2000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]")).Click();
                    Thread.Sleep(2000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]")).Clear();
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]")).SendKeys(txt);
                    commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {

                    Thread.Sleep(2000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@id,'" + IdContains.Get(context) + "')]")).Click();
                    Thread.Sleep(2000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@id,'" + IdContains.Get(context) + "')]")).Clear();
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@id,'" + IdContains.Get(context) + "')]")).SendKeys(txt);
                    commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                }

                else if (IdStartWith.Get(context) != null && IdContains.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {

                    Thread.Sleep(10000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id,'" + IdStartWith.Get(context) + "')]")).Click();
                    Thread.Sleep(2000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id,'" + IdStartWith.Get(context) + "')]")).Clear();
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id,'" + IdStartWith.Get(context) + "')]")).SendKeys(txt);
                    commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                }
                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && FollowingSibling.Get(context) != null)
                {

                    Thread.Sleep(2000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/following-sibling::" + FollowingSibling.Get(context) + "")).Click();
                    Thread.Sleep(2000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/following-sibling::" + FollowingSibling.Get(context) + "")).Clear();
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/following-sibling::" + FollowingSibling.Get(context) + "")).SendKeys(txt);
                    commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && precedingSibling.Get(context) != null)
                {

                    Thread.Sleep(2000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/preceding-sibling::" + precedingSibling.Get(context) + "")).Click();
                    Thread.Sleep(2000);
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/preceding-sibling::" + precedingSibling.Get(context) + "")).Clear();
                    commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/preceding-sibling::" + FollowingSibling.Get(context) + "")).SendKeys(txt);
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

