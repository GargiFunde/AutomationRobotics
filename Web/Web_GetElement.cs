using CommonLibrary;
using Logger;
using OpenQA.Selenium;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;

namespace Bot.Activity.Web
{
    [Designer(typeof(Web_GetElement_ActivityDesigner))]
    [ToolboxBitmap("Resources/WebGetElement.png")]
    public class Web_GetElement : BaseNativeActivity
    {
        [Category("Input")]
        [DisplayName("Xpath [priority 1]")]
        [Description("Get the Xpath ")]
        public InArgument<string> xpath { get; set; }

        [Category("Input Paramaters")]
        [RequiredArgument]
        [DisplayName("Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

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

        //[Category("Input")]
        //[DisplayName("Contains Text [priority 7]")]
        //[Description("Element Contains Text")]
        //public InArgument<string> ContainsText { get; set; }

       


        [Category("Output Paramaters")]
        [DisplayName("IWebElementObject")]
        public OutArgument<IWebElement> IWE { get; set; }

        [Category("Options")]
        [DisplayName("Click")]
        public Boolean click { get; set; }

       
        public Web_GetElement()
        {
            click = false;

        }

        protected override void Execute(NativeActivityContext context)
        {

            string xp = xpath.Get(context);
            string AppId = ApplicationIDToAttach.Get(context);
            //string Att = Attribute.Get(context);
            //string Tag = Tagname.Get(context);
            //string Avalue = AV.Get(context);
            //string text = Text.Get(context);

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
                                goto done;
                            }
                            commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                        }
                    }
                    else
                    {
                        //if no frame exists , normal scenario

                        Element = commonWebBrowser_Current.driver.FindElement(By.XPath(xp));
                        goto done;

                    }

                    //***********************************************************

                }
                if (Title.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "')]"));
                    goto done;

                }

                else if (Title.Get(context) != null && IdContains.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') ]"));
                    goto done;
                }
                else if (Title.Get(context) != null && IdContains.Get(context) != null && IdStartWith.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and starts-with(@id, '" + IdStartWith.Get(context) + "') ]"));
                    goto done;
                }
                else if (IdContains.Get(context) == null && IdStartWith.Get(context) == null && ElementClass.Get(context) != null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.ClassName(ElementClass.Get(context)));
                    goto done;
                }
                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and contains(@class,'" + ElementClass.Get(context) + "')]"));
                    goto done;
                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]"));
                    goto done;

                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@id,'" + IdContains.Get(context) + "')]"));
                    goto done;

                }

                else if (IdStartWith.Get(context) != null && IdContains.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id,'" + IdStartWith.Get(context) + "')]"));
                    goto done;

                }
                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && FollowingSibling.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/following-sibling::" + FollowingSibling.Get(context) + ""));
                    goto done;

                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && precedingSibling.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/preceding-sibling::" + precedingSibling.Get(context) + ""));
                    goto done;
                }


                //if (xp != null)
                //{
                //    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(xp));
                //    goto done;
                //}
                //if (text != null)
                //{
                //    Element = commonWebBrowser_Current.driver.FindElement(By.XPath("//" + Tag + "[contains(@" + Att + ", '" + Avalue + "') and text()='" + text + "']"));
                //}

                //if (text == null)
                //{
                //    Element = commonWebBrowser_Current.driver.FindElement(By.XPath("//" + Tag + "[contains(@" + Att + ", '" + Avalue + "')]"));
                //}

            done:
                if (click == true)
                {
                    Element.Click();
                }

                IWE.Set(context, Element);
            }
            catch (Exception e)
            {
                Log.Logger.LogData("Error in activity Web_GetElement" + e.Message, Logger.LogLevel.Error);
            }
        }
    }
}

