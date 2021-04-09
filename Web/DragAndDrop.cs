///-----------------------------------------------------------------
///   Namespace:      Bot.Activity.Web
///   Class:               DragAndDrop
///   Description:    <para>Drag And Drop Activity </para>
///                         <para> Drag and Drop Activity using X-Path and Offset.</para>
///   Author:           E2E BOTS Team                   Date: 24 March,2020
///   Notes:            Web-Slider Activity
///   Revision History: 2020.1.2.3
///   Name:           Date:        Description:
///-----------------------------------------------------------------

#region Headers
using CommonLibrary;
using Logger;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace Bot.Activity.Web
{
    [Designer(typeof(DragAndDrop_ActivityDesigner))]
    [ToolboxBitmap("Icons/Web/draganddrop.png")]
    #region class DragAndDrop
    public class DragAndDrop : BaseNativeActivity
    {
        //Constructor
        DragAndDrop()
        {
            Offset = false;
        }

        #region Input and Output Parameters
        [Category("Input Paramaters")]
        [RequiredArgument]
        [DisplayName("Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Source XPath")]
        public InArgument<string> Sourcexpath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Destination XPath")]
        public InArgument<string> Destinationxpath { get; set; }


        [Category("Input Paramaters")]
        [DisplayName("X Offset")]
        public InArgument<int> X { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Y Offset")]
        public InArgument<int> Y { get; set; }

        [Category("Options")]
        [DisplayName("Use Offsets")]
        [Description("Check this if you want to drag to a location with known x and y coordinates")]
        public Boolean Offset { get; set; }
        #endregion

        #region Execute Method
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string sxp = Sourcexpath.Get(context);
                string dxp = Destinationxpath.Get(context);
                string AppId = ApplicationIDToAttach.Get(context);
                int x = X.Get(context);
                int y = Y.Get(context);

                CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

                IWebElement SourceElement = null;
                IWebElement DestinationElement = null;
                if (Offset == true)
                {
                    SourceElement = commonWebBrowser_Current.driver.FindElement(By.XPath(sxp));
                    Actions act = new Actions(commonWebBrowser_Current.driver);
                    act.DragAndDropToOffset(SourceElement, x, y).Build().Perform();
                }

                if (Offset == false)
                {
                    SourceElement = commonWebBrowser_Current.driver.FindElement(By.XPath(sxp));
                    DestinationElement = commonWebBrowser_Current.driver.FindElement(By.XPath(dxp));
                    Actions act = new Actions(commonWebBrowser_Current.driver);
                    act.DragAndDrop(SourceElement, DestinationElement).Build().Perform();

                }
            }
            catch (NullReferenceException e)
            {
                Log.Logger.LogData("NullReferenceException Error in activity Web_DragandDrop " + e.Message, Logger.LogLevel.Error);
            }
            catch (OverflowException e)
            {
                Log.Logger.LogData("OverflowException Error in activity Web_DragandDrop " + e.Message, Logger.LogLevel.Error);
            }
            catch (Exception e)
            {
                Log.Logger.LogData("Error in activity Web_DragandDrop " + e.Message, Logger.LogLevel.Error);
            }
        }
        #endregion
    }
    #endregion
}
