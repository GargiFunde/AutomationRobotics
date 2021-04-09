using CommonLibrary;
using Logger;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;

namespace Bot.Activity.Web
{
    [Designer(typeof(Web_ScrollPage_ActivityDesigner))]
    [ToolboxBitmap("Resources/WebScrollPage.png")]
    public class Web_ScrollPage : BaseNativeActivity
    {

        [Category("Input Paramaters")]
        [RequiredArgument]
        [DisplayName("Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [Category("Input")]
        [DisplayName("Xpath [priority 1]")]
        [Description("Get the Xpath ")]
        public InArgument<string> xpath { get; set; }



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
        [DisplayName("Title [priority 5]")]
        [Description("Element Title/Text")]
        public InArgument<string> Title { get; set; }

        [Category("Input")]
        [DisplayName("Following Sibling [priority 6]")]
        [Description("Following Sibling or preceding Sibling")]
        public InArgument<string> FollowingSibling { get; set; }

        [Category("Input")]
        [DisplayName("preceding Sibling [priority 7]")]
        [Description("preceding Sibling or Following Sibling")]
        public InArgument<string> precedingSibling { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            string AppId = ApplicationIDToAttach.Get(context);
            //string Eid = Id.Get(context);
            //string classname = Classname.Get(context);
            //string selector = Selector.Get(context);
            string xp = xpath.Get(context);
            //string linktext = LinkText.Get(context);

            try
            {
                CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                IWebElement Element = null;


                if (xp != null)
                {
                    //*******************to detect iframes***********************

                    if (commonWebBrowser_Current.driver.FindElements(By.XPath(xp)).Count == 0)
                    {
                        int size = commonWebBrowser_Current.driver.FindElements(By.TagName("iframe")).Count;
                        for (int iFrameCounter = 0; iFrameCounter < size; iFrameCounter++)
                        {
                            commonWebBrowser_Current.driver.SwitchTo().Frame(iFrameCounter);
                            if (commonWebBrowser_Current.driver.FindElements(By.XPath(xp)).Count > 0)
                            {
                                Element = commonWebBrowser_Current.driver.FindElement(By.XPath(xp));
                                break;
                            }
                            commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                        }
                    }
                    else
                    {
                        //if no frame exists , normal scenario

                        Element = commonWebBrowser_Current.driver.FindElement(By.XPath(xp));


                    }

                    //***********************************************************

                }


                if (Title.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "')]"));


                }

                else if (Title.Get(context) != null && IdContains.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') ]"));

                }
                else if (Title.Get(context) != null && IdContains.Get(context) != null && IdStartWith.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and starts-with(@id, '" + IdStartWith.Get(context) + "') ]"));

                }

                else if (IdContains.Get(context) == null && IdStartWith.Get(context) == null && ElementClass.Get(context) != null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.ClassName(ElementClass.Get(context)));
                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and contains(@class,'" + ElementClass.Get(context) + "')]"));

                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]"));


                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@id,'" + IdContains.Get(context) + "')]"));


                }

                else if (IdStartWith.Get(context) != null && IdContains.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id,'" + IdStartWith.Get(context) + "')]"));


                }
                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && FollowingSibling.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/following-sibling::" + FollowingSibling.Get(context) + ""));


                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && precedingSibling.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/preceding-sibling::" + precedingSibling.Get(context) + ""));

                }
                //if (Eid != null)
                //{
                //    Element = commonWebBrowser_Current.driver.FindElement(By.Id(Eid));
                //}
                //if (classname != null)
                //{
                //    Element = commonWebBrowser_Current.driver.FindElement(By.ClassName(classname));
                //}
                //if (selector != null)
                //{
                //    Element = commonWebBrowser_Current.driver.FindElement(By.CssSelector(classname));
                //}
                //if (xpath != null)
                //{
                //    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(xpath));
                //}

                //if (linktext != null)
                //{
                //    Element = commonWebBrowser_Current.driver.FindElement(By.LinkText(xpath));
                //}

                Actions actions = new Actions(commonWebBrowser_Current.driver);
                actions.MoveToElement(Element);
                actions.Perform();

            }
            catch (Exception e)
            {
                Log.Logger.LogData(e.Message + " in ScrollWebPage", Logger.LogLevel.Error);
            }
        }
    }
}
