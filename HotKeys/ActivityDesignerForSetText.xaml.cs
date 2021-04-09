using Bot.Activity.ActivityLibrary;


using System.Windows;
using CommonLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Logger;
using System.Collections.ObjectModel;

namespace HotKeys
{
    // Interaction logic for ActivityDesignerForSetText.xaml
    public partial class ActivityDesignerForSetText
    {
        public ActivityDesignerForSetText()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ScrapingWindow scrapingWindow = null;
            if (SelectHelper.scrapingWindow != null)
            {
                scrapingWindow = (ScrapingWindow)SelectHelper.scrapingWindow;
                if (scrapingWindow.Visibility == Visibility.Collapsed)
                {
                    scrapingWindow.Visibility = Visibility.Visible;
                }
            }
            if (SelectHelper.scrapingWindow == null)
            {
                scrapingWindow = null;
                //  string UniqueActivityId = (string)btnIdentifyActivity.Tag;
                SelectHelper._currentscrapfile = SelectHelper._currentworkflowfile;
                Collection<System.Activities.Activity> Activities = null;
                string LaunchUrl = string.Empty;
                string ApplicationId = string.Empty;
                SetText owa = null;
                owa = (SetText)this.ModelItem.GetCurrentValue();

                if ((owa.ApplicationIDToAttach == null))
                {
                    Logger.Log.Logger.LogData("Please Enter ApplicationID To Attach or URL/Titile To Attach", Logger.LogLevel.Error);
                    return;
                }
                //string UniqueId = owa.strUniqueControlld;
                //if (UniqueId != null)
                //{
                //    if (!SelectHelper.MyProperty.ContainsKey(UniqueId)) //require when application will get restart
                //        SelectHelper.MyProperty.Add(UniqueId, owa.Activities);

                //    Activities = owa.Activities;
                //}
                //if (Activities == null)
                //{

                //    owa.strUniqueControlld = DateTime.Now.ToString("yyMMddHHmmss");
                //    owa.Activities = new Collection<System.Activities.Activity>();
                //    SelectHelper.MyProperty.Add(owa.strUniqueControlld, owa.Activities);
                //}
                //SelectHelper.CurrentPluginScrapeProperties = owa.Activities;
                LaunchScrapingEventArgs launchScrapingEventArgs = new LaunchScrapingEventArgs();
                //System.Activities.Presentation.Model.ModelItem mi = (System.Activities.Presentation.Model.ModelItem)WFItemsPresenter.Tag;
                //System.Activities.InArgument inarg = (System.Activities.InArgument)mi.GetCurrentValue();
                if (owa.ApplicationIDToAttach != null)
                {
                    launchScrapingEventArgs.ApplicationID = owa.ApplicationIDToAttach.Expression.ToString();// inarg.Expression.ToString();
                }
                //if (owa.TitleOrUrlToAttach != null)
                //{
                //    launchScrapingEventArgs.strTitleOrUrlToAttach = owa.TitleOrUrlToAttach.Expression.ToString();
                //}

                //launchScrapingEventArgs.UniqueActivityId = owa.strUniqueControlld;

                try
                {
                    if (SelectHelper.WorkflowDictionary.ContainsKey(SelectHelper._currentworkflowfile))
                    {
                        SelectHelper.WorkflowDictionary[SelectHelper._currentworkflowfile].Save(SelectHelper._currentworkflowfile);
                        //SelectHelper.WorkflowDictionary[currentFile] = _wfDesigner;
                    }
                    //CustomWfDesigner.Instance.Save(_currentWorkflowFile);
                }
                catch (Exception ex)
                {
                    Logger.Log.Logger.LogData(ex.Message, LogLevel.Error);
                }

                GC.Collect(0);
                SelectHelper.OnLaunchScraping(launchScrapingEventArgs);

                scrapingWindow = new ScrapingWindow();

                scrapingWindow.ScrapingWindowInitialize(owa.ApplicationIDToAttach.Expression.ToString());

                scrapingWindow.Show();
            }

        }
    }
}
