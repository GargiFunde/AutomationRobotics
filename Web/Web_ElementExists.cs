using CommonLibrary;
using Logger;
using OpenQA.Selenium;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;

namespace Bot.Activity.Web
{
    [Designer(typeof(Web_ElementExists_ActivityDesigner))]
    [ToolboxBitmap("Resources/WebElementExists.png")]

    public class Web_ElementExists : BaseNativeActivity
    {

        [Category("Input")]
        [DisplayName("Xpath")]
        [Description("Get the Xpath ")]
        public InArgument<string> xpath { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Element Exists")]
        [Description("Get Element Exists")]
        public OutArgument<bool> ElementExists { get; set; }


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
        [DisplayName("Following Sibling [priority 5]")]
        [Description("Following Sibling or preceding Sibling")]
        public InArgument<string> FollowingSibling { get; set; }

        [Category("Input")]
        [DisplayName("preceding Sibling [priority 6]")]
        [Description("preceding Sibling or Following Sibling")]
        public InArgument<string> precedingSibling { get; set; }

        [Category("Input")]
        [DisplayName("Contains Text [priority 7]")]
        [Description("Element Contains Text")]
        public InArgument<string> ContainsText { get; set; }

        [Category("Input")]
        [DisplayName("Title [priority 8]")]
        [Description("Element Title Text")]
        public InArgument<string> Title { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            string xp = string.Empty;
            xp = xpath.Get(context);
            string AppId = string.Empty;
            AppId = ApplicationIDToAttach.Get(context);
            try
            {

                CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                IWebElement Element = null;
                bool exists = false;

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
                                //  Console.WriteLine("***************found the element in iframe:" + iFrameCounter.ToString());
                                // perform the actions on element here

                                Element = commonWebBrowser_Current.driver.FindElement(By.XPath(xp));
                                if (Element != null)
                                {
                                    exists = true;
                                    goto done;

                                }
                                else
                                {
                                    Log.Logger.LogData("The element \"" + xp + "\" does not exist in activity Web_ElementExists", Logger.LogLevel.Info);
                                }



                            }
                            commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                        }
                    }
                    else
                    {
                        //if no frame exists , normal scenario

                        Element = commonWebBrowser_Current.driver.FindElement(By.XPath(xp));
                        if (Element != null)
                        {
                            exists = true;
                            goto done;

                        }
                        else
                        {
                            Log.Logger.LogData("The element \"" + xp + "\" does not exist in activity Web_ElementExists", Logger.LogLevel.Info);
                        }



                    }

                    //***********************************************************

                }

                if (Title.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "')]"));
                    if (Element != null)
                    {
                        exists = true;
                        goto done;

                    }
                    else
                    {
                        Log.Logger.LogData("The element \"" + Title + "\" does not exist in activity Web_ElementExists", Logger.LogLevel.Info);
                    }
                }

                else if (Title.Get(context) != null && IdContains.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') ]"));
                    if (Element != null)
                    {
                        exists = true;
                        goto done;

                    }
                    else
                    {
                        Log.Logger.LogData("The element with title \"" + Title.Get(context) + " & idContains" + IdContains.Get(context) + "\" does not exist in activity Web_ElementExists", Logger.LogLevel.Info);
                    }
                }
                else if (Title.Get(context) != null && IdContains.Get(context) != null && IdStartWith.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and starts-with(@id, '" + IdStartWith.Get(context) + "') ]"));
                    if (Element != null)
                    {
                        exists = true;
                        goto done;

                    }
                    else
                    {
                        Log.Logger.LogData("The element with title \"" + Title.Get(context) + " & idContains" + IdContains.Get(context) + " & idStartWith " + IdStartWith + "\" does not exist in activity Web_ElementExists", Logger.LogLevel.Info);
                    }
                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and contains(@class,'" + ElementClass.Get(context) + "')]"));

                    if (Element != null)
                    {
                        exists = true;
                        goto done;

                    }
                    else
                    {
                        Log.Logger.LogData("The element with IdContains \"" + IdContains.Get(context) + " & IdStartWith" + IdStartWith.Get(context) + " & ElementClass " + ElementClass.Get(context) + "\" does not exist in activity Web_ElementExists", Logger.LogLevel.Info);
                    }
                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]"));
                    if (Element != null)
                    {
                        exists = true;
                        goto done;

                    }
                    else
                    {
                        Log.Logger.LogData("The element with IdContains \"" + IdContains.Get(context) + " & IdStartWith" + IdStartWith.Get(context) + "\" does not exist in activity Web_ElementExists", Logger.LogLevel.Info);
                    }

                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@id,'" + IdContains.Get(context) + "')]"));
                    if (Element != null)
                    {
                        exists = true;
                        goto done;

                    }
                    else
                    {
                        Log.Logger.LogData("The element with IdContains \"" + IdContains.Get(context) + "\" does not exist in activity Web_ElementExists", Logger.LogLevel.Info);
                    }


                }

                else if (IdStartWith.Get(context) != null && IdContains.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id,'" + IdStartWith.Get(context) + "')]"));
                    if (Element != null)
                    {
                        exists = true;
                        goto done;

                    }
                    else
                    {
                        Log.Logger.LogData("The element with IdStartWith \"" + IdStartWith.Get(context) + "\" does not exist in activity Web_ElementExists", Logger.LogLevel.Info);
                    }


                }
                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && FollowingSibling.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/following-sibling::" + FollowingSibling.Get(context) + ""));
                    if (Element != null)
                    {
                        exists = true;
                        goto done;

                    }
                    else
                    {
                        Log.Logger.LogData("The element with IdContains \"" + IdContains.Get(context) + " & IdStartWith" + IdStartWith.Get(context) + "\" does not exist in activity Web_ElementExists", Logger.LogLevel.Info);
                    }


                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && precedingSibling.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/preceding-sibling::" + precedingSibling.Get(context) + ""));
                    if (Element != null)
                    {
                        exists = true;
                        goto done;

                    }
                    else
                    {
                        Log.Logger.LogData("The element with IdContains \"" + IdContains.Get(context) + " & IdStartWith" + IdStartWith.Get(context) + " & precedingSibling" + precedingSibling.Get(context) + "\" does not exist in activity Web_ElementExists", Logger.LogLevel.Info);
                    }
                }
            done:
                ElementExists.Set(context, exists);
            }
            catch (Exception e)
            {
                Log.Logger.LogData("Exception is" + e.Message, Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }

        }

    }
}
