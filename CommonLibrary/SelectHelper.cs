using System;
using System.Activities;
using System.Activities.Presentation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows.Controls;

namespace CommonLibrary
{
    public static class SelectHelper
    {
        public static string CurrentExecutablepath = string.Empty;
        public static int count = 1;
        public static Dictionary<string, object> MyProperty { get; set; }
        // public static Visibility ScrapWindowVisibility { get; set; }
        public static bool StartSimulation = false;
        public static Dictionary<string, WorkflowDesigner> WorkflowDictionary = new Dictionary<string, WorkflowDesigner>();
        public static Dictionary<string, object> WorkflowEntityDictionary = new Dictionary<string, object>();
        public static Dictionary<string, RuntimeApplicationHelper> RuntimeApplicationHelperDictionary = new Dictionary<string, RuntimeApplicationHelper>();
        public static RuntimeApplicationHelper CurrentRuntimeApplicationHelper = null;
        public static WorkflowDesigner _wfDesigner { get; set; }
        public static WorkflowApplication _wfApplication { get; set; }
        public static string _currentworkflowfile { get; set; } //use context.abort to abort execution
        public static string _currentscrapfile { get; set; } //use context.abort to abort execution
        public static System.Timers.Timer _timerExecution { get; set; }
        public static Border _wfPropertyBorder { get; set; }
        public static Border _wfOutlineBorder { get; set; }
        public static object CurrentPlugin;
        public static string CurrentProcessName;
        public static int ApplicationProcessId;
        public static Collection<System.Activities.Activity> CurrentPluginScrapeProperties;
        public static System.Windows.Controls.Border Border { get; set; }
        public static string ProjectLocation { get; set; }
        public static string ScrapingWindowTitle = null;

        public static object scrapingWindow = null;
        public static object CurrentScrapControl { get; set; }
        public static bool DialogWacher { get; set; }

        //  public static Window ScrapingWindow1 = null;

        public static ScrapMode CurrentScrapMode = ScrapMode.None;

        static SelectHelper()
        {
            MyProperty = new Dictionary<string, object>();
            CurrentExecutablepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        public static object _automationElementTree;
        public static object visualDesignerHelper;

        public static event EventHandler<OpenXamlFileEventArgs> OpenXamlFile;
        public static void OnOpenXamlFile(OpenXamlFileEventArgs e)
        {
            EventHandler<OpenXamlFileEventArgs> handler = OpenXamlFile;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        public static event EventHandler<LaunchScrapingEventArgs> LaunchScraping;
        public static void OnLaunchScraping(LaunchScrapingEventArgs e)
        {
            EventHandler<LaunchScrapingEventArgs> handler = LaunchScraping;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        public static event EventHandler<ResetEventArgs> Reset;
        public static void OnReset(ResetEventArgs e)
        {
            EventHandler<ResetEventArgs> handler = Reset;
            if (handler != null)
            {
                handler(null, e);
            }
        }


        public static event EventHandler<ScrapingEventArgs> Scraping;
        public static void OnScraping(ScrapingEventArgs e)
        {
            EventHandler<ScrapingEventArgs> handler = Scraping;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        public static event EventHandler<SaveFieldEventArgs> SaveFieldOperations;
        public static void OnSaveFieldOperations(SaveFieldEventArgs e)
        {
            EventHandler<SaveFieldEventArgs> handler = SaveFieldOperations;
            if (handler != null)
            {
                handler(null, e);
            }
        }



        public static event EventHandler<IECreatedEventArgs> IECreated;
        public static void OnIECreated(IECreatedEventArgs e)
        {
            EventHandler<IECreatedEventArgs> handler = IECreated;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        //public static event EventHandler<ImageCaptureEventArgs> ImageCapture;
        //public static void OnImageCapture(ImageCaptureEventArgs e)
        //{
        //    EventHandler<ImageCaptureEventArgs> handler = ImageCapture;
        //    if (handler != null)
        //    {
        //        handler(null, e);
        //    }
        //}
    }
    //public class ImageCaptureEventArgs : EventArgs
    //{
    //    public int Left;
    //    public int Top;
    //}
    public class OpenXamlFileEventArgs : EventArgs
    {
        public string XamlFileName;
        public string XamlFileNameWithPath;
    }

    public class LaunchScrapingEventArgs : EventArgs
    {
        public string ApplicationID;
        public string UniqueActivityId;
        public string strTitleOrUrlToAttach;
        public string strWaitUntilContainText;
    }

    public class ResetEventArgs : EventArgs
    {
        public string ApplicationID;
        public string UniqueActivityId;
    }

    public class ScrapingEventArgs : EventArgs
    {
        public Collection<System.Activities.Activity> ActivityCollection;
        public int Type = 0; // Type = 1 means make ScrapWindow invisible
        //  public string XamlFileNameWithPath;
    }
    public class SaveFieldEventArgs : EventArgs
    {
        public int iSaveFieldOperation;
        public string strUniqueId = string.Empty;
        public Activity activity { get; set; }
        //  public string XamlFileNameWithPath;
    }
    public class IECreatedEventArgs : EventArgs
    {
        SHDocVw.InternetExplorer iexplorer;
        public int Type = 0;
        public string ApplicationId = string.Empty;
        public string searchUrl = string.Empty;
        public IECreatedEventArgs(SHDocVw.InternetExplorer internetExplorer)
        {
            iexplorer = internetExplorer;
        }

        public SHDocVw.InternetExplorer ie
        {
            get { return this.iexplorer; }
        }

    }

    public enum ScrapMode
    {
        None = 0,
        Web = 1,
        Windows = 2,
        SAP = 3,
        Mainframe = 4
    }
}

