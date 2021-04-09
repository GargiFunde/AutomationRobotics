using Bot.Activity.Web;
using CommonLibrary;
using CommonLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bot.Activity.Web
{
    /// <summary>
    /// Interaction logic for XPathWebWindow.xaml
    /// </summary>
    public partial class XPathWebWindow : Window
    {
        Web explorer = null;
        public static XPathWebWindow Webwin = null;
       
        public XPathWebWindow()
        {
            InitializeComponent();
            explorer = Web.Instance;
            Webwin = this;
        }

         void GoXPathWorkThread(string browser , string url)
        {

            try
            {
                explorer.OpenBrowserXPath(browser, url);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Go Thread - " + ex + "\n");

            }
        }
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = Txt_Url.Text;
                string browser = Select_Browser.Text;
                Thread GoThread = new Thread(delegate() { GoXPathWorkThread(browser , url); });
                GoThread.IsBackground = true;
                GoThread.Start();
           
          
        }
            catch (Exception exc)
            {
                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Go Button - " + exc + "\n");
            }
        }

        //void IndicateElemWorkThread(string btncontent)
        //{

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}

       void Work1(object sender, DoWorkEventArgs e)
        {
            explorer.StartScrapingforXPath("Browser");
        }
        void Work1Completed(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void IndicateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //BackgroundWorker worker = new BackgroundWorker();
                //worker.DoWork += Work1;
                //worker.RunWorkerCompleted += Work1Completed;
                //worker.RunWorkerAsync();
                
                if (btnSimulation.Content.ToString() == "Indicate Element")
                {
                    //Thread IndicateThread = new Thread(delegate () { explorer.StartScrapingforXPath("Browser"); });
                    //IndicateThread.IsBackground = true;
                    //IndicateThread.Start();
                    //BackgroundWorker worker = new BackgroundWorker();
                    //worker.DoWork += Work1;
                    //worker.RunWorkerCompleted += Work1Completed;
                    //worker.RunWorkerAsync();
                    explorer.StartScrapingforXPath("Browser");
                    btnSimulation.Content = "Stop";

                }
                else
                {
                    //Thread IndicateThread = new Thread(delegate () { explorer.StopScrapingXPath("Browser"); });
                    //IndicateThread.IsBackground = true;
                    //IndicateThread.Start();
                    explorer.StopScrapingXPath("Browser");
                    btnSimulation.Content = "Indicate Element";
                }

                
            }
            catch (Exception exc)
            {
                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Indicate Button - " + exc + "\n");
            }
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string outxpath = string.Empty;
                
                TextRange textRange = new TextRange(
        // TextPointer to the start of content in the RichTextBox.
         Rtxt_Xpath.Document.ContentStart,
         // TextPointer to the end of content in the RichTextBox.
         Rtxt_Xpath.Document.ContentEnd
    );
                outxpath = textRange.Text;
                Clipboard.SetText(outxpath);

              
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Copy - " + ex + "\n");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                explorer.CloseXPathApplication("Browser");
            }
            catch (Exception ex)
            {

                System.IO.File.AppendAllText(@"C:\Work\XPathLogs", "Window Closed - " + ex + "\n");
            }
        }

        private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
    }
}
