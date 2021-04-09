///-----------------------------------------------------------------
///   Namespace:      Bot.Activity.Web
///   Class:               WebSlider
///   Description:    <para>Web-Slider Activity </para>
///                         <para> Slider Range Set using Selenium</para>
///                         <para>Will work while setting slider ranges in Web pages.</para>
///   Author:           E2E BOTS Team                   Date: 02 March,2020
///   Notes:            Web-Slider Activity
///   Revision History: 2020.1.2.3
///   Name:           Date:        Description:
///-----------------------------------------------------------------


using System;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.Drawing;

namespace Bot.Activity.Web
{
    [Designer(typeof(WebSlider_ActivityDesigner))]
    [ToolboxBitmap("Resources/WebSlider.png")]
    public class WebSlider : BaseNativeActivity
    {
        public WebSlider()
        {
            SlideWithScale = false;
        }


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

        [RequiredArgument]
        [Category("Common")]
        [Description("Percentage value to slider should be droped at")]
        public InArgument<int> SlideBy { get; set; }

        [DisplayName("Wait Before")]
        [Description("in milliseconds")]
        public InArgument<int> WaitBefore { get; set; }

        [DisplayName("Wait After")]
        [Description("in milliseconds")]
        public InArgument<int> WaitAfter { get; set; }

        //[RequiredArgument]
        //public InArgument<string> TableBody { get; set; }

        // public InArgument<int> TimeOutInSecond { get; set; }
        //  public OutArgument<DataTable> TableResult { get; set; }



        [Category("Slider has no scale")]

        [Description("Enter the range of slider For Example 0:100")]
        public InArgument<string> Range { get; set; }


        [Category("Slider has scale")]
        [Description("Check if the slider has scale")]
        public Boolean SlideWithScale { get; set; }

        public enum Direction
        {
            Left = 0, Right = 1
        }

        [Category("Slider has scale")]
        [DisplayName("Direction")]
        [Description("Select the direction to move the slider")]
        public Direction Dir { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            // Table tableResult = null; 
            string xp = xpath.Get(context);
            string AppId = ApplicationIDToAttach.Get(context);
            int iwaitbefore = WaitBefore.Get(context);
            int iwaitafter = WaitAfter.Get(context);

            //string sControlId = ControlId.Get(context);
            //string sControlName = ControlName.Get(context);
            //string sClassName = ClassName.Get(context);
            //string sCSSSelector = CSSSelector.Get(context);
            //string sXPath = XPath.Get(context);
            int slideBy = SlideBy.Get(context);
            //string tableBody = TableBody.Get(context);
            // int timeInSec = TimeOutInSecond.Get(context);
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {
                        IWebElement Element = null;
                        if (iwaitbefore > 0)
                        {
                            Thread.Sleep(iwaitbefore);
                        }
                        CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];


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

                        else if (IdContains.Get(context) == null && IdStartWith.Get(context) == null && ElementClass.Get(context) != null &&  FollowingSibling.Get(context) == null && precedingSibling.Get(context) == null)                 
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
                       //if ((sControlId != string.Empty) && (sControlId != null) && (sControlId.Trim().Length > 0))
                        //{
                        //    if (sControlId.Contains(" "))
                        //    {
                        //        sControlId = "." + sControlId.Replace(" ", ".");
                        //        // IWebElement element = ie.driver.FindElementByClassName(strClassName);
                        //        element = commonWebBrowser_Current.driver.FindElement(By.CssSelector(sControlId));
                        //    }
                        //    else
                        //    {
                        //        element = commonWebBrowser_Current.driver.FindElementById(sControlId);
                        //    }
                        //}
                        //else if ((!string.IsNullOrEmpty(sXPath)) && (sXPath.Trim().Length > 0))
                        //{
                        //    element = commonWebBrowser_Current.driver.FindElementByXPath(sXPath);
                        //}
                        //else if (!string.IsNullOrEmpty(sCSSSelector) && (sCSSSelector.Trim().Length > 0))
                        //{
                        //    element = commonWebBrowser_Current.driver.FindElementByCssSelector(sCSSSelector);
                        //}
                        //else if (!string.IsNullOrEmpty(sControlName) && (sControlName.Trim().Length > 0))
                        //{
                        //    if (sControlName.Contains(" "))
                        //    {
                        //        sControlName = "." + sControlName.Replace(" ", ".");
                        //        // IWebElement element = ie.driver.FindElementByClassName(strClassName);
                        //        element = commonWebBrowser_Current.driver.FindElementByCssSelector(sControlName);
                        //    }
                        //    else
                        //    {
                        //        element = commonWebBrowser_Current.driver.FindElementByName(sControlName);
                        //    }
                        //}

                        //else if (!string.IsNullOrEmpty(sClassName) && (sClassName.Trim().Length > 0))
                        //{
                        //    if (sClassName.Contains(" "))
                        //    {
                        //        sClassName = "." + sClassName.Replace(" ", ".");
                        //        // IWebElement element = ie.driver.FindElementByClassName(strClassName);
                        //        element = commonWebBrowser_Current.driver.FindElementByCssSelector(sClassName);

                        //    }
                        //    else
                        //    {
                        //        element = commonWebBrowser_Current.driver.FindElementByClassName(sClassName);
                        //    }
                        //}


                        if (SlideWithScale == false)
                        {
                            if (Element != null)
                            {

                                string temp = Range.Get(context);
                                char[] a = { ':', ';' };
                                string[] temp2 = temp.Split(a[0]);
                                Decimal val1 = Convert.ToDecimal(temp2[0], null);
                                Decimal val2 = Convert.ToDecimal(temp2[1], null);
                                //  element = commonWebBrowser_Current.driver.FindElement(By.CssSelector("input[type='range']"));
                                //((IJavaScriptExecutor)commonWebBrowser_Current.driver).ExecuteScript("windows.val(" + slideBy + ").change();",element);
                                Actions SliderAction = new Actions(commonWebBrowser_Current.driver);
                                int PixelsToMove = GetPixelsToMove(Element, slideBy, val2, val1);
                                SliderAction.ClickAndHold(Element).MoveByOffset((-(int)Element.Size.Width / 2), 0).MoveByOffset(PixelsToMove, 0).Release().Perform();
                            }
                        }

                        if (SlideWithScale == true)
                        {
                            if (Element != null)
                            {
                                switch (Dir)
                                {
                                    case Direction.Right:
                                        for (int i = 1; i <= slideBy; i++)
                                        {
                                            Element.SendKeys(Keys.ArrowRight);
                                        }
                                        break;

                                    case Direction.Left:
                                        for (int i = 1; i <= slideBy; i++)
                                        {
                                            Element.SendKeys(Keys.ArrowLeft);
                                        }
                                        break;
                                }

                            }
                        }
                        if (iwaitafter > 0)
                        {
                            Thread.Sleep(iwaitafter);
                        }
                    }

                }
                else
                {
                    Logger.Log.Logger.LogData("Please attach Application Id", Logger.LogLevel.Error);
                }

            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in Web activity Slider", Logger.LogLevel.Error);

                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

        public static int GetPixelsToMove(IWebElement Slider, decimal Amount, decimal SliderMax, decimal SliderMin)
        {
            int pixels = 0;
            decimal tempPixels = Slider.Size.Width;
            tempPixels = tempPixels / (SliderMax - SliderMin);
            tempPixels = tempPixels * (Amount - SliderMin);
            pixels = Convert.ToInt32(Decimal.Truncate(tempPixels));
            Logger.Log.Logger.LogData("Pixel is =" + pixels, Logger.LogLevel.Info);
            return pixels;
        }
    }
}
