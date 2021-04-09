using CommonLibrary;
//using RehostedWorkflowDesigner.ImageCapture;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Bot.Activity.ActivityLibrary.Activities
{
    /// <summary>
    /// Interaction logic for AttachApplicationActivitiesDesigner.xaml
    /// </summary>
    public partial class Image_FindeAndSetText1
    {
        public double X { get; set; }
        public double Y { get; set; }

        Image_FindAndSetText owa = null;
        public Image_FindeAndSetText1()
        {
            InitializeComponent();
            SnippingTool.AreaSelected -= SnippingTool_AreaSelected;
            SnippingTool.AreaSelected += SnippingTool_AreaSelected;
            EventManager.RegisterClassHandler(typeof(Button), MouseDownEvent, new RoutedEventHandler(OnMouseDown));


        }
        private void ActivityDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            owa = (Image_FindAndSetText)this.ModelItem.GetCurrentValue();
            if ((owa.ImagePath != null) && (!string.IsNullOrEmpty(owa.ImagePath.Expression.ToString())))
            {
                img.Source = new BitmapImage(new Uri(owa.ImagePath.Expression.ToString(), UriKind.RelativeOrAbsolute));
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Find the window that contains the control
            // window = Window.GetWindow(this);

            // Minimize
            window.WindowState = WindowState.Minimized;
            height = window.Height;
            width = window.Width;
            window.Height = 0;
            window.Width = 0;
            Thread.Sleep(1000);
            // Image_FindAndSetText owa = null;

            string _imageId = Guid.NewGuid().ToString();
            owa.ImageId = _imageId;
            Bitmap printscreen = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(printscreen);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            SnippingTool.ImageBMP = printscreen;
            try
            {
                using (MemoryStream s = new MemoryStream())
                {
                    //save graphic variable into memory
                    printscreen.Save(s, ImageFormat.Bmp);
                    SnippingTool sn = new SnippingTool(System.Drawing.Image.FromStream(s), 0, 0, printscreen.Size.Width, printscreen.Size.Height);
                    //set the picture box with temporary stream
                    SnippingTool.UniqueId = _imageId;
                    SnippingTool.Snip();
                }
            }
            catch (Exception)
            {

            }
        }
        private void OnMouseDown(object sender, RoutedEventArgs e)
        {
            var element = sender as ContentControl;
            if (element != null)
            {
                ShowLocation(element);
            }
        }
        private void ShowLocation(ContentControl element)
        {
            var location = element.PointToScreen(new System.Windows.Point(0, 0));
            X = location.X;
            Y = location.Y;
        }
        Window window = null;
        double height = 0;
        double width = 0;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Find the window that contains the control

            // Find the window that contains the control
            // Minimize
            window.WindowState = WindowState.Minimized;
            height = window.Height;
            width = window.Width;
            window.Height = 0;
            window.Width = 0;
            Thread.Sleep(1000);
            Bitmap bmp = null;

            // Image_FindAndSetText owa = null;
            // owa = (Image_FindAndSetText)this.ModelItem.GetCurrentValue();

            double accuracy = owa.Accuracy;
            string processDirectory = SelectHelper.ProjectLocation + "\\Images";

            string ScreenPath = processDirectory + "\\" + owa.ImageId + ".png";

            if ((ScreenPath != null) && (ScreenPath != string.Empty))
            {
                if (File.Exists(ScreenPath))
                {
                    string sValue = owa.SetText.Expression.ToString();
                    GetSetClick getSetClick = new GetSetClick();
                    ImageRecognition imgRecognition = new ImageRecognition();
                    getSetClick = GetSetClick.Set;
                    bool result = imgRecognition.GetSetClickImage(ScreenPath, getSetClick, sValue, 10000, accuracy);
                }
            }
            window.Height = height;
            window.Width = width;
            window.WindowState = WindowState.Normal;
        }

        private void SnippingTool_AreaSelected(object sender, EventArgs e)
        {

            // owa = (Image_FindAndSetText)this.ModelItem.GetCurrentValue();
            try
            {
                if (SnippingTool.UniqueId == owa.ImageId)
                {
                    string processDirectory = SelectHelper.ProjectLocation + "\\Images";
                    if (!Directory.Exists(processDirectory))
                    {
                        Directory.CreateDirectory(processDirectory);
                    }
                    string ScreenPath = processDirectory + "\\" + owa.ImageId + ".png";
                    var bmp = SnippingTool.Image;
                    bmp.Save(ScreenPath, ImageFormat.Bmp);
                    img.Source = new BitmapImage(new Uri(ScreenPath, UriKind.RelativeOrAbsolute));
                    if (owa != null)
                    {
                        owa.ImagePath = ScreenPath;
                    }

                }
                window.Height = height;
                window.Width = width;
                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity Image_FindAndSetText.xaml.cs", Logger.LogLevel.Error);
            }

        }

        //private void ActivityDesigner_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    e.Handled = true;
        //}
    }
}
