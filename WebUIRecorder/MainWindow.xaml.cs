using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebUIRecorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //public partial class MainWindow : Window
    //{
    //    public MainWindow()
    //    {
    //        InitializeComponent();
    //    }

    //}

    public partial class MainWindow : Window
    {
        Web explorer = null;
        public static MainWindow Webwin = null;

        public MainWindow()
        {
            InitializeComponent();
            explorer = Web.Instance;
            Webwin = this;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = Txt_Url.Text;
                string browser = Select_Browser.Text;


                explorer.OpenBrowserXPath(browser, url);
            }
            catch (Exception exc)
            {

            }
        }

        private void IndicateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnSimulation.Content.ToString() == "Indicate Element")
                {

                    explorer.StartScrapingforXPath("Browser");
                    btnSimulation.Content = "Stop";

                }
                else
                {
                    explorer.StopScrapingXPath("Browser");
                    btnSimulation.Content = "Indicate Element";
                }
            }
            catch (Exception exc)
            {

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


            }
        }

        private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
    }
}
