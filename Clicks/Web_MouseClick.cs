using Bot.Activity.Web;
using CommonLibrary;
using Logger;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicks
{
    public enum Click
    {
        Single = 1, Double = 2,Right=3
    }

    [ToolboxBitmap("Resources/Web_MouseClick.png")]
    [Designer(typeof(Web_MouseClick_ActivityDesigner))]
    public class Web_MouseClick : BaseNativeActivity
    {


        [Category("Input")]
        [DisplayName("Xpath")]
        [Description("Get the Xpath ")]
        public InArgument<string> xpath { get; set; }

        [Category("Input")]
        [DisplayName("Click")]
        [Description("Select Single or Double Click")]
        public Click Clk { get; set; }


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



        protected override void Execute(NativeActivityContext context)
        {

            string xp = string.Empty;
            xp = xpath.Get(context);
            string AppId = string.Empty;
            AppId = ApplicationIDToAttach.Get(context);
            try
            {

                CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                Actions act = new Actions(commonWebBrowser_Current.driver);
                IWebElement Element = null;

                if (xp != null)
                {
                    //*******************to detect iframes***********************
                    if (commonWebBrowser_Current.driver.FindElements(By.XPath(xp)).Count == 0)
                    {
                        int size = commonWebBrowser_Current.driver.FindElements(By.TagName("iframe")).Count;
                        for (int iFrameCounter = 0; iFrameCounter < size; iFrameCounter++)
                        {
                            try
                            {
                                commonWebBrowser_Current.driver.SwitchTo().Frame(iFrameCounter);
                            }
                            catch (NoSuchFrameException)
                            {

                            }
                            if (commonWebBrowser_Current.driver.FindElements(By.XPath(xp)).Count > 0)
                            {
                                //  Console.WriteLine("***************found the element in iframe:" + iFrameCounter.ToString());
                                // perform the actions on element here

                                Element = commonWebBrowser_Current.driver.FindElement(By.XPath(xp));
                                act.MoveToElement(Element);

                                switch (Clk)
                                {
                                    case Click.Single:
                                        try
                                        {
                                            Element.Click();
                                        }
                                        catch(ElementNotVisibleException)
                                        { }
                                        catch(StaleElementReferenceException)
                                        { }
                                        break;

                                    case Click.Double:
                                        act.DoubleClick(Element).Perform();
                                        break;

                                    case Click.Right:
                                        act.ContextClick(Element).Perform();
                                        break;
                                    default: break;

                                }

                            }
                            commonWebBrowser_Current.driver.SwitchTo().DefaultContent();
                        }
                    }
                    else
                    {
                        //if no frame exists , normal scenario
                        //Element= new WebDriverWait(commonWebBrowser_Current.driver, TimeSpan.FromSeconds(20)).Until(ExpectedConditions.ElementToBeClickable(By.XPath("//select[@class='super-attribute-select' and starts-with(@id, 'attribute')]")));
                        Element = commonWebBrowser_Current.driver.FindElement(By.XPath(xp));
                        act.MoveToElement(Element);

                        switch (Clk)
                        {
                            case Click.Single:
                                try
                                {

                                 
                                              
                                    Element.Click();
                                }
                                catch (ElementNotVisibleException)
                                {

                                }
                                catch (StaleElementReferenceException)
                                {

                                }
                                catch (Exception ex)
                                {

                                }
                         
                                break;

                            case Click.Double:
                                act.DoubleClick(Element).Perform();
                                break;

                            case Click.Right:
                                act.ContextClick(Element).Perform();
                                break;
                            default: break;

                        }

                    }

                    //***********************************************************

                }

                if (Title.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "')]"));
                    act.MoveToElement(Element);

                    switch (Clk)
                    {
                        case Click.Single:
                            Element.Click();
                            break;

                        case Click.Double:
                            act.DoubleClick(Element).Perform();
                            break;

                        case Click.Right:
                            act.ContextClick(Element).Perform();
                            break;
                        default:

                            break;
                    }
                }

                else if (Title.Get(context) != null && IdContains.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') ]"));
                    act.MoveToElement(Element);

                    switch (Clk)
                    {
                        case Click.Single:
                            Element.Click();
                            break;

                        case Click.Double:
                            act.DoubleClick(Element).Perform();
                            break;

                        case Click.Right:
                            act.ContextClick(Element).Perform();
                            break;
                        default:

                            break;
                    }
                }
                else if (Title.Get(context) != null && IdContains.Get(context) != null && IdStartWith.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@title, '" + Title.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and starts-with(@id, '" + IdStartWith.Get(context) + "') ]"));
                    act.MoveToElement(Element);

                    switch (Clk)
                    {
                        case Click.Single:
                            Element.Click();
                            break;

                        case Click.Double:
                            act.DoubleClick(Element).Perform();
                            break;

                        case Click.Right:
                            act.ContextClick(Element).Perform();
                            break;
                        default:

                            break;
                    }
                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "') and contains(@class,'" + ElementClass.Get(context) + "')]"));
                    act.MoveToElement(Element);

                    switch (Clk)
                    {
                        case Click.Single:
                            Element.Click();
                            break;

                        case Click.Double:
                            act.DoubleClick(Element).Perform();
                            break;

                        case Click.Right:
                            act.ContextClick(Element).Perform();
                            break;
                        default:

                            break;

                    }

                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && ElementClass.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]"));
                    act.MoveToElement(Element);

                    switch (Clk)
                    {
                        case Click.Single:
                            Element.Click();
                            break;

                        case Click.Double:
                            act.DoubleClick(Element).Perform();
                            break;

                        case Click.Right:
                            act.ContextClick(Element).Perform();
                            break;
                        default: break;

                    }

                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[contains(@id,'" + IdContains.Get(context) + "')]"));
                    act.MoveToElement(Element);

                    switch (Clk)
                    {
                        case Click.Single:
                            Element.Click();
                            break;

                        case Click.Double:
                            act.DoubleClick(Element).Perform();
                            break;

                        case Click.Right:
                            act.ContextClick(Element).Perform();
                            break;
                        default: break;

                    }


                }

                else if (IdStartWith.Get(context) != null && IdContains.Get(context) == null && FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id,'" + IdStartWith.Get(context) + "')]"));
                    act.MoveToElement(Element);

                    switch (Clk)
                    {
                        case Click.Single:
                            Element.Click();
                            break;

                        case Click.Double:
                            act.DoubleClick(Element).Perform();
                            break;

                        case Click.Right:
                            act.ContextClick(Element).Perform();
                            break;
                        default: break;

                    }


                }
                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && FollowingSibling.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/following-sibling::" + FollowingSibling.Get(context) + ""));
                    act.MoveToElement(Element);

                    switch (Clk)
                    {
                        case Click.Single:
                            Element.Click();
                            break;

                        case Click.Double:
                            act.DoubleClick(Element).Perform();
                            break;

                        case Click.Right:
                            act.ContextClick(Element).Perform();
                            break;
                        default: break;

                    }


                }

                else if (IdContains.Get(context) != null && IdStartWith.Get(context) != null && precedingSibling.Get(context) != null)
                {
                    Element = commonWebBrowser_Current.driver.FindElement(By.XPath(".//*[starts-with(@id, '" + IdStartWith.Get(context) + "') and contains(@id,'" + IdContains.Get(context) + "')]/preceding-sibling::" + precedingSibling.Get(context) + ""));
                    act.MoveToElement(Element);

                    switch (Clk)
                    {
                        case Click.Single:
                            Element.Click();
                            break;

                        case Click.Double:
                            act.DoubleClick(Element).Perform();
                            break;

                        case Click.Right:
                            act.ContextClick(Element).Perform();
                            break;
                        default: break;

                    }
                }

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
