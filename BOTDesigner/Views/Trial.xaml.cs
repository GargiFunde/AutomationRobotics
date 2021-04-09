using System.Windows;
using System.Diagnostics;

namespace BOTDesigner.Views
{
    /// <summary>
    /// Interaction logic for Trial.xaml
    /// </summary>
    public partial class Trial : Window
    {
        //Thread loadingThread;
        //Storyboard Showboard;
        //Storyboard Hideboard;
        //private delegate void ShowDelegate(string txt);
        //private delegate void HideDelegate();
        //ShowDelegate showDelegate;
        //HideDelegate hideDelegate;
        public Trial()
        {
          
            InitializeComponent();
            AddVersionNumber();
            //showDelegate = new ShowDelegate(this.showText);
            //hideDelegate = new HideDelegate(this.hideText);
            //Showboard = this.Resources["showStoryBoard"] as Storyboard;
            //Hideboard = this.Resources["HideStoryBoard"] as Storyboard;

        }

        private void AddVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            VersionNumber.Content += $" v.{ versionInfo.FileVersion }";
        }

        //public double Progress
        //{
        //    get { return progressBar.Value; }
        //    set { progressBar.Value = value; }
        //}


        //private void showText(string txt)
        //{
        //    txtLoading.Text = txt;
        //    BeginStoryboard(Showboard);
        //}
        //private void hideText()
        //{
        //    BeginStoryboard(Hideboard);
        //}
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    loadingThread = new Thread(Load);
        //    loadingThread.Start();
        //}
        //private void Load()
        //{
        //    Thread.Sleep(1000);

        //    this.Dispatcher.Invoke(showDelegate, "first data to loading");
        //    Thread.Sleep(6000);
        //    //load data 
        //    this.Dispatcher.Invoke(hideDelegate);

        //    Thread.Sleep(6000);
        //    this.Dispatcher.Invoke(showDelegate, "second data loading");
        //    Thread.Sleep(6000);
        //    //load data
        //    this.Dispatcher.Invoke(hideDelegate);


        //    Thread.Sleep(6000);
        //    this.Dispatcher.Invoke(showDelegate, "last data loading");
        //    Thread.Sleep(6000);
        //    //load data 
        //    this.Dispatcher.Invoke(hideDelegate);



        //    //close the window
        //    Thread.Sleep(6000);
        //    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { Close(); });
        //}
    }
}
