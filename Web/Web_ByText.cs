using CommonLibrary;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;

namespace Bot.Activity.Web
{
    public class Web_ByText : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [RequiredArgument]
        [DisplayName("Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        public InArgument<IWebElement> IWebEle { get; set; }

        [DisplayName("Text")]
        public InArgument<string> Text { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            string text = Text.Get(context);
            IWebElement iWebEle = IWebEle.Get(context);

            string AppId = ApplicationIDToAttach.Get(context);
            CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];


          //  iWebEle.Click();
            var CN = iWebEle.Location;
            var po = iWebEle.GetAttribute("offsetTop");

          
            //  Actions action = new Actions(commonWebBrowser_Current.driver);

            // new Actions(commonWebBrowser_Current.driver).MoveToElement(iWebEle).MoveByOffset(452,114).Click().Perform();
            new Actions(commonWebBrowser_Current.driver).MoveByOffset(CN.X, CN.Y).Click().Build().Perform();



        }
    }
}
