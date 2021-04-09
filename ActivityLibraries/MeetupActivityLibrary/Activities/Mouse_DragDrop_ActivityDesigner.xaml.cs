using CommonLibrary;
//using RehostedWorkflowDesigner.ImageCapture;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Bot.Activity.ActivityLibrary.Activities
{
    // Interaction logic for Mouse_DragDrop_ActivityDesigner.xaml
    public partial class Mouse_DragDrop_ActivityDesigner
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator System.Drawing.Point(POINT point)
            {
                return new System.Drawing.Point(point.X, point.Y);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
        public double X { get; set; }
        public double Y { get; set; }
        System.Timers.Timer myTimer = new System.Timers.Timer();

       
        public Mouse_DragDrop_ActivityDesigner()
        {
            InitializeComponent();
            SnippingTool.AreaSelected -= SnippingTool_AreaSelected;
            SnippingTool.AreaSelected += SnippingTool_AreaSelected;
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.Button), MouseDownEvent, new RoutedEventHandler(OnMouseDown));
            myTimer.Elapsed += new ElapsedEventHandler(OnTick);
            myTimer.Interval = 5000; // 1000 ms is one second
           
        }


        public static System.Drawing.Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

        private void OnTick(object sender, EventArgs eventArgs)
        {
            //Don't forget to stop the timer, or it'll continue to tick
            //System.Drawing.Point coordinates = new System.Drawing.Point();
            //int x = coordinates.X;
            //int y = coordinates.Y;
            myTimer.Stop();            //System.Windows.MessageBox.Show("Coordinates are: X is " + x + " Y is " + y);
           // MouseEventArgs me = (MouseEventArgs) eventArgs;
            //if(me.Button== System.Windows.Forms.MouseButtons.Right)
            //{
                System.Drawing.Point p = GetCursorPosition();
                int Xstart = p.X;
                int Ystart = p.Y;
                System.Windows.MessageBox.Show("X is " + Xstart + ", Y is " + Ystart);
            //Mouse_DragDrop db = null;
            //db = (Mouse_DragDrop)this.ModelItem.GetCurrentValue();
                owa.X_Start = Xstart;
                owa.Y_Start = Ystart;



            //}

        }
        private void ActivityDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            //window = Window.GetWindow(this);
            //owa = (Mouse_DragDrop)this.ModelItem.GetCurrentValue();
            //if ((owa.ImagePath != null) && (owa.ImagePath.Expression != null))
            //{
            //    img.Source = new BitmapImage(new Uri(owa.ImagePath.Expression.ToString(), UriKind.RelativeOrAbsolute));
            //}

        }


        //private void Mouse_Rightclick(object sender, EventArgs e)
        //{
        //     MouseEventArgs me = (MouseEventArgs)e;
        //    if (me.Button == System.Windows.Forms.MouseButtons.Right)
        //    {
        //        System.Drawing.Point p = GetCursorPosition();
        //        int Xstart = p.X;
        //        int Ystart = p.Y;
        //        System.Windows.MessageBox.Show("Hello "+"X is " + Xstart + " Y is " + Ystart);
        //    }
        //}

        private void button12_Click(object sender, EventArgs e)
        {
            //  MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButtons.YesNoCancel);
            //MessageBoxResult result = MessageBox.Show("Pick a position after clicking OK", "OK", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //if (true)
            //{
            //    myTimer.Start();
            //}
          

           
                myTimer.Start();
          
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
            // Image_FindAndClick owa = null;

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
        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    // Find the window that contains the control

        //    // Find the window that contains the control
        //    // Minimize
        //    window.WindowState = WindowState.Minimized;
        //    height = window.Height;
        //    width = window.Width;
        //    window.Height = 0;
        //    window.Width = 0;
        //    Thread.Sleep(1000);
        //    Bitmap bmp = null;
        //    // Image_FindAndClick owa = null;
        //    // owa = (Image_FindAndClick)this.ModelItem.GetCurrentValue();
        //    double accuracy = owa.Accuracy;
        //    string processDirectory = SelectHelper.ProjectLocation + "\\Images";
        //    if (!Directory.Exists(processDirectory))
        //    {
        //        Directory.CreateDirectory(processDirectory);
        //    }
        //    string ScreenPath = processDirectory + "\\" + owa.ImageId + ".png";

        //    if ((ScreenPath != null) && (ScreenPath != string.Empty))
        //    {
        //        if (File.Exists(ScreenPath))
        //        {
        //            GetSetClick getSetClick = new GetSetClick();
        //            ImageRecognition imgRecognition = new ImageRecognition();
        //            getSetClick = GetSetClick.GetSet;
        //            bool result = imgRecognition.GetSetClickImage(ScreenPath, getSetClick, "", 10000, accuracy);
        //        }
        //    }
        //    window.Height = height;
        //    window.Width = width;
        //    window.WindowState = WindowState.Normal;
        //}
        Mouse_DragDrop owa = null;
        private void SnippingTool_AreaSelected(object sender, EventArgs e)
        {

            // owa = (Image_FindAndClick)this.ModelItem.GetCurrentValue();
            try
            {
                if (SnippingTool.UniqueId == owa.ImageId)
                {
                    string processDirectory = SelectHelper.ProjectLocation + "\\Images";

                    string ScreenPath = processDirectory + "\\" + owa.ImageId + ".png";
                    var bmp = SnippingTool.Image;
                    bmp.Save(ScreenPath, ImageFormat.Bmp);
                   // img.Source = new BitmapImage(new Uri(ScreenPath, UriKind.RelativeOrAbsolute));
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
                Logger.Log.Logger.LogData(ex.Message + " in activity Mouse_DragDrop.xaml.cs", Logger.LogLevel.Error);
            }


        }

        //private void ActivityDesigner_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    e.Handled = true;
        //}
    }
}
